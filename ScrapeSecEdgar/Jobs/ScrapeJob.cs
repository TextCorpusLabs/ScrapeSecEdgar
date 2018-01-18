using ScrapeSecEdgar.Models;
using ScrapeSecEdgar.Tools;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ScrapeSecEdgar.Jobs
{
    class ScrapeJob
    {
        const string _indexPattern = "https://www.sec.gov/Archives/edgar/full-index/{0}/QTR{1}/master.zip";
        EdgarConfiguration _config;

        public ScrapeJob(EdgarConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public async Task ScrapeAsync()
        {
            Console.WriteLine($"Getting all the {_config.FormType} forms for {_config.Start.ToString("yyyy/MM/dd")} to {_config.End.ToString("yyyy/MM/dd")}");

            var urlhelpres = new UrlHelpers(_config);

            using (var client = new HttpClient())
                foreach (var indexurl in urlhelpres.IndexList())
                {
                    Console.WriteLine($"Downloading {indexurl}");
                    var indexfile = await client.DownloadAndUnzipIndexAsync(indexurl);
                    Console.WriteLine($"Parsing {indexfile}");
                    var filings = await (new IndexFile(indexfile)).GetFilingsAsync();
                    Directory.Delete(Path.GetDirectoryName(indexfile), true);

                    Console.WriteLine($"Processing {filings.Count} filings");
                    filings = filings
                        .Where(p => Keep(p))
                        .OrderBy(p => p.DateFiled)
                        .ToList();
                    Console.WriteLine($"{filings.Count} {_config.FormType}s found. Downloading...");

                    foreach (var filing in filings)
                    {
                        Console.WriteLine($"{filing.DateFiled.ToString("yyyy/MM/dd")} Downloading {filing.Filename}");
                        await client.DownloadFilingAsync(filing, _config.SaveLocation);
                    }
                }
        }

        bool Keep(EdgarFiling filing)
        {
            return 1 == 1
                && _config.FormType == filing.FormType
                && _config.Start <= filing.DateFiled
                && filing.DateFiled <= _config.End;
        }
    }
}

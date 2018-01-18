using Polly;
using ScrapeSecEdgar.Models;
using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;

namespace ScrapeSecEdgar.Tools
{
    static class HttpClientExtensions
    {
        public static async Task<string> DownloadAndUnzipIndexAsync(this HttpClient client, string url)
        {
            var tmpfile = Path.GetTempFileName();
            var unzippath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));

            await Policy.Handle<Exception>()
                .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
                .ExecuteAsync(async () =>
                {
                    using (var streamin = await client.GetStreamAsync(url))
                    using (var streamout = new FileStream(tmpfile, FileMode.Open, FileAccess.Write))
                        await streamin.CopyToAsync(streamout);
                });

            ZipFile.ExtractToDirectory(tmpfile, unzippath);
            File.Delete(tmpfile);

            return Path.Combine(unzippath, "master.idx");
        }
        public static async Task DownloadFilingAsync(this HttpClient client, EdgarFiling filing, string saveLocation)
        {
            var dir = Path.Combine(saveLocation, filing.CIK.ToString());// filing.FormType, );
            Directory.CreateDirectory(dir);
            var file = Path.Combine(dir, $"{filing.DateFiled.ToString("yyyyMMdd")}.{filing.FormType}{Path.GetExtension(filing.Filename)}");

            await Policy.Handle<Exception>()
                .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
                .ExecuteAsync(async () =>
                {
                    using (var streamin = await client.GetStreamAsync(filing.Url))
                    using (var streamout = new FileStream(file, FileMode.Create, FileAccess.Write))
                        await streamin.CopyToAsync(streamout);
                });

        }
    }
}

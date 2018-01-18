using ScrapeSecEdgar.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ScrapeSecEdgar.Tools
{
    class IndexFile
    {
        const string _httpRootIdentifier = "Cloud HTTP:";
        const string _headerSeperatorIdentifier = "-------";

        string _idxFile;

        public IndexFile(string idxFile)
        {
            _idxFile = idxFile ?? throw new ArgumentNullException(nameof(idxFile));
        }

        public async Task<List<EdgarFiling>> GetFilingsAsync()
        {
            var result = new List<EdgarFiling>();
            string httproot = null;

            using (var reader = new StreamReader(_idxFile))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                    if (line.StartsWith(_httpRootIdentifier))
                    {
                        /// https://www.sec.gov/Archives/
                        httproot = line.Substring(_httpRootIdentifier.Length).Trim();
                        break;
                    }

                do
                {
                    line = await reader.ReadLineAsync();
                }
                while (line != null && !line.StartsWith(_headerSeperatorIdentifier));

                while ((line = await reader.ReadLineAsync()) != null)
                    result.Add(Parse(line, httproot));
            }

            return result;
        }

        static EdgarFiling Parse(string line, string httproot)
        {
            /// 1000032|BINCH JAMES G|4|2017-12-04|edgar/data/1000032/0000913165-17-000048.txt
            var seg = line.Split('|');

            return new EdgarFiling
            {
                CIK = int.Parse(seg[0]),
                CompanyName = seg[1],
                FormType = seg[2],
                DateFiled = DateTime.Parse(seg[3]),
                Filename = seg[4],
                Url = httproot + seg[4]
            };
        }
    }
}

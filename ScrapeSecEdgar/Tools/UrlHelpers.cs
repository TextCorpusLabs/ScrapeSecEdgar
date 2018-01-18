using ScrapeSecEdgar.Models;
using System;
using System.Collections.Generic;

namespace ScrapeSecEdgar.Tools
{
    class UrlHelpers
    {
        const string _indexPattern = "https://www.sec.gov/Archives/edgar/full-index/{0}/QTR{1}/master.zip";
        EdgarConfiguration _config;

        public UrlHelpers(EdgarConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public IEnumerable<string> IndexList()
        {
            var tmp = _config.Start;

            while (tmp < _config.End)
            {
                yield return string.Format(_indexPattern, tmp.Year, MonthToQuarter(tmp.Month));
                tmp = tmp.AddMonths(3);
            }
        }
        int MonthToQuarter(int month)
        {
            if ((1 <= month) && (month <= 3)) return 1;
            else if ((4 <= month) && (month <= 6)) return 2;
            else if ((7 <= month) && (month <= 9)) return 3;
            else if ((10 <= month) && (month <= 12)) return 4;
            else
                throw new ArgumentOutOfRangeException(nameof(month));
        }
    }
}

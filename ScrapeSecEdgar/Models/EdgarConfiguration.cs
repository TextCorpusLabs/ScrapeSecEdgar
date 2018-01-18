using Mono.Options;
using System;
using System.IO;

namespace ScrapeSecEdgar.Models
{
    class EdgarConfiguration
    {
        public string FormType { get; }
        public DateTime Start { get; }
        public DateTime End { get; }
        public string SaveLocation { get; }

        public EdgarConfiguration(string[] arrs)
        {
            var type = "10-K";
            var start = "1993/01/01";
            var end = DateTime.UtcNow.AddDays(1).ToString("yyyy/MM/dd");
            var save = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "EDGAR");

            var options = new OptionSet {
                { "f|formtype=", "The SEC form type. Found in master.idx 'Form Type'. Defaults to all forms.", p => type =  p },
                { "s|start=", "The date to start the pull. Found in master.idx 'Date Filed'. Defaults to 1993/01/10.", p => start = p },
                { "e|end=", "The date to end the pull. Found in master.idx 'Date Filed'. Defaults to today.", p => end = p },
                { "p|path=", "The path to save files to.", p => save = p }
            };

            var extras = options.Parse(arrs);

            FormType = type;
            Start = DateTime.Parse(start);
            End = DateTime.Parse(end);
            SaveLocation = save;
        }
    }
}

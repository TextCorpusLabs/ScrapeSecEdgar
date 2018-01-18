using System;

namespace ScrapeSecEdgar.Models
{
    class EdgarFiling
    {
        public int CIK { get; set; }
        public string CompanyName { get; set; }
        public DateTime DateFiled { get; set; }
        public string FormType { get; set; }
        public string Filename { get; set; }
        public string Url { get; set; }
    }
}

using ScrapeSecEdgar.Jobs;
using ScrapeSecEdgar.Models;
using System;

namespace ScrapeSecEdgar
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var config = new EdgarConfiguration(args);
                var job = new ScrapeJob(config);

                job.ScrapeAsync().Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine(new string('-', 30));
                Console.WriteLine(ex);
            }
            finally
            {
                Console.WriteLine("Done....");
                Console.ReadLine();
            }
        }
    }
}

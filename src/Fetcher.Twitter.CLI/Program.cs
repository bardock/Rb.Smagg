using System;

namespace Fetcher.Twitter.CLI
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            new TwitterFetcher().StartAsync().Wait();

            Console.ReadLine();
        }
    }
}
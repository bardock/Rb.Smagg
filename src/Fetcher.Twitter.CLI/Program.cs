using Fetcher.Core;
using System;
using System.Configuration;
using System.Net.Http;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Fetcher.Twitter.CLI
{
    internal class Program
    {
        private static readonly string _nginxPushStreamModuleUrl =
            ConfigurationManager.AppSettings["nginxPushStreamModuleUrl"];

        private static void Main(string[] args)
        {
            string filter = null;
            while (string.IsNullOrWhiteSpace(filter))
            {
                Console.Write("Tweets filter: ");
                filter = Console.ReadLine();
            }

            IStreamingFetcher fetcher = new TwitterFetcher();
            //IStreamingFetcher fetcher = new FakeStreamingFetcher();

            var sub = ActionExtensions.Decorate<string>(ConsoleLog, ToNginxPushStreamMod);

            Start(filter, fetcher, sub).Wait();

            Console.Write("Done!");
            Console.ReadLine();
        }

        private static async Task Start(string filter, IStreamingFetcher fetcher, Action<string> sub)
        {
            await (await fetcher.StartAsync(filter))
                .ForEachAsync(sub);
        }

        private static void ConsoleLog(string body)
        {
            Console.WriteLine(body + "\n\n");
        }

        private static void ToNginxPushStreamMod(string text)
        {
            var client = new HttpClient();

            client.PostAsync(_nginxPushStreamModuleUrl, new StringContent(text))
                .ContinueWith(_ => client.Dispose());
        }
    }
}
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
        private static string _nginxPushStreamModuleUrl = ConfigurationManager.AppSettings["nginxPushStreamModuleUrl"];

        private static void Main(string[] args)
        {
            Console.Write("Tweets filter: ");
            var filter = Console.ReadLine();

            IStreamingFetcher fetcher = new TwitterFetcher();
            //IStreamingFetcher fetcher = new FakeStreamingFetcher();

            var sub = Decorate<string>(ConsoleLog, ToNginxPushStreamMod);

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

        private static Action<T> Decorate<T>(params Action<T>[] actions)
        {
            Action<T> result = x => { };
            foreach (var action in actions)
            {
                var _r = result;
                result = x => { _r(x); action(x); };
            }
            return result;
        }
    }
}
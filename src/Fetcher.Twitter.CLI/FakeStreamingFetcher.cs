using Fetcher.Core;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Fetcher.Twitter.CLI
{
    internal class FakeStreamingFetcher : IStreamingFetcher
    {
        private static readonly TimeSpan _interval = TimeSpan.FromSeconds(1);

        public Task<IObservable<string>> StartAsync(string filter)
        {
            return Task.FromResult(
                Observable.Interval(_interval)
                    .Zip(GetInfitineSeq(), (_, x) => x));
        }

        private IEnumerable<string> GetInfitineSeq()
        {
            while (true)
            {
                yield return Guid.NewGuid().ToString();
            }
        }
    }
}
using System;
using System.Threading.Tasks;

namespace Fetcher.Core
{
    public interface IStreamingFetcher
    {
        Task<IObservable<string>> StartAsync(string filter);
    }
}
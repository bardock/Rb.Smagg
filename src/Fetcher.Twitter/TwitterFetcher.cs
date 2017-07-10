using Fetcher.Core;
using LinqToTwitter;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Fetcher.Twitter
{
    public class TwitterFetcher : IStreamingFetcher
    {
        public async Task<IObservable<string>> StartAsync(string filter)
        {
            return (await GetQuery(filter).ToObservableAsync())
                .Select(x => (dynamic)JsonConvert.DeserializeObject(x.Content))
                .Select(x => (string)x.text);
        }

        private IQueryable<Streaming> GetQuery(string filter)
        {
            var auth = GetAuthorizer();

            var twitterCtx = new TwitterContext(auth);

            return (from strm in twitterCtx.Streaming
                    where strm.Type == StreamingType.Filter &&
                    strm.Track == filter
                    select strm);
        }

        private IAuthorizer GetAuthorizer()
        {
            return new SingleUserAuthorizer
            {
                CredentialStore = new InMemoryCredentialStore
                {
                    ConsumerKey = ConfigurationManager.AppSettings["twitterConsumerKey"],
                    ConsumerSecret = ConfigurationManager.AppSettings["twitterConsumerSecret"],
                    OAuthToken = ConfigurationManager.AppSettings["twitterAccessToken"],
                    OAuthTokenSecret = ConfigurationManager.AppSettings["twitterAccessTokenSecret"]
                }
            };
        }
    }
}
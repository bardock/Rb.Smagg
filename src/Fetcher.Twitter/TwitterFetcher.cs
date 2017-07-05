using LinqToTwitter;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Fetcher.Twitter
{
    public class TwitterFetcher
    {
        public async Task StartAsync()
        {
            var auth = new SingleUserAuthorizer
            {
                CredentialStore = new InMemoryCredentialStore
                {
                    ConsumerKey = "",//ConfigurationManager.AppSettings["twitterConsumerKey"],
                    ConsumerSecret = "",//ConfigurationManager.AppSettings["twitterConsumerSecret"],
                    OAuthToken = "",//ConfigurationManager.AppSettings["twitterAccessToken"],
                    OAuthTokenSecret = ""//ConfigurationManager.AppSettings["twitterAccessTokenSecret"]
                }
            };

            Console.WriteLine("\nStreamed Content: \n");
            int count = 0;

            var twitterCtx = new TwitterContext(auth);
            await
                (from strm in twitterCtx.Streaming
                 where strm.Type == StreamingType.Filter &&
                 strm.Track == "twitter"//"#ligamundial"
                 select strm)
                .StartAsync(strm =>
                {
                    Console.WriteLine(strm.Content + "\n\n");

                    if (count++ >= 5)
                        strm.CloseStream();

                    return Task.CompletedTask;
                });
        }
    }
}
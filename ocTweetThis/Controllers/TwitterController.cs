using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Exceptions;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Web.BackOffice.Controllers;
using ocTweetThis.Models;
using Umbraco.Extensions;
using ocTweetThis.TwitterContentApp;
using Microsoft.Extensions.Options;
using ocTweetThis.Composers;
using Tweetinvi.Parameters;
using Umbraco.Cms.Core;
using System.Text;

namespace ocTweetThis.TwitterContentApp
{
    [ApiController]
    [Route("[controller]")]
    public class TwitterController : ControllerBase
    {
        private readonly ILogger<TwitterController> _logger;

        private readonly IScopeProvider scopeProvider;

        private readonly IOptions<OCTweetThisSettings> _tweetSettings;

        private readonly IPublishedContentQuery _publishedContentQuery;

        public TwitterController(ILogger<TwitterController> logger, IScopeProvider scopeProvider, IOptions<OCTweetThisSettings> tweetThisSettings, IPublishedContentQuery publishedContentQuery)
        {
            _logger = logger;
            this.scopeProvider = scopeProvider;
            this._tweetSettings = tweetThisSettings;
            this._publishedContentQuery = publishedContentQuery;
        }

        public async Task<IActionResult> Index(string message, int nodeId, string url)
        {

            var curatedMessage =  createTweet(message, url);

            try
            {
                var userClient = new TwitterClient(_tweetSettings.Value.ConsumerKey, _tweetSettings.Value.ConsumerSecret, _tweetSettings.Value.AccessToken, _tweetSettings.Value.AccessSecret);

           
                await userClient.Tweets.PublishTweetAsync(new PublishTweetParameters(curatedMessage));


                var publishedTweet = new TweetsPublished()
                {
                   
                    DatePublished = DateTime.Now,
                    TweetMessage = curatedMessage,
                    BlogPostUmbracoId = nodeId
                };

                try
                {
                    using var scope = scopeProvider.CreateScope();
                    var db = scope.Database;
                    db.Insert<TweetsPublished>(publishedTweet);

                    scope.Complete();
                }
                catch(Exception ex)
                {
                    _logger.LogError("Unable to update database with tweet : ", ex);
                }
               
            }
            catch (TwitterException te)
            {
                _logger.LogError("Error trying to publish the tweet", te);
                return View();
            }
            
            return View();
        }

        private ActionResult View()
        {
            throw new NotImplementedException();
        }

        private string createTweet(string userMessage, string url)
        {
            
           
            StringBuilder sb = new StringBuilder();
            sb.Append(userMessage);
            sb.AppendLine();
            sb.Append(url);

            return sb.ToString();
        }

       
    }

   
}


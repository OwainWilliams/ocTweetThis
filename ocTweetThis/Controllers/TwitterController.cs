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

namespace ocTweetThis.TwitterContentApp
{
    [ApiController]
    [Route("[controller]")]
    public class TwitterController : ControllerBase
    {
        private readonly ILogger<TwitterController> _logger;

        private readonly IScopeProvider scopeProvider;

        private readonly IOptions<OCTweetThisSettings> _tweetSettings; 

        public TwitterController(ILogger<TwitterController> logger, IScopeProvider scopeProvider, IOptions<OCTweetThisSettings> tweetThisSettings)
        {
            _logger = logger;
            this.scopeProvider = scopeProvider;
            this._tweetSettings = tweetThisSettings;
        }

        public async Task<IActionResult> Index(string message, int nodeId)
        {
            try
            {
                var userClient = new TwitterClient(_tweetSettings.Value.ConsumerKey, _tweetSettings.Value.ConsumerSecret, _tweetSettings.Value.AccessToken, _tweetSettings.Value.AccessSecret);

                await userClient.Tweets.PublishTweetAsync(new PublishTweetParameters(message));

                var publishedTweet = new TweetsPublished()
                {
                   
                    DatePublished = DateTime.Now,
                    TweetMessage = message,
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

       
    }

   
}


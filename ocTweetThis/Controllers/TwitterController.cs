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

using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace ocTweetThis.TwitterContentApp
{
    [ApiController]
    [Route("[controller]")]
    public class TwitterController : ControllerBase
    {
        private readonly ILogger<TwitterController> _logger;

        private readonly IScopeProvider scopeProvider;

        private readonly IOptions<OCTweetThisSettings> _tweetSettings;
        private readonly INotificationService _notificationService;

        private readonly IPublishedContentQuery _publishedContentQuery;
        private Tweetinvi.Models.ITweet tweet;

        public TwitterController(ILogger<TwitterController> logger,
            IScopeProvider scopeProvider,
            IOptions<OCTweetThisSettings> tweetThisSettings,
            IPublishedContentQuery publishedContentQuery,
            INotificationService notificationService)
        {
            _logger = logger;
            this.scopeProvider = scopeProvider;
            this._tweetSettings = tweetThisSettings;
            this._publishedContentQuery = publishedContentQuery;
            this._notificationService = notificationService;
        }

        public async Task<IActionResult> Index(string message, int nodeId, string url)
        {

            var curatedMessage = createTweet(message, url);
           
            try
            {
                var userClient = new TwitterClient(_tweetSettings.Value.ConsumerKey, _tweetSettings.Value.ConsumerSecret, _tweetSettings.Value.AccessToken, _tweetSettings.Value.AccessSecret);
                
                if (_tweetSettings.Value.EnableLiveTweeting)
                {
                    //Publish tweet
                     var published= await userClient.Tweets.PublishTweetAsync(new PublishTweetParameters(curatedMessage));
                     tweet = await userClient.Tweets.GetTweetAsync(published.Id);
                   
                }
              
                if(_tweetSettings.Value.EnableLiveTweeting || (!_tweetSettings.Value.EnableLiveTweeting && _tweetSettings.Value.EnableTestMode))
                {
                    var publishedTweet = new TweetsPublished()
                    {
                        TweetUrl = tweet?.Url ?? "",
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
                    catch (Exception ex)
                    {
                        _logger.LogError("Unable to update database with tweet : ", ex);
                    }
                }
               

            }
            catch (TwitterException te)
            {
                _logger.LogError("Error trying to publish the tweet", te);
                var errRes = new JsonResult(new { messge = "Oops something went wrong" });
                errRes.StatusCode = 500;

                return errRes;



            }

            return Ok();
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


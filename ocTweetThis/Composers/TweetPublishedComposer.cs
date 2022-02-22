using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Notifications;
using OCTweets.Migrations;
using Microsoft.Extensions.DependencyInjection;
using ocTweetThis.Models;

namespace ocTweetThis.Composers
{
    public class TweetPublishedComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.AddNotificationHandler<UmbracoApplicationStartingNotification, TweetsPublishedMigration>();
            
            builder.Services.AddOptions<OCTweetThisSettings>().Bind(builder.Config.GetSection("ocTweetThis"));
        }
    }
}

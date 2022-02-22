using ocTweetThis.Migrations;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Notifications;

namespace ocTweetThis.Composers
{
    public class AddTweetUrlColumnComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.AddNotificationHandler<UmbracoApplicationStartingNotification, AddTweetUrlColumnMigration>();
        }
    }
}

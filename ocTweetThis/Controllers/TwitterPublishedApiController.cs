
using System.Collections.Generic;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.Attributes;
using ocTweetThis.Models;
using Umbraco.Extensions;

namespace ocTweetThis.TwitterContentApp
{
    [PluginController("ocTweetThis")]
    public class TwitterPublishedApiController : UmbracoAuthorizedApiController
    {
        private readonly IScopeProvider scopeProvider;

        public TwitterPublishedApiController(IScopeProvider scopeProvider)
        {
            this.scopeProvider = scopeProvider;
        }

        public IEnumerable<TweetsPublished> GetAll(int blognodeId)
        {
            using var scope = scopeProvider.CreateScope(autoComplete:true);
            var fluentSql = scope.SqlContext.Sql()
                .Select("*")
                .From<TweetsPublished>()
                .Where<TweetsPublished>(t => t.BlogPostUmbracoId == blognodeId);

            var tweetsPublished = scope.Database.Fetch<TweetsPublished>(fluentSql);

            return tweetsPublished;
        }
    }

}

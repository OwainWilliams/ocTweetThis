using Microsoft.Extensions.Logging;
using NPoco;
using System;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Migrations;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Scoping;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Migrations;
using Umbraco.Cms.Infrastructure.Migrations.Upgrade;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;

namespace OCTweets.Migrations
{
    public class TweetsPublishedMigration : INotificationHandler<UmbracoApplicationStartingNotification>
    {
        private readonly IMigrationPlanExecutor _migrationPlanExecutor;
        private readonly IScopeProvider _scopeProvider;
        private readonly IKeyValueService _keyValueService;
        private readonly IRuntimeState _runtimeState;

        public TweetsPublishedMigration(
            IScopeProvider scopeProvider,
            IMigrationPlanExecutor migrationPlanExecutor,
            IKeyValueService keyValueService,
            IRuntimeState runtimeState)
        {
            _migrationPlanExecutor = migrationPlanExecutor;
            _scopeProvider = scopeProvider;
            _keyValueService = keyValueService;
            _runtimeState = runtimeState;
        }
        public void Handle(UmbracoApplicationStartingNotification notification)
        {
            if (_runtimeState.Level < RuntimeLevel.Run)
            {
                return;
            }

            // Create a migration plan for a specific project/feature
            // We can then track that latest migration state/step for this project/feature
            var migrationPlan = new MigrationPlan("PublishedTweets");

            // This is the steps we need to take
            // Each step in the migration adds a unique value
            migrationPlan.From(string.Empty)
                .To<AddTweetPublishedTable>("tweetPublished-db");

            // Go and upgrade our site (Will check if it needs to do the work or not)
            // Based on the current/latest step
            var upgrader = new Upgrader(migrationPlan);
            upgrader.Execute(
                _migrationPlanExecutor,
                _scopeProvider,
                _keyValueService);
        }
    }

    public class AddTweetPublishedTable : MigrationBase
    {
        public AddTweetPublishedTable(IMigrationContext context) : base(context)
        {
        }
        protected override void Migrate()
        {
            Logger.LogDebug("Running migration {MigrationStep}", "AddTweetPublishedTable");

            // Lots of methods available in the MigrationBase class - discover with this.
            if (TableExists("TweetsPublished") == false)
            {
                Create.Table<TweetPublishedSchema>().Do();
            }
            else
            {
                Logger.LogDebug("The database table {DbTable} already exists, skipping", "TweetPublished");
            }
        }
        [TableName("TweetsPublished")]
        [PrimaryKey("Id", AutoIncrement = true)]
        [ExplicitColumns]
        public class TweetPublishedSchema
        {
            [PrimaryKeyColumn(AutoIncrement = true, IdentitySeed = 1)]
            [Column("Id")]
            public int Id { get; set; }

            [Column("BlogPostUmbracoId")]
            public int BlogPostUmbracoId { get; set; }

            [Column("TweetMessage")]
            [SpecialDbType(SpecialDbTypes.NTEXT)]
            public string Message { get; set; }

            [Column("DatePublished")]
            public DateTime DatePublished { get; set; }
        }
    }
}

using System;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.Packaging;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Strings;
using Umbraco.Cms.Infrastructure.Migrations;
using Umbraco.Cms.Infrastructure.Packaging;

namespace ocTweetThis
{
    /// <summary>
    ///  we only have this class, so there is a dll in the root
    ///  uSync package.
    ///  
    ///  With a root dll, the package can be stopped from installing
    ///  on .netframework sites.
    /// </summary>
    public static class TweetThisInstalled
    {
        public static string PackageName = "ocTweetThis";
        // private static string Welcome = "uSync all the things";
    }

    /// <summary>
    ///  A package migration plan, allows us to put uSync in the list 
    ///  of installed packages. we don't actually need a migration 
    ///  for uSync (doesn't add anything to the db). but by doing 
    ///  this people can see that it is insalled. 
    /// </summary>
    public class ocTweetThisMigrationPlan : PackageMigrationPlan
    {
        public ocTweetThisMigrationPlan() :
            base(TweetThisInstalled.PackageName)
        { }

        protected override void DefinePlan()
        {
            To<SetupTweetThis>(new Guid("333b33ed-f925-446a-88ec-b623b63ccc45"));
        }
    }

    public class SetupTweetThis : PackageMigrationBase
    {
        public SetupTweetThis(
            IPackagingService packagingService,
            IMediaService mediaService,
            MediaFileManager mediaFileManager,
            MediaUrlGeneratorCollection mediaUrlGenerators,
            IShortStringHelper shortStringHelper,
            IContentTypeBaseServiceProvider contentTypeBaseServiceProvider,
            IMigrationContext context)
            : base(packagingService, mediaService, mediaFileManager, mediaUrlGenerators, shortStringHelper, contentTypeBaseServiceProvider, context)
        {
        }

        protected override void Migrate()
        {
            // we don't actually need to do anything, but this means we end up
            // on the list of installed packages. 
        }
    }
}

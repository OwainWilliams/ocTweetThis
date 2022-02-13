using System;
using NPoco;
using Umbraco.Cms.Infrastructure.Persistence.DatabaseAnnotations;

namespace ocTweetThis.Models
{
    
    [TableName("TweetsPublished")]
    [PrimaryKey("Id")]
    public class TweetsPublished
    {
        [PrimaryKeyColumn(AutoIncrement = true, IdentitySeed = 1)]
       
        public int Id { get; set; }
        public int BlogPostUmbracoId { get; set; }
        public string TweetMessage { get; set; }
        public DateTime DatePublished { get; set; }


    }
}

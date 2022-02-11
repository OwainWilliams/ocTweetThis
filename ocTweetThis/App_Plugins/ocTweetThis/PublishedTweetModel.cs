using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ocTweetThis.TwitterContentApp
{
    public class PublishedTweetModel
    {
        public int Id {get;set;}
        public int BlogPostUmbracoId { get; set; }
        public string TweetMessage { get; set; }
        public DateTime DatePublished { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ocTweetThis.Models
{
    public class OCTweetThisSettings
    {
        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
        public string AccessToken { get; set; }
        public string AccessSecret { get; set; }

        public bool EnableLiveTweeting { get; set; }
    }
}

# ocTweetThis
Tweet This Umbraco Content App Package


## Setup
You need to get a Twitter API key from - https://developer.twitter.com/en/portal/dashboard

Once you have the details for your API, you need to update the appsettings.json file in your Umbraco project
```
"ocTweetThis": {
    "ConsumerKey": "twitter_consumer_key",
    "ConsumerSecret": "twitter_consumer_secret",
    "AccessToken": "twitter_access_token",
    "AccessSecret": "twitter_access_secret",
    "EnableLiveTweeting":  "True"
  }
 ```

You can set the value for "EnableLiveTweeting" to false to allow you to test the package without tweeting out to the real world. You don't need API keys for this either. 

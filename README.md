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
  }
 ```

## Test mode

If you want to test Tweet This without an API Key from Twitter you can use Test Mode. Copy and Paste the snippet below in to the `appsettings.json
```
    "ConsumerKey": "twitter_consumer_key",
    "ConsumerSecret": "twitter_consumer_secret",
    "AccessToken": "twitter_access_token",
    "AccessSecret": "twitter_access_secret",
    "EnableLiveTweeting": "False",
    "EnableTestMode":  "True"
```

Just add the two optional values to the appsettings.json file - `"EnableLiveTweeting": "False"` and `"EnableTestMode": "True"`. This will allow you to see the functionality of the package, minus it actually tweeting out to your account. 


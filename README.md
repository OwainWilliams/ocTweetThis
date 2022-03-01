# <img src="tweet.png" alt="Tweet Icon" width="50"/> Tweet This 
## What is this? 

Tweet This is an Umbraco Content App.
It sits in the backoffice of Umbraco and allows a content editor or any backoffice user of Umbraco to create a tweet for the node and then tweet to a linked account. The tweet will then be published on Twitter with a link back to the webpage.

There is also a history of all tweets created and a link to that tweet from the backoffice. 

//TODO Add image of backoffice


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


<a href="https://www.flaticon.com/free-icons/tweet" title="tweet icons">Tweet icons created by Freepik - Flaticon</a>


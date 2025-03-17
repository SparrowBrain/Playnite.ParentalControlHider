# Playnite Parental Control Hider
![DownloadCountTotal](https://img.shields.io/github/downloads/sparrowbrain/playnite.parentalcontrolhider/total?label=total%20downloads&style=for-the-badge)
![LatestVersion](https://img.shields.io/github/v/release/SparrowBrain/Playnite.ParentalControlHider?label=Latest%20version&style=for-the-badge)
![DownloadCountLatest](https://img.shields.io/github/downloads/SparrowBrain/Playnite.ParentalControlHider/latest/total?style=for-the-badge)


## What is it?
Automatically hide games not appropriate for younger audiences.

![ParentalControlHider settings view](/ci/screenshots/01.jpg)

I created this extension to protect my youngling from accidentally stumbling into game trailers that would not be approprate for him when I step away for couple of minutes. It is not intended as an actual content control. As such, it will only work with fairly young children, since it is very easy to get all the games to be shown, and there aren't any locking mechanisms.

## How to use it?
* Configure the plugin using addon settings. Main -> Add-ons... -> Extension settings -> Generic -> Parental Control Hider.
* Hide the games using extension menu. Main -> Extensions -> Parental Control Hider -> Hide Games.
* When you want to see the hidden games, use the same extension menu. Main -> Extensions -> Parental Control Hider -> Unhide Games.

The hidden games get a `[ParentalControlHider] Hidden` tag to make them easier to find and show again.

## Settings
### General
Configure if/when the games are hidden automatically. You can:
* Hide games on Playnite start
* Hide games after library update
* Hide games some time after they're un-hidden. In this case, the games will be automatically hidden after the specified time

### Age Ratings
Lists all the age ratings available in the library. Some of them will have pre-configured ages. You will have to select which age ratings you want to use and what age each rating corresponds to.

Also you will have to enter the birthday of the child you want to protect. This is used to calculate the age of the child and compare it to the selected age ratings.

Age ratings work quite permisively:
* If the game has none of the selected age ratings, it will be allowed.
* If the game has several of the selected age ratings, the lowest one will be used. Meaning it will pick the age rating for the youngest kid.

This was meant to be the main functionality of the extension. But since not all games have age ratings, I've added couple more ways to filter out the games.

### Tags
Select a list of blacklisted tags. If a game has any of the selected tags, it will be hidden.

### Genres
Select a list of blacklisted genres. If a game has any of the selected genres, it will be hidden.

### Games
Contains a list of whitelisted games. If a game is on the list, it will not be hidden, even if it contains any blacklisted tags, genres or is not approptiate according to the age rating. This lets you selectively pick games that you deem ok.

A game can be added to the whitelist from game's context menu. Parental Control Hider -> Add to Whitelist.

## Installation
You can install it either from Playnite's addon browser, or from [the web addon browser](https://playnite.link/addons.html#ParentalControlHider_134725de-cfcb-4474-849b-5d9c52babb75).

## Localization
You can help translate the extension to your language on the [Crowdin](https://crowdin.com/project/sparrowbrain-playnite-parental) page.
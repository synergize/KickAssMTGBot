# KickAssMTGBot

KickAssMTGBot is a Discord Bot that allows a user to input commands to acquire information about Magic the Gathering cards as well as acquires pricing data and displays it within a configured channel.

## Feedback
I'm always open for feedback on feature implementation or code structure. Please don't hesitate to reach out and let me know! 

## Installation

Currently not being distributed.

## Third-Party Implementation
- [Discord.net Library](https://discord.foxbot.me/docs/)
- [Scryfall API](https://scryfall.com/)
- [Scraping MTGGoldfish Movers and Shakers](https://www.mtggoldfish.com/movers/paper/modern)
- [.NET Core 2.1](https://dotnet.microsoft.com/download/dotnet-core/2.1)
- [Selenium](https://selenium.dev/)

## Current To-Add List
1. Implement caching system for API Calls. 
1. Hyperlinks for card names on Movers and Shakers data.
1. Implement logging system. 
1. Add user ability to configure format on a channel-by-channel basis.



## Usage
![Acquire Card Data](https://i.imgur.com/5x6ASwP.png)
![Acquire Card Rulings](https://i.imgur.com/npeTFkC.png)

## Configure Movers and Shakers
All configuration commands must be made through a channel that the bot can read messages and send messages in. 
Additionally, only users with the ability to manage channels can configure this feature.

![!mtgsetchannel #channel-name](https://i.imgur.com/wNbRNpX.jpg) 
![!mtgaddformat format-name](https://i.imgur.com/4LByQws.jpg)
![!mtgremoveformat format-name](https://i.imgur.com/k38E5hg.jpg)

Enjoy!

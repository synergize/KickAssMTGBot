using Discord;
using Discord.Commands;
using MTGBot.DataLookup;
using MTGBot.Embed_Output;
using MTGBot.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MTGBot.User_Message_Handler
{
    public class UserMessageController
    {
        public EmbedBuilder MessageOutput { get; set; }
        public UserMessageController(MatchCollection matches)
        {
            MessageOutput = DetermineTypeOfMessage(matches);
        }
        public EmbedBuilder DetermineTypeOfMessage(MatchCollection matches)
        {
            MTGCardOutput MessageOutput = new MTGCardOutput();

            foreach (var item in matches)
            {
                var cardData = GetScryFallData.PullScryfallData(item.ToString());
                if (cardData == null)
                {
                    return MessageOutput.DetermineFailure(0);
                }
                
                if (item.ToString().Contains('?'))
                {
                    return MessageOutput.RulingOutput(GetScryFallData.PullScryFallRuleData(cardData.Id), cardData);
                }
                else
                {
                    return MessageOutput.CardOutput(cardData);
                }
            }
            return MessageOutput.DetermineFailure(3);
        }
    }


}

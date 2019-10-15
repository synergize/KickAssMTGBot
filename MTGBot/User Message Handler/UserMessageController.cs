﻿using Discord;
using Discord.Commands;
using MTGBot.DataLookup;
using MTGBot.Embed_Output;
using MTGBot.Helpers;
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
        public UserMessageController(string matchName)
        {
            MessageOutput = DetermineTypeOfMessage(matchName);
        }
        public EmbedBuilder DetermineTypeOfMessage(string matchName)
        {
            MTGCardOutput MessageOutput = new MTGCardOutput();

            var cardData = GetScryFallData.PullScryfallData(matchName);
            if (cardData == null)
            {
                var autoComplete = GetScryFallData.PullScryFallAutoComplete(FormatUserInput.FormatEntry(matchName));

                if (autoComplete.data.Count == 0)
                {
                    return MessageOutput.DetermineFailure(0);
                }
                else
                {
                    return MessageOutput.DetermineFailure(0, autoComplete);
                }

            }

            if (matchName.Contains('?'))
            {
                return MessageOutput.RulingOutput(GetScryFallData.PullScryFallRuleData(cardData.Id), cardData);
            }
            else
            {
                return MessageOutput.CardOutput(cardData);
            }
        }
    }


}

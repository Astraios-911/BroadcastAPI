using HarmonyLib;
using StardewValley;
using StardewValley.Objects;
using System.Collections.Generic;
using System.Linq;

namespace BroadcastAPI
{
    /// <summary>
    /// Contains Harmony patches for custom TV channels.
    /// </summary>
    public static class TvPatches
    {
        /// <summary>
        /// Adds custom and edited channels to the TV menu.
        /// </summary>
        public static bool CreateQuestionDialoguePrefix(string question, ref Response[] answerChoices)
        {
            // Only process TV channel selection
            if (question != Game1.content.LoadString("Strings\\StringsFromCSFiles:TV.cs.13120"))
                return true;

            var choices = new List<Response>(answerChoices);
            int insertPos = choices.Count - 1; // Before "Leave" option

            // Add custom channels that aren't hidden
            foreach (var channel in ModEntry.CustomChannels.Data.Values.Where(c => !c.HideFromMenu))
                choices.Insert(insertPos++, new Response(channel.Name, channel.Displayname));

            // Handle edited channels
            foreach (var (key, edit) in ModEntry.EditChannels.Data)
            {
                if (edit.HideFromMenu == true) continue;

                var displayName = edit.Displayname ?? key;
                var existingIdx = choices.FindIndex(r => r.responseKey == key);

                if (existingIdx >= 0)
                    choices[existingIdx] = new Response(key, displayName);
                else
                    choices.Insert(insertPos++, new Response(key, displayName));
            }

            answerChoices = choices.ToArray();
            return true;
        }

        /// <summary>
        /// Sets up custom channel handling after TV interaction.
        /// </summary>
        public static void CheckForActionPostfix(TV __instance)
        {
            Game1.currentLocation.afterQuestion += (who, which) =>
                ChannelPlayer.SelectCustomChannel(__instance, who, which);
        }

        /// <summary>
        /// Notifies the API when TV is turned off.
        /// </summary>
        public static void TurnOffTVPostfix()
        {
            (ModEntry.Instance?.GetApi() as Api)?.InvokeOnTVTurnedOff();
        }
    }
}

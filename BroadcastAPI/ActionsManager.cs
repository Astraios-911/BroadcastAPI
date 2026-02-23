using StardewValley;
using StardewValley.Triggers;
using System.Collections.Generic;

namespace BroadcastAPI
{
    internal static class ActionsManager
    {
        /// <summary>
        /// Executes the actions defined for a custom or edited channel.
        /// </summary>
        public static void RunChannelActions(object channel)
        {
            var actions = channel switch
            {
                CustomChannelData custom => custom.Actions,
                EditChannelData edit => edit.Actions,
                _ => null
            };

            if (actions == null || actions.Count == 0) 
                return;

            ExecuteActions(actions, "channel");
        }

        /// <summary>
        /// Executes the actions defined for an answer.
        /// </summary>
        public static void RunAnswerActions(QuestionsData.AnswerData answer)
        {
            if (answer?.Actions == null || answer.Actions.Count == 0)
                return;

            ExecuteActions(answer.Actions, "answer");
        }

        /// <summary>
        /// Executes a list of actions.
        /// </summary>
        private static void ExecuteActions(List<string> actions, string context)
        {
            foreach (var actionOrId in actions)
            {
                if (!TriggerActionManager.TryRunAction(actionOrId, out string error, out var exception))
                {
                    var errorMessage = error ?? exception?.Message ?? "Unknown error";
                    ModEntry.ModMonitor?.Log(
                        $"Failed to run {context} action '{actionOrId}': {errorMessage}", 
                        StardewModdingAPI.LogLevel.Error);
                }
            }
        }
    }
}

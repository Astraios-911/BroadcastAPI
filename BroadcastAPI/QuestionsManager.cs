using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Objects;
using System.Linq;

namespace BroadcastAPI
{
    /// <summary>
    /// Manages BQuestions and EQuestions dialog interactions for TV channels.
    /// </summary>
    public static class QuestionsManager
    {
        /// <summary>
        /// Shows the BQuestions dialog AND PLAYS the channel after any answer (hurray)
        /// </summary>
        public static void ShowBQuestions(TV tv, CustomChannelData channel)
        {
            ModEntry.ModMonitor?.Log($"[ShowBQuestions] Starting for channel: {channel.Name}", LogLevel.Debug);
            ModEntry.ModMonitor?.Log($"[ShowBQuestions] Question: {channel.BQuestions.Question}", LogLevel.Debug);
            ModEntry.ModMonitor?.Log($"[ShowBQuestions] Answer count: {channel.BQuestions.Answers.Count}", LogLevel.Debug);

            var responses = channel.BQuestions.Answers
                .Select((ans, idx) => new Response($"bq_{idx}", ans.Text))
                .ToArray();

            ModEntry.ModMonitor?.Log($"[ShowBQuestions] Created {responses.Length} response options", LogLevel.Debug);

            // Store the channel and TV reference for the callback
            var capturedChannel = channel;
            var capturedTV = tv;

            DelayedAction.functionAfterDelay(() =>
            {
                Game1.currentLocation.createQuestionDialogue(
                    channel.BQuestions.Question,
                    responses,
                (who, whichAnswer) =>
                {
                    ModEntry.ModMonitor?.Log($"[ShowBQuestions] BQuestion answered: {whichAnswer}", LogLevel.Debug);

                    // Execute actions from selected answer
                    if (whichAnswer.StartsWith("bq_") && int.TryParse(whichAnswer.Substring(3), out int answerIdx))
                    {
                        if (answerIdx >= 0 && answerIdx < channel.BQuestions.Answers.Count)
                        {
                            var selectedAnswer = channel.BQuestions.Answers[answerIdx];
                            ActionsManager.RunAnswerActions(selectedAnswer);
                        }
                    }

                    ModEntry.ModMonitor?.Log($"[ShowBQuestions] Now playing channel: {capturedChannel.Name}", LogLevel.Debug);
                    ChannelPlayer.PlayChannel(capturedTV, capturedChannel);
                    ModEntry.ModMonitor?.Log($"[ShowBQuestions] PlayChannel call completed", LogLevel.Debug);
                }
                );
                ModEntry.ModMonitor?.Log($"[ShowBQuestions] Question dialog created", LogLevel.Debug);
            }, 0);
        }

        /// <summary>
        /// Shows the EQuestions dialog AFTER the channel plays and before next channel
        /// </summary>
        public static void ShowEQuestions(TV tv, CustomChannelData channel, System.Collections.Generic.List<string>? nextChannelName)
        {
            ModEntry.ModMonitor?.Log($"[ShowEQuestions] Starting for channel: {channel.Name}", LogLevel.Debug);
            ModEntry.ModMonitor?.Log($"[ShowEQuestions] Question: {channel.EQuestions.Question}", LogLevel.Debug);
            ModEntry.ModMonitor?.Log($"[ShowEQuestions] Answer count: {channel.EQuestions.Answers.Count}", LogLevel.Debug);

            var responses = channel.EQuestions.Answers
                .Select((ans, idx) => new Response($"eq_{idx}", ans.Text))
                .ToArray();

            ModEntry.ModMonitor?.Log($"[ShowEQuestions] Created {responses.Length} response options", LogLevel.Debug);

            var capturedTV = tv;
            var capturedNextChannel = nextChannelName;

            DelayedAction.functionAfterDelay(() =>
            {
                Game1.currentLocation.createQuestionDialogue(
                    channel.EQuestions.Question,
                    responses,
                    (who, whichAnswer) =>
                    {
                        ModEntry.ModMonitor?.Log($"[ShowEQuestions] EQuestion answered: {whichAnswer}", LogLevel.Debug);

                        // Execute actions from selected answer
                        if (whichAnswer.StartsWith("eq_") && int.TryParse(whichAnswer.Substring(3), out int answerIdx))
                        {
                            if (answerIdx >= 0 && answerIdx < channel.EQuestions.Answers.Count)
                            {
                                var selectedAnswer = channel.EQuestions.Answers[answerIdx];
                                ActionsManager.RunAnswerActions(selectedAnswer);
                            }
                        }

                        // Clear overlays now that EQuestions are done
                        OverlayManager.ClearOverlays();

                        // Chain to next channel or turn off TV
                        var resolvedNextChannel = ChannelPlayer.ResolveNextChannelName(capturedNextChannel);
                        if (!string.IsNullOrEmpty(resolvedNextChannel))
                        {
                            var nextChannel = ModEntry.CustomChannels.Data.GetValueOrDefault(resolvedNextChannel);
                            if (nextChannel != null)
                            {
                                ModEntry.ModMonitor?.Log($"[ShowEQuestions] Playing next channel: {resolvedNextChannel}", LogLevel.Debug);
                                ChannelPlayer.PlayChannel(capturedTV, nextChannel);
                            }
                            else
                            {
                                ModEntry.ModMonitor?.Log($"[ShowEQuestions] Next channel not found, turning off TV", LogLevel.Debug);
                                capturedTV.turnOffTV();
                            }
                        }
                        else
                        {
                            ModEntry.ModMonitor?.Log($"[ShowEQuestions] No next channel, turning off TV", LogLevel.Debug);
                            capturedTV.turnOffTV();
                        }
                    }
                );
                ModEntry.ModMonitor?.Log($"[ShowEQuestions] Question dialog created", LogLevel.Debug);
            }, 0);
        }
    }
}

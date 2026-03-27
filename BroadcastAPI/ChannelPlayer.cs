using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BroadcastAPI
{
    /// <summary>
    /// Main hub for playing and controlling TV channels and managers.
    /// </summary>
    public static class ChannelPlayer
    {
        // Cache reflection fields
        private static readonly FieldInfo ScreenField = typeof(TV).GetField("screen", BindingFlags.NonPublic | BindingFlags.Instance);
        private static readonly FieldInfo ChannelField = typeof(TV).GetField("currentChannel", BindingFlags.NonPublic | BindingFlags.Instance);

        /// <summary>
        /// Handles the selection of custom channels.
        /// </summary>
        public static void SelectCustomChannel(TV tv, Farmer who, string answer)
        {
            ModEntry.ModMonitor?.Log($"[SelectCustomChannel] Called with answer: {answer}", LogLevel.Debug);
            if (string.IsNullOrWhiteSpace(answer) || answer == "(Leave)")
            {
                return;
            }

            // Try to get custom channel first
            var channel = ModEntry.CustomChannels.Data.GetValueOrDefault(answer);

            // If custom channel found, check for BQuestions
            if (channel != null)
            {
                if (channel.BQuestions != null && channel.BQuestions.Answers.Count > 0)
                {
                    QuestionsManager.ShowBQuestions(tv, channel);
                }
                else
                {
                    PlayChannel(tv, channel);
                }
            }
            else
            {
                ModEntry.ModMonitor?.Log($"[SelectCustomChannel] No custom channel found for: {answer}", LogLevel.Debug);
            }
        }

        /// <summary>
        /// Plays a custom channel
        /// </summary>
        public static void PlayChannel(TV tv, CustomChannelData baseChannel, EditChannelData? editData = null)
        {
            ModEntry.ModMonitor?.Log($"[PlayChannel] Starting for channel: {baseChannel.Name}", LogLevel.Debug);

            // Build the channel with edits applied via ChannelManager
            var channel = ChannelManager.BuildChannel(baseChannel, editData);

            // Notify API
            (ModEntry.Instance?.GetApi() as Api)?.InvokeOnChannelStarted(channel.Name);

            // Clear any existing overlays
            OverlayManager.ClearOverlays();
            
            // Setup TV state
            ChannelField.SetValue(tv, ChannelManager.GetVanillaChannelId(channel.Name));

            // Create and set screen sprite via ChannelManager
            var sprite = ChannelManager.CreateScreenSprite(channel, tv);
            ScreenField.SetValue(tv, sprite);

            // Show overlays via OverlayManager
            var resolvedOverlays = ConditionResolver.ResolveAllConditionalValues(channel.Overlays, "Overlays");
            OverlayManager.ShowOverlays(resolvedOverlays, tv.getScreenPosition(), tv.getScreenSizeModifier(), tv);

            // Display dialogues
            if (channel.Dialogues?.Count > 0)
            {
                Game1.multipleDialogues(channel.Dialogues.ToArray());
            }
            else
            {
                ModEntry.ModMonitor?.Log($"[PlayChannel] No dialogues, showing default", LogLevel.Debug);
                Game1.drawObjectDialogue("...");
            }

            // Setup after-dialogue behavior
            Game1.afterDialogues = () => HandleAfterDialogues(tv, channel);

            ModEntry.ModMonitor?.Log($"[PlayChannel] Channel setup complete", LogLevel.Debug);
        }

        /// <summary>
        /// Handles logic after dialogues are finished.
        /// </summary>
        private static void HandleAfterDialogues(TV tv, CustomChannelData channel)
        {
            ModEntry.ModMonitor?.Log($"[HandleAfterDialogues] Running actions", LogLevel.Debug);
            
            // Run channel actions via ActionsManager
            ActionsManager.RunChannelActions(channel);

            // Check for EQuestions
            if (channel.EQuestions != null && channel.EQuestions.Answers.Count > 0)
            {
                ModEntry.ModMonitor?.Log($"[HandleAfterDialogues] Channel has EQuestions, showing them", LogLevel.Debug);
                QuestionsManager.ShowEQuestions(tv, channel, channel.NextChannel);
            }
            else
            {
                var nextChannelName = ResolveNextChannelName(channel.NextChannel);
                if (!string.IsNullOrEmpty(nextChannelName))
                {
                    // Chain to next channel
                    ChainToNextChannel(tv, nextChannelName);
                }
                else
                {
                    // No next channel or EQuestions, turn off TV
                    ModEntry.ModMonitor?.Log($"[HandleAfterDialogues] No next channel or EQuestions, turning off TV", LogLevel.Debug);
                    OverlayManager.ClearOverlays();
                    tv.turnOffTV();
                }
            }
        }

        internal static string? ResolveNextChannelName(List<string>? nextChannelEntries)
        {
            return ConditionResolver.ResolveFirstConditionalValue(nextChannelEntries, "NextChannel");
        }

        /// <summary>
        /// Chains from the current channel to the next channel.
        /// </summary>
        private static void ChainToNextChannel(TV tv, string nextChannelName)
        {
            ModEntry.ModMonitor?.Log($"[ChainToNextChannel] Chaining to: {nextChannelName}", LogLevel.Debug);

            // Clear overlays before chaining to next channel
            OverlayManager.ClearOverlays();

            // Get and play next channel
            var nextChannel = ModEntry.CustomChannels.Data.GetValueOrDefault(nextChannelName);
            if (nextChannel != null)
            {
                ModEntry.ModMonitor?.Log($"[ChainToNextChannel] Found next channel, playing it", LogLevel.Debug);
                PlayChannel(tv, nextChannel);
            }
            else
            {
                ModEntry.ModMonitor?.Log($"[ChainToNextChannel] Next channel not found, turning off TV", LogLevel.Debug);
                tv.turnOffTV();
            }
        }
    }
}

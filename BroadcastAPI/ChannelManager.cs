using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Objects;
using System;

namespace BroadcastAPI
{
    /// <summary>
    /// Handles building and creating channel sprites.
    /// </summary>
    public static class ChannelManager
    {
        /// <summary>
        /// Builds a channel by applying edits to the base channel data.
        /// </summary>
        public static CustomChannelData BuildChannel(CustomChannelData baseChannel, EditChannelData? editData)
        {
            if (editData == null)
                return baseChannel;

            return new CustomChannelData
            {
                Name = baseChannel.Name,
                Displayname = editData.Displayname ?? baseChannel.Displayname,
                Dialogues = editData.Dialogues ?? baseChannel.Dialogues,
                Texture = editData.Texture ?? baseChannel.Texture,
                SpriteRegion = editData.SpriteRegion ?? baseChannel.SpriteRegion,
                AnimationInterval = editData.AnimationInterval ?? baseChannel.AnimationInterval,
                AnimationLength = editData.AnimationLength ?? baseChannel.AnimationLength,
                Cooldown = editData.Cooldown ?? baseChannel.Cooldown,
                Flicker = editData.Flicker ?? baseChannel.Flicker,
                Flipped = editData.Flipped ?? baseChannel.Flipped,
                AlphaFade = editData.AlphaFade ?? baseChannel.AlphaFade,
                Color = editData.Color ?? baseChannel.Color,
                Scale = editData.Scale ?? baseChannel.Scale,
                ScaleChange = editData.ScaleChange ?? baseChannel.ScaleChange,
                Rotation = editData.Rotation ?? baseChannel.Rotation,
                RotationChange = editData.RotationChange ?? baseChannel.RotationChange,
                Position = editData.Position ?? baseChannel.Position,
                Overlays = editData.Overlays ?? baseChannel.Overlays,
                LayerDepth = editData.LayerDepth ?? baseChannel.LayerDepth,
                Actions = editData.Actions ?? baseChannel.Actions,
                NextChannel = editData.NextChannel != null ? editData.NextChannel : baseChannel.NextChannel,
                HideFromMenu = editData.HideFromMenu ?? baseChannel.HideFromMenu,
                BQuestions = editData.BQuestions ?? baseChannel.BQuestions,
                EQuestions = editData.EQuestions ?? baseChannel.EQuestions
            };
        }

        /// <summary>
        /// Creates a screen sprite for the TV channel.
        /// </summary>
        public static TemporaryAnimatedSprite CreateScreenSprite(CustomChannelData channel, TV tv)
        {
            // Calculate base depth - use FF's depth if available, otherwise vanilla
            float baseDepth = GetBaseDepth(tv, overlay: false);
            
            ModEntry.ModMonitor?.Log($"[CreateScreenSprite] Using depth: {baseDepth}", LogLevel.Debug);

            var sprite = new TemporaryAnimatedSprite(
                textureName: null,
                channel.SpriteRegion,
                channel.AnimationInterval,
                channel.AnimationLength,
                999999,
                tv.getScreenPosition() + channel.Position * tv.getScreenSizeModifier(),
                channel.Flicker,
                channel.Flipped,
                baseDepth + channel.LayerDepth * 1E-05f,
                channel.AlphaFade,
                channel.Color,
                channel.Scale * tv.getScreenSizeModifier(),
                channel.ScaleChange,
                channel.Rotation,
                channel.RotationChange
            )
            {
                delayBeforeAnimationStart = (int)channel.Cooldown
            };

            // Load texture if specified
            if (channel.Texture != null)
            {
                try
                {
                    sprite.texture = Game1.content.Load<Texture2D>(channel.Texture);
                }
                catch (Exception ex)
                {
                    ModEntry.ModMonitor?.Log($"[CreateScreenSprite] Failed to load texture {channel.Texture}: {ex.Message}", LogLevel.Error);
                }
            }

            return sprite;
        }

        /// <summary>
        /// Gets the base depth for TV screen rendering.
        /// </summary>
        public static float GetBaseDepth(TV tv, bool overlay)
        {
            float baseDepth;
            
            if (ModEntry.FurnitureFrameworkAPI?.TryGetScreenDepth(tv, out float? ffDepth, overlay: overlay) == true && ffDepth.HasValue)
            {
                baseDepth = ffDepth.Value;
            }
            else
            {
                baseDepth = (float)(tv.boundingBox.Bottom - 1) / 10000f;
                if (overlay)
                    baseDepth += 1E-05f;  // Add base overlay offset for vanilla
            }

            return baseDepth;
        }

        /// <summary>
        /// Gets the vanilla channel ID for proper cleanup.
        /// </summary>
        public static int GetVanillaChannelId(string name) => name switch
        {
            "Weather" => 2,
            "Fortune" => 3,
            "Livin'" => 4,
            "The" => 5,
            "???" => 666,
            "Fishing" => 6,
            _ => -1
        };
    }
}

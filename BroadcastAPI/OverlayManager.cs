using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Objects;

namespace BroadcastAPI
{
    public static class OverlayManager
    {
        private static readonly List<TemporaryAnimatedSprite> activeOverlays = new();

        /// <summary>
        /// Shows overlay sprites on the TV screen.
        /// </summary>
        public static void ShowOverlays(List<string>? overlayKeys, Vector2 screenPosition, float screenSizeModifier, TV tv)
        {
            if (overlayKeys == null) return;

            foreach (var key in overlayKeys.Where(k => !string.IsNullOrEmpty(k)))
            {
                if (ModEntry.Overlays.Data.TryGetValue(key, out var overlay))
                    CreateOverlay(overlay, screenPosition, screenSizeModifier, tv);
            }
        }

        /// <summary>
        /// Creates an overlay sprite and adds it to the scene.
        /// </summary>
        private static void CreateOverlay(OverlayData overlay, Vector2 screenPosition, float screenSizeModifier, TV tv)
        {
            if (overlay.Texture == null)
            {
                ModEntry.ModMonitor?.Log("Overlay texture is required. Skipping.", LogLevel.Warn);
                return;
            }

            // Calculate base depth for overlay using ChannelManager
            float baseDepth = ChannelManager.GetBaseDepth(tv, overlay: true);
            ModEntry.ModMonitor?.Log($"[CreateOverlay] Using overlay depth: {baseDepth}", LogLevel.Debug);

            var sprite = new TemporaryAnimatedSprite(
                textureName: null,
                overlay.SpriteRegion,
                overlay.AnimationInterval,
                overlay.AnimationLength,
                999999,
                screenPosition + overlay.Position * screenSizeModifier,
                overlay.Flicker,
                overlay.Flipped,
                baseDepth + overlay.LayerDepth * 1E-05f,
                overlay.AlphaFade,
                overlay.Color,
                overlay.Scale * screenSizeModifier,
                overlay.ScaleChange,
                overlay.Rotation,
                overlay.RotationChange
            )
            {
                delayBeforeAnimationStart = (int)overlay.Cooldown
            };

            try
            {
                sprite.texture = Game1.content.Load<Texture2D>(overlay.Texture);
                Game1.currentLocation.temporarySprites.Add(sprite);
                activeOverlays.Add(sprite);
            }
            catch (Exception ex)
            {
                ModEntry.ModMonitor?.Log($"Failed to load overlay texture '{overlay.Texture}': {ex.Message}", LogLevel.Error);
            }
        }

        /// <summary>
        /// Clears all active overlay sprites.
        /// </summary>
        public static void ClearOverlays()
        {
            activeOverlays.ForEach(o => Game1.currentLocation.temporarySprites.Remove(o));
            activeOverlays.Clear();
        }
    }
}
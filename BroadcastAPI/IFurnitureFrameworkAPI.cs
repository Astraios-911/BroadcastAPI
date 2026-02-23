using StardewValley.Objects;
using System.Diagnostics.CodeAnalysis;

namespace BroadcastAPI
{
    /// <summary>Interface for Furniture Framework API.</summary>
    public interface IFurnitureFrameworkAPI
    {
        /// <summary>Requests the depth at which the TV's screen should be drawn.</summary>
        /// <param name="furniture">The TV instance</param>
        /// <param name="depth">The depth computed by FF if the instance is a FF TV</param>
        /// <param name="overlay">Whether or not to give the depth of the base screen or the overlay</param>
        /// <returns>true if the TV instance is a FF TV, else false</returns>
        bool TryGetScreenDepth(TV furniture, [MaybeNullWhen(false)] out float? depth, bool overlay = false);
    }
}

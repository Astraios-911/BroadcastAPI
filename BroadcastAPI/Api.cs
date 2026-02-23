namespace BroadcastAPI
{
    /// <summary>Defines the API for More TV Channels.</summary>
    public interface IBroadcastAPIAPI
    {
        /// <summary>Raised when a custom or edited TV channel starts playing.</summary>
        event Action<string> OnChannelStarted;

        /// <summary>Raised when the TV is turned off after a channel finishes.</summary>
        event Action OnTVTurnedOff;

        /// <summary>Gets the internal name of the channel that is currently playing.</summary>
        string? GetCurrentChannel();

        /// <summary>Register a new custom TV channel at runtime.</summary>
        void RegisterCustomChannel(CustomChannelData channel);

        /// <summary>Edit or override a built-in TV channel at runtime.</summary>
        void EditChannel(string name, EditChannelData edit);

        /// <summary>Register a new overlay sprite at runtime.</summary>
        void RegisterOverlay(OverlayData overlay);
    }

    /// <summary>Implements the IBroadcastAPIAPI interface.</summary>
    public class Api : IBroadcastAPIAPI
    {
        public event Action<string>? OnChannelStarted;
        public event Action? OnTVTurnedOff;

        private string? _currentChannel;

        public string? GetCurrentChannel() => _currentChannel;

        internal void InvokeOnChannelStarted(string channelName)
        {
            _currentChannel = channelName;
            OnChannelStarted?.Invoke(channelName);
        }

        internal void InvokeOnTVTurnedOff()
        {
            _currentChannel = null;
            OnTVTurnedOff?.Invoke();
        }

        public void RegisterCustomChannel(CustomChannelData channel)
            => ModEntry.CustomChannels.Data[channel.Name] = channel;

        public void EditChannel(string name, EditChannelData edit)
            => ModEntry.EditChannels.Data[name] = edit;

        public void RegisterOverlay(OverlayData overlay)
            => ModEntry.Overlays.Data[overlay.Name] = overlay;
    }
}
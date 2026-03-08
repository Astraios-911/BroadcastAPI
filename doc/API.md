# C# API

If you're making a Content Patcher content pack, you don't need this guide. This is for SMAPI mod developers who want to use BroadcastAPI's API.

## Getting the API

First, add BroadcastAPI as a dependency in your mod's `manifest.json`:

```json
{
  "Dependencies": [
    {
      "UniqueID": "Astraios.BroadcastAPI",
    }
  ]
}
```

Then get the API instance in `GameLaunched` :

```csharp
using StardewModdingAPI;
using StardewModdingAPI.Events;
using BroadcastAPI;

public class ModEntry : Mod
{
    private IBroadcastAPI? api;

    public override void Entry(IModHelper helper)
    {
        helper.Events.GameLoop.GameLaunched += this.OnGameLaunched;
    }

    private void OnGameLaunched(object? sender, GameLaunchedEventArgs e)
    {
        // Get the API after all mods are initialized
        this.api = this.Helper.ModRegistry.GetApi<IBroadcastAPI>("Astraios.BroadcastAPI");
        
        if (this.api == null)
        {
            this.Monitor.Log("Failed to load BroadcastAPI.", LogLevel.Error);
            return;
        }

        // Now you can use the API
        this.api.OnChannelStarted += OnChannelStarted;
    }

    private void OnChannelStarted(string channelName)
    {
        this.Monitor.Log($"Channel started: {channelName}", LogLevel.Info);
    }
}
```

## Events

### `OnChannelStarted`

Raised when a custom TV channel starts playing. The event passes the internal name of the channel as a string.

```csharp
this.api.OnChannelStarted += (channelName) =>
{
    if (channelName == "MyMod_SecretChannel")
    {
        // Do something when your secret channel plays
        Game1.addHUDMessage(new HUDMessage("You found the secret channel!", 2));
    }
};
```

### `OnTVTurnedOff`

Raised when the TV is turned off after a channel finishes playing.

```csharp
this.api.OnTVTurnedOff += () =>
{
    this.Monitor.Log("TV turned off", LogLevel.Debug);
    // Clean up any channel-related state
};
```

## Methods

### `GetCurrentChannel()`

Gets the internal name of the channel that is currently playing. Returns `null` if no channel is playing or the TV is off.

```csharp
string? currentChannel = this.api.GetCurrentChannel();

if (currentChannel != null)
{
    this.Monitor.Log($"Currently watching: {currentChannel}", LogLevel.Info);
}
```

### `RegisterCustomChannel`
**Parameters:** `CustomChannelData channel`

Register a new custom TV channel at runtime.

```csharp
var myChannel = new CustomChannelData
{
    Name = "MyMod_DynamicChannel",
    Displayname = "Dynamic Channel",
    Dialogues = new List<string>
    {
        "This channel was created at runtime!",
        $"The current time is {DateTime.Now:HH:mm}"
    },
    Texture = "MyMod/ChannelTexture",
    SpriteRegion = new Rectangle(0, 0, 42, 28),
    AnimationLength = 4,
    AnimationInterval = 150f
};

this.api.RegisterCustomChannel(myChannel);
```

**Note:** Make sure your texture is loaded into the game's content manager before registering the channel. You can use `helper.GameContent.Load<Texture2D>("path")` to load textures.

### `EditChannel`
**Parameters:** 
- `string name` - The internal name of the channel to edit
- `EditChannelData edit` - The edits to apply to the channel

Edit or override an existing TV channel at runtime. Only the fields you set will be changed; any fields left as `null` will keep their original values. This is the C# equivalent of using `EditData` for `Astraios.BroadcastAPI/EditChannels`.

```csharp
var channelEdit = new EditChannelData
{
    Displayname = "Modified Channel",
    Dialogues = new List<string>
    {
        "This dialogue was changed at runtime!"
    },
    NextChannel = new List<string>
    {
        "MyMod_CustomEnding"
    }
};

this.api.EditChannel("SomeOtherMod_Channel", channelEdit);
```

This works for custom channels only vanilla channels support coming soon.

### `RegisterOverlay`

Register a new overlay sprite at runtime. Overlays are `TemporaryAnimatedSprites` that can be applied on top of channels. See [Visuals](Visuals.md) and [Effects](Effects.md) for details on overlay properties.

```csharp
var staticOverlay = new OverlayData
{
    Name = "MyMod_Static",
    Texture = "MyMod/StaticTexture",
    SpriteRegion = new Rectangle(0, 0, 42, 28),
    AnimationLength = 8,
    AnimationInterval = 50f,
    Flicker = true,
    Color = new Color(255, 255, 255, 128), // semi-transparent
    LayerDepth = 2f
};

this.api.RegisterOverlay(staticOverlay);

// Then use it in a channel
var channel = new CustomChannelData
{
    Name = "MyMod_StaticChannel",
    Displayname = "Noisy Channel",
    Dialogues = new List<string> { "The signal is bad today..." },
    Texture = "MyMod/ChannelTexture",
    Overlays = new List<string> { "MyMod_Static" }
};

this.api.RegisterCustomChannel(channel);
```

## Examples

### Weather channel

```csharp
private void CreateWeatherChannel()
{
    string weatherDialogue = Game1.isRaining 
        ? "Looks like rain today. Stay inside and watch TV!" 
        : "It's a beautiful day outside!";

    var channel = new CustomChannelData
    {
        Name = "MyMod_WeatherChannel",
        Displayname = "Weather Report",
        Dialogues = new List<string> { weatherDialogue },
        Texture = Game1.isRaining ? "MyMod/RainyWeather" : "MyMod/SunnyWeather",
        SpriteRegion = new Rectangle(0, 0, 42, 28)
    };

    this.api.RegisterCustomChannel(channel);
}
```

### Quiz channel

```csharp
var interactiveChannel = new CustomChannelData
{
    Name = "MyMod_Quiz",
    Displayname = "Quiz Show",
    Dialogues = new List<string> { "Welcome to the quiz show!" },
    Texture = "MyMod/QuizShow",
    EQuestions = new QuestionsData
    {
        Question = "What's your favorite season?",
        Answers = new List<QuestionsData.AnswerData>
        {
            new QuestionsData.AnswerData
            {
                Text = "Spring",
                Actions = new List<string> { "AddMoney 100" }
            },
            new QuestionsData.AnswerData
            {
                Text = "Summer",
                Actions = new List<string> { "AddMoney 200" }
            }
        }
    }
};

this.api.RegisterCustomChannel(interactiveChannel);
```

### Secret ending channel

```csharp
var mainChannel = new CustomChannelData
{
    Name = "MyMod_MainShow",
    Displayname = "Main Show",
    Dialogues = new List<string> { "Thanks for watching!" },
    Texture = "MyMod/MainShow",
    NextChannel = new List<string>
    {
        "If PLAYER_HAS_ITEM Current 434 ## MyMod_StarfishEnding",
        "If SEASON Spring ## MyMod_SpringEnding",
        "MyMod_DefaultEnding"
    }
};

this.api.RegisterCustomChannel(mainChannel);
```

# Getting Started

BroadcastAPI lets you add custom TV channels to Stardew Valley without writing any C#. This guide walks you through creating your first channel using a Content Patcher content pack.

## Prerequisites

- [SMAPI](https://smapi.io/) installed  
- [Content Patcher](https://www.nexusmods.com/stardewvalley/mods/1915) installed  
- [BroadcastAPI](https://www.nexusmods.com/stardewvalley/mods/41844) installed

## Create the Content Pack

### 1. Set up the folder

Create a folder inside your `Mods` directory. Name it something descriptive, like `[CP] MyModName`.

Inside it, create a `manifest.json`:

```json
{
  "Name": "My Channel",
  "Author": "YourName",
  "Version": "1.0.0",
  "Description": "Adds a custom TV channel.",
  "UniqueID": "YourName.MyChannel",
  "ContentPackFor": {
    "UniqueID": "Pathoschild.ContentPatcher"
  },
  "Dependencies": [
    {
      "UniqueID": "Astraios.BroadcastAPI",
      "IsRequired": true
    }
  ]
}
```

### 2. Load your texture

Your channel needs a sprite to display on the TV screen. Add your texture file to the content pack (e.g. `assets/channel.png`), then load it as a game asset in `content.json`:

```json
{
  "Format": "2.8.0",
  "Changes": [
    {
      "Action": "Load",
      "Target": "{{ModId}}/MyChannel",
      "FromFile": "assets/channel.png"
    },

    // You can also load multiple textures at a time like below
    {
      "Action": "Load",
      "Target": "{{ModId}}/MyChannel1, {{ModId}}/MyChannel2",
      "FromFile": "assets/{{TargetWithoutPath}}.png" 
      // {{TargetWithoutPath}} will make it pick the file with the same name as the texture, in this case MyChannel1.png and MyChannel2.png
    }
  ]
}
```

### 3. Define the channel

In the same `content.json`, add an `EditData` entry targeting `Astraios.BroadcastAPI/CustomChannels`:

```json
{
  "Format": "2.8.0",
  "Changes": [
    {
      "Action": "Load",
      "Target": "{{ModId}}/MyChannel",
      "FromFile": "assets/channel.png"
    },
    {
      "Action": "EditData",
      "Target": "Astraios.BroadcastAPI/CustomChannels",
      "Entries": {
        "YourName.MyChannel": {
          "Name": "YourName.MyChannel", // This is the internal name of the channel like an ID
          "Displayname": "My Channel", // This is the name that will actually be displayed to the player
          "Dialogues": [
            "Welcome to my custom channel!",
            "... I don't know what to say here (>.<)"
          ],
          "Texture": "{{ModId}}/MyChannel", // Use the name of the texture you assigned above
          "SpriteRegion": { "X": 0, "Y": 0, "Width": 42, "Height": 28 }, // The position and size of the channel's sprite
          "AnimationLength": 4, // How many frames are in this channel (default 2)
          "AnimationInterval": 150.0 // The Time between frames (default 150)   
        }
      }
    }
  ]
}
```

**Note:** The channel's name should be a [unique string ID](https://stardewvalleywiki.com/Modding:Common_data_field_types#Unique_string_ID) prefix it with your mod's `UniqueID` (in manifest.json) like this `YourName.ChannelName` to avoid conflicts with other mods.

That's it! Launch the game, interact with any TV, and your channel will appear in the menu. If you want to do more advanced stuff or confused and want more in-depth explanations check [Custom Channels](https://github.com/Astraios-911/BroadcastAPI/blob/main/doc/Custom%20Channel.md)

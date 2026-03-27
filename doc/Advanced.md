# Advanced

If you haven't made a channel yet, start with [Getting Started](author-guide.md) first.

### `Actions`
**Type:** `list of strings` | **Default:** `null`

A list of [actions](https://stardewvalleywiki.com/Modding:Trigger_actions) to run after the channel's dialogues close, before any `EQuestions` or `NextChannel`.

```json
"Actions": [
  "AddMoney 500",
  "If IS_COMMUNITY_CENTER_COMPLETE ## AddFriendshipPoints Robin 100"
]
```

### `NextChannel`
**Type:** `list of strings` | **Default:** `null`

Chains into another channel after this one finishes. BroadcastAPI checks the list top-to-bottom and plays the first valid channel. Entries can be plain channel names or conditionals using [Game State Queries](https://stardewvalleywiki.com/Modding:Game_state_queries).

**Syntax:**
- Plain channel: `"{{ModID}}_NextChannel"`
- Conditional: `"If <condition> ## <channel> ## <channel_if_false> (optional)"`

```json
"NextChannel": [
  "If PLAYER_HAS_ITEM Current MysteryNote ## {{ModID}}_SecretChannel",
  "{{ModID}}_DefaultFollowup"
]
```

### `Overlays`
**Type:** `list of strings` | **Default:** `null`

A list of overlay names to show on top of the channel while it plays. Overlays are defined in `Astraios.BroadcastAPI/Overlays` and supports every [visual](https://github.com/Astraios-911/BroadcastAPI/blob/main/doc/Visuals.md) and [effect](https://github.com/Astraios-911/BroadcastAPI/blob/main/doc/Effects.md). Entries can be plain overlay names or conditionals using [Game State Queries](https://stardewvalleywiki.com/Modding:Game_state_queries).

```json
"Overlays": [ "{{ModID}}_StaticEffect", "{{ModID}}_Border" ]

{
  "Action": "EditData",
  "Target": "Astraios.BroadcastAPI/OverlaySprite",
  "Entries": {
    "{{ModId}}_Overlay": {
      "Name": "{{ModId}}_Overlay",
      "Texture": "{{ModId}}/OverlayTexture",
      "Scale": 0.5,
      "Position": { "X": 12, "Y": 7 },
      "SpriteRegion": { "X": 0, "Y": 0, "Width": 32, "Height": 32 }
    },
  }
}
```

### `EditChannel`

All channel fields (except `Name`) can be edited/overridden at runtime via `EditChannelData` in `Astraios.BroadcastAPI/EditChannels`. Edits are applied on top of the base channel values. Any field set to `null` or unedited leaves the original value unchanged. Currently you can only edit modded channels added by other mods but vanilla channels support is coming soon.

```json
{
  "Action": "EditData",
  "Target": "Astraios.BroadcastAPI/EditChannels",
  "Entries": {
    "{{ModID}}_BaseChannel": {
      "Displayname": "Modified Channel Name",
      "Dialogues": [ "This replaces the original dialogue." ],
      "NextChannel": [ "{{ModID}}_CustomEnding" ]
    }
  }
}
```
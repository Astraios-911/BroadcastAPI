
# Custom Channels

This is the full field reference for channels defined in `Astraios.BroadcastAPI/CustomChannels`. If you haven't made a channel yet, start with [Getting Started](author-guide.md) first.

## Internals

### `Name`
**Type:** `string` | **Required**

The channel's internal unique ID. It's used everywhere — the channel menu, chaining, and editing. Prefix it with your mod's `UniqueID` to avoid conflicts with other mods.

```json
"Name": "YourName.MyChannel"
```

### `Displayname`
**Type:** `string` | **Required**

The name shown to the player in the TV channel selection menu.

```json
"Displayname": "My Awesome Channel"
```

### `HideFromMenu`
**Type:** `bool` | **Default:** `false`

When `true`, the channel won't appear in the TV menu. Useful for channels you only want to trigger via [`NextChannel`](#nextchannel) or a [`BQuestions`](#bquestions--equestions) / [`EQuestions`](#bquestions--equestions) answer.

```json
"HideFromMenu": true
```

## Dialogue

### `Dialogues`
**Type:** `list of strings` | **Default:** `["..."]`

The text boxes shown when the channel plays. Each string is a separate dialogue box, that the player clicks through them in order.

```json
"Dialogues": [
  "Welcome to the channel!",
  "Come back tomorrow for more."
]
```

You can use [Content Patcher tokens](https://github.com/Pathoschild/StardewMods/blob/develop/ContentPatcher/docs/author-guide/tokens.md) inside dialogue strings for dynamic text, like `{{PlayerName}}`.

### `BQuestions` / `EQuestions`
**Type:** `QuestionsData` | **Default:** `null`

Shows a question dialog either **before** the channel even plays (`BQuestions`) or at the ending **after** all dialogue is done but before the channel turns off (`EQuestions`). Each answer can trigger its own list of actions. See [Questions](questions.md).

```json
"BQuestions": {
  "Question": "Do you want to watch?",
  "Answers": [
    { "Text": "Yes, show me.", "Actions": [] },
    { "Text": "Not today.",    "Actions": [ "..." ] }
  ]
}
```

## Visuals

### `Texture`
**Type:** `string` | **Default:** `null`

The game asset path of the sprite sheet to draw on the TV screen. Must match a `Target` you loaded with a `Load` action. If `null`, nothing is drawn on screen.

```json
"Texture": "{{ModId}}/MyChannel"
```

### `SpriteRegion`
**Type:** `Rectangle: (X, Y, Width, Height)` | **Default:** `{ X: 0, Y: 0, Width: 42, Height: 28 }`

The position and size of the first frame in the texture, in pixels. `X` and `Y` are the top-left corner, `Width` and `Height` are the frame size.

For multi-frame animations, frames are read **left-to-right** from this starting point. So a 4-frame animation with `Width: 42` reads frames at X=0, X=42, X=84, and X=126.

```json
"SpriteRegion": { "X": 0, "Y": 0, "Width": 42, "Height": 28 }
```

### `AnimationLength`
**Type:** `int` | **Default:** `2`

The number of frames in the animation. Your texture must have at least this many frames starting from `SpriteRegion`.

```json
"AnimationLength": 4
```

### `AnimationInterval`
**Type:** `float` | **Default:** `150.0`

How long each frame is displayed, in milliseconds. Lower = faster.

```json
"AnimationInterval": 100.0 // faster
"AnimationInterval": 500.0 // slower
```

### `Position`
**Type:** `Vector2: { X, Y }` | **Default:** `{ X: 0, Y: 0 }`

Pixel offset from the TV screen's default position. Automatically scaled to the TV's size, so the same values work across all TV furniture types.

```json
"Position": { "X": 5, "Y": -3 } // slightly right and up
```

### `LayerDepth`
**Type:** `float` | **Default:** `1.0`

Controls draw order when multiple sprites overlap on screen. Higher values are drawn in front of lower values. Overlays default to `2.0`, so channels at `1.0` sit behind them unless you increase this.

```json
"LayerDepth": 1.0 // default, behind overlays
"LayerDepth": 3.0 // in front of overlays
```

## Effects

### `Color`
**Type:** `Color: { R, G, B, A }` | **Default:** `{ R: 255, G: 255, B: 255, A: 255 }` (white = no tint)

A color tint applied over the whole sprite. White means the texture draws as-is. You can use this to darken, colorize, or change the opacity of the sprite.

```json
"Color": { "R": 255, "G": 100, "B": 100, "A": 255 } // red tint
"Color": { "R": 150, "G": 150, "B": 150, "A": 255 } // darker
```

### `Flicker`
**Type:** `bool` | **Default:** `false`

Makes the sprite randomly flicker on and off.

```json
"Flicker": true
```

### `Flipped`
**Type:** `bool` | **Default:** `false`

Mirrors the sprite horizontally.

```json
"Flipped": true
```

### `AlphaFade`
**Type:** `float` | **Default:** `0.0`

How much the sprite's opacity decreases per game tick.

```json
"AlphaFade": 0.005 // slow fade out
```

### `Scale`
**Type:** `float` | **Default:** `1.0`

The size of the sprite relative to the TV screen. `1.0` fills the screen normally. Automatically accounts for different TV furniture sizes, so you don't need to adjust this per TV type. This is useful when having channel sprites bigger/smaller than 42x28, so for example you can have an 84x56 sprite and a scale of 0.5 to make it fit.

```json
"Scale": 0.8 // slightly smaller than the screen
"Scale": 1.5 // bigger than the screen
```

### `ScaleChange`
**Type:** `float` | **Default:** `0.0`

How much the scale changes per game tick, starting from `Scale`. Positive values grow the sprite, negative values shrink it.

```json
"ScaleChange": 0.001  // slowly grows
"ScaleChange": -0.001 // slowly shrinks
```

### `Rotation`
**Type:** `float` | **Default:** `0.0`

The starting rotation of the sprite. `0.0` is upright. Values are in degrees `90.0`.

```json
"Rotation": 90.0 // rotated 90 degrees clockwise
```

### `RotationChange`
**Type:** `float` | **Default:** `0.0`

How much the rotation changes per game tick. Makes the sprite spin continuously.

```json
"RotationChange": 0.01  // slow clockwise spin
"RotationChange": -0.01 // counterclockwise
```

### `Cooldown`
**Type:** `float` (milliseconds) | **Default:** `0.0`

A delay before the animation starts playing, in milliseconds. The sprite is invisible during this delay.

```json
"Cooldown": 500.0 // waits half a second before appearing
```

## Advanced

### `Overlays`
**Type:** `list of strings` | **Default:** `null`

A list of overlay names to show on top of the channel while it plays. Overlays are defined separately in `Astraios.BroadcastAPI/Overlays`. See [Overlays](overlays.md).

```json
"Overlays": [ "YourName.StaticEffect", "YourName.Border" ]
```

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

Chains into another channel after this one finishes. BroadcastAPI walks the list top-to-bottom and plays the first matching entry. Entries can be plain channel names or conditionals using [Game State Queries](https://stardewvalleywiki.com/Modding:Game_state_queries). See [Next Channel](next-channel.md) for the full syntax.

```json
"NextChannel": [
  "If PLAYER_HAS_ITEM Current MysteryNote ## YourName.SecretChannel",
  "YourName.DefaultFollowup"
]
```

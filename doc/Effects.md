# Effects

If you haven't made a channel yet, start with [Getting Started](author-guide.md) first.

### `Color`
**Type:** `Color: { R, G, B, A }` | **Default:** `{ "R": 255, "G": 255, "B": 255, "A": 255 }` (white = no tint)

A color tint applied over the whole sprite. White means the texture draws as-is. You can use this to darken, colorize, or change the opacity of the sprite.

```json
"Color": { "R": 255, "G": 100, "B": 100, "A": 255 } // red tint
"Color": { "R": 150, "G": 150, "B": 150, "A": 255 } // darker
```

### `Flicker`
**Type:** `bool` | **Default:** `false`

Makes the sprite flicker on and off.

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
"RotationChange": -0.01 // slow counterclockwise spin
```

### `Cooldown`
**Type:** `float` (milliseconds) | **Default:** `0.0`

A delay before the animation starts playing, in milliseconds. The sprite is invisible during this delay.

```json
"Cooldown": 500.0 // waits half a second before appearing
```

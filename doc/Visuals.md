# Visuals

If you haven't made a channel yet, start with [Getting Started](author-guide.md) first.

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

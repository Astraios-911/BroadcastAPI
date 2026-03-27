# Internals

If you haven't made a channel yet, start with [Getting Started](author-guide.md) first.

### `Name`
**Type:** `string` | **Required**

The channel's internal unique ID. It's used everywhere - the channel menu, chaining, and editing. Prefix it with your mod's `UniqueID` to avoid conflicts with other mods.

```json
"Name": "{{ModID}}_MyChannel"
```

### `DisplayName`
**Type:** `string` | **Required**

The name shown to the player in the TV channel selection menu.

```json
"DisplayName": "My Awesome Channel"
```

### `HideFromMenu`
**Type:** `bool` | **Default:** `false`

When `true`, the channel won't appear in the TV menu. Useful for channels you only want to trigger via [`NextChannel`](Advanced.md#nextchannel) or a [`BQuestions`](Dialogue.md#bquestions--equestions) / [`EQuestions`](Dialogue.md#bquestions--equestions) answer.

```json
"HideFromMenu": true
```

### `Conditions`
**Type:** `list<string>` | **Default:** `null`

Optional Game State Query conditions that can invert `HideFromMenu`.

- Conditions separated by commas in one list entry are treated as **AND**.
- Multiple list entries are treated as **OR**.
- If any entry matches, `HideFromMenu` is inverted for that channel.

```json
"HideFromMenu": true,
"Conditions": [
	"SEASON spring, DAY_OF_MONTH 13",
	"PLAYER_HAS_ITEM (O)72"
]
```
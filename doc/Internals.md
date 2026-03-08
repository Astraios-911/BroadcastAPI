# Internals

If you haven't made a channel yet, start with [Getting Started](author-guide.md) first.

### `Name`
**Type:** `string` | **Required**

The channel's internal unique ID. It's used everywhere - the channel menu, chaining, and editing. Prefix it with your mod's `UniqueID` to avoid conflicts with other mods.

```json
"Name": "{{ModID}}_MyChannel"
```

### `Displayname`
**Type:** `string` | **Required**

The name shown to the player in the TV channel selection menu.

```json
"Displayname": "My Awesome Channel"
```

### `HideFromMenu`
**Type:** `bool` | **Default:** `false`

When `true`, the channel won't appear in the TV menu. Useful for channels you only want to trigger via [`NextChannel`](Advanced.md#nextchannel) or a [`BQuestions`](Dialogue.md#bquestions--equestions) / [`EQuestions`](Dialogue.md#bquestions--equestions) answer.

```json
"HideFromMenu": true
```

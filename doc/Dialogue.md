# Dialogue

If you haven't made a channel yet, start with [Getting Started](author-guide.md) first.

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

For translation, use `{{i18n:...}}` keys inside dialogue strings (for example, `{{i18n:channel.welcome}}`). See [i18n Translations](https://stardewvalleywiki.com/Modding:Translations) for more info.

### `BQuestions` / `EQuestions`
**Type:** `QuestionsData` | **Default:** `null`

Shows a question dialog either **before** the channel even plays (`BQuestions`) or at the ending **after** all dialogue is done but before the channel turns off (`EQuestions`). Each answer can trigger its own list of [actions](https://stardewvalleywiki.com/Modding:Trigger_actions) and optionally chain to another channel via `NextChannel`.

```json
"EQuestions": {
  "Question": "Do you want to watch?",
  "Answers": [
    { "Text": "Yes, show me.", 
      "NextChannel": [ "{{ModID}}_NextChannel" ]
    },
    { 
      "Text": "Not today.",
      "Actions": [ "AddMoney -999999" ],
    }
  ]
}
```

**Answer fields:**
- `Text` (string, required): The answer text shown to the player.
- `Actions`: (list of strings, optional): Actions to run when this answer is selected.
- `NextChannel` (list of strings, optional): Overrides the channel's [NextChannel](Advanced.md#nextchannel).
- `PlayChannel` (list of strings, optional): Switches to a different channel to play instead of the current one (only for `BQuestions`).


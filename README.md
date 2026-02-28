# BroadcastAPI

A [Stardew Valley](https://www.stardewvalley.net/) framework mod that lets modders add custom TV channels and edit vanilla ones.

**This is a framework mod. It adds no content on its own — it is a tool for other mod authors.**

## Documentation

### For Content Pack Authors (no C# needed)

• [Getting Started](doc/author-guide.md) — a simply guide to making a channel  
• [Custom Channels](doc/channel.md) — full custom channels field reference  
• [Edit Channels](doc/edit-channels.md) — override other mods' channels (vanilla channel support coming soon)  
• [Overlays](doc/overlays.md) — layered animated sprites on the TV screen  
• [Questions](doc/questions.md) — question dialogs before/after a channel plays  
• [Next Channel](doc/next-channel.md) — chaining channels  after one another

### For C# Mod Authors

• [C# API](doc/api.md) — events and methods exposed by `IBroadcastAPI`  

## Features

• New custom TV channels visible in the TV channel menu  
• Full animation control: texture, sprite region, frame count, interval, rotation, scale, flicker  
• Overlay sprites layered on top of channels  
• Actions ran after dialogue closes  
• Questions before (`BQuestions`) or after (`EQuestions`) a channel plays  
• Channel chaining via `NextChannel`, with optional [Game State Queries](https://stardewvalleywiki.com/Modding:Game_state_queries)  
• Edit other mods' channels via `EditChannels`  

## Known Issues

• None yet — please [open an issue](https://github.com/Astraios-911/BroadcastAPI/issues) if you find one.

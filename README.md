# BroadcastAPI

A [Stardew Valley](https://www.stardewvalley.net/) framework that lets modders add custom TV channels.

**This is a framework. It adds no content on its own, it is a tool for other mod authors.**

## Documentation

### For Content Pack Authors (no C# needed)

- [Getting Started](doc/) - a simply guide to making a channel
- [Edit Channels](doc/Advanced.md#editchannel) - override other mods' channels (vanilla channels support coming soon)
- [Overlays](doc/Advanced.md#overlays) - layered animated sprites on the TV screen
- [Questions](doc/Dialogue.md#bquestions--equestions) - question dialogs before/after a channel plays
- [Next Channel](doc/Advanced.md#nextchannel) - chaining channels after one another

### For C# Mod Authors

- [C# API](doc/api.md) - events and methods exposed by `IBroadcastAPI`  

## Features

- New custom TV channels visible in the TV channel menu
- Full animation control: texture, sprite region, frame count, interval, rotation, scale, flicker
- Overlay sprites layered on top of channels
- Actions ran after dialogue closes
- Questions before (`BQuestions`) or after (`EQuestions`) a channel plays  
- Channel chaining via `NextChannel`, with [Game State Queries](https://stardewvalleywiki.com/Modding:Game_state_queries) conditions
- Edit other mods' channels via `EditChannels` (vanilla channels support coming soon)

## Known Issues

- None yet - please [open an issue](https://github.com/Astraios-911/BroadcastAPI/issues) if you find one.

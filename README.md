# TRICKY TOWERS - UNITY DEV TASK

This project was produced over 8 days, specifically for Android (mobile portrait).

## Testing

During development, the fastest way to test the game feel without having to build was using *Unity Remote*, which mirrors the editor *Game* window to a USB connected device, which must be in developer mode.

To test bigger feature updates, a development build was made with the auto-connected profiler and installed directly on the device using *Build And Run*.

Also during development, some non-development builds were created, to test more accurate binary size.

For resolution testing, and to make sure the UI was scaling and fitting well with all modern mobile devices, the *Simulator* window was used instead.

Furthermore, to facilitate fast play-testing using different game variables, a series of *Scriptable Objects* were created and are easily modifiable to update things such as Game, UI, Blocks, CPU and Saved Data. These can be found under Assets/DataObjects.

### Devices used

+ Samsung Galaxy A52s (Main)
+ Xiaomi 11 Lite NE 5G
+ OnePlus 11 5G

## Performance actions taken

**Import settings.** All textures are imported with power of two resolutions and using the minimum viable values for *Max Size*, *Compression Quality* and *Crunch Compression*. This helps reduce binary size and memory footprint on runtime.

**Load/Unload Music.** The biggest file (audio - background music), is loaded and unloaded dynamically using the *Addressables* system. Meaning that if the user disables the Music toggle, in the in-game settings menu, then the audio file is unloaded from memory.

**Object Pooling.** Instead of being continuously destroyed and instantiated during runtime, falling blocks use a pooling system which instantiates a selected few when the game starts and then reuses them as they leave the screen bounds. If the initially instantiated blocks aren't enough to meet the block demand, more are instantiated as needed. However, these aren't destroyed after a game round, but are pooled for the next rounds, so that instantiation stops being required all-together, keeping a stable framerate.

**Garbage creation avoidance.** The best way to prevent the garbage collection spikes and consequent framerate drops, since garbage control is limited in C#, is to avoid creating garbage in the first place. For that, variables used in Update, loops or frequently called methods are usually referenced. Instead of creating new ones every call, and discarding them later on.

**Framerate.** The game ran smoothly on all tested devices, with the most noticeable framedrop happening when instantiating the blocks, in the very first round played, right before the countdown starts. Originally, 120 FPS was set as a toggle, in the settings menu, which ran smoothly on my device. However, the other 2 tested devices, which have 120Hz refresh rates, would lock at 60 FPS. For that reason, and considering the small difference between 60 and 120 FPS in a project like this, the framerate is now capped at 60. To test higher frame rates, simply add it as an option under Assets/DataObjects/Saved Data.

**Build Settings** Code stripping is set to High, under the Player/Optimization options and compression method is LZ4HC. Both reduce binary size.

## Performance actions considered but not implemented

**Addressable blocks.** While I didn't look into it for too long, I did consider loading only blocks that are being used, using the Addressables system. Didn't pursue it for this task since blocks aren't that heavy on memory, prioritizing the big audio file instead.

## Metadata

Created by [André Santos].  
Unity Dev Task, Tricky Towers clone for mobile, 2023.

[André Santos]:https://github.com/andrepucas

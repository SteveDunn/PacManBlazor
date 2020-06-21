PACMAN in Blazor WebAssembly

#FAQ

## How does it interactive with the canvas?
It uses the package `Blazor.Extensions.Canvas`

## What is the ideal dimensions of the canvas?
672/944 (0.711 aspect ratio) (or 224 x 314)
This is scaled by 3x3 (224x3 = 672 & 314 * 3 = 944)

All drawing is done as if the canvas is 224x314.  The automatic
canvas scaling upscales this (3x3) to fill the canvas (672/944)

## How does sound work?
It uses Howler. `sound.js` is loaded and exposes `SoundPlayer`.  In the C# code, `GameSoundPlayer` loads the sound effects via `SoundLounder` which uses `IJSRuntime`.  Each sound in C# is
represented by `SoundEffect` which uses `IJSRuntime` to call methods, e.g. `_runtime.InvokeVoidAsync("soundPlayer.play", name])`.

When a sound has stopped playing, our `end` event on Howler calls back into the C# code.  This is needed to loop things like the sirens and other long playing effects.


## How does scaling work?

## How does touch / swipe / pan work?
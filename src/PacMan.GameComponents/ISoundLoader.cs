using Microsoft.JSInterop;

namespace PacMan.GameComponents;

public interface ISoundLoader
{
    SoundEffect GetSoundEffect(SoundName name);

    IEnumerable<SoundEffect> AllSounds { get; }

    ValueTask LoadAll(IJSRuntime runtime);
}
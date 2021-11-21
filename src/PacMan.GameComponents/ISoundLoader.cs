using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using PacMan.GameComponents.Audio;

namespace PacMan.GameComponents;

public interface ISoundLoader
{
    SoundEffect GetSoundEffect(SoundName name);

    IEnumerable<SoundEffect> AllSounds { get; }

    ValueTask LoadAll(IJSRuntime runtime);
}
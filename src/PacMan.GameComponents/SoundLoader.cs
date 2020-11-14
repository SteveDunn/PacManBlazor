using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using PacMan.GameComponents.Audio;

namespace PacMan.GameComponents
{
    public class SoundLoader : ISoundLoader
    {
        Dictionary<SoundName, SoundEffect> _sounds = new();

        public SoundEffect GetSoundEffect(SoundName name) => _sounds[name];

        public IEnumerable<SoundEffect> AllSounds => _sounds.Values;

        public async ValueTask LoadAll(IJSRuntime runtime)
        {
            _sounds = new() {
                [SoundName.CoinInserted] =
                    await loadFile(SoundName.CoinInserted, "assets/audio/coin.wav"),
                [SoundName.CutScene] = await loadFile(SoundName.CutScene, "assets/audio/cutscene.wav"),
                [SoundName.PacManDying] = await loadFile(SoundName.PacManDying, "assets/audio/dying.wav"),
                [SoundName.ExtraLife] = await loadFile(SoundName.ExtraLife, "assets/audio/extra_life.wav"),
                [SoundName.Frightened] = await loadFile(SoundName.Frightened, "assets/audio/frightened.wav"),
                [SoundName.FruitEaten] = await loadFile(SoundName.FruitEaten, "assets/audio/fruit_eaten.wav"),
                [SoundName.GhostEaten] = await loadFile(SoundName.GhostEaten, "assets/audio/ghost_eaten.wav"),
                [SoundName.GhostEyes] = await loadFile(SoundName.GhostEyes, "assets/audio/ghost_eyes.wav"),
                [SoundName.Munch1] = await loadFile(SoundName.Munch1, "assets/audio/munch1.wav"),
                [SoundName.Munch2] = await loadFile(SoundName.Munch2, "assets/audio/munch2.wav"),
                [SoundName.PlayerStart] = await loadFile(SoundName.PlayerStart, "assets/audio/player_start.wav"),
                [SoundName.Siren1] = await loadFile(SoundName.Siren1, "assets/audio/siren1.wav"),
                [SoundName.Siren2] = await loadFile(SoundName.Siren2, "assets/audio/siren2.wav"),
                [SoundName.Siren3] = await loadFile(SoundName.Siren3, "assets/audio/siren3.wav"),
                [SoundName.Siren4] = await loadFile(SoundName.Siren4, "assets/audio/siren4.wav"),
                [SoundName.Siren5] = await loadFile(SoundName.Siren5, "assets/audio/siren5.wav")
            };

            // ReSharper disable once HeapView.ClosureAllocation
            async ValueTask<SoundEffect> loadFile(SoundName name, string path)
            {
                // ReSharper disable once HeapView.ObjectAllocation.Evident
                // ReSharper disable once HeapView.BoxingAllocation
                var s = name.ToString();
                await runtime.InvokeAsync<object>("soundPlayer.loadSound", new object[] { s, path });

                // ReSharper disable once HeapView.ObjectAllocation.Evident
                return new(runtime, s);
            }
        }
    }
}
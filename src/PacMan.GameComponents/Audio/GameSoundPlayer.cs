// ReSharper disable HeapView.ObjectAllocation.Evident
#pragma warning disable 8618

using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using PacMan.GameComponents.Ghosts;

namespace PacMan.GameComponents.Audio
{
    /// <summary>
    /// The sound player for game related events.
    /// </summary>
    public class GameSoundPlayer : IGameSoundPlayer
    {
        readonly ISoundLoader _loader;
        readonly IGameStats _gameStats;
        SoundEffect[] _sirens;
        SoundEffect _frightened;
        SoundEffect _ghostEyes;
        bool _loaded;
        bool _enabled;

        public GameSoundPlayer(ISoundLoader loader, IGameStats gameStats)
        {
            _loader = loader ?? throw new ArgumentNullException(nameof(loader));
            _gameStats = gameStats;
            _enabled = true;
        }

        public async ValueTask LoadAll(IJSRuntime runtime)
        {
            await _loader.LoadAll(runtime);

            _frightened = _loader.GetSoundEffect(SoundName.Frightened);
            _ghostEyes = _loader.GetSoundEffect(SoundName.GhostEyes);

            _sirens = new[]
            {
                _loader.GetSoundEffect(SoundName.Siren1),
                _loader.GetSoundEffect(SoundName.Siren2),
                _loader.GetSoundEffect(SoundName.Siren3),
                _loader.GetSoundEffect(SoundName.Siren4),
                _loader.GetSoundEffect(SoundName.Siren5)
            };

            _frightened.Loop();
            _ghostEyes.Loop();

            _sirens.ForEach(async s =>
            {
                s.Loop();
                await s.SetVolume(.5f);
            });

            _loaded = true;
        }

        public async ValueTask Reset()
        {
            _sirens.ForEach(async s => await s.Stop());
            await _ghostEyes.Stop();
            await _frightened.Stop();
        }

        public async ValueTask Update()
        {
            if (!_gameStats.AnyonePlaying)
            {
                return;
            }

            if (!_enabled)
            {
                return;
            }

            bool thereAreEyes = _gameStats.AreAnyGhostsInEyeState;

            PlayerStats playerStats = _gameStats.CurrentPlayerStats;

            await handleFright(playerStats, thereAreEyes);
            await handleSiren(playerStats.LevelStats.PillsEaten, thereAreEyes);
            await handleEyes(thereAreEyes);
        }

        async ValueTask handleFright(PlayerStats currentPlayerStats, bool thereAreEyes)
        {
            throwIfNotLoaded();

            if (thereAreEyes)
            {
                return;
            }

            GhostFrightSession? frightSession = currentPlayerStats.FrightSession;

            if (frightSession != null)
            {
                float volume = frightSession.IsFinished ? .5f : 0;

                _sirens.ForEach(async s => await s.SetVolume(volume));

                if (frightSession.IsFinished)
                {
                    await _frightened.Stop();
                }
            }
        }

        async ValueTask handleSiren(int pillsEaten, bool thereAreEyes)
        {
            throwIfNotLoaded();

            if (thereAreEyes)
            {
                return;
            }

            if (_loader.GetSoundEffect(SoundName.PlayerStart).IsPlaying)
            {
                return;
            }

            int level;

            if (pillsEaten < 117)
            {
                level = 0;
            }
            else if (pillsEaten < 180)
            {
                level = 1;
            }
            else if (pillsEaten < 212)
            {
                level = 2;
            }
            else if (pillsEaten < 230)
            {
                level = 3;
            }
            else
            {
                level = 4;
            }

            await playSiren(level);
        }

        async ValueTask playSiren(int level)
        {
            throwIfNotLoaded();

            for (int i = 0; i < _sirens.Length; i++)
            {
                SoundEffect siren = _sirens[i];

                if (i == level)
                {
                    await siren.Play();
                }
                else
                {
                    await siren.Stop();
                }
            }
        }

        async ValueTask handleEyes(bool thereAreEyes)
        {
            throwIfNotLoaded();

            if (thereAreEyes == false)
            {
                await _ghostEyes.Stop();
            }
            else
            {
                await _ghostEyes.Play();
            }
        }

        public async ValueTask Disable()
        {
            throwIfNotLoaded();

            if (!_enabled)
            {
                return;
            }

            _enabled = false;

            foreach (var s in _loader.AllSounds)
            {
                await s.Mute();
            }
        }

        public async ValueTask Enable()
        {
            throwIfNotLoaded();

            if (_enabled)
            {
                return;
            }

            _enabled = true;

            foreach (var s in _loader.AllSounds)
            {
                await s.Unmute();
            }
        }

        public async ValueTask PowerPillEaten()
        {
            throwIfNotLoaded();

            await play(SoundName.Frightened);
        }

        public async ValueTask FruitEaten()
        {
            throwIfNotLoaded();

            await play(SoundName.FruitEaten);
        }

        public async ValueTask GhostEaten()
        {
            throwIfNotLoaded();

            await play(SoundName.GhostEaten);
            await play(SoundName.GhostEyes);
        }

        public async ValueTask GotExtraLife()
        {
            throwIfNotLoaded();

            await play(SoundName.ExtraLife);
        }

        public async ValueTask CutScene()
        {
            throwIfNotLoaded();
            await play(SoundName.CutScene);
        }

        public async ValueTask PacManDying()
        {
            throwIfNotLoaded();
            await play(SoundName.PacManDying);
        }

        public async ValueTask PlayerStart()
        {
            throwIfNotLoaded();
            await play(SoundName.PlayerStart);
        }

        public async ValueTask CoinInserted()
        {
            throwIfNotLoaded();
            await play(SoundName.CoinInserted);

            // it might have been set in the demo mode
            await _frightened.Stop();
        }

        public async ValueTask Munch1()
        {
            await play(SoundName.Munch1);
        }

        public async ValueTask Munch2()
        {
            await play(SoundName.Munch2);
        }

        async ValueTask play(SoundName soundName)
        {
            throwIfNotLoaded(); SoundEffect audio = _loader.GetSoundEffect(soundName);

            await audio.Play();
        }

        public void MarkAsFinished(string name)
        {
            throwIfNotLoaded();
            bool parsed = Enum.TryParse(name, out SoundName val);

            if (!parsed)
            {
                throw new InvalidOperationException($"No such sound: {name}");
            }

            _loader.GetSoundEffect(val).MarkAsFinished();
        }

        void throwIfNotLoaded()
        {
            if (!_loaded)
            {
                throw new InvalidOperationException("sounds not loaded");
            }
        }
    }
}
// ReSharper disable HeapView.ObjectAllocation.Evident
#pragma warning disable 8618

using Microsoft.JSInterop;
using PacMan.GameComponents.Ghosts;

namespace PacMan.GameComponents.Audio;

/// <summary>
/// The sound player for game related events.
/// </summary>
public class GameSoundPlayer : IGameSoundPlayer
{
    private readonly ISoundLoader _loader;
    private readonly IGameStats _gameStats;
    private SoundEffect[] _sirens;
    private SoundEffect _frightened;
    private SoundEffect _ghostEyes;
    private bool _loaded;
    private bool _enabled;

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

        _sirens =
        [
            _loader.GetSoundEffect(SoundName.Siren1),
            _loader.GetSoundEffect(SoundName.Siren2),
            _loader.GetSoundEffect(SoundName.Siren3),
            _loader.GetSoundEffect(SoundName.Siren4),
            _loader.GetSoundEffect(SoundName.Siren5)
        ];

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

        await HandleFright(playerStats, thereAreEyes);
        await HandleSiren(playerStats.LevelStats.PillsEaten, thereAreEyes);
        await HandleEyes(thereAreEyes);
    }

    private async ValueTask HandleFright(PlayerStats currentPlayerStats, bool thereAreEyes)
    {
        ThrowIfNotLoaded();

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

    private async ValueTask HandleSiren(int pillsEaten, bool thereAreEyes)
    {
        ThrowIfNotLoaded();

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

        await PlaySiren(level);
    }

    private async ValueTask PlaySiren(int level)
    {
        ThrowIfNotLoaded();

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

    private async ValueTask HandleEyes(bool thereAreEyes)
    {
        ThrowIfNotLoaded();

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
        ThrowIfNotLoaded();

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
        ThrowIfNotLoaded();

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
        ThrowIfNotLoaded();

        await Play(SoundName.Frightened);
    }

    public async ValueTask FruitEaten()
    {
        ThrowIfNotLoaded();

        await Play(SoundName.FruitEaten);
    }

    public async ValueTask GhostEaten()
    {
        ThrowIfNotLoaded();

        await Play(SoundName.GhostEaten);
        await Play(SoundName.GhostEyes);
    }

    public async ValueTask GotExtraLife()
    {
        ThrowIfNotLoaded();

        await Play(SoundName.ExtraLife);
    }

    public async ValueTask CutScene()
    {
        ThrowIfNotLoaded();
        await Play(SoundName.CutScene);
    }

    public async ValueTask PacManDying()
    {
        ThrowIfNotLoaded();
        await Play(SoundName.PacManDying);
    }

    public async ValueTask PlayerStart()
    {
        ThrowIfNotLoaded();
        await Play(SoundName.PlayerStart);
    }

    public async ValueTask CoinInserted()
    {
        ThrowIfNotLoaded();
        await Play(SoundName.CoinInserted);

        // it might have been set in the demo mode
        await _frightened.Stop();
    }

    public ValueTask Munch1() => Play(SoundName.Munch1);

    public ValueTask Munch2() => Play(SoundName.Munch2);

    private async ValueTask Play(SoundName soundName)
    {
        ThrowIfNotLoaded(); 
        
        SoundEffect audio = _loader.GetSoundEffect(soundName);

        await audio.Play();
    }

    public void MarkAsFinished(string name)
    {
        ThrowIfNotLoaded();
        
        bool parsed = Enum.TryParse(name, out SoundName val);

        if (!parsed)
        {
            throw new InvalidOperationException($"No such sound: {name}");
        }

        _loader.GetSoundEffect(val).MarkAsFinished();
    }

    private void ThrowIfNotLoaded()
    {
        if (!_loaded)
        {
            throw new InvalidOperationException("sounds not loaded");
        }
    }
}
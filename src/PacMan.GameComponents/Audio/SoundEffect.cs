using Microsoft.JSInterop;

namespace PacMan.GameComponents.Audio;

public class SoundEffect
{
    bool _loop;
    float _previousVolume = 0.5f;

    readonly IJSRuntime _runtime;
    readonly string _name;

    // ReSharper disable once HeapView.ObjectAllocation.Evident
    static readonly object[] _setVolumeInvokeArray = new object[2];

    public SoundEffect(IJSRuntime runtime, string name)
    {
        _runtime = runtime;
        _name = name;
    }

    public async ValueTask SetVolume(float volume)
    {
        _setVolumeInvokeArray[0] = _name;

        // ReSharper disable once HeapView.BoxingAllocation
        _setVolumeInvokeArray[1] = volume;

        await _runtime.InvokeAsync<object>("soundPlayer.setVolume", _setVolumeInvokeArray);
    }

    public bool IsPlaying { get; private set; }

    public void Loop() => _loop = true;

    public async ValueTask Mute()
    {
        _previousVolume = await _runtime.InvokeAsync<float>("soundPlayer.getVolume", new object[] { _name });

        await SetVolume(0f);
    }

    public ValueTask Unmute() => SetVolume(_previousVolume);

    public async ValueTask Stop()
    {
        IsPlaying = false;

        await _runtime.InvokeVoidAsync("soundPlayer.stop", _name);
    }

    public async ValueTask Play()
    {
        if (_loop && IsPlaying)
        {
            return;
        }

        await Stop();

        await _runtime.InvokeVoidAsync("soundPlayer.play", _name);

        IsPlaying = true;
    }

    /// <summary>
    /// Called via Howler when finished.
    /// </summary>
    public void MarkAsFinished() => IsPlaying = false;
}
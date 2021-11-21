using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace PacMan.GameComponents;

public class GameStorage : IGameStorage
{
    readonly IJSRuntime _jsRuntime;

    public GameStorage(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async ValueTask<int> GetHighScore()
    {
        var hs = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "highscore");

        if (string.IsNullOrEmpty(hs))
        {
            return 0;
        }

        return Convert.ToInt32(hs);
    }

    public async ValueTask SetHighScore(int highScore)
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "highscore", highScore.ToString());
    }
}
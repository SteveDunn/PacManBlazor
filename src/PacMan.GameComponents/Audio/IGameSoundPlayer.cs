using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace PacMan.GameComponents.Audio
{
    public interface IGameSoundPlayer
    {
        ValueTask LoadAll(IJSRuntime runtime);
        ValueTask Reset();
        ValueTask Update();
        ValueTask Disable();
        ValueTask Enable();
        ValueTask PowerPillEaten();
        ValueTask FruitEaten();
        ValueTask GhostEaten();
        ValueTask GotExtraLife();
        ValueTask CutScene();
        ValueTask PacManDying();
        ValueTask PlayerStart();
        //ValueTask CoinInserted();
        ValueTask Munch1();
        ValueTask Munch2();
        void MarkAsFinished(string name);
        ValueTask CoinInserted();
    }
}
using Blazor.Extensions.Canvas.Canvas2D;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using PacMan.GameComponents.Ghosts;

namespace PacMan.GameComponents;

public interface IGame
{
    ValueTask Initialise(IJSRuntime jsRuntime);

    ValueTask FruitEaten(Points points);

    ValueTask RunGameLoop(float timestamp);

    void PostRenderInitialize(Canvas2DContext outputCanvasContext, Canvas2DContext player1MazeCanvas, Canvas2DContext player2MazeCanvas, in ElementReference spritesheetReference);

    void SetAct(IAct act);

    ValueTask GhostEaten(IGhost ghost, Points points);
}
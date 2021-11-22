using PacMan.GameComponents.Canvas;

namespace PacMan.GameComponents;

public interface IMaze
{
    // Vector2 SpriteSheetPos { get; }
    // bool Visible { get; }
    // Size Size { get; }
    // Vector2 Position { get; }
    // Vector2 Origin { get; }
    ValueTask Update(CanvasTimingInformation timing);

    ValueTask Draw(CanvasWrapper session);

    ValueTask ClearCell(CellIndex cell);

    bool CanContinueInDirection(Direction direction, Tile tile);

    DirectionChoices GetChoicesAtCellPosition(CellIndex cellPos);

    void StartFlashing();

    void StopFlashing();

    TileContent GetTileContent(Tile cell);

    ValueTask HandlePlayerStarted(PlayerStats playerStats, MazeCanvas playerCanvas);
}
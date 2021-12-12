using PacMan.GameComponents.Ghosts;

namespace PacMan.GameComponents;

public interface IGhostCollection
{
    IGhost GetGhost(GhostNickname nickName);

    IGhost[] Ghosts { get; }

    ValueTask DrawAll(CanvasWrapper canvas);

    ValueTask Update(CanvasTimingInformation timing);
}
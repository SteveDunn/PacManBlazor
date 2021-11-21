namespace PacMan.GameComponents.Ghosts;

public enum GhostState
{
    // heading towards pacman or their home corner (scatter)
    Normal,

    // blue - running away from pacman (in a random pattern)
    Frightened,

    // heading back to the 'House'
    Eyes
}
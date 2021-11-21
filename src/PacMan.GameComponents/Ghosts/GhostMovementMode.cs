namespace PacMan.GameComponents.Ghosts;

public enum GhostMovementMode
{
    Undecided,

    /// <summary>
    /// Chasing pacman
    /// </summary>
    Chase,

    /// <summary>
    /// Heading back to their 'home corner'
    /// </summary>
    Scatter,

    /// <summary>
    /// Heading back to the house (after they've been eaten)
    /// </summary>
    GoingToHouse,

    /// <summary>
    /// In the ghost house
    /// </summary>
    InHouse,

    /// <summary>
    /// Blue / frightened
    /// </summary>
    Frightened
}
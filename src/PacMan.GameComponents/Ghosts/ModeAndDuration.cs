﻿namespace PacMan.GameComponents.Ghosts;

[CannotApplyEqualityOperator]
public class ModeAndDuration
{
    public ModeAndDuration(GhostMovementMode mode, TimeSpan duration)
    {
        Mode = mode;
        Duration = duration;
    }

    public GhostMovementMode Mode { get; }

    public TimeSpan Duration { get; private set; }

    public void DecreaseDurationBy(in TimeSpan elapsedTime) => Duration -= elapsedTime;
}
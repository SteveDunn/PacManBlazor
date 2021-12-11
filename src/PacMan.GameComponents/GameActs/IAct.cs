namespace PacMan.GameComponents.GameActs;

/// An 'act' is something that's run in a loop.  The main window continually updates and draws whatever
/// the 'current act' is.  Acts are things such as DemoAct, GameAct, GameOverAct etc.
public interface IAct
{
    string Name { get; }

    ValueTask Reset();

    ValueTask<ActUpdateResult> Update(CanvasTimingInformation timing);

    ValueTask Draw(CanvasWrapper session);
}
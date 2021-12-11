namespace PacMan.GameComponents.GameActs;

public class NullAct : IAct
{
    public string Name => "NullAct";

    public ValueTask Reset() => default;

    public ValueTask<ActUpdateResult> Update(CanvasTimingInformation timing) =>
        new(ActUpdateResult.Running);

    public ValueTask Draw(CanvasWrapper session) => default;
}
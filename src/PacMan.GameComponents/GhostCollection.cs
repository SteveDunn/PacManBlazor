using PacMan.GameComponents.Ghosts;

namespace PacMan.GameComponents;

public class GhostCollection : IGhostCollection
{
    [SuppressMessage("ReSharper", "HeapView.ObjectAllocation.Evident")]
    public GhostCollection(IEnumerable<IGhost> ghosts)
    {
        Ghosts = ghosts.ToArray();
    }

    public IGhost GetGhost(GhostNickname nickName)
    {
        var index = Ghosts.Single(g => g.NickName == nickName);
        return index;
    }

    public IGhost[] Ghosts { get; }

    public async ValueTask DrawAll(CanvasWrapper canvas)
    {
        foreach (IGhost eachGhost in Ghosts)
        {
            await eachGhost.Draw(canvas);
        }
    }

    public async ValueTask Update(CanvasTimingInformation timing)
    {
        foreach (IGhost eachGhost in Ghosts)
        {
            await eachGhost.Update(timing);
        }
    }

    public void ForAll(Action<IGhost> action)
    {
        foreach (var eachGhost in Ghosts)
        {
            action(eachGhost);
        }
    }
}
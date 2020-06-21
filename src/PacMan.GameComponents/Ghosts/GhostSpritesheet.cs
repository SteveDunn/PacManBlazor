using System.Collections.Generic;
using System.Numerics;

namespace PacMan.GameComponents.Ghosts
{
    public class GhostSpritesheet
    {
        readonly Dictionary<GhostNickname, GhostSpritesheetInfo> _entries;
        readonly FrightenedSpritesheet _frightened;

        public GhostSpritesheet()
        {
            const int left = 457;

            int x = left + 8 * 16;

            _frightened = new FrightenedSpritesheet(
                new FramePair(new Vector2(x, 64), new Vector2(x += 16, 64)),
                new FramePair(new Vector2(x += 16, 64), new Vector2(x + 16, 64)));

            x = left + 8 * 16;
            Eyes = new EyesSpritesheetInfo(new Vector2(x, 64 + 16));

            _entries = new Dictionary<GhostNickname, GhostSpritesheetInfo>
            {
                [GhostNickname.Blinky] = new GhostSpritesheetInfo(new Vector2(left, 64)),
                [GhostNickname.Pinky] = new GhostSpritesheetInfo(new Vector2(left, 64 + 16)),
                [GhostNickname.Inky] = new GhostSpritesheetInfo(new Vector2(left, 64 + 32)),
                [GhostNickname.Clyde] = new GhostSpritesheetInfo(new Vector2(left, 64 + 48))
            };
        }

        public EyesSpritesheetInfo Eyes { get; }

        public GhostSpritesheetInfo GetEntry(GhostNickname nickname) => _entries[nickname];

        public FrightenedSpritesheet GetFrightened() => _frightened;
    }
}

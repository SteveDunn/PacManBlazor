using System.Collections.Generic;

namespace PacMan.GameComponents.Ghosts
{
    public class GhostSpritesheet
    {
        readonly Dictionary<GhostNickname, GhostSpritesheetInfo> _entries;
        readonly FrightenedSpritesheet _frightened;

        public GhostSpritesheet()
        {
            const int left = 457;

            int x = left + (8 * 16);

            _frightened = new(
                new(new(x, 64), new(x += 16, 64)),
                new(new(x += 16, 64), new(x + 16, 64)));

            x = left + (8 * 16);
            Eyes = new(new(x, 64 + 16));

            _entries = new() {
                [GhostNickname.Blinky] = new(new(left, 64)),
                [GhostNickname.Pinky] = new(new(left, 64 + 16)),
                [GhostNickname.Inky] = new(new(left, 64 + 32)),
                [GhostNickname.Clyde] = new(new(left, 64 + 48))
            };
        }

        public EyesSpritesheetInfo Eyes { get; }

        public GhostSpritesheetInfo GetEntry(GhostNickname nickname) => _entries[nickname];

        public FrightenedSpritesheet GetFrightened() => _frightened;
    }
}

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Numerics;
using System.Threading.Tasks;
using PacMan.GameComponents.Canvas;
using PacMan.GameComponents.Ghosts;

namespace PacMan.GameComponents
{
    public class AttractScenePacMan : ISprite
    {
        readonly Dictionary<Directions, FramePair> _velocitiesLookup;
        readonly TwoFrameAnimation _animDirection;

        Vector2 _frame1InSpriteMap;
        Vector2 _frame2InSpriteMap;

        [SuppressMessage("ReSharper", "HeapView.ObjectAllocation.Evident")]
        public AttractScenePacMan()
        {
            Visible = true;
            _animDirection = new TwoFrameAnimation(65.Milliseconds());

            Direction = Directions.Left;

            const float left = 456;
            const float left2 = 472;

            _velocitiesLookup = new Dictionary<Directions, FramePair>
            {
                [Directions.Up] = new FramePair(
                    new Vector2(left, 32), new Vector2(left2, 32)),
                [Directions.Down] = new FramePair(
                    new Vector2(left, 48), new Vector2(left2, 48)),
                [Directions.Left] = new FramePair(
                    new Vector2(left, 16), new Vector2(left2, 16)),
                [Directions.Right] = new FramePair(
                    new Vector2(left, 0), new Vector2(left2, 0))
            };

            Position = Tile.ToCenterCanvas(new Vector2(13.5f, 23));

            setSpriteSheetPointers();
        }

        public Vector2 SpriteSheetPos { get; private set; }

        public bool Visible { get; set; }

        public Vector2 Position { get; set; }

        public ValueTask Draw(CanvasWrapper session)
        {
            return session.DrawSprite(this, Spritesheet.Reference);
        }

        public Size Size { get; } = new Size(16, 16);

        public Vector2 Origin => Vector2s.Eight;

        public Directions Direction { private get; set; }

        ValueTask updateAnimation(CanvasTimingInformation context)
        {
            _animDirection.Run(context);

            setSpriteSheetPointers();

            return default;
        }

        void setSpriteSheetPointers()
        {
            _frame1InSpriteMap = _velocitiesLookup[Direction].First;
            _frame2InSpriteMap = _velocitiesLookup[Direction].Second;

            SpriteSheetPos = _animDirection.Flag ? _frame1InSpriteMap : _frame2InSpriteMap;
        }

        public async ValueTask Update(CanvasTimingInformation timing)
        {
            await updateAnimation(timing);
        }
    }
}
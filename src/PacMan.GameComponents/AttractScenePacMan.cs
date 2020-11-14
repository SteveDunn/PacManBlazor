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
            _animDirection = new(65.Milliseconds());

            Direction = Directions.Left;

            const float left = 456;
            const float left2 = 472;

            _velocitiesLookup = new() {
                [Directions.Up] = new(
                    new(left, 32), new(left2, 32)),
                [Directions.Down] = new(
                    new(left, 48), new(left2, 48)),
                [Directions.Left] = new(
                    new(left, 16), new(left2, 16)),
                [Directions.Right] = new(
                    new(left, 0), new(left2, 0))
            };

            Position = Tile.ToCenterCanvas(new(13.5f, 23));

            setSpriteSheetPointers();
        }

        public Vector2 SpriteSheetPos { get; private set; }

        public bool Visible { get; set; }

        public Vector2 Position { get; set; }

        public ValueTask Draw(CanvasWrapper session)
        {
            return session.DrawSprite(this, Spritesheet.Reference);
        }

        public Size Size { get; } = new(16, 16);

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
using PacMan.GameComponents.Canvas;

namespace PacMan.GameComponents
{
    public interface IHumanInterfaceParser
    {
        bool IsLeftKeyDown { get; }

        bool IsUpKeyDown { get; }

        bool IsDownKeyDown { get; }

        bool IsRightKeyDown { get; }

        bool WasTapped { get; }

        bool WasLongPress { get; }

        void Update(CanvasTimingInformation timing);

        bool IsKeyCurrentlyDown(byte key);

        /// <summary>
        /// A 'press' is pressing and releasing, not just 'pushed'
        /// To see if a key is 'down', use 'IsKeyCurrentlyDown'
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool WasKeyPressedAndReleased(byte key);

        void KeyDown(byte key);

        void KeyUp(byte key);

        bool IsPanning(byte key);

        void Swiped(byte key);

        void TapHappened();

        void LongPressHappened();
    }
}
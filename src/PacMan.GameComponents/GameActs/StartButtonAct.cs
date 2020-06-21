using System.Numerics;
using System.Threading.Tasks;
using MediatR;
using PacMan.GameComponents.Canvas;
using PacMan.GameComponents.Events;

namespace PacMan.GameComponents.GameActs
{
    /// <summary>
    /// The 'act' that shows the '1 or 2 players' screen.
    /// </summary>
    public class StartButtonAct : IAct
    {
        readonly IMediator _mediator;
        readonly IHumanInterfaceParser _input;
        readonly ICoinBox _coinBox;

        public string Name => "StartButtonAct";

        public StartButtonAct(IMediator mediator, IHumanInterfaceParser input, ICoinBox coinBox)
        {
            _mediator = mediator;
            _input = input;
            _coinBox = coinBox;
        }

        public ValueTask Reset() => default;

        public async ValueTask<ActUpdateResult> Update(CanvasTimingInformation timing)
        {
            if (_input.WasKeyPressedAndReleased(Keys.One))
            {
                await _mediator.Publish(new NewGameEvent(1));

                return ActUpdateResult.Finished;
            }

            if (_input.WasKeyPressedAndReleased(Keys.Two) && _coinBox.Credits >= 2)
            {
                await _mediator.Publish(new NewGameEvent(2));
                
                return ActUpdateResult.Finished;
            }

            if (_input.WasKeyPressedAndReleased(Keys.Five))
            {
                await _mediator.Publish(new CoinInsertedEvent());
            }

            return ActUpdateResult.Running;
        }

        public async ValueTask Draw(CanvasWrapper session)
        {
            await session.DrawMyText("PUSH START BUTTON", new Vector2(50, 115), Colors.Orange);

            var text = _coinBox.Credits < 2 ? "1 PLAYER ONLY" : "1 OR 2 PLAYERS";

            await session.DrawMyText(text, new Vector2(70, 145), Colors.Cyan);
            await session.DrawMyText("BONUS PAC-MAN FOR 10000 PTS", new Vector2(0, 175), Colors.White);
            await session.DrawMyText("(C) 1980 MIDWAY MFG. CO.", new Vector2(15, 190), Colors.White);
        }
    }
}
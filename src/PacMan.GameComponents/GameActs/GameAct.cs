using System.Threading.Tasks;
using MediatR;
using PacMan.GameComponents.Audio;
using PacMan.GameComponents.Canvas;
using PacMan.GameComponents.Events;

namespace PacMan.GameComponents.GameActs;

/// <summary>
/// The main 'Game' act.  Draws everything (maze, ghosts, pacman), and updates everything (keyboard, sound etc.). Transitions to the 'player intro' act.
/// </summary>
public class GameAct : IAct
{
    readonly ICoinBox _coinBox;
    readonly IMediator _mediator;
    readonly IHumanInterfaceParser _input;
    readonly IGameSoundPlayer _gameSoundPlayer;
    readonly IGameStats _gameStats;
    readonly IGhostCollection _ghostCollection;
    readonly IMaze _maze;
    readonly IPacMan _pacman;
    readonly IFruit _fruit;

    bool _paused;

    public GameAct(
        ICoinBox coinBox,
        IMediator mediator,
        IHumanInterfaceParser input,
        IGameSoundPlayer gameSoundPlayer,
        IGameStats gameStats,
        IGhostCollection ghostCollection,
        IMaze maze,
        IPacMan pacman,
        IFruit fruit)
    {
        _coinBox = coinBox;
        _mediator = mediator;
        _input = input;
        _gameSoundPlayer = gameSoundPlayer;
        _gameStats = gameStats;
        _ghostCollection = ghostCollection;
        _maze = maze;
        _pacman = pacman;
        _fruit = fruit;
    }

    public string Name { get; } = "GameAct";

    public ValueTask Reset()
    {
        Pnrg.ResetPnrg();

        return default;
    }

    public async ValueTask<ActUpdateResult> Update(CanvasTimingInformation timing)
    {
        if (_gameStats.IsDemo)
        {
            if (_input.WasKeyPressedAndReleased(Keys.Space) ||
                _input.WasKeyPressedAndReleased(Keys.One) ||
                _input.WasTapped)
            {
                await _gameSoundPlayer.Enable();
                _coinBox.CoinInserted();
                await _mediator.Publish(new NewGameEvent(1));

                return ActUpdateResult.Running;
            }

            if (_input.WasKeyPressedAndReleased(Keys.Two) || _input.WasLongPress)
            {
                await _gameSoundPlayer.Enable();

                _coinBox.CoinInserted();
                _coinBox.CoinInserted();

                await _mediator.Publish(new NewGameEvent(2));

                return ActUpdateResult.Running;
            }

            if (_input.WasKeyPressedAndReleased(Keys.Five))
            {
                await _mediator.Publish(new CoinInsertedEvent());

                return ActUpdateResult.Finished;
            }
        }

        if (_input.WasKeyPressedAndReleased(Keys.P))
        {
            _paused = !_paused;

            if (_paused)
            {
                await _gameSoundPlayer.Disable();
            }

            else
            {
                await _gameSoundPlayer.Enable();
            }
        }

        if (_paused)
        {
            return ActUpdateResult.Running;
        }

        // play sounds first, as if we play them last, that state of the game
        // might've updated and the 'current act' might've changed
        // so we don't want to be playing our sounds if
        // another act is queued up.
        await _gameSoundPlayer.Update();

        _gameStats.Update(timing);

        await _maze.Update(timing);
        await _pacman.Update(timing);
        await _fruit.Update(timing);

        await _ghostCollection.Update(timing);

        return ActUpdateResult.Running;
    }

    public async ValueTask Draw(CanvasWrapper session)
    {
        await _maze.Draw(session);
        await _pacman.Draw(session);

        await _fruit.Draw(session);

        await _ghostCollection.DrawAll(session);

        if (_gameStats.IsDemo)
        {
            await session.DrawMyText("GAME OVER", TextPoints.GameOverPoint, Colors.Red);
        }
    }
}
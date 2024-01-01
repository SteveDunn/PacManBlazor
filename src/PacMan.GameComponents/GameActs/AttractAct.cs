using System.Drawing;
using System.Reflection;
using PacMan.GameComponents.Ghosts;

namespace PacMan.GameComponents.GameActs;

public record struct ColouredText(string Text = "", Color Color = default);

/// Show's the attract screen (ghost names and pictures and the 'chase' sub act).  Transitions to either
/// the 'player intro' act (to start the demo mode if nothing's was pressed/clicked/touched),
/// or the 'start button' act if a coin was 'inserted'.
public class AttractAct : IAct
{
    readonly ICoinBox _coinBox;
    readonly IMediator _mediator;
    readonly IHumanInterfaceParser _input;
    readonly IGameSoundPlayer _gameSoundPlayer;
    readonly Marquee _marquee;
    readonly GeneralSprite _pacmanLogo;

    struct Instruction
    {
        public required TimeSpan When { get; init; }
        public required Vector2 Where { get; init; }
        public SimpleGhost? Ghost { get; init; }

        public ColouredText ColouredText { get; init; }
    }

    readonly SimpleGhost _blinky;
    readonly SimpleGhost _pinky;
    readonly SimpleGhost _inky;
    readonly SimpleGhost _clyde;

    readonly List<Instruction> _instructions;

    readonly object _lock;
    readonly BlazorLogo _blazorLogo;
    readonly TimeSpan _chaseSubActReadyAt;

    ChaseSubAct _chaseSubAct;
    TimeSpan _startTime;
    TimeSpan _drawUpTo;
    bool _chaseSubActReady;

    bool _finished;

    [SuppressMessage("ReSharper", "HeapView.ObjectAllocation.Evident")]
    public AttractAct(
        ICoinBox coinBox,
        IMediator mediator,
        IHumanInterfaceParser input,
        IGameSoundPlayer gameSoundPlayer)
    {
        string version = Assembly
            .GetEntryAssembly()
            ?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
            ?.InformationalVersion ?? "??";

        MarqueeText[] texts =
        [
            new() {
                Text = $"v{version}",
                YPosition = 195,
                TimeIdle = 1.Seconds(),
                TimeIn = 1.Seconds(),
                TimeStationary = 1.Seconds(),
                TimeOut = .5f.Seconds()
            },
            new() {
                Text = "tap/space - 1 player",
                YPosition = 195,
                TimeIdle = 1.Seconds(),
                TimeIn = 2.Seconds(),
                TimeStationary = 2.Seconds(),
                TimeOut = 1.Seconds()
            },
            new() {
                Text = "long press/2 - 2 players",
                YPosition = 195,
                TimeIdle = 0.Seconds(),
                TimeIn = 2.Seconds(),
                TimeStationary = 2.Seconds(),
                TimeOut = 1.Seconds()
            }
        ];

        _marquee = new(texts);
        _coinBox = coinBox;
        _mediator = mediator;
        _input = input;
        _gameSoundPlayer = gameSoundPlayer;
        _pacmanLogo =
            new(new(192, 25), new(36, 152), Vector2.Zero, new(456, 173));
        _blazorLogo = new();

        _instructions = [];

        _startTime = TimeSpan.MinValue;

        _blinky = new(GhostNickname.Blinky, Direction.Right);
        _pinky = new(GhostNickname.Pinky, Direction.Right);
        _inky = new(GhostNickname.Inky, Direction.Right);
        _clyde = new(GhostNickname.Clyde, Direction.Right);
        _startTime = TimeSpan.MinValue;
        _chaseSubActReadyAt = 9.Seconds();

        _chaseSubAct = new();
        _lock = new();
    }

    public string Name { get; } = "AttractAct";

    public ValueTask Reset()
    {
        _startTime = TimeSpan.MinValue;
        _finished = false;
        _instructions.Clear();
        _chaseSubAct = new();
        PopulateDelayedInstructions();

        return default;
    }

    public async ValueTask<ActUpdateResult> Update(CanvasTimingInformation timing)
    {
        await _blazorLogo.Update(timing);

        await _marquee.Update(timing);

        if (_startTime == TimeSpan.MinValue)
        {
            _startTime = timing.TotalTime;
        }

        _drawUpTo = timing.TotalTime - _startTime;

        if (_input.WasKeyPressedAndReleased(Keys.Left))
        {
            await StartDemoGame();
            return ActUpdateResult.Running;
        }

        if (_input.WasKeyPressedAndReleased(Keys.Five))
        {
            await _gameSoundPlayer.Enable();
            await _mediator.Publish(new CoinInsertedEvent());

            return ActUpdateResult.Running;
        }

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
            _coinBox.CoinInserted();

            _coinBox.CoinInserted();
            _coinBox.CoinInserted();

            await _mediator.Publish(new NewGameEvent(2));

            return ActUpdateResult.Running;
        }

        _chaseSubActReady = timing.TotalTime - _startTime >= _chaseSubActReadyAt;

        if (_chaseSubActReady)
        {
            if (await _chaseSubAct.Update(timing) == ActUpdateResult.Finished)
            {
                if (!_finished)
                {
                    await StartDemoGame();
                    _finished = true;
                }

                return ActUpdateResult.Finished;
            }
        }

        return ActUpdateResult.Running;
    }

    public async ValueTask Draw(CanvasWrapper session)
    {
        for (int i = 0; i < _instructions.Count; i++)
        {
            var inst = _instructions[i];

            if (inst.When > _drawUpTo)
            {
                break;
            }

            SimpleGhost? ghost = inst.Ghost;

            if (ghost != null)
            {
                ghost.Position = inst.Where;

                await session.DrawSprite(ghost, Spritesheet.Reference);
            }
            else
            {
                // if the ghost is null, it's a text instruction

                await DrawText(session, inst.ColouredText.Text, inst.Where, inst.ColouredText.Color);
            }
        }

        if (_chaseSubActReady)
        {
            await _chaseSubAct.Draw(session);
        }

        await _marquee.Draw(session);

        await session.SetGlobalAlphaAsync(.75f);
        await session.DrawSprite(_pacmanLogo, Spritesheet.Reference);
        await session.SetGlobalAlphaAsync(1f);

        await _blazorLogo.Draw(session);
    }

    void PopulateDelayedInstructions()
    {
        lock (_lock)
        {
            TimeSpan clock = 1500.Milliseconds();

            _instructions.Add(new Instruction {
                When = clock,
                Where = new(32, 12),
                ColouredText = new("CHARACTER / NICKNAME", Color.White),
            });

            var gap = new Vector2(0, 24);

            var pos = new Vector2(16, 30);

            var timeForEachOne = 600.Milliseconds();

            WriteInstructionsForGhost(ref clock, _blinky, Colors.Red, "SHADOW", "BLINKY", pos);

            clock += timeForEachOne;
            pos += gap;
            WriteInstructionsForGhost(ref clock, _pinky, Colors.Pink, "SPEEDY", "PINKY", pos);

            clock += timeForEachOne;
            pos += gap;
            WriteInstructionsForGhost(ref clock, _inky, Colors.Cyan, "BASHFUL", "INKY", pos);

            clock += timeForEachOne;
            pos += gap;
            WriteInstructionsForGhost(ref clock, _clyde, Colors.Yellow, "POKEY", "CLYDE", pos);
        }
    }

    static ValueTask DrawText(CanvasWrapper canvasWrapper, string text, Vector2 point, Color color)
    {
        return canvasWrapper.DrawMyText(text, point, color);
    }

    void WriteInstructionsForGhost(
        ref TimeSpan clock,
        SimpleGhost ghost,
        Color color,
        string name,
        string nickname,
        Vector2 point)
    {
        _instructions.Add(new() {
            Ghost = ghost,
            When = clock,
            Where = point
        });

        point += new Vector2(18, -4);

        clock += 1.Seconds();

        _instructions.Add(new() {
            Where = point,
            ColouredText = new($@" - {name}", color),
            When = clock,
        });

        point += new Vector2(90, 0);

        clock += 500.Milliseconds();

        _instructions.Add(new() {
            Where = point,
            ColouredText = new($@"""{nickname}""", color),
            When = clock,
        });
    }

    async ValueTask StartDemoGame() => await _mediator.Publish(new DemoStartedEvent());
}
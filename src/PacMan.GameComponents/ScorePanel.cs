using System.Numerics;
using PacMan.GameComponents.Canvas;

namespace PacMan.GameComponents;

public class ScorePanel : IScorePanel
{
    readonly IGameStats _gameStats;
    readonly Vector2 _scorePos2Up = new(206, 8);
    readonly Vector2 _scorePos1Up = new(62, 8);
    readonly Vector2 _highScorePos = new(140, 8);
    readonly Vector2 _highScoreTextPos = new(72, 0);

    readonly LoopingTimer _timer;
    readonly LoopingTimer _trialTimer;
    readonly Vector2 _playerOneTextPos = new(30, 0);
    readonly Vector2 _playerTwoTextPos = new(180, 0);

    bool _tickTock = true;
    bool _trialTickTock;

    public ScorePanel(IGameStats gameStats)
    {
        _gameStats = gameStats;

        // ReSharper disable HeapView.ObjectAllocation.Evident
        // ReSharper disable HeapView.DelegateAllocation
        _timer = new(250.Milliseconds(), () => _tickTock = !_tickTock);
        _trialTimer = new(2.Seconds(), () => _trialTickTock = !_trialTickTock);

        // ReSharper restore HeapView.DelegateAllocation
        // ReSharper restore HeapView.ObjectAllocation.Evident
    }

    public void Update(CanvasTimingInformation timingInformation)
    {
        _timer.Run(timingInformation);
        _trialTimer.Run(timingInformation);
    }

    public async ValueTask Draw(CanvasWrapper ds)
    {
        await drawPlayerOneScore(ds);
        await drawPlayerTwoScore(ds);
        await drawHighScore(ds);
    }

    async ValueTask drawHighScore(CanvasWrapper ds)
    {
        await ds.DrawMyText("HIGH SCORE", _highScoreTextPos, Colors.White);

        await drawRightAlignedScoreText(ds, _gameStats.HighScore, _highScorePos);
    }

    async ValueTask drawPlayerOneScore(CanvasWrapper ds)
    {
        await drawPlayerText(ds, 0, "1UP", _playerOneTextPos);

        var score = 0;

        if (_gameStats.HasPlayerStats(0))
        {
            score = _gameStats.GetPlayerStats(0).Score;
        }

        await drawRightAlignedScoreText(ds, score, _scorePos1Up);
    }

    async static ValueTask drawRightAlignedScoreText(CanvasWrapper ds, int score, Vector2 pos)
    {
        var scoreText = score.ToString();

        if (scoreText == "0")
        {
            scoreText = "00";
        }

        var length = new Vector2(scoreText.Length * 8, 0);

        var left = pos - length;

        await ds.DrawMyText(scoreText, left, Colors.White);
    }

    async ValueTask drawPlayerTwoScore(CanvasWrapper ds)
    {
        if (_gameStats.AmountOfPlayers > 1)
        {
            await drawPlayerText(ds, 1, "2UP", _playerTwoTextPos);
            var score = 0;

            if (_gameStats.HasPlayerStats(1))
            {
                score = _gameStats.GetPlayerStats(1).Score;
            }

            await drawRightAlignedScoreText(ds, score, _scorePos2Up);
        }
    }

    async ValueTask drawPlayerText(CanvasWrapper ds, int playerIndex, string text, Vector2 pos)
    {
        var shouldFlash = _gameStats.AnyonePlaying && !_gameStats.IsGameOver && _gameStats.CurrentPlayerStats.PlayerIndex == playerIndex;

        var shouldDraw = !shouldFlash || _tickTock;

        if (shouldDraw)
        {
            await ds.DrawMyText(text, pos, Colors.White);
        }
    }
}
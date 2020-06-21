using System.ComponentModel;

namespace PacMan.GameComponents
{
    public interface IGameModel
    {
        bool OnePlayerGamePossible { get; }
        bool TwoPlayerGamePossible { get; }
        bool ShouldShowCoinGrid { get; }
        string CanContinueIn { get; }
        string PauseResumeImage { get; }
        bool ShouldShowDirectionGrid { get; }
        bool Paused { get; set; }
        string PurchaseLicenseResult { get; set; }
        bool CanContinueFromLicenseScreen { get; }
        event PropertyChangedEventHandler PropertyChanged;
    }
}
namespace DuelGame
{
    public interface IAnalyticService
    {
        public void LogBattleStarted();
        public void LogBattleFinished(int arrowsCount, int swordSwingsCount, int wizardFireCastCount);
        public void LogArrowFired();
        public void LogSwordSwung();
        public void LogWizardFireCasted();
    }
}
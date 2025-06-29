using System.Collections.Generic;
using Firebase.Analytics;
using UnityEngine;
using UnityEngine.Rendering.UI;

namespace DuelGame
{
    public class AnalyticService
    {
        private static string BATTLE_STARTED = "battle_started";
        private static string BATTLE_FINISHED = "battle_finished";
        
        private static string ARROW_FIRED = "arrow_fired";
        private static string SWORD_SWUNG = "sword_swung";
        private static string WIZARD_FIRE_CASTED = "wizard_fire_casted";
        
        public void LogBattleStarted()
        {
            Debug.Log("Battle Started");
            FirebaseAnalytics.LogEvent(BATTLE_STARTED);
        }

        public void LogBattleFinished(int arrowsCount, int swordSwingsCount, int wizardFireCastCount)
        {
            Debug.Log("Battle finished:\n" +
                      $"\tArrows count:  {arrowsCount}" +
                      $"\tSword swings count: {swordSwingsCount}" +
                      $"\tWizard fire cast count: {wizardFireCastCount}");
            
            FirebaseAnalytics.LogEvent(
                BATTLE_FINISHED, 
                new Parameter("Arrows count", arrowsCount),
                new Parameter("Sword swings count", swordSwingsCount),
                new Parameter("Wizard fire cast count", wizardFireCastCount));
        }

        public void LogArrowFired()
        {
            Debug.Log("Arrow Fired");
            FirebaseAnalytics.LogEvent(ARROW_FIRED);
        }

        public void LogSwordSwung()
        {
            Debug.Log("Sword swung");
            FirebaseAnalytics.LogEvent(SWORD_SWUNG);
        }

        public void LogWizardFireCasted()
        {
            Debug.Log("Wizard fire casted");
            FirebaseAnalytics.LogEvent(WIZARD_FIRE_CASTED);
        }
    }
}
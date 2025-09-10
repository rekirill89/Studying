using System;

namespace DuelGame
{
    public class SceneState
    {
        public event Action OnSceneChanged;
        public SceneEnum CurrentScene { get; private set; } = SceneEnum.None;
        public SceneEnum PreviousScene { get; private set; } = SceneEnum.None;

        public void ChangeScene(SceneEnum newScene)
        {
            PreviousScene = CurrentScene;
            CurrentScene = newScene;
            
            OnSceneChanged?.Invoke();
        }
    }

    public enum SceneEnum
    {
        MenuScene,
        BattleScene,
        None
    }
}
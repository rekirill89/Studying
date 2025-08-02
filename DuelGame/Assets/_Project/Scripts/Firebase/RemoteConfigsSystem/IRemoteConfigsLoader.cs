namespace DuelGame
{
    public delegate void RemoteConfigsApplied(GameLocalConfigs localConfigs, BuffsRemoteConfigs buffsRemoteConfigs);
    
    public interface IRemoteConfigsLoader
    {
        public event RemoteConfigsApplied OnRemoteConfigsApplied;
        
        public bool IsSystemReady { get; }

        public void Init();
    }
}
namespace DuelGame
{
    public delegate void RemoteConfigsApplied(GameLocalConfigs localConfigs, BuffsRemoteConfigs buffsRemoteConfigs);
    public interface IRemoteConfigsManager
    {
        //public delegate void RemoteConfigsApplied(GameLocalConfigs localConfigs, BuffsRemoteConfigs buffsRemoteConfigs);
        public event RemoteConfigsApplied OnRemoteConfigsApplied;
        
        public bool IsSystemReady { get; }

        public void Init();
    }
}
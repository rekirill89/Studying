namespace DuelGame
{
    public interface IAuthService
    {
        public bool IsSystemReady { get; }
        
        public void Init();
    }
}
using System;
using Cysharp.Threading.Tasks;

namespace DuelGame
{
    public interface IInternetConnector
    {
        public event Action OnConnected;
        public bool IsConnected { get; }

        public  UniTask CheckInternetConnection();

        public UniTask MonitorInternetConnection();
    }
}
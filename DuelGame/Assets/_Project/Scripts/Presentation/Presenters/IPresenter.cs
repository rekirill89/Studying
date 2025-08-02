using System;

namespace DuelGame
{
    public interface IPresenter<out TView>: IDisposable where TView : BasePanelView
    {
        public void Initialize();
    }
}
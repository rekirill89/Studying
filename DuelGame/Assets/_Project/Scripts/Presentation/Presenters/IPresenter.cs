using System;

namespace DuelGame
{
    public interface IPresenter<out TView>: IDisposable where TView : BaseView
    {
        public void Initialize();
    }
}
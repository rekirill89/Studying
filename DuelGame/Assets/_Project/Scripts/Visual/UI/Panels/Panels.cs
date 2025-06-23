using UnityEngine;
using UnityEngine.Serialization;

namespace DuelGame
{
    public class Panels : MonoBehaviour
    {
        [SerializeField] public StartPanelView StartPanelView;
        [SerializeField] public ContinuePanelView ContinuePanelView;
        [SerializeField] public RestartPanelView RestartPanelView;
        [SerializeField] public ReloadPanelView ReloadPanelView;
        [SerializeField] public SavePanelView SavePanelView;
        [SerializeField] public LoadPanelView LoadPanelView;
    }
}
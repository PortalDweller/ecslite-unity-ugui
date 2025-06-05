using UnityEngine;

namespace Leopotam.EcsLite.Unity.Ugui
{
    public struct EcsUguiSubmitEvent
    {
        public string WidgetName;
        public GameObject Sender;
        public bool used;
    }
}

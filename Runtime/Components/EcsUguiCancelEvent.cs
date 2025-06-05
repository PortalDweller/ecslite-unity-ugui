using UnityEngine;

namespace Leopotam.EcsLite.Unity.Ugui
{
    public struct EcsUguiCancelEvent
    {
        public string WidgetName;
        public GameObject Sender;
        public bool used;
    }
}

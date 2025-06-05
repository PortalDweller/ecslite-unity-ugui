using UnityEngine;
using UnityEngine.EventSystems;

namespace Leopotam.EcsLite.Unity.Ugui
{
    public struct EcsUguiClickOrSubmitEvent
    {
        public string WidgetName;
        public GameObject Sender;
        public Vector2 Position;
        public PointerEventData.InputButton Button;
        public bool used;
    }
}

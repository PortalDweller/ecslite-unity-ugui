using UnityEngine;

namespace Leopotam.EcsLite.Unity.Ugui {
    public struct EcsUguiSelectEvent {
        public string WidgetName;
        public GameObject Sender;
        public GameObject SelectedObject;
        public bool Used;
    }
}
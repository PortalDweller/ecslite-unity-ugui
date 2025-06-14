// ----------------------------------------------------------------------------
// The MIT-Red License
// Copyright (c) 2012-2025 Leopotam <leopotam@yandex.ru>
// ----------------------------------------------------------------------------

using UnityEngine;

namespace Leopotam.EcsLite.Unity.Ugui {
    public struct EcsUguiExitEvent {
        public string WidgetName;
        public GameObject Sender;
        public Vector2 Position;
    }
}
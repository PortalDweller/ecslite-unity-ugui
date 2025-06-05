using UnityEngine.EventSystems;

namespace Leopotam.EcsLite.Unity.Ugui
{
    public sealed class EcsUguiCancelAction : EcsUguiActionBase<EcsUguiCancelEvent>, ICancelHandler
    {
        public void OnCancel(BaseEventData eventData)
        {
            if (IsValidForEvent())
            {
                ref var msg = ref CreateEvent();
                msg.WidgetName = GetWidgetName();
                msg.Sender = eventData.selectedObject;
                msg.used = eventData.used;
            }
        }
    }
}

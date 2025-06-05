using UnityEngine.EventSystems;

namespace Leopotam.EcsLite.Unity.Ugui
{
    public sealed class EcsUguiSelectAction : EcsUguiActionBase<EcsUguiSelectEvent>, ISelectHandler
    {
        public void OnSelect(BaseEventData eventData)
        {
            if (IsValidForEvent())
            {
                ref var msg = ref CreateEvent();
                msg.WidgetName = GetWidgetName();
                msg.Sender = eventData.selectedObject;
                msg.Used = eventData.used;
                msg.SelectedObject = eventData.selectedObject;
            }
        }
    }
}
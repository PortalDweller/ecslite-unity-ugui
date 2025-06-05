using UnityEngine.EventSystems;

namespace Leopotam.EcsLite.Unity.Ugui
{
    public sealed class EcsUguiClickOrSubmitAction : EcsUguiActionBase<EcsUguiClickOrSubmitEvent>, IPointerClickHandler, ISubmitHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            if (IsValidForEvent())
            {
                ref var msg = ref CreateEvent();
                msg.WidgetName = GetWidgetName();
                msg.Sender = gameObject;
                msg.Position = eventData.position;
                msg.Button = eventData.button;
                msg.used = eventData.used;
            }
        }
        public void OnSubmit(BaseEventData eventData)
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
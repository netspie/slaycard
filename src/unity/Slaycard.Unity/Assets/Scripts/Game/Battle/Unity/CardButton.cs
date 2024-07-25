using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Battle.Unity
{
    public class CardButton : MonoBehaviour,
        IPointerDownHandler,
        IPointerUpHandler,
        IDragHandler
    {
        private readonly List<Action<PointerEventData>> _onDragStartHandlers = new();
        private readonly List<Action<PointerEventData>> _onDrag = new();
        private readonly List<Action<PointerEventData>> _onDragEndHandlers = new();

        public void OnDragStart(Action<PointerEventData> action) =>
            _onDragStartHandlers.Add(action);

        public void OnDrag(Action<PointerEventData> action) =>
            _onDrag.Add(action);

        public void OnDragEnd(Action<PointerEventData> action) =>
            _onDragEndHandlers.Add(action);

        void IPointerDownHandler.OnPointerDown(PointerEventData data) =>
            _onDragStartHandlers.ForEach(h => h(data));

        void IPointerUpHandler.OnPointerUp(PointerEventData data) =>
            _onDragEndHandlers.ForEach(h => h(data));

        void IDragHandler.OnDrag(PointerEventData data) =>
            _onDrag.ForEach(h => h(data));
    }
}

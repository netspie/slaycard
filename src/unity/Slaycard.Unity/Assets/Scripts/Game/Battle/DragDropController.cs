using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Battle
{
    public class DragDropController : 
        MonoBehaviour, 
        IPointerDownHandler,
        IPointerUpHandler,
        IDragHandler
    {
        private RectTransform _rt;
        private Canvas _canvas;

        private Vector3? _pos;
        private RectTransform _parent;

        public Action<PointerEventData> OnDragStart;
        public Action<PointerEventData> OnDragging;
        public Action<PointerEventData> OnDragEnd;

        private void Start()
        {
            _rt = GetComponent<RectTransform>();
            _canvas = _rt.GetComponentInParent<Canvas>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _pos = _rt.position;
            
            _parent = _rt.parent.GetComponent<RectTransform>();
            _rt.SetParent(_canvas.transform, true);

            OnDragStart?.Invoke(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _rt.position = _pos ?? _rt.position;
            _pos = null;

            _rt.SetParent(_parent, true);

            OnDragEnd?.Invoke(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            OnDragging?.Invoke(eventData);
            //_rt.anchoredPosition = eventData.position;
            _rt.position = eventData.position;
            Debug.Log("OnDrag");
        }
    }
}

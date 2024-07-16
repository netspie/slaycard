using Game.Battle.UseCases.Queries;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Battle
{
    public class ActionCard : MonoBehaviour,
        IPointerDownHandler,
        IPointerUpHandler,
        IDragHandler
    {
        [SerializeField] private TMP_Text _nameText;

        private RectTransform _rt;

        public RectTransform RT => _rt;

        private void Start()
        {
            _rt = GetComponent<RectTransform>();
        }

        public static ActionCard Instantiate(
            ActionCard prefab,
            Transform parent,
            ActionCardDTO dto)
        {
            var go = Instantiate(prefab, parent);

            go._nameText.text = dto.Name;

            return go;
        }

        private List<Action<ActionCard, PointerEventData>> _onDragStartHandlers = new();
        private List<Action<ActionCard, PointerEventData>> _onDrag = new();
        private List<Action<ActionCard, PointerEventData>> _onDragEndHandlers = new();

        public void OnDragStart(Action<ActionCard, PointerEventData> action) =>
            _onDragStartHandlers.Add(action);

        public void OnDrag(Action<ActionCard, PointerEventData> action) =>
            _onDrag.Add(action);

        public void OnDragEnd(Action<ActionCard, PointerEventData> action) =>
            _onDragEndHandlers.Add(action);

        void IPointerDownHandler.OnPointerDown(PointerEventData data) =>
            _onDragStartHandlers.ForEach(h => h(this, data));

        void IPointerUpHandler.OnPointerUp(PointerEventData data) =>
            _onDragEndHandlers.ForEach(h => h(this, data));

        void IDragHandler.OnDrag(PointerEventData data) =>
            _onDrag.ForEach(h => h(this, data));
    }
}

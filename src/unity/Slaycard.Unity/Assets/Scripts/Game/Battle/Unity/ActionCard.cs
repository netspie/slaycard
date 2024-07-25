using Game.Battle.Unity;
using Game.Battle.UseCases.Queries;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Unity.Battle
{
    public class ActionCard : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private CardButton _button;

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

        public void OnDragStart(Action<ActionCard, PointerEventData> action) =>
            _button.OnDragStart(data => action(this, data));

        public void OnDrag(Action<ActionCard, PointerEventData> action) =>
            _button.OnDrag(data => action(this, data));

        public void OnDragEnd(Action<ActionCard, PointerEventData> action) =>
            _button.OnDragEnd(data => action(this, data));
    }
}

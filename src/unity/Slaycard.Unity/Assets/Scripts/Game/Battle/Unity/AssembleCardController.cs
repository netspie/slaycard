﻿using Core.Collections;
using Core.Unity.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Unity.Battle
{
    public class AssembleCardController
    {
        private readonly Transform _dragArea;
        private readonly Transform _assemblyField;
        private readonly Transform _cardPhantomPrefab;
        private readonly ActionCard[] _cards;

        private int _originalIndex;
        private Vector3 _originalPos;
        private Transform _originalParent;

        private List<GameObject> _temporaryObjects = new();

        private RectTransform _assemblyPhantomCard;
        private bool _isCardSnap;

        public AssembleCardController(
            Transform dragArea, 
            ActionCard[] cards,
            Transform cardPhantomPrefab,
            Transform assemblyField)
        {
            _dragArea = dragArea;
            _cardPhantomPrefab = cardPhantomPrefab;
            _assemblyField = assemblyField;

            cards.ForEach(card =>
            {
                card.OnDragStart(OnCardDragStart);
                card.OnDragEnd(OnCardDragEnd);
                card.OnDrag(OnCardDrag);
            });
        }

        private void OnCardDragStart(ActionCard card, PointerEventData data)
        {
            _originalPos = card.transform.position;
            _originalParent = card.transform.parent.GetComponent<Transform>();
            _originalIndex = card.transform.GetSiblingIndex();

            var cardPhantom = Object.Instantiate(_cardPhantomPrefab, _originalParent);
            cardPhantom.SetSiblingIndex(_originalIndex);
            Object.Destroy(cardPhantom.GetComponent<Image>());
            _temporaryObjects.Add(cardPhantom.gameObject);

            _assemblyPhantomCard = Object.Instantiate(_cardPhantomPrefab, _assemblyField).GetComponent<RectTransform>();
            _temporaryObjects.Add(_assemblyPhantomCard.gameObject);

            card.transform.SetParent(_dragArea, true);
        }

        private void OnCardDragEnd(ActionCard card, PointerEventData data)
        {
            if (_isCardSnap)
            {
                Object.Destroy(_assemblyPhantomCard.gameObject);
            }
            else
            {
                card.transform.position = _originalPos;
                card.transform.SetParent(_originalParent, true);
                card.transform.SetSiblingIndex(_originalIndex);

                _temporaryObjects.ForEach(Object.Destroy);
            }

            _isCardSnap = false;
        }

        private void OnCardDrag(ActionCard card, PointerEventData data)
        {
            if (data.position.IsInRect(_assemblyPhantomCard))
            {
                card.transform.position = _assemblyPhantomCard.position;
                _isCardSnap = true;
            }
            else
            {
                card.transform.position = data.position;
                _isCardSnap = false;
            }
        }
    }
}

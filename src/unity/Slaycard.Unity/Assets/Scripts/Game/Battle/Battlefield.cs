using Core.Collections;
using Game.Battle.UseCases.Queries;
using System.Linq;
using UnityEngine;

namespace Game.Battle
{
    public class Battlefield : MonoBehaviour
    {
        [SerializeField] private Transform _myRow;
        [SerializeField] private Transform _enemyRow;
        [SerializeField] private Transform _assemblyFieldPanel;
        [SerializeField] private Transform _actionCardsPanel;

        [SerializeField] private CharacterCard _characterCardPrefab;
        [SerializeField] private ActionCard _actionCardPrefab;

        public static Battlefield Instantiate(
            Battlefield prefab,
            Transform parent,
            BattleDTO dto)
        {
            var go = GameObject.Instantiate(prefab, parent);

            dto.Players.ForEach((player, i) =>
            {
                if (i == 0)
                    InstantiateCharacterCards(player.Characters, go._myRow);
                if (i == 1)
                    InstantiateCharacterCards(player.Characters, go._enemyRow);
            });

            if (dto.CurrentTurnPlayerId == dto.Players.First().Id)
            {
                dto.Players[0].ActionCards.ForEach(dto =>
                {
                    ActionCard.Instantiate(
                        go._actionCardPrefab,
                        go._actionCardsPanel,
                        dto);
                });
            }

            return go;

            void InstantiateCharacterCards(CharacterCardDTO[] dtos, Transform parent) =>
                dtos.ForEach(dto =>
                {
                    var characterCard = CharacterCard.Instantiate(
                        go._characterCardPrefab,
                        parent,
                        dto);
                });
        }
    }
}

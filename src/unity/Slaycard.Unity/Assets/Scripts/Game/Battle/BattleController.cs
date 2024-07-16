using Game.Battle.UseCases.Queries;
using UnityEngine;

namespace Game.Battle
{
    public class BattleController : MonoBehaviour
    {
        [SerializeField] private Battlefield _battlefieldPrefab;

        private void Start()
        {
            var canvas = FindObjectOfType<Canvas>();
            Battlefield.Instantiate(_battlefieldPrefab, canvas.transform, BattleDTO.Battlefield);
        }

        private static readonly GetBattleQueryResponse BattleDTO = new(
            new BattleDTO(
                Id: "battle-1",
                CurrentTurnPlayerId: "player-1",
                Players: 
                    new[]
                    {
                        new PlayerDTO(
                            Id: "player-1",
                            Characters: new[]
                            {
                                new CharacterCardDTO(
                                    "character-1-1", "Miki"),
                                new CharacterCardDTO(
                                    "character-1-2", "Koko"),
                                new CharacterCardDTO(
                                    "character-1-3", "Kiri"),
                            },
                            ActionCards: new[]
                            {
                                new ActionCardDTO("orb-1", "Orb", ""),
                                new ActionCardDTO("orb-1", "Magic Missile", ""),
                                new ActionCardDTO("orb-1", "Attack", ""),
                                new ActionCardDTO("orb-1", "Potion", ""),
                                new ActionCardDTO("orb-1", "Potion", ""),
                                new ActionCardDTO("orb-1", "Attack", ""),
                                new ActionCardDTO("orb-1", "Magic Missile", ""),
                                new ActionCardDTO("orb-1", "Orb", ""),
                            }),
                        new PlayerDTO(
                            Id: "player-2",
                            Characters: new[]
                            {
                                new CharacterCardDTO(
                                    "character-2-1", "Shachi"),
                                new CharacterCardDTO(
                                    "character-2-2", "Aiko"),
                                new CharacterCardDTO(
                                    "character-2-3", "Himiko"),
                            })
                    }));
    }
}

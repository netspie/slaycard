using Game.Battle.UseCases.Queries;
using UnityEngine;

namespace Game.Battle
{
    public class BattleController : MonoBehaviour
    {
        [SerializeField] private Battlefield _boardPrefab;

        private void Start()
        {
            Battlefield.Instantiate(_boardPrefab);
        }

        private static GetBattleQueryResponse BattleDTO = new(
            new BattlefieldDTO(
                Players: 
                    new[]
                    {
                        new PlayerDTO(
                            Characters: new[]
                            {
                                new CharacterDTO(),
                            },
                            ActionCards: new[]
                            {
                                new ActionCardDTO(),
                            })
                    },
                AssemblyField:
                    new AssemblyFieldDTO(
                        PlayerId: "",
                        null)));
    }
}

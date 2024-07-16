using Game.Battle.UseCases.Queries;
using UnityEngine;

namespace Game.Battle
{
    public class Battlefield : MonoBehaviour
    {
        [SerializeField] private RectTransform _myRow;
        [SerializeField] private RectTransform _enemyRow;
        [SerializeField] private RectTransform _assemblyFieldPanel;
        [SerializeField] private RectTransform _actionCardsPanel;

        private void Start()
        {

        }

        public static Battlefield Instantiate(
            Battlefield prefab,
            BattlefieldDTO dto)
        {
            var board = GameObject.Instantiate(prefab);

            return board;
        }
    }
}

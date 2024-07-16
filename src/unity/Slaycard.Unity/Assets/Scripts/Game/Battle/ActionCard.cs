using Game.Battle.UseCases.Queries;
using TMPro;
using UnityEngine;

namespace Game.Battle
{
    public class ActionCard : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nameText;

        public static ActionCard Instantiate(
            ActionCard prefab,
            Transform parent,
            ActionCardDTO dto)
        {
            var go = Instantiate(prefab, parent);

            go._nameText.text = dto.Name;

            return go;
        }
    }
}

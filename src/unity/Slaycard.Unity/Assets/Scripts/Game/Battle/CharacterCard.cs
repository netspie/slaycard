using Game.Battle.UseCases.Queries;
using TMPro;
using UnityEngine;

namespace Game.Battle
{
    public class CharacterCard : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nameText;

        public static CharacterCard Instantiate(
            CharacterCard prefab,
            Transform parent,
            CharacterCardDTO dto)
        {
            var go = Instantiate(prefab, parent);

            go._nameText.text = dto.Name;

            return go;
        }
    }
}

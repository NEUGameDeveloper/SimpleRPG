// Animancer // Copyright 2020 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Animancer.Examples.AnimatorControllers.GameKit
{
    /// <summary>
    /// A centralised group of references to the common parts of a creature and a state machine for their actions.
    /// </summary>
    [AddComponentMenu(Strings.ExamplesMenuPrefix + "Game Kit - Character Selector")]
    [HelpURL(Strings.ExampleAPIDocumentationURL + nameof(AnimatorControllers) + "." + nameof(GameKit) + "/" + nameof(CharacterSelector))]
    public sealed class CharacterSelector : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField] private Text _Text;
        [SerializeField] private GameObject[] _Characters;

        /************************************************************************************************************************/

        private void Awake()
        {
            SelectCharacter(0);
        }

        /************************************************************************************************************************/

        private void Update()
        {
            for (int i = 0; i < _Characters.Length; i++)
            {
                var key = KeyCode.Alpha1 + i;
                if (Input.GetKeyUp(key))
                    SelectCharacter(i);
            }
        }

        /************************************************************************************************************************/

        private void SelectCharacter(int index)
        {
            var text = ObjectPool.AcquireStringBuilder();

            for (int i = 0; i < _Characters.Length; i++)
            {
                var active = i == index;
                _Characters[i].SetActive(active);

                if (i > 0)
                    text.AppendLine();

                if (active)
                    text.Append("<b>");

                text.Append(1 + i)
                    .Append(" = ")
                    .Append(_Characters[i].name);

                if (active)
                    text.Append("</b>");
            }

            _Text.text = text.ReleaseToString();
        }

        /************************************************************************************************************************/
    }
}

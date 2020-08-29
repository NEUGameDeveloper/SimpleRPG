// Animancer // Copyright 2020 Kybernetik //

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

namespace Animancer.Examples.StateMachines.Weapons
{
    /// <summary>
    /// Holds various animations relating to the use of a weapon. In a real game, this class might have other details
    /// like damage, damage type, weapon category, etc. It could also inherit from a base Item class for things like
    /// weight, cost, and description.
    /// </summary>
    [AddComponentMenu(Strings.ExamplesMenuPrefix + "Weapons - Weapon")]
    [HelpURL(Strings.ExampleAPIDocumentationURL + nameof(StateMachines) + "." + nameof(Weapons) + "/" + nameof(Weapon))]
    public sealed class Weapon : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField]
        private ClipState.Transition[] _AttackAnimations;
        public ClipState.Transition[] AttackAnimations => _AttackAnimations;

        /************************************************************************************************************************/

        [SerializeField]
        private ClipState.Transition _EquipAnimation;
        public ClipState.Transition EquipAnimation => _EquipAnimation;

        [SerializeField]
        private ClipState.Transition _UnequipAnimation;
        public ClipState.Transition UnequipAnimation => _UnequipAnimation;

        /************************************************************************************************************************/
    }
}

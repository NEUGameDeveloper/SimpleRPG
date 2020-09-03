// Animancer // Copyright 2020 Kybernetik //

using UnityEngine;

namespace Animancer
{
    /// <summary>
    /// A <see cref="ScriptableObject"/> which holds a <see cref="Float1ControllerState.Transition"/>.
    /// </summary>
    [CreateAssetMenu(menuName = Strings.MenuPrefix + "Controller Transition/Float 1", order = Strings.AssetMenuOrder + 5)]
    [HelpURL(Strings.APIDocumentationURL + "/" + nameof(Float1ControllerTransition))]
    public class Float1ControllerTransition : AnimancerTransition<Float1ControllerState.Transition> { }
}

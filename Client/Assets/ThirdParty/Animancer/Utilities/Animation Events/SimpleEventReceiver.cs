// Animancer // Copyright 2020 Kybernetik //

using UnityEngine;
using UnityEngine.Serialization;

namespace Animancer
{
    /// <summary>
    /// A component which uses Animation Events with the Function Name "Event" to trigger a callback.
    /// <para></para>
    /// This component must always be attached to the same <see cref="GameObject"/> as the <see cref="Animator"/> in
    /// order to receive Animation Events from it.
    /// </summary>
    [AddComponentMenu(Strings.MenuPrefix + "Simple Event Receiver")]
    [HelpURL(Strings.APIDocumentationURL + "/" + nameof(SimpleEventReceiver))]
    public class SimpleEventReceiver : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField, FormerlySerializedAs("onEvent")]
        private AnimationEventReceiver _OnEvent;

        /// <summary>[<see cref="SerializeField"/>] A callback for Animation Events with the Function Name "Event".</summary>
        public ref AnimationEventReceiver OnEvent => ref _OnEvent;

        /************************************************************************************************************************/

        /// <summary>Called by Animation Events with the Function Name "Event".</summary>
        private void Event(AnimationEvent animationEvent)
        {
            _OnEvent.SetFunctionName(nameof(Event));
            _OnEvent.HandleEvent(animationEvent);
        }

        /************************************************************************************************************************/
    }
}

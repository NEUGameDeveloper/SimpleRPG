// Animancer // Copyright 2020 Kybernetik //

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Object = UnityEngine.Object;

namespace Animancer
{
    /// <summary>
    /// Bitwise flags used by <see cref="Validate.IsEnabled"/> and <see cref="Validate.Disable"/> to determine which
    /// warnings Animancer should give. All warnings are enabled by default, but are entirely compiled out of runtime
    /// builds (except development builds).
    /// </summary>
    [Flags]
    public enum WarningType
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member.

        ProOnly = 1 << 0,
        CreateGraphWhileDisabled = 1 << 1,
        CreateGraphDuringGuiEvent = 1 << 2,
        DuplicateEvent = 1 << 3,
        EndEventInterrupt = 1 << 4,
        UnsupportedEvents = 1 << 5,
        UnsupportedIK = 1 << 6,
        MixerMinChildren = 1 << 7,
        CustomFadeBounds = 1 << 8,
        CustomFadeNotNull = 1 << 9,
        AnimatorSpeed = 1 << 10,

        All = ~0,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member.
    }

    /************************************************************************************************************************/

    /// <summary>
    /// Enforces various rules throughout the system, most of which are compiled out if UNITY_ASSERTIONS is not defined
    /// (by default, it is only defined in the Unity Editor and in Development Builds).
    /// </summary>
    public static class Validate
    {
        /************************************************************************************************************************/
        #region Warnings
        /************************************************************************************************************************/

#if UNITY_ASSERTIONS
        private static WarningType _DisabledWarnings;
#endif

        /************************************************************************************************************************/

        /// <summary>[Assert-Conditional] Disables the specified warning type. Supports bitwise combinations.</summary>
        /// <example>
        /// You can put the following method in any class to have it disable all warnings on startup:
        /// <code>
        /// #if UNITY_ASSERTIONS
        ///     [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.BeforeSceneLoad)]
        ///     private static void DisableAnimancerWarnings()
        ///     {
        ///         Animancer.WarningType.All.Disable();
        ///     }
        /// #endif
        /// </code></example>
        [System.Diagnostics.Conditional(Strings.Assertions)]
        public static void Disable(this WarningType type)
        {
#if UNITY_ASSERTIONS
            _DisabledWarnings |= type;
#endif
        }

        /************************************************************************************************************************/

        /// <summary>[Assert-Conditional] Re-enables the specified warning type. Supports bitwise combinations.</summary>
        [System.Diagnostics.Conditional(Strings.Assertions)]
        public static void Enable(this WarningType type)
        {
#if UNITY_ASSERTIONS
            _DisabledWarnings &= ~type;
#endif
        }

        /************************************************************************************************************************/

        /// <summary>[Assert-Conditional] Enables or disables the specified warning type. Supports bitwise combinations.</summary>
        [System.Diagnostics.Conditional(Strings.Assertions)]
        public static void SetEnabled(this WarningType type, bool enable)
        {
#if UNITY_ASSERTIONS
            if (enable)
                type.Enable();
            else
                type.Disable();
#endif
        }

        /************************************************************************************************************************/

        /// <summary>[Assert-Conditional] Logs the `message` as a warning if the `type` is enabled.</summary>
        [System.Diagnostics.Conditional(Strings.Assertions)]
        public static void Log(this WarningType type, string message, object context = null)
        {
#if UNITY_ASSERTIONS
            if (message == null || type.IsDisabled())
                return;

            Debug.LogWarning($"{message}\nThis message can be disabled by calling " +
                $"{nameof(Animancer)}.{nameof(WarningType)}.{type}.{nameof(Disable)}()" +
                " and it will automatically be compiled out of Runtime Builds (except for Development Builds).",
                context as Object);
#endif
        }

        /************************************************************************************************************************/
#if UNITY_ASSERTIONS
        /************************************************************************************************************************/

        /// <summary>[Assert-Only] Returns true if none of the specified warning types have been disabled.</summary>
        public static bool IsEnabled(this WarningType type)
        {
            return (_DisabledWarnings & type) == 0;
        }

        /************************************************************************************************************************/

        /// <summary>[Assert-Only] Returns true if all of the specified warning types are disabled.</summary>
        public static bool IsDisabled(this WarningType type)
        {
            return (_DisabledWarnings & type) != 0;
        }

        /************************************************************************************************************************/

        /// <summary>[Assert-Only] Disables the specified warnings and returns those that were previously enabled.</summary>
        /// <example><code>
        /// var warnings = WarningType.All.DisableTemporarily();
        /// // Do stuff.
        /// warnings.Enable();
        /// </code></example>
        public static WarningType DisableTemporarily(this WarningType type)
        {
            var previous = type;
            type.Disable();
            return previous & type;
        }

        /************************************************************************************************************************/
#endif
        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/

        /// <summary>[Assert-Conditional] Throws if the `clip` is marked as <see cref="AnimationClip.legacy"/>.</summary>
        /// <exception cref="ArgumentException"/>
        [System.Diagnostics.Conditional(Strings.Assertions)]
        public static void AssertNotLegacy(AnimationClip clip)
        {
#if UNITY_ASSERTIONS
            if (clip.legacy)
                throw new ArgumentException($"Legacy clip '{clip.name}' cannot be used by Animancer." +
                    " Set the legacy property to false before using this clip." +
                    " If it was imported as part of a model then the model's Rig type must be changed to Humanoid or Generic." +
                    " Otherwise you can use the 'Toggle Legacy' function in the clip's context menu" +
                    " (via the cog icon in the top right of its Inspector).");
#endif
        }

        /************************************************************************************************************************/

        /// <summary>[Assert-Conditional] Throws if the <see cref="AnimancerNode.Root"/> is not the `root`.</summary>
        /// <exception cref="ArgumentException"/>
        [System.Diagnostics.Conditional(Strings.Assertions)]
        public static void AssertRoot(AnimancerNode node, AnimancerPlayable root)
        {
#if UNITY_ASSERTIONS
            if (node.Root != root)
                throw new ArgumentException($"{nameof(AnimancerNode)}.{nameof(AnimancerNode.Root)} mismatch:" +
                    $" cannot use a node in an {nameof(AnimancerPlayable)} that is not its {nameof(AnimancerNode.Root)}: " +
                    node.GetDescription());
#endif
        }

        /************************************************************************************************************************/

        /// <summary>[Assert-Conditional] Throws if the state's <see cref="Playable"/> is invalid.</summary>
        /// <exception cref="InvalidOperationException"/>
        [System.Diagnostics.Conditional(Strings.Assertions)]
        public static void AssertPlayable(AnimancerNode node)
        {
#if UNITY_ASSERTIONS
            if (node._Playable.IsValid())
                return;

            if (node.Root == null)
                throw new InvalidOperationException($"{nameof(AnimancerNode)}.{nameof(AnimancerNode.Root)} hasn't been set so it's" +
                    $" {nameof(Playable)} hasn't been created. It can be set by playing the state" +
                    $" or calling {nameof(AnimancerState.SetRoot)} on it directly." +
                    $" {nameof(AnimancerState.SetParent)} would also work if the parent has a {nameof(AnimancerNode.Root)}." +
                    $"\nState: {node}");
            else
                throw new InvalidOperationException($"{nameof(AnimancerNode)}.{nameof(IPlayableWrapper.Playable)} has not been created." +
                    $" {nameof(AnimancerNode.CreatePlayable)} likely needs to be called on it before performing this operation." +
                    $"\nState: {node}");
#endif
        }

        /************************************************************************************************************************/

        /// <summary>[Assert-Conditional]
        /// Throws if the `state` was not actually assigned to its specified <see cref="AnimancerNode.Index"/> in
        /// the `states`.
        /// </summary>
        /// <exception cref="InvalidOperationException"/>
        /// <exception cref="IndexOutOfRangeException">
        /// The <see cref="AnimancerNode.Index"/> is larger than the number of `states`.
        /// </exception>
        [System.Diagnostics.Conditional(Strings.Assertions)]
        public static void AssertCanRemoveChild(AnimancerState state, IList<AnimancerState> states)
        {
#if UNITY_ASSERTIONS
            var index = state.Index;

            if (index < 0)
                throw new InvalidOperationException(
                    "Cannot remove a child state that did not have an Index assigned");

            if (index > states.Count)
                throw new IndexOutOfRangeException(
                    "AnimancerState.Index (" + state.Index + ") is outside the collection of states (count " + states.Count + ")");

            if (states[state.Index] != state)
                throw new InvalidOperationException(
                    "Cannot remove a child state that was not actually connected to its port on " + state.Parent + ":" +
                    "\n    Port: " + state.Index +
                    "\n    Connected Child: " + AnimancerUtilities.ToStringOrNull(states[state.Index]) +
                    "\n    Disconnecting Child: " + AnimancerUtilities.ToStringOrNull(state));
#endif
        }

        /************************************************************************************************************************/
    }
}


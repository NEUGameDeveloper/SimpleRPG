// Animancer // Copyright 2020 Kybernetik //

using System.Collections.Generic;
using UnityEngine;

namespace Animancer
{
    /// <summary>[Pro-Only]
    /// A system which fades animation weights animations using a custom calculation rather than linear interpolation.
    /// </summary>
    /// <example><code>
    /// [SerializeField] private AnimancerComponent _Animancer;
    /// [SerializeField] private AnimationClip _Clip;
    /// 
    /// private void Awake()
    /// {
    ///     // Start fading the animation normally.
    ///     var state = _Animancer.Play(_Clip, 0.25f);
    ///     
    ///     // Then apply the custom fade to modify it.
    ///     CustomFade.Apply(state, Interpolation.Function.SineInOut);
    ///     
    ///     // Or apply it to whatever the current state happens to be.
    ///     CustomFade.Apply(_Animancer, Interpolation.Function.SineInOut);
    ///     
    ///     // Anything else you play after that will automatically cancel the custom fade.
    /// }
    /// </code></example>
    public abstract partial class CustomFade : Key, IUpdatable
    {
        /************************************************************************************************************************/

        private float _Time;
        private float _FadeSpeed;
        private StateWeight _TargetState;
        private AnimancerLayer _TargetLayer;
        private int _CommandCount;

        private readonly List<StateWeight> OtherStates = new List<StateWeight>();

        /************************************************************************************************************************/

        private struct StateWeight
        {
            public AnimancerState state;
            public float startingWeight;

            public StateWeight(AnimancerState state)
            {
                this.state = state;
                startingWeight = state.Weight;
            }
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Gather the current details of the <see cref="AnimancerPlayable"/> and register this
        /// <see cref="CustomFade"/> to be updated by it so that it can replace the regular fade behaviour.
        /// </summary>
        protected void Apply(AnimancerPlayable animancer) => Apply(animancer.States.Current);

        /// <summary>
        /// Gather the current details of the <see cref="AnimancerNode.Root"/> and register this
        /// <see cref="CustomFade"/> to be updated by it so that it can replace the regular fade behaviour.
        /// </summary>
        protected void Apply(AnimancerState state)
        {
#if UNITY_ASSERTIONS
            Debug.Assert(state != null, "State is null.");
            Debug.Assert(state.IsValid, "State is not valid.");
            Debug.Assert(state.IsPlaying, "State is not playing.");
            Debug.Assert(state.LayerIndex >= 0, "State is not connected to a layer.");
            Debug.Assert(state.TargetWeight > 0, "State is not fading in.");

            var animancer = state.Root;
            Debug.Assert(animancer != null, $"{nameof(state)}.{nameof(state.Root)} is null.");

            if (WarningType.CustomFadeBounds.IsEnabled())
            {
                if (CalculateWeight(0) != 0)
                    WarningType.CustomFadeBounds.Log("CalculateWeight(0) != 0.", animancer.Component);
                if (CalculateWeight(1) != 1)
                    WarningType.CustomFadeBounds.Log("CalculateWeight(1) != 1.", animancer.Component);
            }
#endif

            _FadeSpeed = state.FadeSpeed;
            if (_FadeSpeed == 0)
                return;

            _Time = 0;
            _TargetState = new StateWeight(state);
            _TargetLayer = state.Layer;
            _CommandCount = _TargetLayer.CommandCount;

            OtherStates.Clear();
            for (int i = _TargetLayer.ChildCount - 1; i >= 0; i--)
            {
                var other = _TargetLayer.GetChild(i);
                other.FadeSpeed = 0;
                if (other != state && other.Weight != 0)
                    OtherStates.Add(new StateWeight(other));
            }

            state.Root.RequireUpdate(this);
        }

        /************************************************************************************************************************/

        protected abstract float CalculateWeight(float progress);
        protected abstract void Release();

        /************************************************************************************************************************/

        void IUpdatable.EarlyUpdate()
        {
            // Stop fading if the state was destroyed or something else was played.
            if (!_TargetState.state.IsValid() ||
                _TargetLayer != _TargetState.state.Layer ||
                _CommandCount != _TargetLayer.CommandCount)
            {
                OtherStates.Clear();
                AnimancerPlayable.Current.CancelUpdate(this);
                Release();
                return;
            }

            _Time += AnimancerPlayable.DeltaTime * _TargetLayer.Speed * _FadeSpeed;

            if (_Time < 1)// Fade.
            {
                var weight = CalculateWeight(_Time);

                _TargetState.state.Weight = _TargetState.startingWeight + (1 - _TargetState.startingWeight) * weight;

                weight = 1 - weight;
                for (int i = OtherStates.Count - 1; i >= 0; i--)
                {
                    var other = OtherStates[i];
                    other.state.Weight = other.startingWeight * weight;
                }
            }
            else// End.
            {
                _Time = 1;
                _TargetState.state.Weight = 1;

                for (int i = OtherStates.Count - 1; i >= 0; i--)
                    OtherStates[i].state.Stop();

                OtherStates.Clear();
                AnimancerPlayable.Current.CancelUpdate(this);
                Release();
            }
        }

        /************************************************************************************************************************/

        void IUpdatable.LateUpdate() { }

        void IUpdatable.OnDestroy() { }

        /************************************************************************************************************************/
    }
}

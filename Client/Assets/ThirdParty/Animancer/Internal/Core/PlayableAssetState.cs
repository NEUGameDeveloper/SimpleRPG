// Animancer // Copyright 2020 Kybernetik //

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Audio;
using UnityEngine.Playables;
using Object = UnityEngine.Object;

namespace Animancer
{
    /// <summary>[Pro-Only]
    /// An <see cref="AnimancerState"/> which plays a <see cref="PlayableAsset"/>.
    /// </summary>
    public sealed class PlayableAssetState : AnimancerState
    {
        /************************************************************************************************************************/
        #region Fields and Properties
        /************************************************************************************************************************/

        /// <summary>The <see cref="PlayableAsset"/> which this state plays.</summary>
        private PlayableAsset _Asset;

        /// <summary>The <see cref="PlayableAsset"/> which this state plays.</summary>
        public PlayableAsset Asset
        {
            get => _Asset;
            set => ChangeMainObject(ref _Asset, value);
        }

        /// <summary>The <see cref="PlayableAsset"/> which this state plays.</summary>
        public override Object MainObject
        {
            get => _Asset;
            set => _Asset = (PlayableAsset)value;
        }

        /************************************************************************************************************************/

        private float _Length;

        /// <summary>The <see cref="PlayableAsset.duration"/>.</summary>
        public override float Length => _Length;

        /************************************************************************************************************************/

        /// <summary>IK cannot be dynamically enabled on a <see cref="PlayableAssetState"/>.</summary>
        public override bool ApplyAnimatorIK
        {
            get => false;
            set
            {
                if (value)
                    WarningType.UnsupportedIK.Log(
                        $"IK cannot be dynamically enabled on a {nameof(PlayableAssetState)}.", Root?.Component);
            }
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Methods
        /************************************************************************************************************************/

        /// <summary>Constructs a new <see cref="PlayableAssetState"/> to play the `asset`.</summary>
        /// <exception cref="ArgumentNullException">The `asset` is null.</exception>
        public PlayableAssetState(PlayableAsset asset)
        {
            if (asset == null)
                throw new ArgumentNullException(nameof(asset));

            _Asset = asset;
        }

        /************************************************************************************************************************/

        /// <summary>Creates and assigns the <see cref="Playable"/> managed by this node.</summary>
        protected override void CreatePlayable(out Playable playable)
        {
            var root = Root;
            playable = _Asset.CreatePlayable(root._Graph, root.Component.gameObject);
            _Length = (float)_Asset.duration;
            InitialiseBindings(root);
        }

        /************************************************************************************************************************/

        private IList<Object> _Bindings;
        private bool _HasInitialisedBindings;

        /// <summary>The objects controlled by each Timeline Track.</summary>
        public IList<Object> Bindings
        {
            get => _Bindings;
            set
            {
                _Bindings = value;
                _HasInitialisedBindings = false;
                InitialiseBindings(Root);
            }
        }

        /// <summary>Sets the <see cref="Bindings"/>.</summary>
        public void SetBindings(params Object[] bindings)
        {
            Bindings = bindings;
        }

        private void InitialiseBindings(AnimancerPlayable root)
        {
            if (_HasInitialisedBindings || _Bindings == null || root == null)
                return;

            _HasInitialisedBindings = true;

            var bindingCount = _Bindings.Count;
            if (bindingCount == 0)
                return;

            var output = _Asset.outputs.GetEnumerator();
            var graph = root._Graph;

            for (int i = 0; i < bindingCount; i++)
            {
                if (!output.MoveNext())
                    return;

                var binding = _Bindings[i];
                if (binding == null)
                    continue;

                var name = output.Current.streamName;
                var type = output.Current.outputTargetType;

#if UNITY_ASSERTIONS
                if (type != null && !type.IsAssignableFrom(binding.GetType()))
                {
                    Debug.LogError(
                        $"Binding Type Mismatch: bindings[{i}] is '{binding}' but should be a {type.FullName} for {name}",
                        Root?.Component as Object);
                    continue;
                }

                Validate.AssertPlayable(this);
#endif

                var playable = _Playable.GetInput(i);

                if (type == typeof(Animator))
                {
                    var playableOutput = AnimationPlayableOutput.Create(graph, name, (Animator)binding);
                    playableOutput.SetSourcePlayable(playable);
                }
                else if (type == typeof(AudioSource))
                {
                    var playableOutput = AudioPlayableOutput.Create(graph, name, (AudioSource)binding);
                    playableOutput.SetSourcePlayable(playable);
                }
                else// ActivationTrack, SignalTrack, ControlTrack, PlayableTrack.
                {
                    var playableOutput = ScriptPlayableOutput.Create(graph, name);
                    playableOutput.SetUserData(binding);
                    playableOutput.SetSourcePlayable(playable);
                }
            }
        }

        /************************************************************************************************************************/

        /// <summary>Destroys the <see cref="Playable"/> and cleans up this state.</summary>
        /// <remarks>
        /// This method is NOT called automatically, so when implementing a custom state type you must use
        /// <see cref="AnimancerPlayable.Disposables"/> if you need to guarantee that things will get cleaned up.
        /// </remarks>
        public override void Destroy()
        {
            _Asset = null;
            base.Destroy();
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
        #region Transition
        /************************************************************************************************************************/

        /// <summary>
        /// A serializable <see cref="ITransition"/> which can create a <see cref="PlayableAssetState"/> when
        /// passed into <see cref="AnimancerPlayable.Play(ITransition)"/>.
        /// </summary>
        /// <remarks>
        /// Unfortunately the tool used to generate this documentation does not currently support nested types with
        /// identical names, so only one <c>Transition</c> class will actually have a documentation page.
        /// </remarks>
        [Serializable]
        public class Transition : Transition<PlayableAssetState>, IAnimationClipCollection
        {
            /************************************************************************************************************************/

            [SerializeField, Tooltip("The asset to play")]
            private PlayableAsset _Asset;

            /// <summary>[<see cref="SerializeField"/>] The asset to play.</summary>
            public ref PlayableAsset Asset => ref _Asset;

            /// <summary>
            /// The <see cref="Asset"/> will be used as the <see cref="AnimancerState.Key"/> for the created state to
            /// be registered with.
            /// </summary>
            public override object Key => _Asset;

            /************************************************************************************************************************/

            [SerializeField, Tooltip(Strings.ProOnlyTag +
                "How fast the animation plays (1x = normal speed, 2x = double speed)")]
            private float _Speed = 1;

            /// <summary>[<see cref="SerializeField"/>]
            /// Determines how fast the animation plays (1x = normal speed, 2x = double speed).
            /// </summary>
            public override float Speed
            {
                get => _Speed;
                set => _Speed = value;
            }

            /************************************************************************************************************************/

            [SerializeField, Tooltip(Strings.ProOnlyTag + "If enabled, the animation's time will start at this value when played")]
            private float _NormalizedStartTime = float.NaN;

            /// <summary>[<see cref="SerializeField"/>]
            /// Determines what <see cref="AnimancerState.NormalizedTime"/> to start the animation at.
            /// <para></para>
            /// The default value is <see cref="float.NaN"/> which indicates that this value is not used so the
            /// animation will continue from its current time.
            /// </summary>
            public override float NormalizedStartTime
            {
                get => _NormalizedStartTime;
                set => _NormalizedStartTime = value;
            }

            /// <summary>
            /// If this transition will set the <see cref="AnimancerState.NormalizedTime"/>, then it needs to use
            /// <see cref="FadeMode.FromStart"/>.
            /// </summary>
            public override FadeMode FadeMode => float.IsNaN(_NormalizedStartTime) ? FadeMode.FixedSpeed : FadeMode.FromStart;

            /************************************************************************************************************************/

            [SerializeField, Tooltip("The objects controlled by each of the tracks in the Timeline Asset")]
            private Object[] _Bindings;

            /// <summary>[<see cref="SerializeField"/>] The objects controlled by each of the tracks in the Timeline Asset.</summary>
            public ref Object[] Bindings => ref _Bindings;

            /************************************************************************************************************************/

            /// <summary>[<see cref="ITransitionDetailed"/>]
            /// The maximum amount of time the animation is expected to take (in seconds).
            /// </summary>
            public override float MaximumDuration => _Asset != null ? (float)_Asset.duration : 0;

            /************************************************************************************************************************/

            /// <summary>Indicates whether this transition can create a valid <see cref="AnimancerState"/>.</summary>
            public override bool IsValid => _Asset != null;

            /************************************************************************************************************************/

            /// <summary>
            /// Creates and returns a new <see cref="PlayableAssetState"/>.
            /// <para></para>
            /// Note that using methods like <see cref="AnimancerPlayable.Play(ITransition)"/> will also call
            /// <see cref="ITransition.Apply"/>, so if you call this method manually you may want to call that method
            /// as well. Or you can just use <see cref="AnimancerUtilities.CreateStateAndApply"/>.
            /// <para></para>
            /// This method also assigns it as the <see cref="AnimancerState.Transition{TState}.State"/>.
            /// </summary>
            public override PlayableAssetState CreateState()
            {
                State = new PlayableAssetState(_Asset);
                State.SetBindings(_Bindings);
                return State;
            }

            /************************************************************************************************************************/

            /// <summary>
            /// Called by <see cref="AnimancerPlayable.Play(ITransition)"/> to apply the <see cref="Speed"/>
            /// and <see cref="NormalizedStartTime"/>.
            /// </summary>
            public override void Apply(AnimancerState state)
            {
                base.Apply(state);

                if (!float.IsNaN(_Speed))
                    state.Speed = _Speed;

                if (!float.IsNaN(_NormalizedStartTime))
                    state.NormalizedTime = _NormalizedStartTime;
                else if (state.Weight == 0)
                    state.NormalizedTime = AnimancerEvent.Sequence.GetDefaultNormalizedStartTime(_Speed);
            }

            /************************************************************************************************************************/

            /// <summary>Gathers all the animations associated with this object.</summary>
            void IAnimationClipCollection.GatherAnimationClips(ICollection<AnimationClip> clips) => clips.GatherFromAsset(_Asset);

            /************************************************************************************************************************/
#if UNITY_EDITOR
            /************************************************************************************************************************/

            /// <summary>[Editor-Only] Draws the Inspector GUI for a <see cref="Transition"/>.</summary>
            [UnityEditor.CustomPropertyDrawer(typeof(Transition), true)]
            public class Drawer : Editor.TransitionDrawer
            {
                /************************************************************************************************************************/

                /// <summary>Constructs a new <see cref="Drawer"/>.</summary>
                public Drawer() : base(nameof(_Asset)) { }

                /************************************************************************************************************************/
            }

            /************************************************************************************************************************/
#endif
            /************************************************************************************************************************/
        }

        /************************************************************************************************************************/
        #endregion
        /************************************************************************************************************************/
    }
}


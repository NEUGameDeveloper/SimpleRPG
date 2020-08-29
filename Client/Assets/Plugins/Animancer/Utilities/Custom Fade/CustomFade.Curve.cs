// Animancer // Copyright 2020 Kybernetik //

using UnityEngine;

namespace Animancer
{
    public partial class CustomFade
    {
        /************************************************************************************************************************/

        /// <summary>Modify the current fade to use the specified `curve` to calculate the weight.</summary>
        /// <example>See <see cref="CustomFade"/>.</example>
        public static void Apply(AnimancerPlayable animancer, AnimationCurve curve)
            => Curve.Acquire(curve).Apply(animancer);

        /// <summary>Modify the current fade to use the specified `curve` to calculate the weight.</summary>
        /// <example>See <see cref="CustomFade"/>.</example>
        public static void Apply(AnimancerState state, AnimationCurve curve)
            => Curve.Acquire(curve).Apply(state);

        /************************************************************************************************************************/

        /// <summary>A <see cref="CustomFade"/> which uses a delegate to calculate the weight.</summary>
        private sealed class Curve : CustomFade
        {
            /************************************************************************************************************************/

            private AnimationCurve _Curve;

            /************************************************************************************************************************/

            public static Curve Acquire(AnimationCurve curve)
            {
                if (curve == null)
                {
                    WarningType.CustomFadeNotNull.Log($"{nameof(curve)} is null.");
                    return null;
                }

                var fade = ObjectPool<Curve>.Acquire();
                fade._Curve = curve;
                return fade;
            }

            /************************************************************************************************************************/

            protected override float CalculateWeight(float progress) => _Curve.Evaluate(progress);

            /************************************************************************************************************************/

            protected override void Release() => ObjectPool<Curve>.Release(this);

            /************************************************************************************************************************/
        }
    }
}

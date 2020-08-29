// Animancer // Copyright 2020 Kybernetik //
// Compare to the original script: https://github.com/Unity-Technologies/animation-jobs-samples/blob/master/Assets/animation-jobs-samples/Samples/Scripts/TwoBoneIK/TwoBoneIK.cs

#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.

using UnityEngine;

namespace Animancer.Examples.Jobs
{
    /// <summary>
    /// An example of how to use Animation Jobs in Animancer to apply simple two bone Inverse Kinematics, even to
    /// Generic Rigs which are not supported by Unity's inbuilt IK system.
    /// </summary>
    /// <remarks>
    /// This example is based on Unity's Animation Jobs Samples: https://github.com/Unity-Technologies/animation-jobs-samples
    /// <list type="bullet">
    /// <item>This script sets up the job in place of https://github.com/Unity-Technologies/animation-jobs-samples/blob/master/Assets/animation-jobs-samples/Samples/Scripts/TwoBoneIK/TwoBoneIK.cs</item>
    /// <item>The <see cref="TwoBoneIKJob"/> script is almost identical to the original from https://github.com/Unity-Technologies/animation-jobs-samples/blob/master/Assets/animation-jobs-samples/Runtime/AnimationJobs/TwoBoneIKJob.cs</item>
    /// </list>
    /// Note that the Animation Rigging package has an IK system which is much better than this example.
    /// </remarks>
    [AddComponentMenu(Strings.ExamplesMenuPrefix + "Jobs - Two Bone IK")]
    [HelpURL(Strings.ExampleAPIDocumentationURL + nameof(Jobs) + "/" + nameof(TwoBoneIK))]
    public class TwoBoneIK : MonoBehaviour
    {
        /************************************************************************************************************************/

        [SerializeField] private AnimancerComponent _Animancer;
        [SerializeField] private Transform _EndBone;
        [SerializeField] private Transform _Target;

        /************************************************************************************************************************/

        private void Awake()
        {
            // Get the bones we want to affect.
            var midBone = _EndBone.parent;
            var topBone = midBone.parent;

            // Create the job and setup its details.
            var twoBoneIKJob = new TwoBoneIKJob();
            twoBoneIKJob.Setup(_Animancer.Animator, topBone, midBone, _EndBone, _Target);

            // Add it to Animancer's output.
            _Animancer.Playable.InsertOutputJob(twoBoneIKJob);
        }

        /************************************************************************************************************************/
    }
}

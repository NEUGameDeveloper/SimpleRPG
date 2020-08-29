// Animancer // Copyright 2020 Kybernetik //

using UnityEngine;

namespace Animancer
{
    /// <summary>Various string constants used throughout Animancer.</summary>
    public static class Strings
    {
        /************************************************************************************************************************/

        /// <summary>The standard prefix for <see cref="CreateAssetMenuAttribute.menuName"/>.</summary>
        public const string MenuPrefix = "Animancer/";

        /// <summary>The standard prefix for <see cref="CreateAssetMenuAttribute.menuName"/>.</summary>
        public const string ExamplesMenuPrefix = "Animancer/Examples/";

        /// <summary>
        /// The base value for <see cref="CreateAssetMenuAttribute.order"/> to group
        /// "Assets/Create/Animancer/..." menu items just under "Avatar Mask".
        /// </summary>
        public const int AssetMenuOrder = 410;

        /************************************************************************************************************************/

        /// <summary>The URL of the website where the Animancer documentation is hosted.</summary>
        public const string DocumentationURL = "https://kybernetik.com.au/animancer";

        /// <summary>The URL of the website where the Animancer API documentation is hosted.</summary>
        public const string APIDocumentationURL = DocumentationURL + "/api/Animancer";

        /// <summary>The URL of the website where the Animancer API documentation is hosted.</summary>
        public const string ExampleAPIDocumentationURL = APIDocumentationURL + ".Examples.";

        /// <summary>The email address which handles support for Animancer.</summary>
        public const string DeveloperEmail = "animancer@kybernetik.com.au";

        /************************************************************************************************************************/

        /// <summary>The conditional compilation symbol for Editor-Only code.</summary>
        public const string UnityEditor = "UNITY_EDITOR";

        /// <summary>The conditional compilation symbol for assertions.</summary>
        public const string Assertions = "UNITY_ASSERTIONS";

        /************************************************************************************************************************/

        /// <summary>4 spaces for indentation.</summary>
        public const string Indent = "    ";

        /// <summary>[Internal]
        /// A prefix for tooltips on Pro-Only features.
        /// <para></para>
        /// "[Pro-Only] " in Animancer Lite or "" in Animancer Pro.
        /// </summary>
        internal const string ProOnlyTag = "";

        /// <summary>[Editor-Only] URLs of various documentation pages.</summary>
        public static class DocsURLs
        {
            /************************************************************************************************************************/
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member.
            /************************************************************************************************************************/
#if UNITY_ASSERTIONS
            /************************************************************************************************************************/

            public const string DocsURL = DocumentationURL + "/docs/";

            public const string SharedEventSequences = DocsURL + "manual/events/animancer#shared-event-sequences";

            /************************************************************************************************************************/
#endif
            /************************************************************************************************************************/
#if UNITY_EDITOR
            /************************************************************************************************************************/

            public const string Examples = DocsURL + "examples";

            public const string UnevenGround = DocsURL + "examples/ik/uneven-ground";

            public const string TheAnimatorControllerField = DocsURL + "manual/animator-controllers#the-animator-controller-field";

            public const string Fading = DocsURL + "manual/blending/fading";

            public const string LayerBlending = DocsURL + "manual/blending/layers#blending";

            public const string EndEvents = DocsURL + "manual/events/end";

            public const string UpdateModes = DocsURL + "bugs/update-modes";

            public const string ChangeLogPrefix = DocsURL + "changes/animancer-";

            /************************************************************************************************************************/
#endif
            /************************************************************************************************************************/
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member.
            /************************************************************************************************************************/
        }

        /************************************************************************************************************************/
    }
}


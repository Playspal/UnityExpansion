using System;

namespace UnityExpansion.UI
{
    /// <summary>
    /// Makes method visible in UiLayout editor.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class UiLayoutMethod : Attribute
    {
        /// <summary>
        /// Group name.
        /// </summary>
        public string Group = UiLayoutAttribute.GROUP_CUSTOM;

        /// <summary>
        /// Order in group.
        /// </summary>
        public int Order = 0;
    }

    /// <summary>
    /// Makes event visible in UiLayout editor.
    /// </summary>
    [AttributeUsage(AttributeTargets.Event)]
    public class UiLayoutEvent : Attribute
    {
        /// <summary>
        /// Group name.
        /// </summary>
        public string Group = UiLayoutAttribute.GROUP_CUSTOM;

        /// <summary>
        /// Order in group.
        /// </summary>
        public int Order = 0;
    }

    public class UiLayoutAttribute : Attribute
    {
        public const string GROUP_MAIN = "Main";
        public const string GROUP_CUSTOM = "Custom methods and events";
    }
}
using System;

namespace UnityExpansion.UI.Layout.Processor
{
    /// <summary>
    /// Makes method visible in UiLayout editor.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class UiLayoutProcessorHandler : Attribute
    {
        /// <summary>
        /// Group name.
        /// </summary>
        public string Group = UiLayoutProcessorAttribute.GROUP_CUSTOM;

        /// <summary>
        /// Methods in editor block sorted by weight.
        /// Bigger weight - lower position.
        /// </summary>
        public int Weight = 0;

        /// <summary>
        /// If true method will be not visible as separated block in editor.
        /// Normally you don't need to change this field.
        /// </summary>
        public bool ExcludeFromLayoutObject = false;
    }

    /// <summary>
    /// Makes event visible in UiLayout editor.
    /// </summary>
    [AttributeUsage(AttributeTargets.Event)]
    public class UiLayoutProcessorEvent : Attribute
    {
        /// <summary>
        /// Group name.
        /// </summary>
        public string Group = UiLayoutProcessorAttribute.GROUP_CUSTOM;

        /// <summary>
        /// Events in editor block sorted by weight.
        /// Bigger weight - lower position.
        /// </summary>
        public int Weight = 0;

        /// <summary>
        /// If true event will be not visible as separated block in editor.
        /// Normally you don't need to change this field.
        /// </summary>
        public bool ExcludeFromLayoutObject = false;
    }

    public class UiLayoutProcessorAttribute : Attribute
    {
        /// <summary>
        /// Main group name.
        /// It's not reccomended to use this group for custom methods.
        /// </summary>
        public const string GROUP_MAIN = "Main";

        /// <summary>
        /// Default group name.
        /// </summary>
        public const string GROUP_CUSTOM = "Custom methods and events";
    }
}
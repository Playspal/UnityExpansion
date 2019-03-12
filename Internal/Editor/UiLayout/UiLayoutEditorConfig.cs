using UnityEngine;

namespace UnityExpansionInternal.UiLayoutEditor
{
    public static class UiLayoutEditorConfig
    {
        // Predefined basic methods and events for some types
        public const string PREDEFINED_METHOD_ANIMATION_PLAY = "Play";
        public const string PREDEFINED_EVENT_ANIMATION_ON_COMPLETE = "OnComplete";

        // Colors
        public static Color ColorNodeLabel = CreateColor("#999999");
        public static Color ColorNodeLabelSpecial = CreateColor("#CCCCCC");

        public static Color ColorNodeBackground = CreateColor("#3A3A3A");
        public static Color ColorNodeBorder = CreateColor("#4D4D4D");

        public static Color ColorElementRootMain = CreateColor("#A73D3F");
        public static Color ColorElementRootDark = CreateColor("#72292A");
        public static Color ColorElementRootLight = CreateColor("#FFCCCC");

        public static Color ColorElementChildMain = CreateColor("#3C52A6");
        public static Color ColorElementChildDark = CreateColor("#283771");
        public static Color ColorElementChildLight = CreateColor("#CCD6FF");

        public static Color ColorElementSystemMain = CreateColor("#186C15");
        public static Color ColorElementSystemDark = CreateColor("#0F490E");
        public static Color ColorElementSystemLight = CreateColor("#B9E3B9");

        public static Color ColorDropDownListItemLabelNormal = CreateColor("#999999");
        public static Color ColorDropDownListItemLabelHover = CreateColor("#FFFFFF");

        private static Color CreateColor(string value)
        {
            Color output = new Color();

            if (ColorUtility.TryParseHtmlString(value, out output))
            {
                return output;
            }

            return Color.red;
        }
    }
}
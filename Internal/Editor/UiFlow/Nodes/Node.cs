using UnityEngine;
using UnityExpansion.Editor;

namespace UnityExpansionInternal.UiFlow
{
    public class Node : EditorLayoutObject
    {
        private const string COLOR_BACKGROUND = "#3A3A3A";
        private const string COLOR_BACKGROUND_BORDER = "#4D4D4D";

        protected EditorLayoutTexture2D _textureBackground;

        public Color ColorMain { get; private set; }
        public Color ColorDark { get; private set; }
        public Color ColorLight { get; private set; }

        public Node(EditorLayout layout, int width, int height) : base (layout, width, height)
        {
            _textureBackground = new EditorLayoutTexture2D(layout, Width, Height);
            _textureBackground.Fill(COLOR_BACKGROUND);
            _textureBackground.DrawBorder(1, COLOR_BACKGROUND_BORDER);
            _textureBackground.SetParent(this);

            SetupColors();
        }

        protected void SetupColors()
        {
            ColorMain = Color.red;
            ColorDark = Color.red;
            ColorLight = Color.red;

            if (this is NodeLayoutElement)
            {
                SetupColors(InternalUiFlowEditorConfig.COLOR_BLOCK_MAIN, InternalUiFlowEditorConfig.COLOR_BLOCK_DARK, InternalUiFlowEditorConfig.COLOR_BLOCK_LIGHT);
            }

            if (this is NodeLayoutElementScreen)
            {
                SetupColors(InternalUiFlowEditorConfig.COLOR_SCREEN_MAIN, InternalUiFlowEditorConfig.COLOR_SCREEN_DARK, InternalUiFlowEditorConfig.COLOR_SCREEN_LIGHT);
            }
        }

        protected void SetupColors(string main, string dark, string light)
        {
            ColorMain = Color.red.Parse(main);
            ColorDark = Color.red.Parse(dark);
            ColorLight = Color.red.Parse(light);
        }

        public override void Render()
        {
            base.Render();
        }
    }
}
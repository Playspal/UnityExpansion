using System.Collections.Generic;

using UnityEngine;
using UnityExpansion.Editor;

namespace UnityExpansionInternal.UiFlow
{
    public class Node : EditorLayoutObject
    {
        protected EditorLayoutObjectTexture _textureBackground;

        public Color ColorMain { get; private set; }
        public Color ColorDark { get; private set; }
        public Color ColorLight { get; private set; }
        public Color ColorBackground { get; private set; }
        public Color ColorBackgroundBorder { get; private set; }

        private List<NodeLink> _links = new List<NodeLink>();

        public Node(EditorLayout layout, int width, int height) : base (layout, width, height)
        {
            ColorBackground = Color.red.Parse(InternalUiFlowEditorConfig.COLOR_NODE_BACKGROUND);
            ColorBackgroundBorder = Color.red.Parse(InternalUiFlowEditorConfig.COLOR_NODE_BACKGROUND_BORDER);

            _textureBackground = new EditorLayoutObjectTexture(layout, Width, Height);
            _textureBackground.Fill(ColorBackground);
            _textureBackground.DrawBorder(1, ColorBackgroundBorder);
            _textureBackground.SetParent(this);

            SetupColors();
        }

        public void Resize(int height)
        {

        }

        public void AddLink(Node node)
        {
            NodeLink link = new NodeLink(Layout, this, node);

            _links.Add(link);

            for (int i = 0; i < _links.Count; i++)
            {
                _links[i].SetPosition(i, _links.Count);
            }
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

            if (this is NodeSignal)
            {
                SetupColors(InternalUiFlowEditorConfig.COLOR_SIGNAL_MAIN, InternalUiFlowEditorConfig.COLOR_SIGNAL_DARK, InternalUiFlowEditorConfig.COLOR_SIGNAL_LIGHT);
            }
        }

        protected void SetupColors(string main, string dark, string light)
        {
            ColorMain = Color.red.Parse(main);
            ColorDark = Color.red.Parse(dark);
            ColorLight = Color.red.Parse(light);
        }

        protected void RenderCurveTo(int toX, int toY)
        {
            int fromX = GetPositionGlobalX() + Width / 2;
            int fromY = GetPositionGlobalY() + Height;

            ((InternalUiFlowEditor)Layout).Curves.AddToBackground(InternalUiFlowEditorCurve.Type.Vertical, fromX, fromY, toX, toY, 5, ColorBackground);
        }

        public override void Render()
        {
            for (int i = 0; i < _links.Count; i++)
            {
                _links[i].Render();
            }

            base.Render();
        }
    }
}
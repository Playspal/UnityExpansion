using System.Collections.Generic;

using UnityEngine;
using UnityExpansion.Editor;

namespace UnityExpansionInternal.UiLayoutEditor
{
    public class Node : EditorLayoutObject
    {
        public readonly InternalUiLayoutData.NodeData NodeData;

        public string ID { get { return NodeData.ID; } }

        public Color ColorMain { get; private set; }
        public Color ColorDark { get; private set; }
        public Color ColorLight { get; private set; }
        public Color ColorBackground { get; private set; }
        public Color ColorBackgroundBorder { get; private set; }

        public List<NodeConnectorInput> Input { get; private set; }
        public List<NodeConnectorOutput> Output { get; private set; }

        protected EditorLayoutObjectTexture _textureBackground;

        private List<NodeLink> _links = new List<NodeLink>();

        public Node(InternalUiLayoutData.NodeData nodeData, EditorLayout layout, int width, int height) : base (layout, width, height)
        {
            NodeData = nodeData;

            X = nodeData.X;
            Y = nodeData.Y;

            Input = new List<NodeConnectorInput>();
            Output = new List<NodeConnectorOutput>();

            ColorBackground = Color.red.Parse(UiLayoutEditorConfig.COLOR_NODE_BACKGROUND);
            ColorBackgroundBorder = Color.red.Parse(UiLayoutEditorConfig.COLOR_NODE_BACKGROUND_BORDER);

            _textureBackground = new EditorLayoutObjectTexture(layout, Width, Height);
            _textureBackground.Fill(ColorBackground);
            _textureBackground.DrawBorder(1, ColorBackgroundBorder);
            _textureBackground.SetParent(this);

            SetupColors();
        }

        public virtual void AddLink(Node node)
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

            if (this is NodeLayoutElementRoot)
            {
                SetupColors(UiLayoutEditorConfig.COLOR_SCREEN_MAIN, UiLayoutEditorConfig.COLOR_SCREEN_DARK, UiLayoutEditorConfig.COLOR_SCREEN_LIGHT);
            }

            else if (this is NodeLayoutElement)
            {
                SetupColors(UiLayoutEditorConfig.COLOR_BLOCK_MAIN, UiLayoutEditorConfig.COLOR_BLOCK_DARK, UiLayoutEditorConfig.COLOR_BLOCK_LIGHT);
            }

            if (this is NodeSignal)
            {
                SetupColors(UiLayoutEditorConfig.COLOR_SIGNAL_MAIN, UiLayoutEditorConfig.COLOR_SIGNAL_DARK, UiLayoutEditorConfig.COLOR_SIGNAL_LIGHT);
            }

            if (this is NodeLayoutEvent)
            {
                SetupColors(UiLayoutEditorConfig.COLOR_SYSTEM_MAIN, UiLayoutEditorConfig.COLOR_SYSTEM_DARK, UiLayoutEditorConfig.COLOR_SYSTEM_LIGHT);
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

            ((UiLayoutEditor)Layout).Curves.AddToBackground(UiLayoutEditorCurve.Type.Vertical, fromX, fromY, toX, toY, 5, ColorBackground);
        }

        public override void SetSize(int width, int height)
        {
            base.SetSize(width, height);

            _textureBackground.SetSize(Width, Height);
            _textureBackground.Fill(ColorBackground);
            _textureBackground.DrawBorder(1, ColorBackgroundBorder);
        }

        public override void Render()
        {
            for (int i = 0; i < _links.Count; i++)
            {
                _links[i].Render();
            }

            base.Render();
        }

        public override void Update()
        {
            base.Update();

            if(NodeData != null)
            {
                NodeData.X = X;
                NodeData.Y = Y;
            }
        }
    }
}
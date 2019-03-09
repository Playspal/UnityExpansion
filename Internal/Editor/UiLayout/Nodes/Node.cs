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

        public Node ParentNode { get; private set; }

        /// <summary>
        /// List of links.
        /// </summary>
        public List<NodeLink> Links = new List<NodeLink>();
        public List<NodeConnectorHandler> Handlers { get; private set; }
        public List<NodeConnectorSender> Senders { get; private set; }

        protected EditorLayoutObjectTextureCachable _textureBackground;

        public Node(InternalUiLayoutData.NodeData nodeData, EditorLayout layout, int width, int height) : base (layout, width, height)
        {
            NodeData = nodeData;

            X = nodeData.X;
            Y = nodeData.Y;

            Handlers = new List<NodeConnectorHandler>();
            Senders = new List<NodeConnectorSender>();

            _textureBackground = new EditorLayoutObjectTextureCachable(layout, Width, Height, "node-background");
            _textureBackground.SetParent(this);

            if(!_textureBackground.LoadFromCache())
            {
                _textureBackground.Fill(UiLayoutEditorConfig.ColorNodeBackground);
                _textureBackground.DrawBorder(1, UiLayoutEditorConfig.ColorNodeBackgroundBorder);
                _textureBackground.SaveToCache();
            }

            SetupColors();
        }

        public void SetParentNode(Node parent)
        {
            ParentNode = parent;
        }

        public virtual void AddLink(Node node)
        {
            NodeLink link = new NodeLink(Layout, this, node);

            Links.Add(link);

            for (int i = 0; i < Links.Count; i++)
            {
                Links[i].SetPosition(i, Links.Count);
            }
        }

        protected void SetupColors()
        {
            ColorMain = Color.red;
            ColorDark = Color.red;
            ColorLight = Color.red;

            if (this is NodeLayoutElementRoot)
            {
                SetupColors(UiLayoutEditorConfig.ColorElementRootMain, UiLayoutEditorConfig.ColorElementRootDark, UiLayoutEditorConfig.ColorElementRootLight);
            }

            else if (this is NodeLayoutElement)
            {
                SetupColors(UiLayoutEditorConfig.ColorElementChildMain, UiLayoutEditorConfig.ColorElementChildDark, UiLayoutEditorConfig.ColorElementChildLight);
            }

            if (this is NodeSystemEvent)
            {
                SetupColors(UiLayoutEditorConfig.ColorElementSystemMain, UiLayoutEditorConfig.ColorElementSystemDark, UiLayoutEditorConfig.ColorElementSystemLight);
            }

            if (this is NodeSystemMethod)
            {
                SetupColors(UiLayoutEditorConfig.ColorElementSystemMain, UiLayoutEditorConfig.ColorElementSystemDark, UiLayoutEditorConfig.ColorElementSystemLight);
            }
        }

        protected void SetupColors(Color main, Color dark, Color light)
        {
            ColorMain = main;
            ColorDark = dark;
            ColorLight = light;
        }

        protected void RenderCurveTo(int toX, int toY)
        {
            int fromX = GlobalX + Width / 2;
            int fromY = GlobalY + Height;

            ((UiLayoutEditor)Layout).Curves.AddToBackground(UiLayoutEditorCurve.Type.Vertical, fromX, fromY, toX, toY, 5, UiLayoutEditorConfig.ColorNodeBackground);
        }

        public override void SetSize(int width, int height)
        {
            base.SetSize(width, height);

            _textureBackground.SetSize(Width, Height);

            if (!_textureBackground.LoadFromCache())
            {
                _textureBackground.Fill(UiLayoutEditorConfig.ColorNodeBackground);
                _textureBackground.DrawBorder(1, UiLayoutEditorConfig.ColorNodeBackgroundBorder);
                _textureBackground.SaveToCache();
            }
        }

        public override void Render()
        {
            for (int i = 0; i < Links.Count; i++)
            {
                Links[i].Render();
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
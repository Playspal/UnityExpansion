using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

using UnityExpansion;
using UnityExpansion.Editor;
using UnityExpansion.UI;

namespace UnityExpansionInternal.UiLayoutEditor
{
    public class UiLayoutEditor : EditorLayout
    {
        public static UiLayoutEditor Instance { get; private set; }

        public UiLayoutEditorSelection Selection { get; private set; }
        public UiLayoutEditorCurves Curves { get; private set; }

        private List<Node> _nodes = new List<Node>();

        private UiLayoutEditorDragAndDrop _dragAndDrop;
        private UiLayoutEditorBackground _background;
        private EditorLayoutObjectText _message;

        public override void Initialization()
        {
            base.Initialization();

            OnWindowResized += OnWindowResize;

            _message = new EditorLayoutObjectText(this, WindowWidth, 20);
            _message.SetAlignment(TextAnchor.MiddleCenter);
            _message.X = 0;
            _message.Y = WindowHeight / 2 - _message.Height / 2;

            _dragAndDrop = new UiLayoutEditorDragAndDrop(this);
            _dragAndDrop.OnSuccess += OnDragAndDrop;

            _background = new UiLayoutEditorBackground(this);

            Selection = new UiLayoutEditorSelection();
            Curves = new UiLayoutEditorCurves();

            Refresh();
        }

        protected override void OnGUI()
        {
            if (Selection.Target == null)
            {
                _message.SetText("No UiLayout selected");
                _message.SetActive(true);

                base.OnGUI();
                return;
            }

            _message.SetActive(false);
            _background.OnGUI();

            Curves.RenderBackground();

            base.OnGUI();

            Curves.RenderFrontground();
        }

        private void Refresh()
        {
            if(Selection.Target == null)
            {
                return;
            }

            for (int i = 0; i < Selection.Data.Nodes.Count; i++)
            {
                InternalUiLayoutData.NodeData nodeData = Selection.Data.Nodes[i];

                switch (nodeData.Type)
                {
                    case InternalUiLayoutData.NodeType.LayoutElementRoot:
                        SetupLayoutElementRoot(nodeData);
                        break;
                }
            }
        }

        private void SetupLayoutElementRoot(InternalUiLayoutData.NodeData nodeData)
        {
            GameObject gameObject = AssetDatabase.LoadAssetAtPath<GameObject>(nodeData.LayoutPreset.AssetPath);
            UiLayoutElement layoutElement = gameObject.GetComponent<UiLayoutElement>();

            if (layoutElement != null)
            {
                NodeLayoutElement node = CreateNode(layoutElement as UiLayoutElement, null, nodeData.X, nodeData.Y);

                node.SetNodeData(nodeData);
                node.SetLayoutPreset(nodeData.LayoutPreset);
                node.SetAsRootNode();

                for (int i = 0; i < gameObject.transform.childCount; i++)
                {
                    SetupLayoutElementRecursively(gameObject.transform.GetChild(i).gameObject, node);
                }
            }
        }

        private Node SetupLayoutElementRecursively(GameObject gameObject, Node parentNode)
        {
            UiLayoutElement layoutElement = gameObject.GetComponent<UiLayoutElement>();

            if (layoutElement != null)
            {
                string id = parentNode.ID + "/" + gameObject.name;

                InternalUiLayoutData.NodeData nodeData = Selection.Data.Find(id);

                if(nodeData == null)
                {
                    nodeData = Selection.Data.CreateNodeDataLayoutElement();

                    nodeData.ID = id;
                    nodeData.X = parentNode.X;
                    nodeData.Y = parentNode.Y + 100;

                    Selection.Data.AddNodeData(nodeData);
                }

                parentNode = CreateNode(layoutElement as UiLayoutElement, parentNode, nodeData.X, nodeData.Y);
                parentNode.SetNodeData(nodeData);
            }

            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                SetupLayoutElementRecursively(gameObject.transform.GetChild(i).gameObject, parentNode);
            }

            return parentNode;
        }

        private NodeLayoutElement CreateNode(UiLayoutElement element, Node parentNode, int x, int y)
        {
            NodeLayoutElement node = new NodeLayoutElement(this);
            node.SetLayoutElement(element);
            node.X = x;
            node.Y = y;

            AddNode(node);
            CreateLink(parentNode, node);

            return node;
        }

        private NodeSignal CreateNodeSignal(int x, int y)
        {
            NodeSignal node = new NodeSignal(this);
            node.X = x;
            node.Y = y;

            AddNode(node);

            return node;
        }

        private void AddNode(Node node)
        {
            _nodes.Add(node);
        }

        private void CreateLink(Node a, Node b)
        {
            if(a != null && b != null)
            {
                a.AddLink(b);
            }
        }

        private void OnDragAndDrop(UiLayoutPreset layoutPreset)
        {
            Selection.Target.AddPreset(layoutPreset);

            InternalUiLayoutData.NodeData nodeData = Selection.Data.CreateNodeDataLayoutElementRoot();

            nodeData.ID = layoutPreset.AssetPath;
            nodeData.LayoutPreset = layoutPreset;
            nodeData.X = Mouse.X - CanvasX;
            nodeData.Y = Mouse.Y - CanvasY;

            Selection.Data.AddNodeData(nodeData);

            Refresh();
        }

        private void OnWindowResize()
        {
            _message.SetSize(WindowWidth, _message.Height);
            _message.X = 0;
            _message.Y = WindowHeight / 2 - _message.Height / 2;
        }

        /// <summary>
        /// Shows the window.
        /// </summary>
        [MenuItem("Window/UiLayout Editor")]
        public static void ShowWindow()
        {
            Instance = GetWindow(typeof(UiLayoutEditor)) as UiLayoutEditor;
            Instance.titleContent = new GUIContent("UiLayout editor");
            Instance.Initialization();
        }
    }
}
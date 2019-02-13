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

            SetupLayoutEventOnEnable();

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

            for(int i = 0; i < _nodes.Count; i++)
            {
                Node nodeA = _nodes[i];

                for (int j = 0; j < nodeA.Output.Count; j++)
                {
                    NodeConnectorOutput connectorOutput = nodeA.Output[j];

                    if(string.IsNullOrEmpty(connectorOutput.Data))
                    {
                        continue;
                    }

                    for (int n = 0; n < _nodes.Count; n++)
                    {
                        Node nodeB = _nodes[n];

                        if(nodeA == nodeB)
                        {
                            continue;
                        }

                        for(int m = 0; m < nodeB.Input.Count; m++)
                        {
                            NodeConnectorInput connectorInput = nodeB.Input[m];

                            if (connectorOutput.Data == connectorInput.Data)
                            {
                                NodeConnector.ConnectionCreate(connectorOutput, connectorInput);
                            }
                        }
                    }
                }
            }
        }

        private void SetupLayoutEventOnEnable()
        {
            string id = Selection.Target.SignalOnEnable.Name;

            InternalUiLayoutData.NodeData nodeData = Selection.Data.Find(id);

            if (nodeData == null)
            {
                nodeData = Selection.Data.CreateNodeDataLayoutElement();
                nodeData.ID = id;
                nodeData.X = 0;
                nodeData.Y = 0;

                Selection.Data.AddNodeData(nodeData);
            }

            NodeLayoutEvent node = CreateNodeLayoutEvent(nodeData, NodeLayoutEvent.Type.OnEnable, nodeData.X, nodeData.Y);
        }

        private void SetupLayoutElementRoot(InternalUiLayoutData.NodeData nodeData)
        {
            GameObject gameObject = AssetDatabase.LoadAssetAtPath<GameObject>(nodeData.LayoutPreset.AssetPath);
            UiLayoutElement layoutElement = gameObject.GetComponent<UiLayoutElement>();

            if (layoutElement != null)
            {
                NodeLayoutElementRoot node = CreateNodeLayoutElementRoot(nodeData, layoutElement as UiLayoutElement, nodeData.X, nodeData.Y);

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
                string id = parentNode.ID + gameObject.name;

                InternalUiLayoutData.NodeData nodeData = Selection.Data.Find(id);

                if(nodeData == null)
                {
                    nodeData = Selection.Data.CreateNodeDataLayoutElement();

                    nodeData.ID = id;
                    nodeData.X = parentNode.X;
                    nodeData.Y = parentNode.Y + 100;

                    Selection.Data.AddNodeData(nodeData);
                }

                parentNode = CreateNodeLayoutElement(nodeData, layoutElement as UiLayoutElement, parentNode, nodeData.X, nodeData.Y);
            }

            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                SetupLayoutElementRecursively(gameObject.transform.GetChild(i).gameObject, parentNode);
            }

            return parentNode;
        }

        private NodeLayoutElement CreateNodeLayoutElement(InternalUiLayoutData.NodeData nodeData, UiLayoutElement element, Node parentNode, int x, int y)
        {
            NodeLayoutElement node = new NodeLayoutElement(nodeData, this);
            node.SetLayoutElement(element);

            AddNode(node, x, y);
            CreateLink(parentNode, node);

            return node;
        }

        private NodeLayoutElementRoot CreateNodeLayoutElementRoot(InternalUiLayoutData.NodeData nodeData, UiLayoutElement element, int x, int y)
        {
            NodeLayoutElementRoot node = new NodeLayoutElementRoot(nodeData, this);
            node.SetLayoutElement(element);

            AddNode(node, x, y);

            return node;
        }

        private NodeSignal CreateNodeSignal(InternalUiLayoutData.NodeData nodeData, int x, int y)
        {
            NodeSignal node = new NodeSignal(nodeData, this);

            AddNode(node, x, y);

            return node;
        }

        private NodeLayoutEvent CreateNodeLayoutEvent(InternalUiLayoutData.NodeData nodeData, NodeLayoutEvent.Type eventType, int x, int y)
        {
            NodeLayoutEvent node = new NodeLayoutEvent(nodeData, this, eventType);

            node.SetUiLayout(Selection.Target);


            AddNode(node, x, y);

            return node;
        }

        private void AddNode(Node node, int x, int y)
        {
            node.X = x;
            node.Y = y;

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
            GameObject gameObject = AssetDatabase.LoadAssetAtPath<GameObject>(layoutPreset.AssetPath);

            nodeData.ID = "__" + (gameObject.GetInstanceID() < 0 ? "n" : "p") + Mathf.Abs(gameObject.GetInstanceID());// layoutPreset.AssetPath;
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
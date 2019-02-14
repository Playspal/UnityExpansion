using UnityEditor;
using UnityEngine;

using UnityExpansion.Editor;
using UnityExpansion.UI;

namespace UnityExpansionInternal.UiLayoutEditor
{
    public class UiLayoutEditor : EditorLayout
    {
        public static UiLayoutEditor Instance { get; private set; }

        public UiLayoutEditorSelection Selection { get; private set; }
        public UiLayoutEditorCurves Curves { get; private set; }
        public Nodes Nodes { get; private set; }
        
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
            Nodes = new Nodes();

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

            Nodes.Clear();

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

            RefreshConnections();
        }

        private void RefreshConnections()
        {
            for (int i = 0; i < Nodes.Items.Count; i++)
            {
                Node node = Nodes.Items[i];

                for (int n = 0; n < node.Output.Count; n++)
                {
                    RefreshConnection(node.Output[n]);
                }
            }
        }

        private void RefreshConnection(NodeConnectorOutput connectorOutput)
        {
            if (string.IsNullOrEmpty(connectorOutput.Data))
            {
                return;
            }

            for (int i = 0; i < Nodes.Items.Count; i++)
            {
                Node node = Nodes.Items[i];

                for (int n = 0; n < node.Input.Count; n++)
                {
                    NodeConnectorInput connectorInput = node.Input[n];

                    if (connectorOutput.Data == connectorInput.Data)
                    {
                        NodeConnector.ConnectionCreate(connectorOutput, connectorInput);
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

                Selection.Data.AddNodeData(nodeData);
            }

            NodeLayoutEvent node = Nodes.CreateNodeLayoutEvent(nodeData, NodeLayoutEvent.Type.OnEnable);
        }

        private void SetupLayoutElementRoot(InternalUiLayoutData.NodeData nodeData)
        {
            GameObject gameObject = AssetDatabase.LoadAssetAtPath<GameObject>(nodeData.LayoutPreset.AssetPath);
            UiLayoutElement layoutElement = gameObject.GetComponent<UiLayoutElement>();

            if (layoutElement != null)
            {
                NodeLayoutElementRoot node = Nodes.CreateNodeLayoutElementRoot(nodeData, layoutElement);

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

                parentNode = Nodes.CreateNodeLayoutElement(nodeData, layoutElement as UiLayoutElement, parentNode);
            }

            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                SetupLayoutElementRecursively(gameObject.transform.GetChild(i).gameObject, parentNode);
            }

            return parentNode;
        }

        private void OnDragAndDrop(UiLayoutPreset layoutPreset)
        {
            Selection.Target.AddPreset(layoutPreset);

            InternalUiLayoutData.NodeData nodeData = Selection.Data.CreateNodeDataLayoutElementRoot();
            GameObject gameObject = AssetDatabase.LoadAssetAtPath<GameObject>(layoutPreset.AssetPath);

            nodeData.ID = "__" + (gameObject.GetInstanceID() < 0 ? "n" : "p") + Mathf.Abs(gameObject.GetInstanceID());
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
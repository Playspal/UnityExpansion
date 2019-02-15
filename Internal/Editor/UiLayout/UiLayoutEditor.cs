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

            Mouse.OnClickRight += () =>
            {
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("Add signal"), true, OnSignalCreate);
                menu.ShowAsContext();
            };

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

                    case InternalUiLayoutData.NodeType.Signal:
                        SetupSignal(nodeData);
                        break;
                }
            }


            for (int i = 0; i < Selection.Target.Actions.Length; i++)
            {
                NodeConnectorOutput output = null;
                NodeConnectorInput input = null;

                UiLayout.UiAction action = Selection.Target.Actions[i];

                for (int j = 0; j < Nodes.Items.Count; j++)
                {
                    Node node = Nodes.Items[j];

                    for (int n = 0; n < node.Output.Count; n++)
                    {
                        if
                        (
                            node.Output[n].DataID == action.SenderID &&
                            node.Output[n].DataMethod == action.SenderMethod
                        )
                        {
                            output = node.Output[n];
                        }
                    }

                    for (int n = 0; n < node.Input.Count; n++)
                    {
                        if
                        (
                            node.Input[n].DataID == action.TargetID &&
                            node.Input[n].DataMethod == action.TargetMethod
                        )
                        {
                            input = node.Input[n];
                        }
                    }
                }

                if(output != null && input != null)
                {
                    NodeConnector.ConnectionCreate(output, input);
                }
            }
        }
        
        private void SetupSignal(InternalUiLayoutData.NodeData nodeData)
        {
            string id = "__newSignal"; // TODO: generate random one

            NodeSignal node = Nodes.CreateNodeSignal(nodeData);
        }

        private void SetupLayoutEventOnEnable()
        {
            string id = Selection.Target.UniqueID + "OnEnable";

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
                InternalUiLayoutData.NodeData nodeData = Selection.Data.Find(layoutElement.UniqueID);

                if(nodeData == null)
                {
                    nodeData = Selection.Data.CreateNodeDataLayoutElement();
                    nodeData.ID = layoutElement.UniqueID;
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

        private void OnSignalCreate()
        {
            InternalUiLayoutData.NodeData nodeData = Selection.Data.CreateNodeDataSignal();

            nodeData.ID = "__signal" + Random.Range(1000, 9999);
            nodeData.X = Mouse.X - CanvasX;
            nodeData.Y = Mouse.Y - CanvasY;

            Selection.Data.AddNodeData(nodeData);

            Refresh();
        }

        private void OnDragAndDrop(UiLayoutPreset layoutPreset)
        {
            Selection.Target.AddPreset(layoutPreset);

            InternalUiLayoutData.NodeData nodeData = Selection.Data.CreateNodeDataLayoutElementRoot();
            GameObject gameObject = AssetDatabase.LoadAssetAtPath<GameObject>(layoutPreset.AssetPath);
            UiLayoutElement element = gameObject.GetComponent<UiLayoutElement>();

            nodeData.ID = element.UniqueID;
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
using UnityEditor;
using UnityEngine;

using UnityExpansion.Editor;
using UnityExpansion.UI;
using UnityExpansion.Utilities;

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
                menu.AddItem(new GUIContent("Add signal"), true, ()=> { });
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

            ValidateObjects();

            Nodes.Clear();

            //
            string[] layoutMethods = UtilityReflection.GetMethodsWithAttribute(Selection.Target, typeof(UiLayoutMethod));
            
            for (int i = 0; i < layoutMethods.Length; i++)
            {
                SetupSystemMethod(layoutMethods[i]);
            }

            string[] layoutEvents = UtilityReflection.GetEventsWithAttribute(Selection.Target, typeof(UiLayoutEvent));

            for(int i = 0; i < layoutEvents.Length; i++)
            {
                SetupSystemEvent(layoutEvents[i]);
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
                            node.Output[n].DataMethod == action.SenderEvent
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

        private void ValidateObjects()
        {
            CheckID(Selection.Target);

            for(int i = 0; i < Selection.Target.Presets.Length; i++)
            {
                ValidateObject(Selection.Target.Presets[i].Prefab);
            }

            // TODO: remove presets with missing prefabs
            // TODO: actions with missing prefabs
        }

        private void ValidateObject(UiLayoutObject layoutObject)
        {
            if(layoutObject == null)
            {
                return;
            }

            CheckID(layoutObject);

            for (int i = 0; i < layoutObject.transform.childCount; i++)
            {
                ValidateObject(layoutObject.transform.GetChild(i).GetComponent<UiLayoutObject>());
            }
        }

        private void CheckID(UiLayoutObject layoutObject)
        {
            if (string.IsNullOrEmpty(layoutObject.UniqueID))
            {
                UiLayoutEditorUtils.LayoutObjectGenerateUniqueID(layoutObject);
            }
        }
        
        private void SetupSystemMethod(string methodName)
        {
            string id = Selection.Target.UniqueID + "." + methodName;

            InternalUiLayoutData.NodeData nodeData = Selection.Data.Find(id);

            if (nodeData == null)
            {
                nodeData = Selection.Data.CreateNodeDataSystemEvent();
                nodeData.ID = id;

                Selection.Data.AddNodeData(nodeData);
            }

            NodeSystemMethod node = Nodes.CreateNodeSystemMethod(nodeData, Selection.Target, methodName);
        }

        private void SetupSystemEvent(string eventName)
        {
            string id = Selection.Target.UniqueID + "." + eventName;

            InternalUiLayoutData.NodeData nodeData = Selection.Data.Find(id);

            if (nodeData == null)
            {
                nodeData = Selection.Data.CreateNodeDataSystemEvent();
                nodeData.ID = id;

                Selection.Data.AddNodeData(nodeData);
            }

            NodeSystemEvent node = Nodes.CreateNodeSystemEvent(nodeData, Selection.Target, eventName);
        }

        private void SetupLayoutElementRoot(InternalUiLayoutData.NodeData nodeData)
        {
            UiLayoutElement layoutElement = nodeData.LayoutPreset.Prefab;

            if (layoutElement != null)
            {
                NodeLayoutElementRoot node = Nodes.CreateNodeLayoutElementRoot(nodeData, layoutElement);

                for (int i = 0; i < layoutElement.transform.childCount; i++)
                {
                    SetupLayoutElementRecursively(layoutElement.transform.GetChild(i).gameObject, node);
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

        private void OnDragAndDrop(UiLayoutPreset layoutPreset)
        {
            Selection.Target.AddPreset(layoutPreset);

            InternalUiLayoutData.NodeData nodeData = Selection.Data.CreateNodeDataLayoutElementRoot();
            //GameObject gameObject = AssetDatabase.LoadAssetAtPath<GameObject>(layoutPreset.AssetPath);
            //UiLayoutElement element = gameObject.GetComponent<UiLayoutElement>();

            nodeData.ID = layoutPreset.Prefab.UniqueID;
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
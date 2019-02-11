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
            int x = 0;
            int y = 0;

            for (int i = 0; i < Selection.Target.Presets.Count; i++)
            {
                UiLayoutPreset preset = Selection.Target.Presets[i];

                GameObject gameObject = AssetDatabase.LoadAssetAtPath<GameObject>(preset.AssetPath);
                NodeLayoutElement node = SetupElementRecursively(gameObject, null, x, y) as NodeLayoutElement;

                node.SetLayoutPreset(preset);
                node.SetAsRootNode();

                if (preset.SignalsShow.Length > 0)
                {
                    NodeLayoutElement nodeLayoutElement = node as NodeLayoutElement;

                    NodeSignal signal = CreateNodeSignal(x, y);

                    signal.SetSignal(preset.SignalsShow[0]);
                    signal.OutputOnReceive.ConnectTo(nodeLayoutElement.BlockShowAndHide.InputShow);
                }

                if (preset.SignalsHide.Length > 0)
                {
                    NodeLayoutElement nodeLayoutElement = node as NodeLayoutElement;

                    NodeSignal signal = CreateNodeSignal(x, y);

                    signal.SetSignal(preset.SignalsHide[0]);
                    signal.OutputOnReceive.ConnectTo(nodeLayoutElement.BlockShowAndHide.InputHide);
                }

                x += 500;
            }
        }

        private Node SetupElementRecursively(GameObject gameObject, Node parentNode, int x, int y)
        {
            UiLayoutElement layoutElement = gameObject.GetComponent<UiLayoutElement>();

            if (layoutElement != null)
            {
                parentNode = CreateNode(layoutElement as UiLayoutElement, parentNode, x, y);
                y += parentNode.Height + 40;
            }

            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                SetupElementRecursively(gameObject.transform.GetChild(i).gameObject, parentNode, x, y);
                y += 340;
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
            Selection.Target.Presets.Add(layoutPreset);
            Refresh();

            //Node node = SetupElementRecursively(layoutElement.gameObject, null, Mouse.X - CanvasX, Mouse.Y - CanvasY);
            //node.SetAsRootNode();
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
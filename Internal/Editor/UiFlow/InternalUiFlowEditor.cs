using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

using UnityExpansion;
using UnityExpansion.Editor;
using UnityExpansion.UI;

namespace UnityExpansionInternal.UiFlow
{
    public class InternalUiFlowEditor : EditorLayout
    {
        public static InternalUiFlowEditor Instance { get; private set; }
        
        private List<Node> _nodes = new List<Node>();

        public InternalUiFlowEditorCurves Curves { get; private set; }
        public InternalUiFlowEditorHierarchy Hierarchy { get; private set; }

        private Texture2D _background;

        public override void Initialization()
        {
            base.Initialization();

            Color main = Color.white.Parse("#909090");
            Color line = Color.white.Parse("#7E7E7E");

            _background = new Texture2D(WindowWidth + 40, WindowHeight + 40);
            _background.Fill(main);

            for (int x = 0; x < _background.width; x += 20)
            {
                _background.DrawRect(x, 0, 1, _background.height, line);
            }

            for (int y = 0; y < _background.height; y += 20)
            {
                _background.DrawRect(0, y + 8, _background.width, 1, line);
            }


            Curves = new InternalUiFlowEditorCurves();

            Expansion expansion = FindObjectOfType<Expansion>();

            if(expansion != null)
            {
                int x = 0;
                int y = 0;

                for(int i = 0; i < expansion.LayoutSettings.Screens.Count; i++)
                {
                    UiLayoutPreset preset = expansion.LayoutSettings.Screens[i];
                    string path = "Assets/Resources/" + preset.PrefabPath + ".prefab";

                    GameObject gameObject = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                    NodeLayoutElement node = SetupElement(gameObject, null, x, y) as NodeLayoutElement;

                    node.SetLayoutPreset(preset);

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

            Mouse.OnDragContext += () =>
            {
                CanvasX += Mouse.DeltaX;
                CanvasY += Mouse.DeltaY;
            };
        }

        protected override void OnGUI()
        {
            int x = CanvasX % 20 - 20;
            int y = CanvasY % 20 - 20;
            Rect bounds = new Rect(x, y, _background.width, _background.height);
            GUI.DrawTexture(bounds, _background, ScaleMode.StretchToFill, true, 1f);

            Curves.RenderBackground();

            base.OnGUI();

            Curves.RenderFrontground();
        }

        private Node SetupElement(GameObject gameObject, Node parentNode, int x, int y)
        {
            UiLayoutElement layoutElement = gameObject.GetComponent<UiLayoutElement>();

            if (layoutElement != null)
            {
                if (layoutElement is UiLayoutElementScreen)
                {
                    parentNode = CreateNode(layoutElement as UiLayoutElementScreen, parentNode, x, y);
                    y += parentNode.Height + 40;
                }
                else
                {
                    parentNode = CreateNode(layoutElement as UiLayoutElement, parentNode, x, y);
                    y += parentNode.Height + 40;
                }
            }

            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                SetupElement(gameObject.transform.GetChild(i).gameObject, parentNode, x, y);
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

        private NodeLayoutElementScreen CreateNode(UiLayoutElementScreen element, Node parentNode, int x, int y)
        {
            NodeLayoutElementScreen node = new NodeLayoutElementScreen(this);
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

        /// <summary>
        /// Shows the window.
        /// </summary>
        [MenuItem("Window/UI Flow Editor")]
        public static void ShowWindow()
        {
            Instance = GetWindow(typeof(InternalUiFlowEditor)) as InternalUiFlowEditor;
            Instance.Initialization();
            //Instance = DisplayWizard<>("UI Flow Editor", "sdfsdfs", "asdadas");
        }
    }
}
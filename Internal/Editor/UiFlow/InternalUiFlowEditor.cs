using System;
using System.Collections;
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

        Rect window1;
        Rect window2;

        private List<Node> _nodes = new List<Node>();

        public InternalUiFlowEditorCurves Curves { get; private set; }

        public override void Initialization()
        {
            base.Initialization();

            Curves = new InternalUiFlowEditorCurves();

            window1 = new Rect(10, 10, 100, 100);
            window2 = new Rect(210, 210, 100, 100);

            Expansion expansion = FindObjectOfType<Expansion>();

            if(expansion != null)
            {
                for(int i = 0; i < expansion.LayoutSettings.Screens.Count; i++)
                {
                    UiLayoutPreset preset = expansion.LayoutSettings.Screens[0];
                    string path = "Assets/Resources/" + preset.PrefabPath + ".prefab";

                    GameObject gameObject = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                    SetupElement(gameObject);
                }
            }
        }

        protected override void OnGUI()
        {
            Curves.RenderBackground();

            base.OnGUI();

            Curves.RenderFrontground();
        }

        private void SetupElement(GameObject gameObject)
        {
            UiLayoutElement layoutElement = gameObject.GetComponent<UiLayoutElement>();

            if (layoutElement != null)
            {
                if (layoutElement is UiLayoutElementScreen)
                {
                    CreateNode(layoutElement as UiLayoutElementScreen);
                }
                else
                {
                    CreateNode(layoutElement as UiLayoutElement);
                }
            }

            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                SetupElement(gameObject.transform.GetChild(i).gameObject);
            }
        }

        private void CreateNode(UiLayoutElement element)
        {
            NodeLayoutElement node = new NodeLayoutElement(this);
            node.SetLayoutElement(element);

            AddNode(node);
        }

        private void CreateNode(UiLayoutElementScreen element)
        {
            NodeLayoutElementScreen node = new NodeLayoutElementScreen(this);
            node.SetLayoutElement(element);

            AddNode(node);
        }

        private void AddNode(Node node)
        {
            node.X = UnityEngine.Random.Range(0, 100);
            _nodes.Add(node);
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
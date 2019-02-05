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

        private List<NodeLayoutElement> _nodes = new List<NodeLayoutElement>();

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
                    UiLayoutElement element = gameObject.GetComponent<UiLayoutElement>();

                    if (element != null)
                    {
                        Debug.LogError(path + " > " + element);

                        AddNode(element);
                    }
                }
            }
        }

        protected override void OnGUI()
        {
            Curves.RenderBackground();

            base.OnGUI();

            Curves.RenderFrontground();

            EditorGUILayout.HelpBox("123", MessageType.Info);
            DrawNodeCurve(window1, window2);
            BeginWindows();
            window1 = GUI.Window(1, window1, DrawNodeWindow, "Window 1");   // Updates the Rect's when these are dragged
            window2 = GUI.Window(2, window2, DrawNodeWindow, "Window 2");
            EndWindows();

        }

        void DrawNodeWindow(int id)
        {
            GUI.DragWindow();
        }

        void DrawNodeCurve(Rect start, Rect end)
        {
            Vector3 startPos = new Vector3(start.x + start.width, start.y + start.height / 2, 0);
            Vector3 endPos = new Vector3(end.x, end.y + end.height / 2, 0);
            Vector3 startTan = startPos + Vector3.right * 50;
            Vector3 endTan = endPos + Vector3.left * 50;
            Color shadowCol = new Color(0, 0, 0, 0.06f);
            for (int i = 0; i < 3; i++) // Draw a shadow
                Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);
            Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 1);
        }

        private void AddNode(UiLayoutElement layoutElement)
        {
            NodeLayoutElement node = new NodeLayoutElement(this);
            node.SetLayoutElement(layoutElement);
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
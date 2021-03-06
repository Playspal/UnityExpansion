﻿using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using UnityExpansion.Editor;
using UnityExpansion.UI.Layout;
using UnityExpansion.UI.Layout.Processor;
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

        public float scale = 0.5f;

        public override void Initialization()
        {
            base.Initialization();
            EnableZoomByMouseWheel(0.3f, 1f, 0.1f);

            Instance = this;

            OnWindowResized += OnWindowResize;

            _message = new EditorLayoutObjectText(this, CanvasWidth, 20);
            _message.SetAlignment(TextAnchor.MiddleCenter);
            _message.X = 0;
            _message.Y = CanvasHeight / 2 - _message.Height / 2;

            _dragAndDrop = new UiLayoutEditorDragAndDrop(this);
            _dragAndDrop.OnSuccess += OnDragAndDrop;

            _background = new UiLayoutEditorBackground(this);

            Selection = new UiLayoutEditorSelection();
            Selection.OnChanged += OnSelectionChanged;

            Curves = new UiLayoutEditorCurves();
            Nodes = new Nodes();

            Refresh();
        }

        public void RefreshNode(Node node)
        {
            if (node.ParentNode != null)
            {
                RefreshNode(node.ParentNode);
                return;
            }

            InternalUiLayoutData.NodeData nodeData = node.NodeData;

            Nodes.DestroyWithLinkedNodes(node);
            SetupNode(nodeData);
            RefreshEdicts();
        }

        protected override void Render()
        {
            if (Selection == null)
            {
                Initialization();
            }

            Selection.Update();

            if (Selection.Target == null)
            {
                _message.X = -CanvasX;
                _message.Y = CanvasHeight / 2 - _message.Height / 2 - CanvasY;
                _message.SetText("No UiLayout selected");
                _message.SetActive(true);

                SetScale(1);

                base.Render();
                return;
            }

            _message.SetActive(false);
            _background.OnGUI();

            Curves.RenderBackground();

            base.Render();

            Curves.RenderFrontground();
        }

        private void OnSelectionChanged()
        {
            Refresh();
        }

        private void Refresh()
        {
            Nodes.Clear();

            if (Selection.Target == null)
            {
                return;
            }

            //
            string[] layoutMethods = UtilityReflection.GetMembersWithAttribute(Selection.Target, typeof(UiLayoutProcessorHandler));
            
            for (int i = 0; i < layoutMethods.Length; i++)
            {
                UiLayoutProcessorHandler attribute = UtilityReflection.GetAttribute<UiLayoutProcessorHandler>(Selection.Target, layoutMethods[i]);

                if(!attribute.ExcludeFromLayoutObject)
                {
                    SetupSystemMethod(layoutMethods[i]);
                }
            }

            string[] layoutEvents = UtilityReflection.GetMembersWithAttribute(Selection.Target, typeof(UiLayoutProcessorEvent));

            for(int i = 0; i < layoutEvents.Length; i++)
            {
                UiLayoutProcessorEvent attribute = UtilityReflection.GetAttribute<UiLayoutProcessorEvent>(Selection.Target, layoutEvents[i]);

                if (!attribute.ExcludeFromLayoutObject)
                {
                    SetupSystemEvent(layoutEvents[i]);
                }
            }

            for (int i = 0; i < Selection.Data.Nodes.Count; i++)
            {
                SetupNode(Selection.Data.Nodes[i]);
            }

            RefreshEdicts();
        }

        private void RefreshEdicts()
        {
            UiLayoutProcessorEdict[] edicts = Selection.TargetProcessor.Edicts.ToArray();

            List<UiLayoutProcessorEdict> unused = new List<UiLayoutProcessorEdict>();

            for (int i = 0; i < edicts.Length; i++)
            {
                UiLayoutProcessorEdict edict = edicts[i];

                NodeConnectorSender sender = null;
                NodeConnectorHandler handler = null;

                for (int j = 0; j < Nodes.Items.Count; j++)
                {
                    Node node = Nodes.Items[j];

                    for (int n = 0; n < node.Senders.Count; n++)
                    {
                        if
                        (
                            node.Senders[n].DataID == edict.SenderID &&
                            node.Senders[n].DataMethod == edict.SenderEvent
                        )
                        {
                            sender = node.Senders[n];
                            break;
                        }
                    }

                    for (int n = 0; n < node.Handlers.Count; n++)
                    {
                        if
                        (
                            node.Handlers[n].DataID == edict.HandlerID &&
                            node.Handlers[n].DataMethod == edict.HandlerMethod
                        )
                        {
                            handler = node.Handlers[n];
                            break;
                        }
                    }
                }

                if (sender != null && handler != null)
                {
                    NodeConnector.ConnectionCreate(sender, handler);
                }
                else
                {
                    unused.Add(edict);
                }
            }

            for(int i = 0; i < unused.Count; i++)
            {
                Selection.ProcessorEdictRemove
                (
                    unused[i].SenderID,
                    unused[i].SenderEvent,
                    unused[i].HandlerID,
                    unused[i].HandlerMethod
                );
            }
        }
      
        private void SetupSystemMethod(string methodName)
        {
            string id = Selection.Target.PersistantID + "." + methodName;

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
            string id = Selection.Target.PersistantID + "." + eventName;

            InternalUiLayoutData.NodeData nodeData = Selection.Data.Find(id);

            if (nodeData == null)
            {
                nodeData = Selection.Data.CreateNodeDataSystemEvent();
                nodeData.ID = id;

                Selection.Data.AddNodeData(nodeData);
            }

            NodeSystemEvent node = Nodes.CreateNodeSystemEvent(nodeData, Selection.Target, eventName);
        }

        private void SetupNode(InternalUiLayoutData.NodeData nodeData)
        {
            switch (nodeData.Type)
            {
                case InternalUiLayoutData.NodeType.LayoutElementRoot:
                    SetupLayoutElementRoot(nodeData);
                    break;
            }
        }

        private void SetupLayoutElementRoot(InternalUiLayoutData.NodeData nodeData)
        {
            UiLayoutProcessorPreset preset = Selection.ProcessorPresetFind(nodeData.LayoutPresetID);
            UiLayoutElement layoutElement = preset.Prefab;
            
            if(preset.Container == null)
            {
                preset.Container = Selection.Target.GetComponent<RectTransform>();
            }

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
                InternalUiLayoutData.NodeData nodeData = Selection.Data.Find(layoutElement.PersistantID.Value);

                if(nodeData == null)
                {
                    nodeData = Selection.Data.CreateNodeDataLayoutElement();
                    nodeData.ID = layoutElement.PersistantID.Value;
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

        private void OnDragAndDrop(UiLayoutElement layoutElement)
        {
            UiLayoutProcessorPreset preset = Selection.ProcessorPresetCreate(layoutElement);

            InternalUiLayoutData.NodeData nodeData = Selection.Data.CreateNodeDataLayoutElementRoot();

            nodeData.ID = layoutElement.PersistantID.Value;
            nodeData.LayoutPresetID = preset.ID;
            nodeData.X = Mouse.X - CanvasX;
            nodeData.Y = Mouse.Y - CanvasY;

            Selection.Data.AddNodeData(nodeData);

            Refresh();
        }

        private void OnWindowResize()
        {
            _message.SetSize(CanvasWidth, _message.Height);
            _message.X = 0;
            _message.Y = CanvasHeight / 2 - _message.Height / 2;
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
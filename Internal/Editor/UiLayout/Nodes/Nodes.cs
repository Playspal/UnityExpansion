using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityExpansion.UI;

namespace UnityExpansionInternal.UiLayoutEditor
{
    public class Nodes
    {
        public List<Node> Items { get; private set; }

        public Nodes()
        {
            Items = new List<Node>();
        }

        public void Clear()
        {
            Debug.LogError("!CLEAR 1 > " + Items.Count);

            for(int i = 0; i < Items.Count; i++)
            {
                Items[i].Destroy();
            }

            Items.Clear();

            Debug.LogError("!CLEAR 2 > " + Items.Count);
        }

        public NodeLayoutElementRoot CreateNodeLayoutElementRoot(InternalUiLayoutData.NodeData nodeData, UiLayoutElement element)
        {
            NodeLayoutElementRoot node = new NodeLayoutElementRoot(nodeData, UiLayoutEditor.Instance);
            node.SetLayoutElement(element);

            Add(node);

            return node;
        }

        public NodeLayoutElement CreateNodeLayoutElement(InternalUiLayoutData.NodeData nodeData, UiLayoutElement element, Node parentNode)
        {
            NodeLayoutElement node = new NodeLayoutElement(nodeData, UiLayoutEditor.Instance);
            node.SetLayoutElement(element);

            Add(node);
            CreateLink(parentNode, node);

            return node;
        }

        public NodeSignal CreateNodeSignal(InternalUiLayoutData.NodeData nodeData)
        {
            NodeSignal node = new NodeSignal(nodeData, UiLayoutEditor.Instance);

            Add(node);

            return node;
        }

        public NodeLayoutEvent CreateNodeLayoutEvent(InternalUiLayoutData.NodeData nodeData, NodeLayoutEvent.Type eventType)
        {
            NodeLayoutEvent node = new NodeLayoutEvent(nodeData, UiLayoutEditor.Instance, eventType);

            node.SetUiLayout(UiLayoutEditor.Instance.Selection.Target);


            Add(node);

            return node;
        }

        /// <summary>
        /// Creates the link from parent node to child one.
        /// </summary>
        public void CreateLink(Node a, Node b)
        {
            if (a != null && b != null)
            {
                a.AddLink(b);
            }
        }

        public void Add(Node node)
        {
            Items.Add(node);
        }
    }
}
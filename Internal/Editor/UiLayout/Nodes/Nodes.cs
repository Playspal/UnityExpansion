using System.Collections.Generic;

using UnityExpansion.UI.Layout;

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
            for(int i = 0; i < Items.Count; i++)
            {
                Items[i].Destroy();
            }

            Items.Clear();
        }

        public NodeLayoutElementRoot CreateNodeLayoutElementRoot(InternalUiLayoutData.NodeData nodeData, UiLayoutElement element)
        {
            NodeLayoutElementRoot node = new NodeLayoutElementRoot(nodeData, UiLayoutEditor.Instance, element);

            Add(node);

            return node;
        }

        public NodeLayoutElement CreateNodeLayoutElement(InternalUiLayoutData.NodeData nodeData, UiLayoutElement element, Node parentNode)
        {
            NodeLayoutElement node = new NodeLayoutElement(nodeData, UiLayoutEditor.Instance, element);

            Add(node);
            CreateLink(parentNode, node);

            return node;
        }

        public NodeSystemEvent CreateNodeSystemEvent(InternalUiLayoutData.NodeData nodeData, UiLayout uiLayout, string eventName)
        {
            NodeSystemEvent node = new NodeSystemEvent(nodeData, UiLayoutEditor.Instance);
            node.SetData(uiLayout, eventName);

            Add(node);

            return node;
        }

        public NodeSystemMethod CreateNodeSystemMethod(InternalUiLayoutData.NodeData nodeData, UiLayout uiLayout, string methodName)
        {
            NodeSystemMethod node = new NodeSystemMethod(nodeData, UiLayoutEditor.Instance);
            node.SetData(uiLayout, methodName);

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
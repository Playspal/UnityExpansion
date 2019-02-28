using System;
using System.Collections.Generic;

using UnityEngine;

using UnityExpansion.UI.Layout.Processor;

[Serializable]
public class InternalUiLayoutData : MonoBehaviour
{
    [Serializable]
    public enum NodeType
    {
        Undefined,
        LayoutElementRoot,
        LayoutElement,
        SystemEvent
    }

    [Serializable]
    public class NodeData
    {
        [SerializeField]
        public string ID;

        [SerializeField]
        public NodeType Type = NodeType.Undefined;

        [SerializeField]
        public UiLayoutProcessorPreset LayoutPreset;

        [SerializeField]
        public int X;

        [SerializeField]
        public int Y;
    }

    [SerializeField]
    public List<NodeData> Nodes = new List<NodeData>();

    public void AddNodeData(NodeData nodeData)
    {
        Nodes.Add(nodeData);
    }

    public NodeData CreateNodeDataLayoutElementRoot()
    {
        return new NodeData { Type = NodeType.LayoutElementRoot };
    }

    public NodeData CreateNodeDataLayoutElement()
    {
        return new NodeData { Type = NodeType.LayoutElement };
    }

    public NodeData CreateNodeDataSystemEvent()
    {
        return new NodeData { Type = NodeType.SystemEvent };
    }
    
    public NodeData Find(string id)
    {
        return Nodes.Find(x => x.ID == id);
    }
}

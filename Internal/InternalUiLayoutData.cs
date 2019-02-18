using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityExpansion.UI;

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
        public UiLayoutPreset LayoutPreset;

        [SerializeField]
        public int X;

        [SerializeField]
        public int Y;
    }

    [SerializeField]
    public List<NodeData> Nodes = new List<NodeData>();

    [SerializeField]
    public List<UnityEngine.Object> RegisteredLayoutObjects = new List<UnityEngine.Object>();

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

    public void Register(UiLayoutObject layoutObject)
    {
        if(!RegisteredLayoutObjects.Contains(layoutObject))
        {
            Debug.LogError("REGISTERED " + layoutObject.name);

            UnityExpansion.Utilities.UtilityReflection.SetMemberValue
            (
                layoutObject,
                "_uniqueID",
                (layoutObject.GetInstanceID() < 0 ? "n" : "p") + Mathf.Abs(layoutObject.GetInstanceID())
            );

            RegisteredLayoutObjects.Add(layoutObject);
        }
    }

    public NodeData Find(string id)
    {
        return Nodes.Find(x => x.ID == id);
    }
}

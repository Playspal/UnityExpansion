using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InternalUiLayoutData : MonoBehaviour
{
    public class NodeData
    {
        public string AssetPath { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }

    [SerializeField]
    public List<NodeData> Nodes = new List<NodeData>();

    public void AddNodeData(string path, int x, int y)
    {
        NodeData nodeData = GetNodeData(path);

        if (nodeData == null)
        {
            nodeData = new NodeData();
            nodeData.AssetPath = path;
        }

        nodeData.X = x;
        nodeData.Y = y;
    }

    public NodeData GetNodeData(string path)
    {
        return Nodes.Find(x => x.AssetPath == path);
    }
}

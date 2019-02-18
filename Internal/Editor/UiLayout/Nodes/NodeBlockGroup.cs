using System.Collections.Generic;
using UnityEngine;
using UnityExpansion.Editor;

namespace UnityExpansionInternal.UiLayoutEditor
{
    public class NodeBlockGroup : EditorLayoutObject
    {
        public readonly Node Node;
        public readonly string GroupName;

        private EditorLayoutObjectText _title;
        private EditorLayoutObjectTexture _hline;

        private List<NodeConnectorInput> _input = new List<NodeConnectorInput>();
        private List<NodeConnectorOutput> _output = new List<NodeConnectorOutput>();

        public NodeBlockGroup(EditorLayout layout, Node node, string name) : base(layout, node.Width, 30)
        {
            Node = node;
            GroupName = name;

            _hline = new EditorLayoutObjectTexture(layout, Width, 1);
            _hline.Fill(node.ColorBackgroundBorder);
            _hline.SetParent(this);

            _title = new EditorLayoutObjectText(layout, Width, 20);
            _title.SetAlignment(UnityEngine.TextAnchor.MiddleLeft);
            _title.SetFontStyle(UnityEngine.FontStyle.Bold);
            _title.SetColor(UiLayoutEditorConfig.COLOR_NODE_LABEL);
            _title.SetText(GroupName);
            _title.SetParent(this);
            _title.Y = _hline.Height + 5;
            _title.X = 9;

            Refresh();
        }

        public void AddInput(string uniqueID, string method, int weight)
        {
            NodeConnectorInput item = new NodeConnectorInput(Layout, Node, method);
            item.SetWeight(weight);
            item.SetData(uniqueID, method);
            item.SetParent(this);
            
            _input.Add(item);
            _input.Sort((a, b) => a.Weight.CompareTo(b.Weight));

            Refresh();
        }

        public void AddOutput(string uniqueID, string method, int weight)
        {
            NodeConnectorOutput item = new NodeConnectorOutput(Layout, Node, method);
            item.SetWeight(weight);
            item.SetData(uniqueID, method);
            item.SetParent(this);

            _output.Add(item);
            _output.Sort((a, b) => a.Weight.CompareTo(b.Weight));

            Refresh();
        }

        private void Refresh()
        {
            int offset = _title.Y + _title.Height + 5;
            int height = offset;

            for (int i = 0; i < _input.Count; i++)
            {
                _input[i].Y = i * (_input[i].Height + 5) + offset;

                height = Mathf.Max(height, _input[i].Y + _input[i].Height);
            }

            for (int i = 0; i < _output.Count; i++)
            {
                _output[i].Y = i * (_output[i].Height + 5) + offset;

                height = Mathf.Max(height, _output[i].Y + _output[i].Height);
            }

            SetSize(Width, height);
        }
    }
}
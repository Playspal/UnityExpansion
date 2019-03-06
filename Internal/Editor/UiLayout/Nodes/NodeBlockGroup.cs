using System.Collections.Generic;

using UnityEngine;

using UnityExpansion.Editor;
using UnityExpansion.UI.Layout.Processor;

namespace UnityExpansionInternal.UiLayoutEditor
{
    public class NodeBlockGroup : EditorLayoutObject
    {
        public readonly NodeLayoutElement Node;
        public readonly string GroupName;

        public readonly bool IsMainGroup;

        private EditorLayoutObjectText _title;
        private EditorLayoutObjectTexture _hline;

        private List<NodeConnectorHandler> _handlers = new List<NodeConnectorHandler>();
        private List<NodeConnectorSender> _senders = new List<NodeConnectorSender>();

        public NodeBlockGroup(EditorLayout layout, NodeLayoutElement node, string name) : base(layout, node.Width, 30)
        {
            Node = node;
            GroupName = name;

            IsMainGroup = GroupName == UiLayoutProcessorAttribute.GROUP_MAIN;

            if (!IsMainGroup)
            {
                _hline = new EditorLayoutObjectTexture(layout, Width, 1);
                _hline.Fill(node.ColorBackgroundBorder);
                _hline.SetParent(this);

                _title = new EditorLayoutObjectText(layout, Width, 20);
                _title.SetAlignment(TextAnchor.MiddleLeft);
                _title.SetFontStyle(FontStyle.Bold);
                _title.SetColor(UiLayoutEditorConfig.COLOR_NODE_LABEL);
                _title.SetText(GroupName);
                _title.SetParent(this);
                _title.Y = _hline.Height + 5;
                _title.X = 9;
            }

            Refresh();
        }

        public void AddHandler(string uniqueID, string method, int weight)
        {
            NodeConnectorHandler item = null;// new NodeConnectorHandler(Layout, Node, method);

            switch(method)
            {
                case "Show":
                    item = new NodeConnectorHandlerShow(Layout, Node);
                    break;

                default:
                    item = new NodeConnectorHandler(Layout, Node, method);
                    break;
            }

            item.SetWeight(weight);
            item.SetData(uniqueID, method);
            item.SetParent(this);
            
            _handlers.Add(item);
            _handlers.Sort((a, b) => a.Weight.CompareTo(b.Weight));

            Refresh();
        }

        public void AddSender(string uniqueID, string method, int weight)
        {
            NodeConnectorSender item = new NodeConnectorSender(Layout, Node, method);
            item.SetWeight(weight);
            item.SetData(uniqueID, method);
            item.SetParent(this);

            _senders.Add(item);
            _senders.Sort((a, b) => a.Weight.CompareTo(b.Weight));

            Refresh();
        }

        private void Refresh()
        {
            int offset = IsMainGroup ? 0 : _title.Y + _title.Height + 5;
            int height = offset;

            for (int i = 0; i < _handlers.Count; i++)
            {
                _handlers[i].Y = i * (_handlers[i].Height + 5) + offset;

                height = Mathf.Max(height, _handlers[i].Y + _handlers[i].Height);
            }

            for (int i = 0; i < _senders.Count; i++)
            {
                _senders[i].Y = i * (_senders[i].Height + 5) + offset;

                height = Mathf.Max(height, _senders[i].Y + _senders[i].Height);
            }

            SetSize(Width, height);
        }
    }
}
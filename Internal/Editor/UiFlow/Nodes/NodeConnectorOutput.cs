using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityExpansion.Editor;

namespace UnityExpansionInternal.UiFlow
{
    public class NodeConnectorOutput : EditorLayoutObject
    {
        private const string COLOR_LABEL = "#999999";

        private const int HEIGHT = 15;
        private const int LABEL_OFFSET = 10;
        private const int CONNECTOR_OFFSET = 8;

        private NodeConnector _connector;
        private EditorLayoutObjectText _label;

        public NodeConnectorOutput(EditorLayout layout, int width, string label) : base(layout, width, HEIGHT)
        {
            _connector = new NodeConnector(layout, NodeConnector.Type.Output);
            _connector.SetParent(this);
            _connector.X = Width - CONNECTOR_OFFSET;
            _connector.Y = 0;

            _label = new EditorLayoutObjectText(layout, width - LABEL_OFFSET, Height);
            _label.SetAlignment(TextAnchor.MiddleRight);
            _label.SetColor(COLOR_LABEL);
            _label.SetText(label);
            _label.SetParent(this);
            _label.X = 0;
            _label.Y = -1;
        }
    }
}
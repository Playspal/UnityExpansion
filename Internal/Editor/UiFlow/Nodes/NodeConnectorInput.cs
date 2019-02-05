using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityExpansion.Editor;

namespace UnityExpansionInternal.UiFlow
{
    public class NodeConnectorInput : EditorLayoutObject
    {
        private const string COLOR_LABEL = "#999999";

        private const int HEIGHT = 15;
        private const int LABEL_X = 10;
        private const int CONNECTOR_X = -7;

        private NodeConnector _connector;
        private EditorLayoutObjectText _label;

        public NodeConnectorInput(EditorLayout layout, int width, string label) : base(layout, width, HEIGHT)
        {
            _connector = new NodeConnector(layout, NodeConnector.Type.Input);
            _connector.SetParent(this);
            _connector.X = CONNECTOR_X;
            _connector.Y = 0;

            _label = new EditorLayoutObjectText(layout, width - LABEL_X, Height);
            _label.SetAlignment(TextAnchor.MiddleLeft);
            _label.SetColor(COLOR_LABEL);
            _label.SetText(label);
            _label.SetParent(this);
            _label.X = LABEL_X;
        }
    }
}
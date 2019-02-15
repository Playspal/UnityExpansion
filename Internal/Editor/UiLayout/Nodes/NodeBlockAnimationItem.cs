using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityExpansion.Editor;

namespace UnityExpansionInternal.UiLayoutEditor
{
    public class NodeBlockAnimationItem : EditorLayoutObject
    {
        public readonly NodeConnectorInput InputPlay;

        public readonly NodeConnectorOutput OutputOnComplete;

        public NodeBlockAnimationItem(EditorLayout layout, Node node) : base(layout, node.Width, 20)
        {
            InputPlay = new NodeConnectorInput(layout, node, "Play Animation Name");
            InputPlay.SetParent(this);
            InputPlay.Y = 0;

            OutputOnComplete = new NodeConnectorOutput(layout, node, "OnComplete");
            OutputOnComplete.SetParent(this);
            OutputOnComplete.Y = 0;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTreeStuff
{
    public class Sequence : Node
    {
        protected List<Node> nodes = new List<Node>();

        public Sequence(List<Node> nodes)
        {
            this.nodes = nodes;
        }

        public override NodeState Evaluate()
        {
            foreach (var node in nodes)
            {
                switch (node.Evaluate())
                {
                    case NodeState.RUNNING:
                        nodeState = NodeState.RUNNING;
                        return nodeState;
                    case NodeState.FAILURE:
                        nodeState = NodeState.FAILURE;
                        return nodeState;
                }
            }
            nodeState = NodeState.SUCCESS;
            return nodeState;
        }
    }
}

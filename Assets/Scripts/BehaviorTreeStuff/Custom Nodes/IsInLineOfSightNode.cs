using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTreeStuff
{
    public class IsInLineOfSightNode : Node
    {
        private Transform amePosition;
        private Transform targetPosition;

        public IsInLineOfSightNode(Transform amePosition, Transform targetPosition)
        {
            this.amePosition = amePosition;
            this.targetPosition = targetPosition;
        }

        public override NodeState Evaluate()
        {
            throw new System.NotImplementedException();
        }
    }
}

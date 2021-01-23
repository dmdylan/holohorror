using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTreeStuff
{
    //Could just be an in range node that works for the player and waypoints
    //Might be the first of a branch that also checks for line of sight
    public class IsInRangeNode : Node
    {
        private AmeAI ameAI;
        private Transform targetPosition;

        public IsInRangeNode(AmeAI ameAI, Transform targetPosition)
        {
            this.ameAI = ameAI;
            this.targetPosition = targetPosition;
        }

        public override NodeState Evaluate()
        {
            float distance = Vector3.Distance(ameAI.transform.position, targetPosition.position);
            return distance <= ameAI.AmeStats.Range ? NodeState.SUCCESS : NodeState.FAILURE;
        }
    }
}

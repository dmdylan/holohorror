using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTreeStuff
{
    //Could just be an in range node that works for the player and waypoints
    //Might be the first of a branch that also checks for line of sight
    public class IsInRangeNode : Node
    {
        private Transform amePosition;
        private Transform targetPosition;
        private float range;

        public IsInRangeNode(Transform amePosition, Transform targetPosition, float range)
        {
            this.amePosition = amePosition;
            this.targetPosition = targetPosition;
            this.range = range;
        }

        public override NodeState Evaluate()
        {
            float distance = Vector3.Distance(amePosition.position, targetPosition.position);
            return distance <= range ? NodeState.SUCCESS : NodeState.FAILURE;
        }
    }
}

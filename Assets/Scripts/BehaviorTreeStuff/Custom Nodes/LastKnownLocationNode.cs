using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTreeStuff
{
    public class LastKnownLocationNode : Node
    {
        private AmeAI ameAI;

        public LastKnownLocationNode(AmeAI ameAI)
        {
            this.ameAI = ameAI;
        }

        public override NodeState Evaluate()
        {
            if (ameAI.PlayerLastKnownLocation == null)
                return NodeState.FAILURE;

            ameAI.NavMeshAgent.SetDestination(ameAI.PlayerLastKnownLocation.position);
            return NodeState.SUCCESS;

            //if(ameAI.NavMeshAgent.remainingDistance >= .1f)
            //{
            //    Debug.Log("Moving to last known location");
            //    return NodeState.RUNNING;
            //}
            //else
            //{
            //    Debug.Log("Reached last known location");
            //    return NodeState.SUCCESS;
            //}
        }
    }
}
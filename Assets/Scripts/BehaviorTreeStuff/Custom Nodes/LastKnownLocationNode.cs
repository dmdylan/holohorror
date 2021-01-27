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

            if(ameAI.NavMeshAgent.remainingDistance >= 1.5f)
            {
                return NodeState.RUNNING;
            }

            ameAI.PlayerLastKnownLocation = null;
            return NodeState.SUCCESS;
        }
    }
}
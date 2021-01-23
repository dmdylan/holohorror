using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorTreeStuff
{
    public class MoveToWaypointNode : Node
    {
        private AmeAI ameAI;

        public MoveToWaypointNode(AmeAI ameAI)
        {
            this.ameAI = ameAI;
        }

        public override NodeState Evaluate()
        {
            if (ameAI.CurrentWaypoint == null)
                return NodeState.FAILURE;

            ameAI.NavMeshAgent.speed = ameAI.AmeStats.NormalSpeed;
            ameAI.NavMeshAgent.isStopped = false;

            if(ameAI.NavMeshAgent.remainingDistance > .1f)
            {
                return NodeState.RUNNING;
            }

            ameAI.NavMeshAgent.SetDestination(ameAI.CurrentWaypoint.position);
            return NodeState.SUCCESS;
        }
    }
}

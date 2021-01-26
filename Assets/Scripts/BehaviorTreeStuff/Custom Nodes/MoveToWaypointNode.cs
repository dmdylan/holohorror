using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorTreeStuff
{
    public class MoveToWaypointNode : Node
    {
        private AmeAI ameAI;
        private bool isMovingToWaypoint = false;

        public MoveToWaypointNode(AmeAI ameAI)
        {
            this.ameAI = ameAI;
        }

        public override NodeState Evaluate()
        {
            if (ameAI.CurrentWaypoint == null)
                return NodeState.FAILURE;

            if(isMovingToWaypoint == false)
            {
                ameAI.NavMeshAgent.SetDestination(ameAI.CurrentWaypoint.position);
                ameAI.NavMeshAgent.speed = ameAI.AmeStats.NormalSpeed;
                ameAI.NavMeshAgent.isStopped = false;
                isMovingToWaypoint = true;
                return NodeState.RUNNING;
            }

            if(ameAI.NavMeshAgent.remainingDistance > .1f)
            {
                return NodeState.RUNNING;
            }

            isMovingToWaypoint = false;
            ameAI.NeedsToSelectWaypoint = true;
            return NodeState.SUCCESS;
        }
    }
}

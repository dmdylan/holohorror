using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorTreeStuff
{
    public class MoveToWaypointNode : Node
    {
        private AmeAI ameAI;
        private float speed;

        public MoveToWaypointNode(AmeAI ameAI, float speed)
        {
            this.ameAI = ameAI;
            this.speed = speed;
        }

        public override NodeState Evaluate()
        {
            if (ameAI.CurrentWaypoint == null)
                return NodeState.FAILURE;

            ameAI.NavMeshAgent.speed = speed;
            ameAI.NavMeshAgent.isStopped = false;

            if(ameAI.NavMeshAgent.remainingDistance > .1f)
            {
                Debug.Log("Moving to waypoint");
                return NodeState.RUNNING;
            }

            ameAI.NavMeshAgent.SetDestination(ameAI.CurrentWaypoint.position);
            Debug.Log("Move to waypoint success");
            return NodeState.SUCCESS;
        }
    }
}

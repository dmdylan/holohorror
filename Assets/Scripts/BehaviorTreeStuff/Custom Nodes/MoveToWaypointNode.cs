using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorTreeStuff
{
    public class MoveToWaypointNode : Node
    {
        private AmeAI ameAI;
        private NavMeshAgent navMeshAgent;
        private float speed;

        public MoveToWaypointNode(AmeAI ameAI, NavMeshAgent navMeshAgent, float speed)
        {
            this.ameAI = ameAI;
            this.navMeshAgent = navMeshAgent;
            this.speed = speed;
        }

        public override NodeState Evaluate()
        {
            navMeshAgent.speed = speed;
            float distance = Vector3.Distance(ameAI.CurrentWaypoint.position, navMeshAgent.transform.position);
            Debug.Log(ameAI.CurrentWaypoint);
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(ameAI.CurrentWaypoint.position);
            if(navMeshAgent.remainingDistance < .2f)
            {
                Debug.Log("Moved to waypoint");
                return NodeState.SUCCESS;
            }
            return NodeState.RUNNING;
        }
    }
}

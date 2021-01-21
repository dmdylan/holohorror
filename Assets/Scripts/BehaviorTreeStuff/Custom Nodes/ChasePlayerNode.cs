using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorTreeStuff
{
    public class ChasePlayerNode : Node
    {
        private Transform playerPosition;
        private NavMeshAgent navMeshAgent;
        private float chaseSpeed;

        public ChasePlayerNode(Transform playerPosition, NavMeshAgent navMeshAgent, float chaseSpeed)
        {
            this.playerPosition = playerPosition;
            this.navMeshAgent = navMeshAgent;
            this.chaseSpeed = chaseSpeed;
        }

        public override NodeState Evaluate()
        {
            float distance = Vector3.Distance(playerPosition.position, navMeshAgent.transform.position);
            if(distance > 1.5f)
            {
                navMeshAgent.speed = chaseSpeed;
                navMeshAgent.SetDestination(playerPosition.position);
                return NodeState.RUNNING;
            }
            else if(distance <= 1.5f)
            {
                GameEvents.Instance.GameOver();
                return NodeState.SUCCESS;
            }
            else
            {
                return NodeState.FAILURE;
            }
        }
    }
}

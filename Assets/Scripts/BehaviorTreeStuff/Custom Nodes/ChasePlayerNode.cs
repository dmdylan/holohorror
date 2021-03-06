﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//TODO: Could enable the always face player script
namespace BehaviorTreeStuff
{
    public class ChasePlayerNode : Node
    {
        private Transform playerPosition;
        private AmeAI ameAI;

        public ChasePlayerNode(Transform playerPosition, AmeAI ameAI)
        {
            this.playerPosition = playerPosition;
            this.ameAI = ameAI;
        }

        public override NodeState Evaluate()
        {
            float distance = Vector3.Distance(playerPosition.position, ameAI.NavMeshAgent.transform.position);
            ameAI.NavMeshAgent.speed = ameAI.AmeStats.ChaseSpeed;

            if(distance > 1.5f)
            {
                Debug.Log("Chasing player");
                ameAI.IsChasing = true;
                ameAI.NavMeshAgent.SetDestination(playerPosition.position);
                ameAI.PlayerLastKnownLocation = playerPosition;
                return NodeState.RUNNING;
            }
            else if(distance <= 1.5f)
            {
                GameEvents.Instance.GameOver();
                ameAI.IsChasing = false;
                ameAI.AudioSource.Stop();
                ameAI.enabled = false;
                return NodeState.SUCCESS;
            }
            else
            {
                ameAI.IsChasing = false;
                return NodeState.FAILURE;
            }
        }
    }
}

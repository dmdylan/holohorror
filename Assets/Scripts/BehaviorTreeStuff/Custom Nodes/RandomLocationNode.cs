﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorTreeStuff
{
    public class RandomLocationNode : Node
    {
        private Transform playerPosition;
        private AmeAI ameAI;
        private bool isWaiting = false;

        //TODO: Might just have it chase player unless they can find hiding spot
        public RandomLocationNode(Transform playerPosition, AmeAI ameAI)
        {
            this.playerPosition = playerPosition;
            this.ameAI = ameAI;
        }

        //TODO: Pretty sure it picks once and then immediately moves to waypoint as opposed to picks, waiting, picking again, then moving
        //TODO: Never returns a success lol
        public override NodeState Evaluate()
        {
            if(Physics.Linecast(ameAI.NavMeshAgent.transform.position, playerPosition.position, out RaycastHit hit))
            {
                if(hit.collider.CompareTag("Wall"))
                {
                    return NodeState.FAILURE;
                }
            }

            Debug.Log("Picking random location near player");
            GameManager.Instance.StartCoroutine(PickNewRandomLocation());
            return NodeState.RUNNING;   
        }

        private IEnumerator PickNewRandomLocation()
        {
            if (isWaiting == true)
                yield break;

            ameAI.NavMeshAgent.speed = ameAI.AmeStats.NormalSpeed;
            ameAI.NavMeshAgent.SetDestination(GetNewRandomLocation());
            isWaiting = true;
            yield return new WaitForSeconds(ameAI.AmeStats.NoLOSWaitTime);
            isWaiting = false;
        }

        private Vector3 GetNewRandomLocation()
        {
            //gets random location near player within the wander radius
            Vector3 randomDirection = playerPosition.position + UnityEngine.Random.insideUnitSphere * ameAI.AmeStats.RandomWanderRadius;

            //Looks for random closest point on navmesh
            NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, ameAI.AmeStats.RandomWanderRadius, 1);
            Vector3 finalPosition = hit.position;

            Debug.DrawLine(ameAI.transform.position, finalPosition, Color.red, 20);

            return finalPosition;
        }
    }
}

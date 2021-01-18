using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace BehaviorTreeStuff
{
    public class RandomLocationNode : Node
    {
        private Transform playerPosition;
        private NavMeshAgent navMeshAgent;
        private float moveSpeed;
        private float waitTime;
        private float wanderRadius;
        private bool isWaiting = false;

        public RandomLocationNode(Transform playerPosition, NavMeshAgent navMeshAgent, float moveSpeed, float waitTime, float wanderRadius)
        {
            this.playerPosition = playerPosition;
            this.navMeshAgent = navMeshAgent;
            this.moveSpeed = moveSpeed;
            this.waitTime = waitTime;
            this.wanderRadius = wanderRadius;
        }

        public override NodeState Evaluate()
        {
            if(Physics.Linecast(navMeshAgent.transform.position, playerPosition.position, out RaycastHit hit))
            {
                if(hit.collider.CompareTag("Wall"))
                {
                    return NodeState.FAILURE;
                }
            }

            GameManager.Instance.StartCoroutine(PickNewRandomLocation());
            return NodeState.RUNNING;   
        }

        private IEnumerator PickNewRandomLocation()
        {
            if (isWaiting == true)
                yield break;

            navMeshAgent.speed = moveSpeed;
            navMeshAgent.SetDestination(GetNewRandomLocation());
            isWaiting = true;
            yield return new WaitForSeconds(waitTime);
            isWaiting = false;
        }

        private Vector3 GetNewRandomLocation()
        {
            //gets random location near player within the wander radius
            Vector3 randomDirection =  UnityEngine.Random.insideUnitSphere * wanderRadius;

            randomDirection += playerPosition.position;
            NavMeshHit hit;

            //Looks for random closest point on navmesh
            NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1);
            Vector3 finalPosition = hit.position;

            return finalPosition;
        } 
    }
}

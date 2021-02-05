using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTreeStuff
{
    public class IsInLineOfSightNode : Node
    {
        private AmeAI ameAI;
        private Transform targetPosition;
        private bool isWaiting = false;

        public IsInLineOfSightNode(AmeAI ameAI, Transform targetPosition)
        {
            this.ameAI = ameAI;
            this.targetPosition = targetPosition;
        }

        public override NodeState Evaluate()
        {
            Vector3 heading = targetPosition.position - ameAI.transform.position;
            float front = Vector3.Dot(heading, ameAI.transform.forward);

            //Checks if player is not blocked by obstacles and is in front of ame
            if(Physics.Linecast(ameAI.transform.position, targetPosition.position, out RaycastHit hit) && front >= 0)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    Debug.Log("In line of sight node success");
                    return NodeState.SUCCESS;   
                }
                return NodeState.FAILURE;
            }
            //checks if player is behind ame but within certain distance
            else if(Vector3.Distance(ameAI.transform.position, targetPosition.position) <= ameAI.AmeStats.PlayerBackDetectionRange && front <= 0)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    Debug.Log("In line of sight node success");
                    return NodeState.SUCCESS;
                }
                return NodeState.FAILURE;
            }
            else
            {
                Debug.Log("Is in line of sight node failed");
                //GameManager.Instance.StartCoroutine(ChaseTime());
                return NodeState.FAILURE;
            }
        }

        IEnumerator ChaseTime()
        {
            if (isWaiting == true)
                yield break;

            isWaiting = true;
            yield return new WaitForSeconds(ameAI.AmeStats.NoLOSChaseTime);
            isWaiting = false;
            yield return NodeState.FAILURE;
        }
    }
}

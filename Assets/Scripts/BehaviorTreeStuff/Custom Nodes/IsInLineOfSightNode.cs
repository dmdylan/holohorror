using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTreeStuff
{
    public class IsInLineOfSightNode : Node
    {
        private Transform amePosition;
        private Transform targetPosition;
        private float noLineOfSightChaseTime;
        private bool isWaiting = false;

        public IsInLineOfSightNode(Transform amePosition, Transform targetPosition, float noLineOfSightChaseTime)
        {
            this.amePosition = amePosition;
            this.targetPosition = targetPosition;
            this.noLineOfSightChaseTime = noLineOfSightChaseTime;
        }

        public override NodeState Evaluate()
        {
            Vector3 heading = targetPosition.position - amePosition.position;
            float front = Vector3.Dot(heading, amePosition.forward);

            if(!Physics.Linecast(amePosition.position, targetPosition.position) && front >= 0)
            {
                return NodeState.SUCCESS;   
            }
            else
            {
                GameManager.Instance.StartCoroutine(ChaseTime());
                return NodeState.RUNNING;
            }
        }

        IEnumerator ChaseTime()
        {
            if (isWaiting == true)
                yield break;

            isWaiting = true;
            yield return new WaitForSeconds(noLineOfSightChaseTime);
            isWaiting = false;
            yield return NodeState.FAILURE;
        }
    }
}

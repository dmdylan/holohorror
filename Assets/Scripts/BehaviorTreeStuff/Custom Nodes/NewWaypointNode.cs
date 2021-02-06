using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTreeStuff
{
    public class NewWaypointNode : Node
    {
        private AmeAI ameAI;

        public NewWaypointNode(AmeAI ameAI)
        {
            this.ameAI = ameAI;
        }

        public override NodeState Evaluate()
        {
            if (ameAI.NeedsToSelectWaypoint == false)
                return NodeState.RUNNING;

            if(ameAI.NeedsToSelectWaypoint == true)
            {
                IterateThroughWaypoints();
                Debug.Log(ameAI.CurrentWaypoint);
                ameAI.NeedsToSelectWaypoint = false;
                return NodeState.SUCCESS;
            }

            return NodeState.RUNNING;
        }

        private void IterateThroughWaypoints()
        {
            List<Waypoint> possibleWaypoints = ameAI.GetNearestWaypoints();

            possibleWaypoints.Remove(ameAI.CurrentWaypoint);
            possibleWaypoints.Remove(ameAI.PreviousWaypoint);

            //Checks if there is an obstacle between ame and the waypoint, if not it checks if it is behind ame or not
            for(int i = 0; i <= possibleWaypoints.Count-1; i++)
            {
                if (Physics.Linecast(ameAI.transform.position, possibleWaypoints[i].transform.position, out RaycastHit hit))
                {
                    //Debug.Log(possibleWaypoints[i]);
                    possibleWaypoints.Remove(possibleWaypoints[i]);
                }
                else
                {
                    Vector3 heading = possibleWaypoints[i].transform.position - ameAI.transform.position;
                    float front = Vector3.Dot(heading, ameAI.transform.forward);
                    
                    if(front <= -.5f)
                    {
                        //Debug.Log(possibleWaypoints[i] + " " + front);
                        possibleWaypoints.Remove(possibleWaypoints[i]);
                    }
                    else
                    {
                        //Debug.Log(possibleWaypoints[i] + " " + front);
                        continue;
                    }
                }
            }

            foreach(Waypoint waypoint in possibleWaypoints)
            {
                Debug.Log(waypoint);
            }
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTreeStuff
{
    public class NewWaypointNode : Node
    {
        private AmeAI ameAI;
        private List<Transform> wayPoints = new List<Transform>();
        private bool isSelectingWaypoint = true;

        public NewWaypointNode(AmeAI ameAI)
        {
            this.ameAI = ameAI;
        }

        public override NodeState Evaluate()
        {
            foreach (var waypoint in ameAI.WayPoints)
            {
                if (Vector3.Distance(waypoint.position, ameAI.transform.position) <= ameAI.AmeStats.Range)
                {
                    wayPoints.Add(waypoint);
                }
            }

            if(isSelectingWaypoint == true)
            {
                IterateThroughWaypoints();
                wayPoints.Clear();
                return NodeState.SUCCESS;
            }

            return NodeState.RUNNING;
        }

        //TODO: Both current and previous waypoint are set to the same waypoint
        private void IterateThroughWaypoints()
        {
            isSelectingWaypoint = false;
            List<Transform> possibleWaypoints = new List<Transform>();
            //ameAI.PreviousWaypoint = ameAI.CurrentWaypoint;
            //Transform tempWaypoint;

            foreach (Transform waypoint in wayPoints)
            {
                if (!Physics.Linecast(ameAI.transform.position, waypoint.position))
                {
                    possibleWaypoints.Add(waypoint);
                }
            }

            //If it is only two, only possible option should be current/previous waypoints
            //if (possibleWaypoints.Count.Equals(2))
            //{
            //    tempWaypoint = ameAI.CurrentWaypoint;
            //    ameAI.CurrentWaypoint = ameAI.PreviousWaypoint;
            //    ameAI.PreviousWaypoint = tempWaypoint;
            //        
            //    return;
            //}

            foreach(Transform waypoint in possibleWaypoints)
            {
                Vector3 heading = waypoint.position - ameAI.transform.position;
                float front = Vector3.Dot(heading, ameAI.transform.forward);

                if(waypoint == possibleWaypoints[possibleWaypoints.Count - 1])
                {
                    //ameAI.PreviousWaypoint = ameAI.CurrentWaypoint;
                    ameAI.CurrentWaypoint = waypoint;
                    break;
                }

                if (front >= 0f && waypoint != ameAI.CurrentWaypoint && ameAI.PreviousWaypoint)
                {
                    if(Random.Range(0,1) == 1)
                    {
                        //ameAI.PreviousWaypoint = ameAI.CurrentWaypoint;
                        ameAI.CurrentWaypoint = waypoint;
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
            }        
            isSelectingWaypoint = true;
        }
    }
}

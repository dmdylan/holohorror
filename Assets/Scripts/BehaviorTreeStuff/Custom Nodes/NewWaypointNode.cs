using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTreeStuff
{
    public class NewWaypointNode : Node
    {
        private AmeAI ameAI;
        private Transform tempWaypoint = null;
        private List<Transform> wayPoints = new List<Transform>();

        public NewWaypointNode(AmeAI ameAI)
        {
            this.ameAI = ameAI;
        }

        public override NodeState Evaluate()
        {
            if (ameAI.NeedsToSelectWaypoint == false)
                return NodeState.RUNNING;

            foreach (var waypoint in ameAI.WayPoints)
            {
                if (Vector3.Distance(waypoint.position, ameAI.transform.position) <= ameAI.AmeStats.WaypointRange)
                {
                    wayPoints.Add(waypoint);
                }
            }

            if(ameAI.NeedsToSelectWaypoint == true)
            {
                tempWaypoint = ameAI.CurrentWaypoint;
                IterateThroughWaypoints();
                wayPoints.Clear();
                ameAI.NeedsToSelectWaypoint = false;
                Debug.Log(ameAI.CurrentWaypoint);
                return NodeState.SUCCESS;
            }

            return NodeState.RUNNING;
        }

        private void IterateThroughWaypoints()
        {
            List<Transform> possibleWaypoints = new List<Transform>();

            foreach (Transform waypoint in wayPoints)
            {
                if (!Physics.Linecast(ameAI.transform.position, waypoint.position))
                {
                    possibleWaypoints.Add(waypoint);
                }
            }

            //If it is only two, only possible option should be current/previous waypoints
            if (possibleWaypoints.Count.Equals(2) && ameAI.CurrentWaypoint != null)
            {
                ameAI.CurrentWaypoint = ameAI.PreviousWaypoint;
                ameAI.PreviousWaypoint = tempWaypoint;
                    
                return;
            }

            possibleWaypoints.Remove(ameAI.CurrentWaypoint);
            possibleWaypoints.Remove(ameAI.PreviousWaypoint);

            foreach(Transform waypoint in possibleWaypoints)
            {
                Vector3 heading = waypoint.position - ameAI.transform.position;
                float front = Vector3.Dot(heading, ameAI.transform.forward);
                int random = Random.Range(0, 2);


                if(waypoint == possibleWaypoints[possibleWaypoints.Count - 1])
                {
                    ameAI.CurrentWaypoint = waypoint;
                    break;
                }

                if (front >= 0)// && waypoint != ameAI.CurrentWaypoint || ameAI.PreviousWaypoint)
                {
                    if(random >= 1)
                    {
                        ameAI.CurrentWaypoint = waypoint;
                        break;
                    }
                }
            }                   
        }
    }
}

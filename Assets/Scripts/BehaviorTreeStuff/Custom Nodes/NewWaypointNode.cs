using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTreeStuff
{
    public class NewWaypointNode : Node
    {
        private AmeAI ameAI;
        private List<Transform> wayPoints = new List<Transform>();

        public NewWaypointNode(AmeAI ameAI)
        {
            this.ameAI = ameAI;
        }

        //TODO: choose nodes in front of player
        public override NodeState Evaluate()
        {
            foreach (var waypoint in ameAI.WayPoints)
            {
                if (Vector3.Distance(waypoint.position, ameAI.transform.position) <= ameAI.AmeStats.Range)
                {
                    wayPoints.Add(waypoint);
                }
            }

            IterateThroughWaypoints();
            wayPoints.Clear();
            //Debug.Log("new waypoint chosen");
            return NodeState.SUCCESS;
        }

        private void FindFirstWayPoint()
        {
            ameAI.CurrentWaypoint = ameAI.WayPoints[0];
            ameAI.PreviousWaypoint = ameAI.WayPoints[0];

            foreach (Transform waypoint in ameAI.WayPoints)
            {
                if (Vector3.Distance(ameAI.transform.position, waypoint.position) <=
                   Vector3.Distance(ameAI.transform.position, ameAI.CurrentWaypoint.position))
                {
                    ameAI.CurrentWaypoint = waypoint;
                }
            }

            ameAI.PreviousWaypoint = ameAI.CurrentWaypoint;
        }

        private void IterateThroughWaypoints()
        {
            List<Transform> possibleWaypoints = new List<Transform>();
            Transform tempWaypoint;

            foreach (Transform waypoint in wayPoints)
            {
                if (!Physics.Linecast(ameAI.transform.position, waypoint.position))
                {
                    possibleWaypoints.Add(waypoint);
                }
            }

            //If it is only two, only possible option should be current/previous waypoints
            if (possibleWaypoints.Count.Equals(2))
            {
                tempWaypoint = ameAI.CurrentWaypoint;
                ameAI.CurrentWaypoint = ameAI.PreviousWaypoint;
                ameAI.PreviousWaypoint = tempWaypoint;

                Debug.Log("Only two waypoints, returning to " + ameAI.CurrentWaypoint);

                return;
            }

            //TODO: Keeps choosing the same waypoint
            foreach(Transform waypoint in possibleWaypoints)
            {
                Vector3 heading = waypoint.position - ameAI.transform.position;
                float front = Vector3.Dot(heading, ameAI.transform.forward);

                Debug.Log(waypoint);

                //if (waypoint == ameAI.CurrentWaypoint || ameAI.PreviousWaypoint)
                //    continue;

                if (front >= 0f && waypoint != ameAI.CurrentWaypoint || ameAI.PreviousWaypoint)
                {
                    ameAI.PreviousWaypoint = ameAI.CurrentWaypoint;
                    ameAI.CurrentWaypoint = waypoint;
                    return;
                }
            }        
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTreeStuff
{
    public class NewWaypointNode : Node
    {
        private float wayPointRange;
        private AmeAI ameAI;
        private List<Transform> wayPoints = new List<Transform>();

        public NewWaypointNode(AmeAI ameAI, float wayPointRange)
        {
            this.ameAI = ameAI;
            this.wayPointRange = wayPointRange;
        }

        public override NodeState Evaluate()
        {
            foreach (var waypoint in ameAI.WayPoints)
            {
                if (Vector3.Distance(waypoint.position, ameAI.transform.position) <= wayPointRange)
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

            foreach(Transform waypoint in wayPoints)
            {
                if (!Physics.Linecast(ameAI.transform.position, waypoint.position))
                    possibleWaypoints.Add(waypoint);
            }

            Transform newWaypoint = possibleWaypoints[Random.Range(0, possibleWaypoints.Count-1)];

            if (newWaypoint == ameAI.PreviousWaypoint) 
            {
                if(Random.Range(0, 10) == 0)
                {
                    ameAI.PreviousWaypoint = ameAI.CurrentWaypoint;
                    ameAI.CurrentWaypoint = newWaypoint;
                    //Debug.Log("Returning to previous waypoint");
                    return;
                }
                else
                {
                    possibleWaypoints.Remove(newWaypoint);
                    if(possibleWaypoints.Count == 0)
                    {
                        Transform temp = ameAI.CurrentWaypoint;
                        ameAI.CurrentWaypoint = ameAI.PreviousWaypoint;
                        ameAI.PreviousWaypoint = temp;
                        return;
                    }
                    newWaypoint = possibleWaypoints[Random.Range(0, possibleWaypoints.Count-1)];
                }
            }

            ameAI.PreviousWaypoint = ameAI.CurrentWaypoint;
            ameAI.CurrentWaypoint = newWaypoint;
            //Debug.Log("Moving to new waypoint");          
        }
    }
}

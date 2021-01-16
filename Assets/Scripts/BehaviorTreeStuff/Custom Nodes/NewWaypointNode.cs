using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTreeStuff
{
    public class NewWaypointNode : Node
    {
        private Transform amePosition;
        private List<Transform> wayPoints;
        private float wayPointRange;
        private AmeAI ameAI;

        public NewWaypointNode(AmeAI ameAI, Transform amePosition, List<Transform> wayPoints, float wayPointRange)
        {
            this.ameAI = ameAI;
            this.amePosition = amePosition;
            this.wayPoints = wayPoints;
            this.wayPointRange = wayPointRange;
        }

        public override NodeState Evaluate()
        {
            if(ameAI.CurrentWaypoint == null)
            {
                FindFirstWayPoint();
                return NodeState.SUCCESS;
            }
            else
            {
                Debug.Log("Search for a new waypoint");
                var colliders = Physics.OverlapSphere(amePosition.position, wayPointRange);
                List<GameObject> waypoints = new List<GameObject>();

                foreach(var collider in colliders)
                {
                    if(collider.gameObject.tag == "Waypoint")
                    {
                        waypoints.Add(collider.gameObject);
                        Debug.Log(collider.gameObject.name);
                    }
                }

                IterateThroughWaypoints(waypoints);

                return NodeState.SUCCESS;
            }
        }

        private void FindFirstWayPoint()
        {
            ameAI.CurrentWaypoint = wayPoints[0];
            ameAI.CurrentWaypoint = wayPoints[0];

            foreach (Transform waypoint in wayPoints)
            {
                if (Vector3.Distance(amePosition.position, waypoint.position) <=
                   Vector3.Distance(amePosition.position, ameAI.CurrentWaypoint.position))
                {
                    ameAI.CurrentWaypoint = waypoint;
                }
            }

            ameAI.PreviousWaypoint = ameAI.CurrentWaypoint;
        }

        private void IterateThroughWaypoints(List<GameObject> wayPoints)
        {
            Transform tempWayPoint;

            foreach (GameObject waypoint in wayPoints)
            {
                if (Physics.Linecast(amePosition.position, waypoint.transform.position))
                    continue;

                if (wayPoints.Count >= 2 && waypoint.transform == ameAI.PreviousWaypoint)
                {
                    if (Random.Range(0, 3) == 0)
                    {
                        tempWayPoint = ameAI.CurrentWaypoint;
                        ameAI.CurrentWaypoint = ameAI.PreviousWaypoint;
                        ameAI.PreviousWaypoint = tempWayPoint;
                        break;
                    }
                }
                else if (wayPoints.Count >= 2 && waypoint.transform != ameAI.PreviousWaypoint && waypoint.transform != ameAI.CurrentWaypoint)
                {
                    if (Random.Range(0, 1) == 0) 
                    {
                        ameAI.PreviousWaypoint = ameAI.CurrentWaypoint;
                        ameAI.CurrentWaypoint = waypoint.transform;
                        break;
                    }
                }
                else if (wayPoints.Count == 2)
                {
                    if(waypoint == ameAI.PreviousWaypoint)
                    {
                        ameAI.PreviousWaypoint = ameAI.CurrentWaypoint;
                        ameAI.CurrentWaypoint = waypoint.transform;
                    }
                    else
                        continue;
                }
                else if (wayPoints.Count == 1)
                {
                    ameAI.CurrentWaypoint = waypoint.transform;
                }
                //TODO: Probably need another clause
            }

            //previousWaypoint = currentWaypoint;
        }
    }
}

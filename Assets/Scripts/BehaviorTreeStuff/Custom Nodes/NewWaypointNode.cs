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
                ameAI.NeedsToSelectWaypoint = false;
                return NodeState.SUCCESS;
            }

            return NodeState.RUNNING;
        }

        private void IterateThroughWaypoints()
        {
            List<Waypoint> possibleWaypoints = ameAI.WayPoints;
            List<Waypoint> goodWaypoints = new List<Waypoint>();

            if(possibleWaypoints.Count <= 1)
            {
                ameAI.CurrentWaypoint = ameAI.PreviousWaypoint;
                return;
            }

            foreach(Waypoint waypoint in possibleWaypoints)
            {
                Vector3 heading = waypoint.transform.position - ameAI.transform.position;
                float front = Vector3.Dot(heading, ameAI.transform.forward);
                
                if(front >= -.5f)
                {
                    goodWaypoints.Add(waypoint);
                }
            }

            foreach (Waypoint waypoint in goodWaypoints)
            {
                if (goodWaypoints.Count == 1)
                {
                    ameAI.CurrentWaypoint = waypoint;
                    return;
                }

                int random = Random.Range(0, 2);

                if (random >= 1 || waypoint == goodWaypoints[goodWaypoints.Count-1])
                {
                    ameAI.CurrentWaypoint = waypoint;
                    return;
                }
            }
        }
    }
}

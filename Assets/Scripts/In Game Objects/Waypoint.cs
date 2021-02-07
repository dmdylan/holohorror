using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public AmeSO ameStats;
    private List<Waypoint> waypointsInRange;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<FirstPersonAIO>())
            return; 

        other.TryGetComponent(out AmeAI ameAI);
        ameAI.WayPoints = waypointsInRange;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<FirstPersonAIO>())
            return;

        other.TryGetComponent(out AmeAI ameAi);
        ameAi.PreviousWaypoint = this;
    }

    private void Start()
    {
        waypointsInRange = new List<Waypoint>();
        foreach (Collider collider in Physics.OverlapSphere(transform.position, ameStats.WaypointRange))
        {
            if (Physics.Linecast(transform.position, collider.transform.position))
                continue;

            if (collider.TryGetComponent(out Waypoint waypoint))
            {
                if (waypoint == this)
                    continue;

                waypointsInRange.Add(waypoint);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        if (GameManager.Instance.isDebug != true)
            return;

        foreach(Collider collider in Physics.OverlapSphere(transform.position, ameStats.WaypointRange))
        {
            if (Physics.Linecast(transform.position, collider.transform.position))
                continue;

            if (collider.TryGetComponent(out Waypoint waypoint))
            {
                Gizmos.DrawLine(transform.position, waypoint.transform.position);
            }
        }
    }
}

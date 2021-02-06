using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public AmeSO ameStats;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        other.TryGetComponent(out AmeAI ameAi);
        ameAi.PreviousWaypoint = this;
    }

    private void OnDrawGizmosSelected()
    {
        if (GameManager.Instance.isDebug != true)
            return;

        foreach(Collider collider in Physics.OverlapSphere(transform.position, ameStats.WaypointRange))
        {
            if (collider.TryGetComponent(out Waypoint waypoint))
            {
                Gizmos.DrawLine(transform.position, collider.transform.position);
            }
        }
    }
}

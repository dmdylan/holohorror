using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public AmeSO ameStats;

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, ameStats.WaypointRange);
    }
}

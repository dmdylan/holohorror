using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorTreeStuff;

public class AmeAI : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    [SerializeField] private List<Transform> wayPoints = null;
    private Transform previousWaypoint = null;
    private Transform currentWaypoint = null;
    private Node topNode = null;
    public AmeSO AmeStats = null;

    public Transform PreviousWaypoint { get => previousWaypoint; set => previousWaypoint = value; }
    public Transform CurrentWaypoint { get => currentWaypoint; set => currentWaypoint = value; }

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count - 1)].position);
        StartCoroutine(SelectAWayPoint());
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, AmeStats.Range);
    }

    private IEnumerator SelectAWayPoint()
    {
        Debug.Log(navMeshAgent.destination);
        yield return new WaitForSeconds(2f);
        if(navMeshAgent.remainingDistance <= 1f)
        {
            navMeshAgent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count - 1)].position);
            StartCoroutine(SelectAWayPoint());
        }
        else
        {
            StartCoroutine(SelectAWayPoint());
        }
    }
}

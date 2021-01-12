using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AmeMovement : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    [SerializeField] private List<Transform> wayPoints = null;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count - 1)].position);
        StartCoroutine(SelectAWayPoint());
    }

    // Update is called once per frame
    void Update()
    {
        
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

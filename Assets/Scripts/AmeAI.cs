using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorTreeStuff;

public class AmeAI : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    [SerializeField] private Transform playerTransform = null;
    [SerializeField] private List<Transform> wayPoints = null;
    private Transform previousWaypoint = null;
    private Transform currentWaypoint = null;
    private Node topNode = null;
    public AmeSO AmeStats = null;

    public Transform PreviousWaypoint { get => previousWaypoint; set => previousWaypoint = value; }
    public Transform CurrentWaypoint { get => currentWaypoint; set => currentWaypoint = value; }
    public List<Transform> WayPoints { get => wayPoints; set => wayPoints = value; }
    public NavMeshAgent NavMeshAgent => navMeshAgent;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        //navMeshAgent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count - 1)].position);
        //StartCoroutine(SelectAWayPoint());
        navMeshAgent.autoBraking = false;
        InitializeBehaviorTree();
    }

    private void Update()
    {
        topNode.Evaluate();
        Debug.Log(topNode.NodeState);
        //Debug.Log($"Previous waypoint: {PreviousWaypoint}");
        //Debug.Log($"Current waypoint: {CurrentWaypoint}");
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

    private void InitializeBehaviorTree()
    {
        IsInRangeNode isInRangeNode = new IsInRangeNode(transform, playerTransform, AmeStats.Range);
        IsInLineOfSightNode lineOfSightNode = new IsInLineOfSightNode(transform, playerTransform, AmeStats.NoLOSChaseTime);
        ChasePlayerNode chasePlayerNode = new ChasePlayerNode(playerTransform, navMeshAgent, AmeStats.ChaseSpeed);
        RandomLocationNode randomLocationNode = new RandomLocationNode(playerTransform, navMeshAgent, AmeStats.NormalSpeed, AmeStats.NoLOSWaitTime, AmeStats.RandomWanderRadius);
        NewWaypointNode newWaypointNode = new NewWaypointNode(this, AmeStats.Range);
        MoveToWaypointNode moveToWaypointNode = new MoveToWaypointNode(this, AmeStats.NormalSpeed);

        Sequence isPlayerLOS = new Sequence(new List<Node> { lineOfSightNode, chasePlayerNode });
        Selector moveTowardsPlayer = new Selector(new List<Node> { isPlayerLOS, randomLocationNode });

        Sequence moveToPlayer = new Sequence(new List<Node> { isInRangeNode, moveTowardsPlayer });
        Sequence moveToWaypoint = new Sequence(new List<Node> { newWaypointNode, moveToWaypointNode });

        topNode = new Selector(new List<Node> { moveToPlayer, moveToWaypoint });
    }
}

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
    private Transform playerLastKnownLocation = null;
    private Node topNode = null;
    public AmeSO AmeStats = null;

    public bool NeedsToSelectWaypoint { get; set; } = true;
    public Transform PreviousWaypoint { get => previousWaypoint; set => previousWaypoint = value; }
    public Transform CurrentWaypoint { get => currentWaypoint; set => currentWaypoint = value; }
    public List<Transform> WayPoints { get => wayPoints; set => wayPoints = value; }
    public Transform PlayerLastKnownLocation { get => playerLastKnownLocation; set => playerLastKnownLocation = value; }
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
        //Debug.Log(topNode.NodeState);
        //Debug.Log(NeedsToSelectWaypoint);
        //Debug.Log($"Current waypoint: {CurrentWaypoint}");
        //Debug.Log($"Previous waypoint: {PreviousWaypoint}");
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, AmeStats.Range);
    }

    private void InitializeBehaviorTree()
    {
        IsInRangeNode isInRangeNode = new IsInRangeNode(this, playerTransform);
        IsInLineOfSightNode lineOfSightNode = new IsInLineOfSightNode(this, playerTransform);
        ChasePlayerNode chasePlayerNode = new ChasePlayerNode(playerTransform, this);
        RandomLocationNode randomLocationNode = new RandomLocationNode(playerTransform, this);
        NewWaypointNode newWaypointNode = new NewWaypointNode(this);
        MoveToWaypointNode moveToWaypointNode = new MoveToWaypointNode(this);
        LastKnownLocationNode lastKnownLocationNode = new LastKnownLocationNode(this);

        Sequence moveToPlayer = new Sequence(new List<Node> { isInRangeNode, lineOfSightNode, chasePlayerNode });
        Sequence moveToLastKnownLocation = new Sequence(new List<Node> { lastKnownLocationNode, isInRangeNode, randomLocationNode });
        Sequence moveToWaypoint = new Sequence(new List<Node> { newWaypointNode, moveToWaypointNode });

        topNode = new Selector(new List<Node> { moveToPlayer, moveToLastKnownLocation, moveToWaypoint });
    }

    public void GetNearestWaypoint()
    {
        //Transform wayPoint = null;

        foreach (var waypoint in WayPoints)
        {
            if (Vector3.Distance(waypoint.position, transform.position) <= AmeStats.Range)
            {
                wayPoints.Add(waypoint);
            }
        }
    }
}

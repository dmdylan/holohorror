using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorTreeStuff;

public class AmeAI : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] ameFootSteps = null;
    [SerializeField] private AudioClip[] ameChaseNosie = null;

    [Header("Ame Stats")]
    public AmeSO AmeStats = null;

    private List<Transform> wayPoints = null;
    private Transform playerTransform;
    private AudioSource audioSource;
    private NavMeshAgent navMeshAgent;
    private Transform previousWaypoint = null;
    private Transform currentWaypoint = null;
    private Transform playerLastKnownLocation = null;
    private Node topNode = null;
    private FaceCamera faceCamera;
    private bool isSoundPlaying = false;

    public bool IsChasing { get; set; }
    public bool NeedsToSelectWaypoint { get; set; } = true;
    public Transform PreviousWaypoint { get => previousWaypoint; set => previousWaypoint = value; }
    public Transform CurrentWaypoint { get => currentWaypoint; set => currentWaypoint = value; }
    public List<Transform> WayPoints { get => wayPoints; set => wayPoints = value; }
    public Transform PlayerLastKnownLocation { get => playerLastKnownLocation; set => playerLastKnownLocation = value; }
    public NavMeshAgent NavMeshAgent => navMeshAgent;

    private void Awake()
    {
        playerTransform = FindObjectOfType<FirstPersonAIO>().transform;
        wayPoints = GameManager.Instance.AmeWayPoints;
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        faceCamera = GetComponent<FaceCamera>();
        navMeshAgent.autoBraking = false;
        InitializeBehaviorTree();
    }

    private void Update()
    {
        topNode.Evaluate();

        if(IsChasing == true)
        {
            faceCamera.enabled = true;
            if(isSoundPlaying == false)
                StartCoroutine(PlayChaseNoise());
        }
        else
        {
            faceCamera.enabled = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, AmeStats.WaypointRange);
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

    private void PlayStepSound()
    {
        audioSource.PlayOneShot(ameFootSteps[Random.Range(0,4)]);
    }

    IEnumerator PlayChaseNoise()
    {
        isSoundPlaying = true;
        AudioClip tempClip = ameChaseNosie[Random.Range(0, ameChaseNosie.Length)];
        audioSource.PlayOneShot(tempClip);
        yield return new WaitForSeconds(tempClip.length);
        isSoundPlaying = false;
    }
}

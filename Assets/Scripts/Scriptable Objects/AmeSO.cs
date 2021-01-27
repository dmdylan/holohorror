using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ame/Ame Scriptable Object")]
public class AmeSO : ScriptableObject
{
    [SerializeField] private float normalSpeed = 0f;
    [SerializeField] private float chaseSpeed = 0f;
    [SerializeField] private float waypointRange = 0f;
    [SerializeField] private float playerFrontDetectionRange = 0f;
    [SerializeField] private float playerBackDetectionRange = 0f;
    [SerializeField] private float noLOSChaseTime = 0f;
    [SerializeField] private float noLOSWaitTime = 0f;
    [SerializeField] private float beginPathingTimer = 0f;
    [SerializeField] private float randomWanderRadius = 0f;

    public float NormalSpeed => normalSpeed;
    public float ChaseSpeed => chaseSpeed;
    public float WaypointRange => waypointRange;
    public float PlayerFrontDetectionRange => playerFrontDetectionRange;
    public float PlayerBackDetectionRange => playerBackDetectionRange;
    public float NoLOSChaseTime => noLOSChaseTime;
    public float NoLOSWaitTime => noLOSWaitTime;
    public float BeginPathingTimer => beginPathingTimer;
    public float RandomWanderRadius => randomWanderRadius;
}

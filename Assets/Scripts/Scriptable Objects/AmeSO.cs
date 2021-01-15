using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ame/Ame Scriptable Object")]
public class AmeSO : ScriptableObject
{
    [SerializeField] private float normalSpeed = 0f;
    [SerializeField] private float chaseSpeed = 0f;
    [SerializeField] private float range = 0f;

    public float NormalSpeed => normalSpeed;
    public float ChaseSpeed => chaseSpeed;
    public float Range => range;
}

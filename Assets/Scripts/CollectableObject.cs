using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableObject : MonoBehaviour
{
    public IntValue playerPoints;
    
    public void PickUpObject()
    {
        playerPoints.Value++;
        Debug.Log(playerPoints.Value);
        Destroy(this.gameObject);
    }
}

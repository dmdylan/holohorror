using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableObject : MonoBehaviour
{  
    public void PickUpObject()
    {
        GameEvents.Instance.PickedUpAKey();
        Destroy(this.gameObject);
    }
}

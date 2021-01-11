using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, IInteractableObject
{
    public void Interact()
    {
        GameEvents.Instance.PickedUpAKey();
        Destroy(this.gameObject);
    }
}

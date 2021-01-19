using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour, IInteractableObject
{
    public void Interact()
    {
        if(GameManager.Instance.PlayerKeys > 0)
        {
            GameEvents.Instance.OpenedALock();
            Destroy(this.gameObject);
        }
    }
}

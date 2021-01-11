using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : MonoBehaviour, IInteractableObject
{
    public void Interact()
    {
        if(GameManager.Instance.playerKeys.Value > 0)
        {
            GameEvents.Instance.OpenedALock();
            Destroy(this.gameObject);
        }
    }
}

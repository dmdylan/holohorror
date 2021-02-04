using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private Camera playerCamera;
    int playerMask;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = Camera.main;

        playerMask = 1 << 8;
        playerMask = ~playerMask;
    }

    void FixedUpdate()
    {
        CheckForObjects();
    }

    private void CheckForObjects()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit, 5f, playerMask))
        {
            //Debug.Log(hit.collider.gameObject.name);

            if(hit.collider.gameObject.TryGetComponent(out IInteractableObject interactableObject))
            {
                if(Input.GetKeyDown(KeyCode.E))
                    interactableObject.Interact();
            }
        }
    }
}

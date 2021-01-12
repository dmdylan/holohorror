using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private Camera playerCamera;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = Camera.main;
    }

    void FixedUpdate()
    {
        CheckForObjects();
    }

    private void CheckForObjects()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition); ;
        if(Physics.Raycast(ray, out RaycastHit hit, 5f))
        {
            Debug.Log(hit.collider.gameObject.name);

            if(hit.collider.gameObject.TryGetComponent(out IInteractableObject interactableObject))
            {
                if(Input.GetKeyDown(KeyCode.E))
                    interactableObject.Interact();
            }
        }
    }
}

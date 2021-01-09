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
        //Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * 5, Color.blue);
        CheckForObjects();
    }

    private void CheckForObjects()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition); ;
        if(Physics.Raycast(ray, out RaycastHit hit, 5f))
        {
            Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.GetComponent<CollectableObject>() && Input.GetKeyDown(KeyCode.E))
            {
                hit.collider.gameObject.GetComponent<CollectableObject>().PickUpObject();
            }
        }
    }
}

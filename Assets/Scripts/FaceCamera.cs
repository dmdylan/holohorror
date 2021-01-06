using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Camera playerCamera = null;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lookRotation = playerCamera.transform.position - transform.position;

        lookRotation.y = 0;
        //lookRotation.x = 0;
        //lookRotation.z = 0;

        transform.rotation = Quaternion.LookRotation(lookRotation);
        Debug.Log(lookRotation);
        //Debug.Log(lookRotation);
    }
}

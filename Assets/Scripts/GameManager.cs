using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public IntValue playerPoints;

    // Start is called before the first frame update
    void Start()
    {
        playerPoints.Value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

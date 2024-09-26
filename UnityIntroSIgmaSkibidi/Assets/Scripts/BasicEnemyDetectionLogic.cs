using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyDetectionLogic : MonoBehaviour
{

    public int detectPlayer = 0;
    public GameObject detectionBox;
    // Start is called before the first frame update
    void Start()
    {
        detectionBox = GameObject.Find("Detection");
    }

    // Update is called once per frame
    void Update()
    {
    
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            detectPlayer = 1;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            detectPlayer = 0;
        }
    }
}

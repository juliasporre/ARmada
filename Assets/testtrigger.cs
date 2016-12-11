using UnityEngine;
using System.Collections;

public class testtrigger : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Placed a boat");
    }


    void OnTriggerExit(Collider other)
    {
        Debug.Log("Boat gone");
    }
}

using UnityEngine;
using System.Collections;

public class testTrigger : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Placed a boat");
    
    }

}

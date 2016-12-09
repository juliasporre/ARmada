using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class testTrigger : MonoBehaviour {

    List<GameObject> list = new List<GameObject>();
  

void OnTriggerEnter(Collider other)
    {
        Debug.Log("Placed a boat" + other.name);

        list.Add(other.GetComponent<GameObject>());



    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Boat gone");
        foreach (GameObject go in list)
        {
            if (other.GetComponent<GameObject>() == go)
            {
                list.Remove(go);
            }
        }
        

    }

}

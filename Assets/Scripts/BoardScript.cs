using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardScript : MonoBehaviour {

    List<GameObject> list = new List<GameObject>();

    string[,] array = new string[,]
    {
        {"A1", "A2", "A3", "A4", "A5"},
        {"B1", "B2", "B3", "B4", "B5"},
        {"C1", "C2", "C3", "C4", "C5"},
        {"D1", "D2", "D3", "D4", "D5"},
        {"E1", "E2", "E3", "E4", "E5"},
    };


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

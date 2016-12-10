using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardScript : MonoBehaviour {

    string[,] array = new string[,]
    {
        {"A1", "A2", "A3", "A4", "A5"},
        {"B1", "B2", "B3", "B4", "B5"},
        {"C1", "C2", "C3", "C4", "C5"},
        {"D1", "D2", "D3", "D4", "D5"},
        {"E1", "E2", "E3", "E4", "E5"},
    };

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Debug.Log("PRESSED");
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Rigidbody gameObjectsRigidBody = cube.AddComponent<Rigidbody>();
            //var superMe = gameObject.AddComponent(boxscript); add this script to box! add boxcollider and debug
            cube.transform.position = new Vector3(0, 3, -2); ;
            StartCoroutine(Example());
        }


    }

    IEnumerator Example()
    {
        print(Time.time);
        yield return new WaitForSeconds(5);
        print(Time.time);
    }
}


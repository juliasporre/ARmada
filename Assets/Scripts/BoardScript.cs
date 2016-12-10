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

    public string url = "http://130.229.163.161:8000/game";
    private IEnumerator printMessage;
    int t = -1;

    void Start()
    {
        InvokeRepeating("callHelper",0.5f,0.5f);
    }

    void callHelper()
    {
        printMessage = getMessage();
        StartCoroutine(printMessage);
    }

    IEnumerator getMessage()
    {
        WWW www = new WWW(url + "?t=" + t);
        Debug.Log("HERE");
        yield return www;

        if (www.text.Length > 0)
        {
            t++;
            Debug.Log(www.text);
        }

    }

    /*void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            printMessage = getMessage();
            StartCoroutine(printMessage);
            Debug.Log("PRESSED");
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            Rigidbody gameObjectsRigidBody = cube.AddComponent<Rigidbody>();
            //var superMe = gameObject.AddComponent(boxscript); add this script to box! add boxcollider and debug
            cube.transform.position = new Vector3(0, 3, -2); ;
        }


    }*/

}


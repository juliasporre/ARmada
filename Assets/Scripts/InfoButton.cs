using UnityEngine;
using System.Collections;
using Vuforia;
using System;
using UnityEngine.UI;

public class InfoButton : MonoBehaviour, IVirtualButtonEventHandler {

    private GameObject infoButton;
    public GameObject userDisplay;
    // Use this for initialization
    void Start()
    {
        infoButton = GameObject.Find("infoButton");
        infoButton.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
        //userDisplay = GameObject.Find("userDisplay");
    }
    public void OnButtonPressed(VirtualButtonAbstractBehaviour vb)
    {
        Debug.Log("Pressed");
        userDisplay.GetComponent<Text>().text = "HERE IS SOM AWESOME INFORMATION" ;
    }

    public void OnButtonReleased(VirtualButtonAbstractBehaviour vb)
    {
        Debug.Log("Released");
        userDisplay.GetComponent<Text>().text = "";
    }


	

}

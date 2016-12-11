using UnityEngine;
using System.Collections;

public class BoxScript : MonoBehaviour {

    public BoardScript parent;

    void OnTriggerEnter(Collider other)
    {
       Debug.Log("Sending to Parent from " + name);
       parent.OnChildsTriggerEnter(name, other);
    }

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

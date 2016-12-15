using UnityEngine;
using System.Collections;

public class OnCollision : MonoBehaviour
{
    public GameObject explosion;

	void OnCollisionEnter (Collision collision)
	{
		Debug.Log ("missile collided with boat");

        Vector3 place = transform.position;
        //Vector3 rot = place; 
        //Debug.Log(place);
        //place.x += 0;
        //place.y += 0;
        //place.z += 0;

        Object explosionBoat = Instantiate(explosion, place, transform.rotation * Quaternion.Euler(90, 0, 0));
        explosion.transform.parent = transform; //fäster raketen på board
        Destroy(this.gameObject);

    }
}


using UnityEngine;
using System.Collections;

public class OnCollision : MonoBehaviour
{
    public GameObject explosion;

	void OnCollisionEnter (Collision collision)
	{
		Debug.Log ("missile collided with boat");

        Vector3 place = transform.position;

        Object explosionBoat = Instantiate(explosion, place, transform.rotation * Quaternion.Euler(90, 0, 0));
        explosion.transform.parent = transform; //fäster raketen på board
        Destroy(this.gameObject);

    }
}


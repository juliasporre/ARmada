using UnityEngine;
using System.Collections;

public class OnCollision : MonoBehaviour
{
    public GameObject explosion;

	void OnCollisionEnter (Collision collision)
	{
		Debug.Log ("missile collided");

        Vector3 place = transform.position;

        GameObject explosionBoat = Instantiate(explosion, place, transform.rotation * Quaternion.Euler(90, 0, 0));
        explosionBoat.transform.SetParent(GameObject.Find(collision.ToString()).transform, false);
        Destroy(this.gameObject);

    }
}


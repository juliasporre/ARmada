using UnityEngine;
using System.Collections;

public class OnCollision : MonoBehaviour
{
    public GameObject explosion;

    //void OnCollisionEnter(Collision collision)
    void OnTriggerEnter(Collider other)
	{
        Vector3 place = other.transform.position;
        place.y += 1;

        Debug.Log ("missile collided with " + other.name);
        GameObject explosionBoat = Instantiate(explosion, place, other.transform.rotation);
        explosionBoat.GetComponent<Transform>().SetParent(other.transform);
        Destroy(this.gameObject);

    }
}


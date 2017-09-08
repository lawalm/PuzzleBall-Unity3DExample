using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour {

    public float force = 150f;

    private void OnTriggerEnter(Collider target) {
        if(target.tag == "Ball") {
            target.gameObject.GetComponent<Rigidbody>().AddForce(
            transform.forward * - force, ForceMode.Impulse);
        }
    }
}

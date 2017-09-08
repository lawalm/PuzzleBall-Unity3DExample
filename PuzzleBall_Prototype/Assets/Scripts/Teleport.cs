using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour {

    [SerializeField]
    private Vector3 teleportPos;

    [SerializeField]
    private bool getArrows;

    private GameObject[] pathArrows;
    private GameObject[] wrongWayArrows;

    // Use this for initialization
    void Awake () {
		if(getArrows) {
            pathArrows = GameObject.FindGameObjectsWithTag("PathArrow");
            wrongWayArrows = GameObject.FindGameObjectsWithTag("WrongPathArrow");
        }
	}
	
	// Update is called once per frame
	void OnTriggerEnter (Collider target) {
        if(target.tag == "Ball") {
            target.transform.position = teleportPos;
        }
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyBallTrigger : MonoBehaviour {

    private Rigidbody myBody;

    [SerializeField]
    private AudioSource ballRollAudio, audioSource, stunnedAudio;

    [SerializeField]
    private AudioClip wallHit, stunnedClip;

    private Vector3 velocityLastFrame;
    private Vector3 collisionNormal;
    private float xAxisAngle, xFactor;
    private float yAxisAngle, yFactor;
    private float zAxisAngle, zFactor;

    private EnemyBall enemyBall;
    private MeshRenderer myRenderer;
    

    void Awake () {
        myBody = GetComponent<Rigidbody>();
        enemyBall = GetComponent<EnemyBall>();
        myRenderer = GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        BallRollSoundController();
    }

    private void LateUpdate() {
        velocityLastFrame = myBody.velocity;
    }

    void BallRollSoundController() {
        if (myBody.velocity.sqrMagnitude > 0) {
            // sqrMagnitude is the speed of the rigid body
            ballRollAudio.volume = myBody.velocity.sqrMagnitude * 0.0002f;
            ballRollAudio.pitch = 0.4f + ballRollAudio.volume;
            ballRollAudio.mute = false;
        } else {
            ballRollAudio.mute = true;
        }
    }

    void SetSoundVolumeOnCollision(Collision collision) {
        // contacts is an array of contact points
        // the contact point is a point where two colliders collided
        // the collision normal is a vector which is used to calculate
        // impulses after the collision
        collisionNormal = collision.contacts[0].normal;

        // vector3.angle returns the angle in degrees between from and to
        // since we want the x angle we are using vector3.right whic his 1, 0, 0
        xAxisAngle = Vector3.Angle(Vector3.right, collisionNormal);
        xFactor = (1.0f / 8100f) * xAxisAngle * xAxisAngle + (-1 / 45f) + 1f;

        yAxisAngle = Vector3.Angle(Vector3.up, collisionNormal);
        yFactor = (1.0f / 8100f) * yAxisAngle * yAxisAngle + (-1 / 45f) + 1f;

        zAxisAngle = Vector3.Angle(Vector3.forward, collisionNormal);
        zFactor = (1.0f / 8100f) * zAxisAngle * zAxisAngle + (-1 / 45f) + 1f;

        audioSource.volume = (Mathf.Abs(velocityLastFrame.x) * xFactor * 0.001f) +
            (Mathf.Abs(velocityLastFrame.y) * yFactor * 0.001f) +
            (Mathf.Abs(velocityLastFrame.z) * zFactor * 0.001f);

    }

    IEnumerator BallStunned() {
        yield return new WaitForSeconds(2f);
        myRenderer.material.color = new Color(241f, 0, 255f);
        enemyBall.stunned = false;
    }

    private void OnCollisionEnter(Collision target) {
        if(target.gameObject.tag == "Wall") {
            SetSoundVolumeOnCollision(target);

            if(!enemyBall.stunned && (Mathf.Abs(velocityLastFrame.x) * xFactor + Mathf.Abs(velocityLastFrame.y) * yFactor + Mathf.Abs(velocityLastFrame.z) * zFactor) > 15f) {
                enemyBall.stunned = true;
                myRenderer.material.color = Color.yellow;
                stunnedAudio.PlayOneShot(stunnedClip);
                StartCoroutine(BallStunned());
            }
        }
        if(target.gameObject.tag == "Ball") {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void OnTriggerEnter(Collider target) {
        if(target.tag == "Ball") {
            gameObject.SendMessage("GetBallTarget", target.transform);
            gameObject.SendMessage("CanAttackToggle", true);
        }
    }

    private void OnTriggerExit(Collider target) {
        if(target.tag == "Ball") {
            gameObject.SendMessage("CanAttackToggle", false);
        }
    }
}

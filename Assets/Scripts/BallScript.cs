using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour {

	public GameObject playerObject;
	public float speed = 40.0f;
	private bool ballIsActive;
	private Vector3 ballPosition;
	private Vector2 ballInitialForce;

	void Start () {
		ballIsActive = false;
		ballPosition = transform.position;
	}

	void Update () {
		if (Input.GetButtonDown ("Jump") == true) {
			if(!ballIsActive) {
				GetComponent<Rigidbody2D>().isKinematic = false;
				GetComponent<Rigidbody2D> ().velocity = Vector2.up * speed;
				ballIsActive = !ballIsActive;
			}
		}

		if(!ballIsActive && playerObject != null) {
			ballPosition.x = playerObject.transform.position.x;
			transform.position = ballPosition;
		}

		if (ballIsActive && transform.position.y < -8.0f) {
			ballIsActive = !ballIsActive;
			ballPosition.x = playerObject.transform.position.x;
			ballPosition.y = -6.92f;
			transform.position = ballPosition;

			GetComponent<Rigidbody2D>().isKinematic = true;
			playerObject.SendMessage("takeLife");
		}
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.name == "Player") {
			float x = hitFactor (transform.position, collision.transform.position, ((BoxCollider2D)collision.collider).size.x);
			Vector2 dir = new Vector2 (x, 1).normalized;
			GetComponent<Rigidbody2D> ().velocity = dir * speed;
		}
	}

	float hitFactor(Vector2 ballPos, Vector2 racketPos, float racketWidth) {
		return (ballPos.x - racketPos.x) / racketWidth;
	}
}

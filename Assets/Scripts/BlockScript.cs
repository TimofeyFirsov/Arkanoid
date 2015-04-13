using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BlockScript : MonoBehaviour {

	public int hitsToKill;
	public int points;
	private int numberOfHits;

	void Start () {
		numberOfHits = 0;

	}

	void OnCollisionEnter2D(Collision2D collision){
		if (collision.gameObject.tag == "Ball"){
			numberOfHits++;
			if(numberOfHits == hitsToKill) {
				GameObject player = GameObject.FindGameObjectsWithTag("Player")[0];

				player.SendMessage("addPoints", points);
				Destroy(this.gameObject);
			}
		}
	}
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class ScoreBoardScript : MonoBehaviour {

	private string scoreboardPath;
	
	void Start () {
		scoreboardPath = Directory.GetCurrentDirectory () + "\\score_board.txt";
		List<string> scores = new List<string> (readFromFile());
		scores = scores.OrderByDescending(item => int.Parse(item)).ToList();
		foreach(string score in scores) {
			GameObject objectScore = (GameObject)Instantiate (Resources.Load ("HighScore_Text"));
			objectScore.transform.SetParent (this.transform);
			objectScore.GetComponent<Text> ().text = score.ToString();
			objectScore.transform.localScale = new Vector3 (1, 1, 1);
		}
		GameObject backButton = (GameObject)Instantiate (Resources.Load ("Button"));
		backButton.transform.SetParent (this.transform);
		backButton.transform.localScale = new Vector3 (1, 1, 1);
		Button b = backButton.GetComponent<Button> ();
		b.onClick.AddListener (() =>  loadScene(0));
	}

	string[] readFromFile() {
		return File.ReadAllLines (scoreboardPath);
	}

	void loadScene(int level) {
		Application.LoadLevel(level);
	}
}

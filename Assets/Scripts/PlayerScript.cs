using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class PlayerScript : MonoBehaviour {

	public float playerVelocity;
	public float boundary;

	private int playerLives;
	private int playerPoints;
	private GUIStyle style;
	private Vector3 playerPosition;
	private string pointsPath;
	private string scoreboardPath;
	private FileStream file;

	void Start () {
		playerPosition = gameObject.transform.position;
		playerLives = 3;
		style = new GUIStyle ();
		style.fontSize = 40;
		pointsPath = Directory.GetCurrentDirectory () + "\\points.txt";
		scoreboardPath = Directory.GetCurrentDirectory () + "\\score_board.txt";
		updatePointsFromFile ();
	}

	void WinLose() {
		if (playerLives == 0) {
			saveToScoreBoard();
			resetPointsFile();
			Application.LoadLevel(1);
		}
		else if(GameObject.FindGameObjectsWithTag("Yellow").Length == 0 && GameObject.FindGameObjectsWithTag("Green").Length == 0
		        && GameObject.FindGameObjectsWithTag("Blue").Length == 0) {
			if (Application.loadedLevel < 5) {
				updatePointsFromFile();
				writePointsToFile();
				Application.LoadLevel(Application.loadedLevel + 1);
			} else {
				saveToScoreBoard();
				resetPointsFile();
				Application.LoadLevel(0);
			}
		}
	}

	void Update () {
		playerPosition.x += Input.GetAxis ("Horizontal") * playerVelocity;
		if (playerPosition.x < -boundary) {
			playerPosition.x = -boundary;
		} else if (playerPosition.x > boundary) {
			playerPosition.x = boundary;
		}

		transform.position = playerPosition;
		WinLose ();
		LeaveGame ();
	}

	void OnGUI() {
		GUI.Label(new Rect(5.0f, Camera.main.pixelHeight / 2 - 12.0f, 200.0f, 200.0f), "Score: " + playerPoints, style);
		GUI.Label(new Rect(Camera.main.pixelWidth - 180.0f, Camera.main.pixelHeight / 2 - 12.0f, 200.0f, 200.0f), "Lives: " + playerLives, style);
	}

	void addPoints(int points) {
		playerPoints += points;
	}

	void takeLife() {
		playerLives--;
	}

	void writePointsToFile() {
		System.IO.File.WriteAllText(pointsPath, playerPoints.ToString());
	}

	void updatePointsFromFile() {
		playerPoints += int.Parse(File.ReadAllLines(pointsPath)[0]);
	}

	void resetPointsFile() {
		File.WriteAllText(pointsPath, "0");
	}

	void saveToScoreBoard() {
		ArrayList resultList = new ArrayList (System.IO.File.ReadAllLines (scoreboardPath));
		if (resultList.Count < 10) {
			resultList.Add (playerPoints.ToString ());
		} else {
			resultList.Sort();
			if(int.Parse(resultList[0].ToString()) < playerPoints) {
				resultList[0] = playerPoints.ToString();
			}
		}
		System.IO.File.WriteAllLines( scoreboardPath, (string[])resultList.ToArray(typeof (string)));
	}
	void LeaveGame() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			saveToScoreBoard();
			resetPointsFile();
			Application.LoadLevel(0);
		}
	}
}

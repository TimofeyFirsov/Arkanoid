using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {

	public float playerVelocity;
	public float boundary;
	public GameObject lives;
	public GameObject pointsGameObject;

	private int playerLives;
	private int playerPoints;
	private Vector3 playerPosition;
	private string pointsPath;
	private string scoreboardPath;
	private FileStream file;


	void Start () {
		playerPosition = gameObject.transform.position;
		playerLives = 3;
		lives.GetComponent<Text>().text = "Lives: " + playerLives;
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

	void addPoints(int points) {
		playerPoints += points;
		pointsGameObject.GetComponent<Text>().text = playerPoints.ToString(); 
	}

	void takeLife() {
		playerLives--;
		lives.GetComponent<Text>().text = "Lives: " + (int.Parse(Regex.Split(lives.GetComponent<Text>().text, " ")[1]) - 1).ToString();
	}

	void writePointsToFile() {
		File.WriteAllText(pointsPath, playerPoints.ToString());
	}

	void updatePointsFromFile() {
		playerPoints += int.Parse(File.ReadAllLines(pointsPath)[0]);
		pointsGameObject.GetComponent<Text>().text = playerPoints.ToString(); 
	}

	void resetPointsFile() {
		File.WriteAllText(pointsPath, "0");
	}

	void saveToScoreBoard() {
		List<string> resultList = new List<string> (File.ReadAllLines (scoreboardPath));
		if (resultList.Count < 10) {
			resultList.Add (playerPoints.ToString ());
		} else {
			resultList = resultList.OrderBy(item => int.Parse(item)).ToList();
			print (resultList);
			print(resultList[0]);
			print(playerPoints);
			if(int.Parse(resultList[0].ToString()) < playerPoints) {
				resultList[0] = playerPoints.ToString();
			}
			print(resultList[0]);
		}
		File.WriteAllLines(scoreboardPath, resultList.ToArray());
	}
	void LeaveGame() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			saveToScoreBoard();
			resetPointsFile();
			Application.LoadLevel(0);
		}
	}
}

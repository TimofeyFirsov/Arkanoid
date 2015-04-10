using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour {
	public void loadScene(int level) {
		Application.LoadLevel(level);
	}

	public void exitApplication() {
		Application.Quit();
	}
}

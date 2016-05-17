using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Menu : MonoBehaviour {
	public Text highScore;
	private int highScore1;
	private float highScore2;
	// Use this for initialization
	void Start () {
		highScore1 = PlayerPrefs.GetInt ("highScore1");
		highScore2 = PlayerPrefs.GetFloat ("highScore2");

		if (highScore1 == 0 && highScore2 == 0) {
			highScore1 = 0;
			highScore2 = 50;
		}
	}
	
	// Update is called once per frame
	void Update () {
		highScore.text = highScore1.ToString () + ":" + highScore2.ToString ("f2");
	}
}

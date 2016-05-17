using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {

	public GameObject panel;
	public Text steps;
	static public int highScore1;
	static public float highScore2;
	private int highScoreCompareMin;
	private float highScoreCompareSec;
	public Text timerText;
	// Use this for initialization

		void Start()
		{
		highScoreCompareMin = PlayerPrefs.GetInt ("highScore1");
		highScoreCompareSec = PlayerPrefs.GetFloat ("highScore2");

		if (highScoreCompareMin == 0 && highScoreCompareSec == 0) {
			highScoreCompareMin = 0;
			highScoreCompareSec = 50;
		}
		}

	
	// Update is called once per frame
	void Update () {

		timerText.text = Timer.minutes.ToString() + ":" + Timer.seconds.ToString("f2");
		steps.text = Game.noOfSteps.ToString();

		if (highScoreCompareMin > Timer.minutes || (highScoreCompareMin >= Timer.minutes && highScoreCompareSec > Timer.seconds)) {
			panel.SetActive(true);
			PlayerPrefs.SetInt ("highScore1", Timer.minutes);
			PlayerPrefs.SetFloat ("highScore2", Timer.seconds);
		}
	}
}

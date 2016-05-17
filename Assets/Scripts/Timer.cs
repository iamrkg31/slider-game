using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	public Text timerText;
	private float startTime;
	static public int minutes;
	static public float seconds;
	// Use this for initialization
	void Start () {
		startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		float t = Time.time - startTime;

		minutes = ((int)t / 60);
		seconds = (t % 60);

		timerText.text = minutes.ToString () + ":" + seconds.ToString ("f2");
	}
}

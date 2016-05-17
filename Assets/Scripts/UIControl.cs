using UnityEngine;
using System.Collections;


public class UIControl : MonoBehaviour {



	public void NewScene (string newScene)
	{
		Application.LoadLevel(newScene);//to load new scene
	}

	public void HighScore (GameObject panel)
	{
		panel.SetActive (true);
	}


	public void HighScoreBack (GameObject panel1)
	{
		panel1.SetActive (false);
	}

	public void Exit (GameObject panel)
	{
		panel.SetActive (true);
	}
	
	
	public void ExitNo (GameObject panel)
	{
		panel.SetActive (false);
	}

	public void ExitYes ()
	{
		Application.Quit();
	}
}

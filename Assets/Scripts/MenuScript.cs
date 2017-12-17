using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

	public GameObject levelChanger;
	public GameObject exitPanel;
	private bool isPaused = false;
	public GameObject pp;

	void Update()
	{
		if (levelChanger.activeSelf == true && Input.GetKeyDown (KeyCode.Escape)) 
		{
			levelChanger.SetActive (false);
		}
		else if (exitPanel.activeSelf == false && Input.GetKeyDown(KeyCode.Escape))
			{
			exitPanel.SetActive (true);
			}
		else if (Input.GetKeyDown(KeyCode.Escape))
		{
			exitPanel.SetActive(false);
		}
		if (Input.GetKeyDown (KeyCode.Escape) && !isPaused && exitPanel.activeSelf == false) {
			pp.SetActive (true);
			Time.timeScale = 0;
			isPaused = true;
		} else if (Input.GetKeyDown (KeyCode.Escape) && isPaused && exitPanel.activeSelf == false) {
			pp.SetActive (false);
			Time.timeScale = 1;
			isPaused = false;
		}
	}

	public void OnClickStart()
	{
		levelChanger.SetActive (true);
	}
	public void OnClickExit()
	{
		Application.Quit ();
	}
	public void levelBttns(int level)
	{
		SceneManager.LoadScene (level);
	}
	public void pauseOn()
	{
		pp.SetActive (true);
		Time.timeScale = 0;
		isPaused = true;
	}
	public void _continue()
	{
		pp.SetActive (false);
		Time.timeScale = 1;
		isPaused = false;
	}
	public void gotomenu()
	{
		Time.timeScale = 1;
		SceneManager.LoadScene("Menu");
	}
}

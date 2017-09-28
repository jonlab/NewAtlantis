using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class NewGUIScript : MonoBehaviour {

	public GameObject mainPanel;
	public GameObject defaultControl;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Toggle ()
	{
		bool show = mainPanel.activeSelf;
		show=!show;
		mainPanel.SetActive(show);

		if (show)
		{
			NA.ShowAvatars(false);
			Debug.Log("toggle GUI: default control=" + defaultControl);
			EventSystem.current.SetSelectedGameObject(defaultControl,null);		
		}
		else
		{
			NA.ShowAvatars(true);
		}

	}

	public void ReturnToLobby () {
		NA.ShowAvatars(true);
		NA.app.GoToSpace(232);
		
		// hide the gui panel
		mainPanel.SetActive(false);
	}

}

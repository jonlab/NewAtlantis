using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NewGUIScript : MonoBehaviour {

	public GameObject mainPanel;
	GameObject button_ReturnToLobby;
	// Use this for initialization
	void Start () {
		button_ReturnToLobby = GameObject.Find("Button_ReturnToLobby");
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
			if (button_ReturnToLobby) button_ReturnToLobby.GetComponent<Button>().Select();
		}
		else
		{
			NA.ShowAvatars(true);
		}

	}


	[RPC]
	void Server_ReturnToLobby()
	{
		NA.app.GoToSpace(232);
	}

	public void ReturnToLobby () {
		
		NA.ShowAvatars(true);
		if (Network.isClient())
		{
			GetComponent<NetworkView>().RPC("Server_ReturnToLobby",RPCMode.Server);	
		
		}
		else 
		{
			NA.app.GoToSpace(232);
		}
		// hide the gui panel
		gameObject.SetActive(false);
	}

}

using UnityEngine;
using System.Collections;

public class MasterServerTest : MonoBehaviour {
	void Awake() {



		//MasterServer.ClearHostList();
		//MasterServer.RequestHostList("NewAtlantis");
	}
	void Update() {
		/*if (MasterServer.PollHostList().Length != 0) {
			HostData[] hostData = MasterServer.PollHostList();
			int i = 0;
			while (i < hostData.Length) 
			{
				Debug.Log("Game name: " + hostData[i].gameName);

				i++;
			}
			MasterServer.ClearHostList();
		}
		*/
	}

	void OnServerInitialized() 
	{
		Debug.Log("Server initialized and ready");
		//MasterServer.RegisterHost("NewAtlantis", "SERVER_" + Network.player.externalIP + "-" +  Network.player.ipAddress , "comment1");
		MasterServer.RegisterHost("NewAtlantis", "SERVER_" + Network.player.externalIP , "comment1");
	}

	void OnGUI()
	{
		if (GUILayout.Button ("Start and register server"))
		{
			Debug.Log ("Master server ip = " + MasterServer.ipAddress);
			bool useNat = !Network.HavePublicAddress();
			//useNat = false; //FIXME
			Debug.Log ("use NAT = " + useNat);
			Network.InitializeServer(32, 7890, useNat); //7890
		}

		GUILayout.BeginHorizontal();
		//GUILayout.Label ("" + Network.player.ipAddress);
		if (Network.isServer)
			GUILayout.Label("Running as a server");
		else
			if (Network.isClient)
				GUILayout.Label("Running as a client");
        GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label ("SERVER - This machine ip : " + Network.player.ipAddress + " (" + Network.player.externalIP + ")");// + " " + Network.player.externalIP);
		GUILayout.Label ("" + Network.player);
		GUILayout.EndHorizontal();

		foreach (NetworkPlayer player in Network.connections)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label ("Player="+player.guid + " ip="+player.ipAddress + " port=" + player.port + " ping=" + Network.GetAveragePing(player) + "ms");
			GUILayout.EndHorizontal();
		}

	}


}
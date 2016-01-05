using UnityEngine;
using System.Collections;

public class ExampleUnityNetworkingBrowseServers : MonoBehaviour
{
	// are we currently trying to download a host list?
	private bool loading = false;
	// the current position within the scrollview
	private Vector2 scrollPos = Vector2.zero;
	void Start()
	{
		// immediately request a list of hosts
		refreshHostList();
	}
	
	void OnGUI()
	{
		//GUILayout.BeginHorizontal();

		GUILayout.BeginHorizontal();


		GUILayout.Label("MasterServer ip = " + MasterServer.ipAddress);
		GUILayout.Label("Player external ip = " + Network.player.externalIP);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("connect to 213.30.176.244"))
		{
			Network.Connect("213.30.176.244", 7890);
		}

		if (GUILayout.Button("connect to 192.168.0.242"))
		{
			Network.Connect("192.168.0.242", 7890);
		}
		//GUILayout.Label ("" + Network.player.ipAddress);
		if (Network.isServer)
			GUILayout.Label("Running as a server");
		else
			if (Network.isClient)
				GUILayout.Label("Running as a client");
		GUILayout.EndHorizontal();
		//GUILayout.BeginHorizontal();
		if( GUILayout.Button( "Refresh" ) )
		{
			refreshHostList();
		}
		
		if( loading )
		{
			GUILayout.Label( "Loading..." );
		}
		else
		{
			scrollPos = GUILayout.BeginScrollView( scrollPos, 
			                                      GUILayout.Width( Screen.width ), GUILayout.Height( 500f ) );
			
			HostData[] hosts = MasterServer.PollHostList();
			for( int i = 0; i < hosts.Length; i++ )
			{
				HostData d = hosts[i];
				string ip = "";
				foreach (string s in d.ip)
				{
					ip += s + ".";
				}

				string caption = d.gameName + "[" + d.connectedPlayers + "/" + d.playerLimit + "]";
				if( GUILayout.Button( caption,
				                     GUILayout.ExpandWidth( true ) ) )
				{
					Network.Connect( hosts[i] );
				}
			}
			if( hosts.Length == 0 )
			{
				GUILayout.Label( "No servers running" );
			}
			
			GUILayout.EndScrollView();
		}

	}
	
	void refreshHostList()
	{
		// let the user know we are awaiting results from the master server
		loading = true;
		MasterServer.ClearHostList();
		MasterServer.RequestHostList( "NewAtlantis" );
	}
	
	// this is called when the Master Server reports an event to the client – for example, server registered successfully, host list received, etc
    void OnMasterServerEvent( MasterServerEvent msevent )
    {
        if( msevent == MasterServerEvent.HostListReceived )
        {
            // received the host list, no longer awaiting results
            loading = false;
        }
    }
}
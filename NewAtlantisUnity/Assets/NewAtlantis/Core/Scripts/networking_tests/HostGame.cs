using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using UnityEngine.Networking.Match;
using System.Collections.Generic;

public class HostGame : MonoBehaviour
{
	List<MatchDesc> matchList = new List<MatchDesc>();
	bool matchCreated;
	NetworkMatch networkMatch;
	string strLog = "";

	
	void Awake()
	{
		networkMatch = gameObject.AddComponent<NetworkMatch>();
	}
	
	void OnGUI()
	{

		GUILayout.Label(strLog);
		// You would normally not join a match you created yourself but this is possible here for demonstration purposes.
		if(GUILayout.Button("Create Room"))
		{
			CreateMatchRequest create = new CreateMatchRequest();
			create.name = "NA " + SystemInfo.deviceModel + " " + SystemInfo.operatingSystem;
			create.size = 4;
			create.advertise = true;
			create.password = "";
			create.projectId = "00b0c817-3b84-4fee-8d83-05f868547da9";
			
			networkMatch.CreateMatch(create, OnMatchCreate);
		}
		
		if (GUILayout.Button("List rooms"))
		{
			networkMatch.ListMatches(0, 20, "", OnMatchList);
		}
		
		if (matchList.Count > 0)
		{
			GUILayout.Label("Current rooms");
		}
		foreach (var match in matchList)
		{

			string str = match.name;
			foreach (MatchDirectConnectInfo i in match.directConnectInfos)
			{
				str += " id= " + match.networkId + " [" + i.publicAddress + " / " + i.privateAddress + "]";
			}
			if (GUILayout.Button(str))
			{
				networkMatch.JoinMatch(match.networkId, "", OnMatchJoined);
			}
		}
	}
	
	public void OnMatchCreate(CreateMatchResponse matchResponse)
	{
		if (matchResponse.success)
		{
			Debug.Log("Create match succeeded");
			matchCreated = true;
			Utility.SetAccessTokenForNetwork(matchResponse.networkId, new NetworkAccessToken(matchResponse.accessTokenString));
			NetworkServer.Listen(new MatchInfo(matchResponse), 9000);
		}
		else
		{
			Debug.LogError ("Create match failed");
		}
	}
	
	public void OnMatchList(ListMatchResponse matchListResponse)
	{

		matchList.Clear();

		if (matchListResponse.success && matchListResponse.matches != null)
		{
			Debug.Log ("found " +matchListResponse.matches.Count);
			foreach (MatchDesc m in matchListResponse.matches)
			{
				Debug.Log (m.name);
				matchList.Add (m);
			}
			//networkMatch.JoinMatch(matchListResponse.matches[0].networkId, "", OnMatchJoined);
		}
	}
	
	public void OnMatchJoined(JoinMatchResponse matchJoin)
	{
		if (matchJoin.success)
		{
			Debug.Log("Join match succeeded");
			strLog = "Join match succeeded";
			/*if (matchCreated)
			{
				Debug.LogWarning("Match already set up, aborting...");
				return;
			}*/
			Utility.SetAccessTokenForNetwork(matchJoin.networkId, new NetworkAccessToken(matchJoin.accessTokenString));
			NetworkClient myClient = new NetworkClient();
			myClient.RegisterHandler(MsgType.Connect, OnConnected);
			myClient.Connect(new MatchInfo(matchJoin));
		}
		else
		{
			Debug.LogError("Join match failed");
			strLog = "Join match failed";
		}
	}
	
	public void OnConnected(NetworkMessage msg)
	{
		Debug.Log("Connected!");
	}
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChatEntry
{
    public string name = "";
	public string str = "";
	public int category = 0;
}
public class ChatManager 
{

	public static bool bGUI = false;
	public static Texture2D texWhite = null;
    public static List<ChatEntry> logs = new List<ChatEntry>();
	// Use this for initialization
	public static void Initialize () 
	{
		texWhite = Resources.Load ("Textures/white") as Texture2D;
	
	}
	
	public static int GetStart(int maxcount)
    {
        int start = logs.Count-maxcount;
        start = Mathf.Max (start, 0);
        return start;
    }

    public static int GetEnd()
    {
        int end = logs.Count-1;
		return end;
    }

	public static void DrawGUI()
	{
		if (bGUI)
		{
			GUI.color = new Color(0,0,0,0.5f);
			GUI.DrawTexture(new Rect(0,100,Screen.width, Screen.height-200), texWhite);
			GUI.color = Color.white;
			int count = (Screen.height-250)/12;
			int start = logs.Count-count;
			int end = logs.Count-1;
			start = Mathf.Max (start, 0);

			float y = 100;
			if (GUI.Button (new Rect(0,y,100,30), "clear"))
			{
				logs.Clear();
			}
			y = 150;
			if (logs.Count == 0)
				return;
			for (int i=start;i<=end;++i)
			{
				ChatEntry e = logs[i];
				if (e.category == 0)
					GUI.color = Color.white;
				else if (e.category == 1)
					GUI.color = Color.yellow;
				else if (e.category == 2)
					GUI.color = Color.red;
				GUI.Label(new Rect(0, y, Screen.width, 30), e.str);
				y += 12;
			}
		}
	}


	public static void Log(string name, string str, int cat)
	{
		Debug.Log (str);
		ChatEntry e = new ChatEntry();
		e.category = cat;
        e.name = name;
		e.str = str;
		logs.Add (e);
	}

	
}

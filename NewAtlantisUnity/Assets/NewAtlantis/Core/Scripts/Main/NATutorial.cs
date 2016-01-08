using UnityEngine;
using System.Collections;

public class NATutorial : MonoBehaviour 
{
	private Texture2D texWhite	= null;

	private bool bActive 		= false;
	private float Duration 		= 10;
	private float timer 		= 0f;
	
    public TextAsset text;
    private string[] instructions = null;
    private int current = 0;
	// Use this for initialization
	void Start () 
	{
		texWhite = Resources.Load ("white") as Texture2D;
	    //text
        instructions = text.text.Split('\n');
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.T))
		{
			bActive = true;
		}
		if (bActive)
		{
			timer += Time.deltaTime;
			if (timer > Duration)
			{
                timer -= Duration;
                current++;
                if (current>instructions.Length-1)
                {
                    current = 0;
                    bActive = false;
                }

			}
		}

	}

	void OnGUI ()
	{
		if (bActive)
		{
			DrawGUI();
		}
	}

	


	void DrawGUI()
	{
		Font bak = GUI.skin.font;
		GUI.skin.font = NA.GetFont(3);
		float h = 200;
		float y = Screen.height-h;
		GUI.color = new Color(0,0,0,0.5f);
		GUI.DrawTexture(new Rect(0,y,Screen.width, h), texWhite);
		GUI.color = Color.white;
		GUI.Label(new Rect(100,y,Screen.width-200,200), instructions[current]);
		GUI.skin.font = bak;
	}
}

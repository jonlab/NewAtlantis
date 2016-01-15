using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NAObjectLabel : MonoBehaviour 
{
	public string Title 		= "Your object name.";
	public string Author 		= "Your name.";
	public string Date 			= "2015";
	public string Synopsis 		= "Your object description.";
	public string Instructions 	= "How to use your object.";

	private Texture2D texWhite	= null;

	private bool bActive 		= false;
	private float Duration 		= 15f;
	private float timer 		= 0f;

    private static List<NAObjectLabel> labels = new List<NAObjectLabel>();
	
	// Use this for initialization
	void Start () 
	{
		texWhite = Resources.Load ("white") as Texture2D;

	}
    void OnEnable()
    {
        labels.Add(this);
    }


    void OnDestroy()
    {
        labels.Remove(this);
    }
	
	// Update is called once per frame
	void Update () 
	{
		if (bActive)
		{
			timer += Time.deltaTime;
			/*if (timer > Duration)
			{
				//bActive = false;
				timer = 0f;
			}
			*/
		}

	}

    void LateUpdate()
    {
        //LabelActive = false;
    }

	void OnGUI ()
	{
        bool LabelActive = false;
        foreach (NAObjectLabel l in labels)
        {
            if (l.bActive)
                LabelActive = true;
        }
		if (bActive && (timer < Duration))
		{
			DrawGUI();
		}
		else if (!LabelActive && (timer < Duration))
		{
            if (!NA.app.bGUI)
            {
                return;
            }
            //NAObjectLabel[] labels = Object.

            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			Vector3 pos2d = Camera.main.WorldToViewportPoint(transform.position);
			if (pos2d.z > 0)
			{
				int x = (int)(pos2d.x*Screen.width);
				int y = (int)(Screen.height-pos2d.y*Screen.height);
				GUI.color = Color.white;
				GUI.Label(new Rect(x-100, y-15, 200, 30), Title + "[" + Author + "]");
			}
            GUI.skin.label.alignment = TextAnchor.UpperLeft;
		}
	}

	void OnTriggerEnter(Collider other) 
	{
		if (NA.isViewer(other.gameObject)) //is the colliding object a viewer object ?
		{
			bActive = true;
			timer = 0f;
		}
	}

	void OnTriggerExit(Collider other) 
	{
		if (NA.isViewer(other.gameObject))
		{
			bActive = false;
		}
	}


	void DrawGUI()
	{
		Font bak = GUI.skin.font;
		float h = 250;
		float y = Screen.height-h;
		GUI.color = new Color(0,0,0,0.5f);
		GUI.DrawTexture(new Rect(0,y,Screen.width, h), texWhite);
		GUI.color = Color.white;
		float w = Screen.width-80;
		GUI.skin.font = NA.GetFont(2);
		GUI.Label(new Rect(40,y,w,30), Title);
		GUI.skin.font = NA.GetFont(1);
		GUI.Label(new Rect(40,y+30,w,30), Author + ", " + Date);
		GUI.Label(new Rect(40,y+70,w,100), Synopsis);
		GUI.Label(new Rect(40,y+170,w,80), Instructions);

		GUI.skin.font = bak;
	}
}

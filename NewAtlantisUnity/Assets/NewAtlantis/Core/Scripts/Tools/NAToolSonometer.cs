using UnityEngine;
using System.Collections;

public class NAToolSonometer : NAToolBase 
{
	AudioSource[] sources = null;
    // Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

	public override void Action() 
	{
		Debug.Log ("NAToolSonometer action");
		sources = GameObject.FindObjectsOfType<AudioSource>();
	}

	public override void DrawExtendedGUI(Vector3 pos2d)
	{
		if (sources == null)
			return;
		foreach (AudioSource s in sources)
		{
			Vector3 spos2d = Camera.main.WorldToViewportPoint(s.transform.position);
			if (spos2d.z > 0)
			{
				spos2d.x = Mathf.Clamp(spos2d.x, -1,1);
				spos2d.y = Mathf.Clamp(spos2d.y, -1,1);
				int x = (int)(spos2d.x*Screen.width);
				int y = (int)(Screen.height-spos2d.y*Screen.height);

				if (s.isPlaying)
				{
					GUI.color = Color.green;
				}
				else
				{
					GUI.color = Color.white;
				}
				if (GUI.Button(new Rect(x-50, y-15, 100, 30), s.name))
				{
					if (s.isPlaying)
						s.Stop();
					else
						s.Play();
				}
			}
		}
		GUI.color = Color.white;

	}

}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class testaudiosources : MonoBehaviour 
{

	int count = 0;
	float currentfreq = 150f;
	List<GameObject> sources = new List<GameObject>();
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnGUI()
	{
		GUI.Label(new Rect(0,0,200,30), "count=" + count);
		GUI.Label(new Rect(200,0,200,30), "currentfreq=" + currentfreq);


		if (GUI.Button(new Rect(0,30,100,30), "Create 4 sources"))
		{
			for (int i=0;i<4;++i)
			{
				CreateAudioSource(new Vector3(Random.value*10f, Random.value*10f, Random.value*10f), currentfreq);
				currentfreq *= 1.05946f;
			}

		}
		if (GUI.Button(new Rect(100,30,100,30), "delete all sources"))
		{
			DeleteAllAudioSources();
		}
		int x = 0;
		int y = 60;

		foreach (GameObject go in sources)
		{
			GUI.color = Color.white;
			if (go.GetComponent<AudioSource>().isPlaying)
			{
				float distance = (Camera.main.transform.position - go.transform.position).magnitude;
				GUI.color = Color.red;
				if (GUI.Button (new Rect(x,y,300,30), "Playing v=" + go.GetComponent<AudioSource>().volume + " d=" + distance))
				{
					go.GetComponent<AudioSource>().Stop();
				}
			}
			else
			{
				GUI.color = Color.white;
				if (GUI.Button (new Rect(x,y,300,30), "Stopped " + go.GetComponent<AudioSource>().volume))
				{
					go.GetComponent<AudioSource>().Play();
				}
			}
			x += 300;
			if (x >=1200)
			{
				x = 0;
				y += 30;
			}


		}

	}

	void DeleteAllAudioSources()
	{
		foreach (GameObject go in sources)
		{
			GameObject.Destroy(go);


		}
		sources.Clear();
		currentfreq = 150f;
		count = 0;
	}

	void CreateAudioSource(Vector3 pos, float freq)
	{
		count++;
		AudioClip clip = AudioClip.Create("generated"+freq, 100000, 1, 44100, true, false);
		float[] samples = new float[100000];
		for (int i=0;i<100000;++i)
		{
			float angle = i * 2f * Mathf.PI / 44100f * freq;
			samples[i] = Mathf.Sin (angle);
		}
		clip.SetData(samples, 0);

		GameObject go = new GameObject("audio");
		go.transform.position = pos;
		go.AddComponent<AudioSource>();
		go.GetComponent<AudioSource>().clip = clip;

		go.GetComponent<AudioSource>().loop = true;
		go.GetComponent<AudioSource>().volume = 0.5f+Random.value/2f;
		go.GetComponent<AudioSource>().Play();

		sources.Add (go);

	}
}

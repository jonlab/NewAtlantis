using UnityEngine;
using System.Collections;

public class NAPlayAtRandomInterval : MonoBehaviour 
{

	public float AveragePeriod = 3f;
	public float Variance = 0.5f;
	public bool waitToFinish = false;

	private float time = 0f;
	private float next = 0f;

	private float startTime = 0f;

	// Use this for initialization
	void Start () 
	{
		Next();
	}
	
	// Update is called once per frame
	void Update () 
	{
		time += Time.deltaTime;
		if (time > next)
		{
			AudioSource audio = GetComponent<AudioSource>();
			if (audio)
			{
				audio.Play();
				startTime = Time.time;
			}
			time -= next;
			Next();
		}
	
	}

	void Next()
	{
		AudioSource audio = GetComponent<AudioSource>();

		next = AveragePeriod+AveragePeriod*(0.5f)*(Random.value-0.5f)*2f;

		if (waitToFinish && audio.isPlaying) {
			next += (audio.clip.length - audio.time);	// wait for the current clip to finish. subtract current play time from clip length, add to 'next' time
		}
	}
}

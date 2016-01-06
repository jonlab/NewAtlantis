using UnityEngine;
using System.Collections;

public class NAPlayAtRandomInterval : MonoBehaviour 
{

	public float AveragePeriod = 3f;
	public float Variance = 0.5f;

	private float time = 0f;
	private float next = 0f;
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
			}
			time -= next;
			Next();
		}
	
	}

	void Next()
	{
		next = AveragePeriod+AveragePeriod*(0.5f)*(Random.value-0.5f)*2f;
	}
}

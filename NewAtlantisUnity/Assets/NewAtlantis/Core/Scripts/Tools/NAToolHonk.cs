using UnityEngine;
using System.Collections;

public class NAToolHonk : NAToolBase 
{
	public AudioSource audioSource;
	public AudioClip[] audioClips;

	// Use this for initialization
	void Start () 
	{
		//GetInstanceID();
	}

	// Update is called once per frame
	void Update () 
	{

	}

	public override void Action() 
	{
		Debug.Log ("NAToolHonk action");
		audioSource.Play();
	}




}

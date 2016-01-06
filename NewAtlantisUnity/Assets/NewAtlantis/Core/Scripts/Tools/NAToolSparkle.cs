using UnityEngine;
using System.Collections;

public class NAToolSparkle : NAToolBase {

	public AudioSource audioSource;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void Action() 
	{
		Debug.Log ("NAToolSparkle action");
		audioSource.Play();
	}
}

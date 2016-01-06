using UnityEngine;
using System.Collections;

public class audiohf : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{

	
	}

	void OnAudioFilterRead(float[] data, int channels)
	{
		Debug.Log ("OnAudioFilterRead " + data.Length);
	}


}

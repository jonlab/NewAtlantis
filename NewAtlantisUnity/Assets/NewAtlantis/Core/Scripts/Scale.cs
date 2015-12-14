using UnityEngine;
using System.Collections;

public class Scale : MonoBehaviour {

	float timer = 0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		timer += Time.deltaTime;
		float s = 1f + Mathf.Sin (timer*3f) * 0.5f;
		transform.localScale = Vector3.one * s;
	
	}
}

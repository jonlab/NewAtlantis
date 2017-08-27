using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalePulse : MonoBehaviour {
	Vector3 baseScale; 
	public float duration=0.3f;

	void Start () {
		baseScale = transform.localScale;
	}
	
	void Update () {
		
	}

	public void Pulse(float signal)
	{
		StartCoroutine(DoScaleAnimation(signal));
	}

	IEnumerator DoScaleAnimation(float signal)
	{
		Vector3 newScale = baseScale * (1.0f+signal) * (1.0f+signal);

		float startTime = Time.time;
		float endTime =startTime+duration;

		while (Time.time<=endTime)
		{
			float t=(Time.time-startTime)/duration; 
			transform.localScale = Vector3.Lerp(newScale, baseScale,t);
			yield return null;
		}

	}

}

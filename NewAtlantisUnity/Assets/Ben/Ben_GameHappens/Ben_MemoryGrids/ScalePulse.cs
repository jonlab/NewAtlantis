using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalePulse : MonoBehaviour {
	Vector3 baseScale; 
	public float duration=0.5f;
	public Color litColor = new Color (1.0f,0.88f,0f);
	Renderer [] renderers; 

	void Start () {
		baseScale = transform.localScale;
		renderers = GetComponentsInChildren<Renderer>();
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
			foreach (Renderer r in renderers)
			{
				r.material.SetColor("_EmissionColor",Color.Lerp (litColor,Color.black,t));
			}



			yield return null;
		}

	}

}

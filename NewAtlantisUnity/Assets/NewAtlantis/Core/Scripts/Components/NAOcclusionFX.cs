using UnityEngine;
using System.Collections;

public class NAOcclusionFX : MonoBehaviour 
{
	public float gain = 1f;
	public bool occluded = false;

	void Start () 
	{
		//Debug.Log ("NAOcclusionFX start");
	}
	
	// Update is called once per frame
	void Update () 
	{
		//occlusion processing
		//if something in the path between this Audiosource and the listener, we lower the sound

		//raycasting :
		Vector3 listenerpos = NA.listener.transform.position;
		Vector3 audiosourcepos = gameObject.transform.position;
		Ray ray = new Ray(audiosourcepos, listenerpos-audiosourcepos);
		//gain = 1f;
		RaycastHit hit;
		//Debug.Log ("ray is " + ray);
		if (Physics.Raycast(ray, out hit))
		{
			//we have occlusion
			//improvement : we could look at the material and change the processing
			//Debug.Log("occlusion with " + hit.collider.gameObject.name);
			if (hit.collider.gameObject != NA.listener.gameObject)
			{
				occluded = true;
			}
			else
			{
				//we collided with camera
				occluded = false;
			}
		}
		else
		{
			occluded = false;
			//Debug.Log("no occlusion");
		}

		if (occluded)
		{
			gain -= Time.deltaTime;
			gain = Mathf.Max(gain, 0.1f);
		}
		else
		{
			gain += Time.deltaTime;
			gain = Mathf.Min(gain, 1);

		}



	}
	
	void OnGUI()
	{
		//nothing
	}

	void OnAudioFilterRead(float[] data, int channels)
	{
		//Debug.Log ("OnAudioFilterRead");
		for (int i=0;i<data.Length;++i)
		{
			data[i] *= gain;
		}
	}

}

using UnityEngine;
using System.Collections;

public class NAPhysicsAudioSource : MonoBehaviour 
{

	public float SoundPressureLevel = 60;
	// Use this for initialization
	void Start () 
	{
		AudioSource audio = GetComponent<AudioSource>();
		if (audio)
		{
			//60 dB -> 1
			//dB to gain
			float dB = SoundPressureLevel;
			float g1m = Mathf.Pow(10, (dB-60)/20);
			/*
			float g10m = getGain(g1m, 10);
			float g100m = getGain(g1m, 100);
			float g200m = getGain(g1m, 200);
			Debug.Log("gain at 1m : " + g1m);
			Debug.Log("gain at 10m : " + g10m);
			Debug.Log("gain at 100m : " + g100m);
			Debug.Log("gain at 200m : " + g200m);

			Debug.Log("Distance at g1 :" + getDistanceAtGain(g1m, 0.001f));
			//distance for unit gain
			float MaxDistance = getDistanceAtGain(g1m, 0.001f);
			audio.maxDistance = MaxDistance;
			*/
			audio.spatialBlend = 1;
			audio.rolloffMode = AudioRolloffMode.Custom;
			audio.maxDistance = 1000;
			float UnitGainDistance = getDistanceAtGain(g1m, 1f);
			audio.minDistance = UnitGainDistance;
			audio.rolloffMode = AudioRolloffMode.Logarithmic;
			/*Keyframe[] ks = new Keyframe[3];
			//distance / gain
			ks[0] = new Keyframe(0, 1);
			//ks[0].inTangent = Mathf.Tan(-Mathf.PI/2);
			ks[0].outTangent = -1;

			ks[1] = new Keyframe(10, getGain(g1m, 10));
			ks[1].inTangent = 0;
			ks[1].outTangent = 0;

			ks[2] = new Keyframe(100, getGain(g1m, 100));
			ks[2].inTangent = 0;
			*/


			//ks[1].outTangent = 0;

			/*int count = 20;
			Keyframe[] ks = new Keyframe[20];
			for (int i=0;i<count;++i)
			{
				//float d = 
				
			}
			ks[0] = new Keyframe(0, g1m);

			ks[1] = new Keyframe(0.5f, getGain(g1m, 0.5f));
			ks[2] = new Keyframe(1, g1m);
			ks[3] = new Keyframe(2, getGain(g1m, 2));
			ks[4] = new Keyframe(20, getGain(g1m, 20));

			AnimationCurve curve = new AnimationCurve(ks);
			//AnimationCurve curve = new AnimationCurve();
			//Keyframe k = new Keyframe(
			//curve.AddKey(
			audio.SetCustomCurve(AudioSourceCurveType.CustomRolloff, curve);
			*/

		}
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	float getGain(float referencegain, float distance)
	{
		if (distance > 0)
		{
			return referencegain*1/(distance*distance);
		}
		else
		{
			return 1;
		}
			
	}

	float getDistanceAtGain(float referencegain, float gain)
	{
		//float g =  referencegain*1/(distance*distance);
		float distance = 1/Mathf.Sqrt(gain) * referencegain;




		return distance;
	}

}

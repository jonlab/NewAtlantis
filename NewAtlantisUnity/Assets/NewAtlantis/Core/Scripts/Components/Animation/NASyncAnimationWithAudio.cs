using UnityEngine;
using System.Collections;

public class NASyncAnimationWithAudio : MonoBehaviour {

	private float a = 1; //mult
	private float b = 0; //offset
	public AudioSource 	audioSource;

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	public void SetCoeffs(float _a, float _b=0)
	{
		a = _a;
		b = _b;
	}

	void LateUpdate () 
	{
		//only server ?
		if (audioSource != null && audioSource.clip != null)
		{
			//get the offsets and compute a and b coeffs
			NAAudioSynthGranulate g = audioSource.GetComponent<NAAudioSynthGranulate>();
			if (g != null)
			{
				b = g.pos;
				a = g.duration;
			}
			NAAudioSynthLooper l = audioSource.GetComponent<NAAudioSynthLooper>();
			if (l != null)
			{
				b = l.pos;
				a = l.duration;
			}
			float time_animation = (audioSource.time / audioSource.clip.length)*a+b;
			//Animation animation = GetComponent<Animation>();
			Animation[] animations = GetComponentsInChildren<Animation>();
			foreach (Animation animation in animations) 
			{
				foreach (AnimationState state in animation) 
				{
					state.normalizedTime = time_animation;
				}
			}

			Animator[] animators = GetComponentsInChildren<Animator>();
			foreach (Animator animator in animators) 
			{
				//Debug.Log("change time : " + time_animation);
				//animator.playbackTime = 0;
				animator.ForceStateNormalizedTime(time_animation);
			}
		}
	}
}

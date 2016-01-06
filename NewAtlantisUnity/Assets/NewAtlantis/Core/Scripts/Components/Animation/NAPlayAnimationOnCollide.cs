using UnityEngine;
using System.Collections;

public class NAPlayAnimationOnCollide : MonoBehaviour 
{
	public bool Toggle = false;
	public GameObject TargetObject = null;

	void Start()
	{
		GameObject go = TargetObject != null ? TargetObject : gameObject;
		Animator[] anims = go.GetComponentsInChildren<Animator>();
		foreach (Animator anim in anims)
		{
			anim.enabled = false;
		}
	}
	void OnCollisionEnter(Collision collision) 
	{
		GameObject go = TargetObject != null ? TargetObject : gameObject;
		Animator[] anims = go.GetComponentsInChildren<Animator>();
		Debug.Log ("OnCollisionEnter - " + anims.Length + " Animator found.");
		foreach (Animator anim in anims)
		{
			if (Toggle)
			{
				anim.enabled = !anim.enabled;
			}
			else
			{
				anim.enabled = true;
			}
		}
	}
	
	void OnCollisionExit(Collision collision) 
	{
		GameObject go = TargetObject != null ? TargetObject : gameObject;
		Animator[] anims = go.GetComponentsInChildren<Animator>();
		Debug.Log ("OnCollisionExit - " + anims.Length + " Animator found.");
		foreach (Animator anim in anims)
		{
			if (!Toggle)
			{
				anim.enabled = false;
			}
		}
    }
}

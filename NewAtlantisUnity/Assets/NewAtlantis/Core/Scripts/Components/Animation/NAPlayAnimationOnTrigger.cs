using UnityEngine;
using System.Collections;

public class NAPlayAnimationOnTrigger : MonoBehaviour {

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
	void OnTriggerEnter(Collider collision) 
	{
		GameObject go = TargetObject != null ? TargetObject : gameObject;

		Animator[] anims = go.GetComponentsInChildren<Animator>();
		Debug.Log ("OnTriggerEnter - " + anims.Length + " Animator found.");
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
	
	void OnTriggerExit(Collider collision) 
	{
		GameObject go = TargetObject != null ? TargetObject : gameObject;

		Animator[] anims = go.GetComponentsInChildren<Animator>();
		Debug.Log ("OnTriggerExit - " + anims.Length + " Animator found.");

		foreach (Animator anim in anims)
		{
			if (!Toggle)
			{
				anim.enabled = false;
			}
		}
	}
}

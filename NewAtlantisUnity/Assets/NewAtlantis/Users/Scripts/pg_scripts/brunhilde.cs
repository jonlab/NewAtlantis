using UnityEngine;
using System.Collections;

public class brunhilde : MonoBehaviour {
	public AudioClip mysound;
	Vector3 target;
	Vector3 startpos1;
	bool on = false;
	// Use this for initialization
	void Start () {

		if (GetComponent<AudioSource>())
			GetComponent<AudioSource>().Stop ();
		startpos1 = transform.position;
		//target = new Vector3 (9.2410f, -7.0f, -1.9652214f);
	
	}
	void OnMouseEnter ()
	{
		GetComponent<Renderer>().material.color = Color.white;
	
		if (on == true)
		{
			//if it's rotating
			on = false;
			transform.parent = null;
			transform.position = startpos1;
			GetComponent<AudioSource>().Stop();
		}
		else
		{
			on = true;
			GameObject parent = GameObject.Find ("Vahalla");
			transform.parent = parent.transform;
			GetComponent<AudioSource>().Play ();
		}
	}
	// Update is called once per frame
	void Update () {

		//return;
		if (on)
		{

			//transform.Rotate(new Vector3(Time.deltaTime*1f, Time.deltaTime*0.3f, Time.deltaTime*2f));\
			Vector3 centerrotation = transform.parent.position;
			transform.RotateAround(centerrotation, new Vector3(0.4f, 0.8f,0.1f), -Time.deltaTime*40f);
			/*Vector3 direction = target - transform.position;


			if (direction.magnitude > 00.0018f)
			{
				direction.Normalize ();
				transform.Translate (Vector3.forward * Time.deltaTime * .1f);
				transform.Translate (direction * Time.deltaTime * 0.265f);
				transform.LookAt (target);
			}

			else
			{
				// go back to the start position
				Vector3 direction1 = target + startpos1;

				if (direction1.magnitude < 2.0015f)
				{
					direction1.Normalize ();
					transform.Translate (Vector3.forward * Time.deltaTime * 1.1f);
					transform.Translate (direction1 * Time.deltaTime * 1.265f);
					transform.LookAt (startpos1);
				}
				else {

				on = false;
				audio.Stop ();
				transform.position = startpos1;
				renderer.material.color = Color.grey;
				//}
			}
			*/
		}

			

	}
}

	
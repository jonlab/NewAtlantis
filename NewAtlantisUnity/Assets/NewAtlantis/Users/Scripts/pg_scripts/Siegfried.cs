using UnityEngine;
using System.Collections;

public class Siegfried : MonoBehaviour {
	public AudioClip mysound;
	Vector3 target;
	Vector3 startpos1;
	bool on = false;

	// Use this for initialization
	void Start () {
		//target = new Vector3 (0.2317217f, -12.655194f, -12.375238f);  orig
		target = new Vector3 (-4.6317217f, -12.655194f, -13.375238f);
		startpos1 = transform.position;
		if (GetComponent<AudioSource>())
			GetComponent<AudioSource>().Stop ();
		GetComponent<AudioChorusFilter>().delay = 45.1F;


		GameObject parent = GameObject.Find ("Vahalla");	//new
		transform.parent = parent.transform;				//new
		
	}
	void OnMouseEnter ()
	{
		GetComponent<Renderer>().material.color = Color.white;
	}
	
	void OnMouseDown()
	{
		on = true;
		GetComponent<AudioSource>().Play ();
	}
	// Update is called once per frame
	void Update () {

		//transform.Rotate(new Vector3(Time.deltaTime*-0.3f, Time.deltaTime*-2.0f, 0));	//old new
		//transform.Rotate(new Vector3(-0.03f, 0.06f, 0f));  //new

		if (on)
		{
			Vector3 direction = target - transform.position;

			transform.Rotate(new Vector3(Time.deltaTime*30f, Time.deltaTime*10f, Time.deltaTime*30f));
			Debug.Log (direction.magnitude);
			//if (direction.magnitude > 00.0252f)
			if (direction.magnitude > 8.5f)
			//if (!audio.)
			{
				direction.Normalize ();
				//transform.Translate (Vector3.forward * Time.deltaTime * 1);

				//transform.Translate (direction * Time.deltaTime * 0.120f);
				Vector3 pos = transform.position;
				//pos += direction * Time.deltaTime * 0.20f;  original
				pos += direction * Time.deltaTime * 0.30f;
				transform.position = pos;
				GetComponent<AudioChorusFilter> ().depth = Time.deltaTime * 10.30f;

				//transform.LookAt (target);
			}
			else
			{
				on = false;
				GetComponent<AudioSource>().Stop ();
				transform.position = startpos1;
				GetComponent<Renderer>().material.color = Color.grey;
				transform.localRotation = Quaternion.identity;

			}
		}
		
		
		
	}
}


using UnityEngine;
using System.Collections;

public class moon1 : MonoBehaviour {
	public AudioClip mysound;
	bool on = false;
    public GameObject myParent;
    GameObject currentParent;
    void Start () {

		GameObject parent = GameObject.Find ("Vahalla");
		transform.parent = parent.transform;
		
	}
	void StartEvent ()
	{
		GetComponent<Renderer>().material.color = Color.white;
		on = !on;
		if (on)
			
			GetComponent<AudioSource>().Play ();

		else
			
			GetComponent<AudioSource>().Stop ();
		}

	// Update is called once per frame
	void Update () {

			
			//transform.Rotate(new Vector3(Time.deltaTime*1f, Time.deltaTime*0.3f, Time.deltaTime*2f));\
			Vector3 centerrotation = transform.parent.position;
			transform.RotateAround(centerrotation, new Vector3(0.1f, 0.8f,0.2f), -Time.deltaTime*1.6f);
			transform.Rotate(new Vector3(Time.deltaTime*-0.5f, Time.deltaTime*-6.0f, 0));
		}


    void OnTriggerEnter(Collider e)
    {

        seekParent(e.transform);

        if (currentParent != myParent) StartEvent();

    }


    void seekParent(Transform t)
    {

        if (t.parent != null)
        {
            seekParent(t.parent);
        }
        else
        {
            currentParent = t.gameObject;
        }

    }
}


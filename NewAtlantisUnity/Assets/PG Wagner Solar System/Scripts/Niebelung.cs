using UnityEngine;
using System.Collections;

public class Niebelung : MonoBehaviour {
	public AudioClip mysound;
	bool on = false;
    public GameObject myParent;
    GameObject currentParent;

    void Start () {
		if (GetComponent<AudioSource>())
			GetComponent<AudioSource>().Stop ();
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
	void Update () 
	{
		transform.Rotate(new Vector3(Time.deltaTime* -0.35f, Time.deltaTime*3.0f, 0.1f));

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
            if (t.gameObject == myParent)
            {
                currentParent = t.gameObject;
                return;
            }
            seekParent(t.parent);

        }
        else
        {
            currentParent = t.gameObject;
        }

    }
}
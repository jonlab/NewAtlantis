using UnityEngine;
using System.Collections;

public class froh_world : MonoBehaviour {
	
	public AudioClip mysound;
	bool on = false;
	float angle = -3.14f;
    public GameObject myParent;
    GameObject currentParent;

    void Start () {
		GameObject parent = GameObject.Find ("earth");
		transform.parent = parent.transform;
		
	}
	void StartEvent()
	{
		on = !on;
		if (on)
			
			GetComponent<AudioSource>().Play ();
		
		else
			
			GetComponent<AudioSource>().Stop ();
	}
	void Update () 
	{
		
		float speed = 0.04f;//speed of rotation
		angle -= Time.deltaTime * speed; // position on the elipse in rads (- is for counterclockwise)
		
		float x = Mathf.Cos (angle) * 1.2f;
		float z = Mathf.Sin (angle) * 0.6f;
		Vector3 offset = new Vector3 (-0.04f, 0, 0.04f); //positions it slightly off-center (0, 0, 0,= centered)
		float y = Mathf.Sin (angle) * 0.7f; // changes y for a 3D elipse
		transform.localPosition = new Vector3 (x, y, z) + offset; // sets new local position

		transform.Rotate(new Vector3(Time.deltaTime*-0.5f, Time.deltaTime*-2.0f, 0));
		
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


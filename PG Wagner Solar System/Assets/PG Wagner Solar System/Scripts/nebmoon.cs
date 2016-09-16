using UnityEngine;
using System.Collections;

public class nebmoon : MonoBehaviour {
	
	public AudioClip mysound;
	bool on = false;
	float angle = -4.16f;
    public GameObject myParent;
    GameObject currentParent;

    void Start () {
		GameObject parent = GameObject.Find ("Niebelung_world");
		transform.parent = parent.transform;
		
	}
	void OnMouseEnter()
	{
		
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

    void StartEvent()
    {

        on = !on;
        if (on)

            GetComponent<AudioSource>().Play();

        else

            GetComponent<AudioSource>().Stop();


    }



	void Update () 
	{
		
		float speed = 0.05f;//speed of rotation
		angle -= Time.deltaTime * speed; // position on the elipse in rads (- is for counterclockwise)
		float x = Mathf.Cos (angle) * 1.2f;
		float z = Mathf.Sin (angle) * 2.6f;
		Vector3 offset = new Vector3 (-0.05f, 0, 0.04f); //positions it slightly off-center (0, 0, 0,= centered)
		float y = Mathf.Sin (angle) * 0.6f; // changes y for a 3D elipse
		transform.localPosition = new Vector3 (x, y, z) + offset; // sets new local position
		Vector3 centerrotation = transform.parent.position;
		transform.RotateAround(centerrotation, new Vector3(0.4f, 1.25f,0f), -Time.deltaTime * -4.0f);
		transform.Rotate(new Vector3(Time.deltaTime*-0.4f, Time.deltaTime*-7.0f, 0));
		
	}	
}


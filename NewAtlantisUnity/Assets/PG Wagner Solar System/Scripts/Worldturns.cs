using UnityEngine;
using System.Collections;

public class Worldturns : MonoBehaviour {
	private AudioSource [] clips;
	GenaLib mycall = new GenaLib ();
	int choice;
	bool on = false;

    public GameObject myParent;
    GameObject currentParent;

	void Start () {

		clips = GetComponents<AudioSource> ();

	}
	void OnMouseEnter ()
	{
		
	}

    void OnTriggerEnter(Collider e)
    {

        seekParent(e.transform);

        if (currentParent != myParent)
        {
            StartEvent();
            print("e." + e.gameObject);
        }
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

    void StartEvent()
    {
        print("play event");
        GetComponent<Renderer>().material.color = Color.white;

        on = !on;
        if (on)
        {

            if (NA.isServer() || NA.isStandalone())
            {
                choice = mycall.Zipf(2);
                GetComponent<NetworkView>().RPC("syncChoice", RPCMode.All, choice);
            }



            //Debug.Log ("Zipf: " + choice);
            if (choice == 1)
            {
                if (!clips[1].isPlaying)
                {
                    clips[0].Stop();
                    clips[1].Play();
                }
            }
            else {
                if (!clips[0].isPlaying)
                {
                    clips[1].Stop();
                    clips[0].Play();
                }
            }






        }

        else {
            clips[0].Stop();
            clips[0].Stop();

        }

    }

	void Update () 
	{
		transform.Rotate(new Vector3(Time.deltaTime*-0.3f, Time.deltaTime*-2.0f, 0));

	}


    [RPC]
    void syncChoice(int _choice)
    {
        choice = _choice;
    }

}
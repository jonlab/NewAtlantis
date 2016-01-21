using UnityEngine;
using System.Collections;

	public class Notung_new : MonoBehaviour {
	private AudioSource [] clips;
	GenaLib mycall = new GenaLib ();
	//public AudioClip mysound;
	Vector3 startpos;
	int choice, abschoice;
	// Use this for initialization
	void Start () 
	{
		clips = GetComponents<AudioSource> ();
		startpos = transform.position;
		choice = 0;
	}

	// Update is called once per frame
	void Update () 
	{
		if (!clips [choice].isPlaying) {
			transform.position = startpos;
			GetComponent<Rigidbody> ().useGravity = false;
			GetComponent<Renderer> ().material.color = Color.yellow;

		}
	}
    void OnMouseDown()
    {




    }

    void OnCollisionEnter(Collision e)
    {


        StartEvent();

    }
	
    void StartEvent()
    {
        GetComponent<Renderer>().material.color = Color.white;

        if (NA.isServer() || NA.isStandalone())
        {
            choice = mycall.Zipf(4);
            GetComponent<NetworkView>().RPC("syncChoice", RPCMode.All, choice);

        }
        //Debug.Log ("Zipf: " + choice);
        clips[choice].Play();
        //GetComponent<AudioSource>().Play ();
       // GetComponent<Rigidbody>().useGravity = true;
    }
	void OnMouseExit ()
	{
		//renderer.material.color = Color.white;
	}
	


	void OnMouseUp () 
	{
						//audio.Stop ();
						//transform.position = startpos;
						//rigidbody.useGravity = false;
						//renderer.material.color = Color.white;
	}

    [RPC]
    void syncChoice(int _choice)
    {
        choice = _choice;
    }

}

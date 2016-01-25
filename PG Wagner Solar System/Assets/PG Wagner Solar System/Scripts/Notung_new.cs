using UnityEngine;
using System.Collections;

	public class Notung_new : MonoBehaviour {
	private AudioSource [] clips;
	GenaLib mycall = new GenaLib ();
	//public AudioClip mysound;
	Vector3 startpos;
	int choice, abschoice;

    public GameObject myParent;
    GameObject currentParent;

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
			GetComponent<Rigidbody>().useGravity = false;
			GetComponent<Renderer>().material.color = Color.yellow;
            GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
		}
	}

    void StartEvent()
    {

        if (NA.isServer() || NA.isStandalone())
        {

            choice = mycall.Zipf(4);
            GetComponent<NetworkView>().RPC("syncChoice", RPCMode.All, choice);
        
        }

        GetComponent<Renderer>().material.color = Color.white;
      
        clips[choice].Play();
        GetComponent<Rigidbody>().useGravity = true;
    }

    void OnTriggerEnter(Collider e)
    {
      
        seekParent(e.transform);

        if (currentParent != myParent)
        {
            StartEvent();
          
        }

      

    }

    void seekParent(Transform t)
    {

        if(t.parent!= null)
        {
            seekParent(t.parent);
        }
        else
        {

            currentParent = t.gameObject;
        }
  
  
    }

    [RPC]
    void syncChoice(int _choice)
    {
        choice = _choice;
    }

}

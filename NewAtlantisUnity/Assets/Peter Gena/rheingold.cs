using UnityEngine;
using System.Collections;

public class rheingold: MonoBehaviour {
	public AudioClip mysound; public float dopplerL;
	private AudioSource audioSource; 
	bool on = false;
	//Vector2 mousepos;
	int[] hexagram = new int[2];
	float[] hexflt = new float[2];
	GenaLib mycall = new GenaLib ();


    float dopplerLevel;



	void Start () {
		if (GetComponent<AudioSource>())
			GetComponent<AudioSource>().Stop ();
	}
	void OnMouseEnter ()
	{
   
	}

    void OnCollisionEnter(Collision e)
    {
        StartEvent();


    }
    void StartEvent()
    {

        GetComponent<Renderer>().material.color = Color.white;

        on = !on;
        if (on)
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.Play();

            if (NA.isServer() || NA.isStandalone())
            {
                hexagram = mycall.iChing();
                dopplerLevel = (hexagram[1] * 7.81f) * .01f;
                GetComponent<NetworkView>().RPC("syncDopplerLevel", RPCMode.All, dopplerLevel);
            }

            audioSource.dopplerLevel = dopplerLevel;



        }
        else

            GetComponent<AudioSource>().Stop();

    }


	void Update () 
	{
		transform.Rotate(new Vector3(Time.deltaTime*0.55f, Time.deltaTime*12.0f, 0.0f));
		//if (Input.GetButtonDown("Fire1"))


	}

    [RPC]
    void syncDopplerLevel(float _dopplerLevel)
    {
        dopplerLevel = _dopplerLevel;
    }
}
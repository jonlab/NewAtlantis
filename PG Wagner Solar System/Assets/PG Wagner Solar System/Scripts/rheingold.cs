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

    public GameObject myParent;
    GameObject currentParent;

	void Start () {
		if (GetComponent<AudioSource>())
			GetComponent<AudioSource>().Stop ();
	}
	void OnMouseEnter ()
	{
       // StartEvent();
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
        GetComponent<Renderer>().material.color = Color.white;

        on = !on;
        if (on)
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.Play();

            if (NA.isServer() || NA.isStandalone())
            {
                hexagram = mycall.iChing();
                hexflt[1] = (hexagram[1] * 7.81f) * .01f;
                GetComponent<NetworkView>().RPC("syncDoppler", RPCMode.All, hexflt[1]);


            }

            //dopplerL = hexflt[1];
            audioSource.dopplerLevel = dopplerL;
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
    void syncDoppler(float _doppler)
    {
        dopplerL = _doppler;
    }
}
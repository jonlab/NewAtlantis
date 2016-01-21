using UnityEngine;
using System.Collections;

public class brunhilde : MonoBehaviour {
	public AudioClip mysound;
    public float dopplerL;
	public float pitchL;

    float dopplerLevel;
    float pitch;

	private AudioSource audioSource;
	AudioSource pitchSource;
	int[] hexagram = new int[2];
	float[] hexflt = new float[2]; float[] hexpit = new float[2];
	GenaLib mycall = new GenaLib ();
	//Vector3 target;
	Vector3 startpos1;
	bool on = false;

	// Use this for initialization
	void Start () {
		startpos1 = transform.position;
		GetComponent<Renderer>().material.color = Color.red;
		pitchSource=GetComponent<AudioSource> ();
		GameObject parent = GameObject.Find ("Vahalla");
		transform.parent = parent.transform;
		//target = new Vector3 (9.2410f, -7.0f, -1.9652214f);
	}
	/*void OnCollisionEnter(Collision collision) {
				if (collision.relativeVelocity.magnitude > 2)
			on = !on;
		}*/

	void OnMouseEnter()
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
            // change Doppler shift with iChing value

            if (NA.isServer() || NA.isStandalone())
            {
                hexagram = mycall.iChing();
                hexflt[0] = (hexagram[0] * 7.81f) * .01f;
                dopplerLevel = hexflt[0];

                GetComponent<NetworkView>().RPC("syncDopplerLevel", RPCMode.All, dopplerLevel);
                //SyncDopplerLevel(dopplerLevel);

                audioSource.dopplerLevel = dopplerLevel;
              
                // change Pitch using hexagram 2 (do nothing if 0)
                if (hexagram[1] > 0)
                {
                    pitch = (hexagram[1] * .09375f) - 3.0f;
                    GetComponent<NetworkView>().RPC("syncPitch", RPCMode.All, pitch);

                    pitchSource.pitch = pitch;

                }

            }

        }
    }
	// Update is called once per frame
	void Update () {

		//return;
		if (on)
		{
			Vector3 centerrotation = transform.parent.position;
			transform.RotateAround(centerrotation, new Vector3(0.4f, 0.8f,0.1f), -Time.deltaTime*40f);
       	}

		else
		{
			//transform.parent = null;
			transform.position = startpos1;
			//return pitch to 0
			pitchSource.pitch = 1.0f;
			GetComponent<AudioSource>().Stop();
		}
	}

     [RPC]
    void syncPitch(float _pitch)
    {
        pitch = _pitch;
    }

    [RPC]
    void syncDopplerLevel(float _dopplerLevel)
    {
        dopplerLevel = _dopplerLevel;
    }

    /*
    [RPC]
    void SyncStatus(bool status)
    {
        on = status;
    }*/
}	
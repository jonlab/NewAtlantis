using UnityEngine;
using System.Collections;

public class NA_SyncRigging : MonoBehaviour {

    Animator anim;
    float time = 0;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

        if (NA.isServer() || NA.isStandalone())
        {
            time = (float)anim.GetTime();

            GetComponent<NetworkView>().RPC("setTime", RPCMode.All, time);
           // setTime(time);
        }

        anim.SetTime(time);

        if (Input.GetKeyDown(KeyCode.Return))
        {

           // anim.SetTime(time);

        }

	}

    [RPC]
    void setTime(float t)
    {
        time = t;
    }

    void OnCollisionEnter(Collision e)
    {
        if (NA.isServer() || NA.isStandalone())
        {
            time = 0;
            anim.SetTime(0);
            GetComponent<NetworkView>().RPC("setTime", RPCMode.All, time);
        }


     }
}

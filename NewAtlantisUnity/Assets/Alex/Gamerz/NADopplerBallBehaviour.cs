using UnityEngine;
using System.Collections;

public class NADopplerBallBehaviour : MonoBehaviour {
    public float lifeDuration = 200;
    float time = 0;
    public float amplitude = 0.1f;
    public float forwardIntensity = 0.1f;
    Vector3 direction = new Vector3(0, 0, 0);

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

   
        if (Network.isServer)
        {
            updateDirection();
        }
        else
        {
            GetComponent<NetworkView>().RPC("updateDirection", RPCMode.AllBuffered);


        }



        time++;
        if (time > lifeDuration) Destroy(this.gameObject);
	}

    
    Vector3 randomDirection()
    {
        return new Vector3(Random.Range(-amplitude, amplitude), Random.Range(-amplitude, amplitude), Random.Range(-amplitude, amplitude));


    }

    [RPC]
    void updateDirection()
    {

        direction = new Vector3(Random.Range(-amplitude, amplitude), Random.Range(-amplitude, amplitude), Random.Range(-amplitude, amplitude));

        transform.Rotate(direction);
        transform.Translate(transform.forward * forwardIntensity);


    }
}

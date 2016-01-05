using UnityEngine;
using System.Collections;

public class CreatureTail : MonoBehaviour {
	float lifeTime = 0;
	public float lifeSpanInSeconds = 10;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// increment lifespan
		lifeTime += Time.fixedDeltaTime;
		
		// look at camera
		//transform.LookAt(this.transform.position + Camera.main.transform.rotation * Vector3.back,Camera.main.transform.rotation * Vector3.up);
		
		// destroy ?
		if(lifeTime > lifeSpanInSeconds ) Destroy(this.gameObject);
	}
}

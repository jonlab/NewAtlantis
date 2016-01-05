using UnityEngine;
using System.Collections;

public class LinkGyzmoPositionToPointPosition : MonoBehaviour {

	public GameObject gyzmo;
	GameObject point;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (point == null)
			return;

		gyzmo.transform.position = point.transform.position;

	}

	public void SetPoint(GameObject o){
		point = o;
	}
}

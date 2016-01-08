using UnityEngine;
using System.Collections;

public class ScaleCamera : MonoBehaviour {
	public GameObject myCamera;
	 GameObject focusPoint;

	public bool noScrollMode = false;

	public KeyCode zoomInKey = KeyCode.A;
	public KeyCode zoomOutKey = KeyCode.E;

	public float minimalDistance = 4.5f;

    public float distance ;
    public GameObject gizmo;


	// Use this for initialization
	void Start () {
	
	}

	public void SetFocusPoint(GameObject o){
		focusPoint = o;
	}

	
	// Update is called once per frame
	void Update () {

		float scroll = Input.GetAxis ("Mouse ScrollWheel");

		if (noScrollMode) {

			if(Input.GetKey(zoomInKey))scroll = 0.01f;
			if(Input.GetKey(zoomOutKey))scroll = -0.01f;

		}

        float scaleGizmo = Vector3.Distance(myCamera.transform.position, focusPoint.transform.position) / 8.8f;
        gizmo.transform.localScale = new Vector3(scaleGizmo, scaleGizmo, scaleGizmo);

        if (scroll > 0 && Vector3.Distance (myCamera.transform.position,focusPoint.transform.position) <= minimalDistance)scroll = 0;

	
			Vector3 cameraPos = myCamera.transform.position;

			if (focusPoint == null) {
			Transform parent = transform.parent;
			TRS_Gizmo trs = (TRS_Gizmo)parent.GetComponent(typeof(TRS_Gizmo));
			trs.stopTRSGizmo();
			print ("The Point Object was destroyed , so we stop the TRS Gizmo process");


			return;
		}

			Vector3 pointPos = focusPoint.transform.position;

			// Si on est en local
			myCamera.transform.Translate(new Vector3(0,0,1)*scroll * (cameraPos  - pointPos).magnitude );

			// Si on est en global
			//myCamera.transform.Translate(transform.forward *scroll * (cameraPos  - pointPos).magnitude );

		//}

	}
}

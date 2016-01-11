using UnityEngine;
using System.Collections;

public class ScaleCamera : MonoBehaviour {
	public GameObject myCamera;
	 GameObject focusPoint;

	public bool noScrollMode = false;

	public KeyCode zoomInKey = KeyCode.A;
	public KeyCode zoomOutKey = KeyCode.E;

	public float minimalDistance = 4.5f;

   public float cameraDistance = 0;


    GyzmoTransformScript gyzmoTransform;
	// Use this for initialization
	void Start () {

        GameObject gyzmo = transform.parent.Find("Gyzmo").gameObject;
        gyzmoTransform = gyzmo.GetComponent<GyzmoTransformScript>();


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

        if (focusPoint == null) return;

        float distance = Vector3.Distance(myCamera.transform.position, focusPoint.transform.position);

        GameObject gizmo = transform.parent.Find("Gyzmo").gameObject;
        float scale = distance / 5.5f;
        if (scale < 1) scale = 1;
        cameraDistance = scale;

        gizmo.transform.localScale = new Vector3(scale, scale, scale);

        gyzmoTransform.NormalizeScaling(scale);
       


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

    public float returnDistance()
    {

        return cameraDistance;
    }
}

using UnityEngine;
using System.Collections;

public class NormalizeScaling : MonoBehaviour {

	public GameObject parentGizmo;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	

	}


    public void NormalizeScale()
    {
        if (transform.parent == null)
            return;

        Vector3 p = transform.parent.localScale;
        Vector3 v = new Vector3(1 / p.x, 1 / p.y, 1 / p.z);

        transform.localScale = v;
    }

    public void NormalizeScale(float ratio)
    {
        if (transform.parent == null)
            return;

        Vector3 p = transform.parent.localScale;
        Vector3 v = new Vector3(1 / p.x, 1 / p.y, 1 / p.z);

        transform.localScale = v * ratio;
    }

}

using UnityEngine;
using System.Collections;

public class rainbow : MonoBehaviour {

    public float alpha = 1f;
    public float speed = 10f;

    float h = 0.0f;
    public float s = 0.3f;
    public float v = 1.0f;

    Color col;
	
	void Update () {
        h = (h+(Time.deltaTime*speed))%1f;
        col = Color.HSVToRGB(h,s,v);
        col.a = alpha;
        gameObject.GetComponent<Renderer>().material.SetColor("_TintColor", col);
	}
}
using UnityEngine;
using System.Collections;

public class ModulateCityMaterial : MonoBehaviour {

    public float min = 1;
    public float max = 3;

    public float minL = 2;
    public float maxL = 50;

    public float acc = 0.01f;
    public float accL = 0.01f;


    Vector4 movePos;
    Material m;

    float val;
    public float lerp;

    int step, stepL;


	// Use this for initialization
	void Start () {

        m = GetComponent<MeshRenderer>().material;
        movePos = m.GetVector("_MovePos");

        lerp = minL;
        val = min;
	}
	
	// Update is called once per frame
	void Update () {
	
        if(val < min || val > max)  step++;
        if (lerp < minL || lerp > maxL) stepL++;

        if (step % 2 == 0)
        {
            val += acc;
        }
        else
        {
            val -= acc;
        }


        if (stepL % 2 == 0)
        {
            lerp += accL;
        }
        else
        {
            lerp -= accL;
        }

        movePos = new Vector4(movePos.x, val, movePos.z, movePos.w);
        m.SetVector("_MovePos", movePos);
        m.SetFloat("_LerpValue", lerp);




    }
}

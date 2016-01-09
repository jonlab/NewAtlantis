using UnityEngine;
using System.Collections;

public class ModulateCityMaterial : MonoBehaviour {
    Vector4 initMovePos,movePos;
    Material m;

    public float min = 0.15f;
    public float max = 0.63f;
    public float acceleration = 0.005f;

    public float minL = 3;
    public float maxL = 50.6f;

    public float accL = 0.1f;

    

    int step = 0;
    int stepL = 0;


    float val = 0;
public float lerp = 0;
	// Use this for initialization
	void Start () {
        m = GetComponent<MeshRenderer>().material;
        initMovePos = m.GetVector("_MovePos");
        lerp = m.GetFloat("_LerpValue");
        val = min;
        lerp = minL;
	}
	
	// Update is called once per frame
	void Update () {

        if (val < min || val > max) step++;
        if (lerp < minL || lerp > maxL) stepL++;

        if (step % 2 == 0)
        {
            val += acceleration;
        }
        else
        {
            val -= acceleration;

        }

        if(stepL % 2 == 0)
        {
            lerp += accL;
        }
        else
        {
            lerp -= accL;

        }


        Vector4 movePos = new Vector4(initMovePos.x,val, initMovePos.z, initMovePos.w);

        m.SetVector("_MovePos", movePos);

        m.SetFloat("_LerpValue", lerp);
    }
}

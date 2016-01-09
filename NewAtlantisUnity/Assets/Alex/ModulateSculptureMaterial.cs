using UnityEngine;
using System.Collections;

public class ModulateSculptureMaterial : MonoBehaviour {
    Vector4 initMovePos,movePos;
    Material m;

    public float min = 0.15f;
    public float max = 0.63f;
    public float acceleration = 0.005f;

    int step = 0;

    float val = 0;
	// Use this for initialization
	void Start () {
        m = GetComponent<MeshRenderer>().material;
        initMovePos = m.GetVector("_MovePos");

        val = min;
	}
	
	// Update is called once per frame
	void Update () {

        if (val < min || val > max) step++;
        if (step % 2 == 0)
        {
            val += acceleration;
        }
        else
        {
            val -= acceleration;

        }

        Vector4 movePos = new Vector4(initMovePos.x,val, initMovePos.z, initMovePos.w);

        m.SetVector("_MovePos", movePos);
    }
}

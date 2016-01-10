using UnityEngine;
using System.Collections;

public class NA_ModulateCityMaterialComplex : MonoBehaviour {

  //  public float red;
 //   public float green;
  //  public float blue;
 //   public float w;

    public float minRed;
    public float maxRed;
    public float minBlue;
    public float maxBlue;
    public float minGreen;
    public float maxGreen;


    public float minW;
    public float maxW;

    public float minO;
    public float maxO;

    public float minM;
    public float maxM;

    public float accR;
    public float accG;
    public float accB;
    public float accW;
    public float accO;
    public float accM;

    int stepR, stepG, stepB, stepW, stepO, stepM;

    float r, g, b, w, o, m;

    Material mat;


	// Use this for initialization
	void Start () {

        r = minRed;
        g = minGreen;
        b = minBlue;
        w = minW;
        o = minO;
        m = minM;

        mat = GetComponent<MeshRenderer>().material;


	}
	
	// Update is called once per frame
	void Update () {

        if (r < minRed || r > maxRed) stepR++;
        if (g < minGreen || g > maxGreen) stepG++;
        if (b < minBlue || b > maxBlue) stepB++;
        if (w < minW || w > maxW) stepW++;
        if (o < minO || o > maxO) stepO++;
        if (m < minM || m > maxM) stepM++;

        if (stepR % 2 == 0) { r += accR; } else { r -= accR; }
        if (stepG % 2 == 0) { g += accG; } else { g -= accG; }
        if (stepB % 2 == 0) { b += accB; } else { b -= accB; }
        if (stepW % 2 == 0) { w += accW; } else { w -= accW; }
        if (stepO % 2 == 0) { o += accO; } else { o -= accO; }
        if (stepM % 2 == 0) { m += accM; } else { m -= accM; }

        Vector4 col = new Vector4(r, g, b, w);
        mat.SetVector("_MovePos", col);
        mat.SetFloat("_LerpValue", m);
        mat.SetFloat("_Opacity", o);


    }
}

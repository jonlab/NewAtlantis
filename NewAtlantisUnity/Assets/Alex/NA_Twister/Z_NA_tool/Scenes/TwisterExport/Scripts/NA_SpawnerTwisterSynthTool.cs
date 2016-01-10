using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NA_SpawnerTwisterSynthTool : MonoBehaviour {

    public int nbCircles = 8;
    public int nbPoints = 8;

    public int nbKeys = 3;
    public int nbFrames = 400;

    List<float[]> allValues;
    List<float[]> interpolatePositions;

    float[] defaultValues;
    public bool wrongMode = false;

    public float scale = 0.5f;

    public GameObject twister;

    public int amplitude = 10;


    public Material material;

	// Use this for initialization
	void Start () {

        allValues = new List<float[]>();
        interpolatePositions = new List<float[]>();

        defaultValues = new float[nbCircles];

        for(int i = 0; i < nbCircles; i++)
        {
            defaultValues[i] = 0.5f;
        }
        for(int i = 0; i < nbKeys; i++)
        {
            allValues.Add(defaultValues);
            interpolatePositions.Add(defaultValues);
        }

	}

    void calculInterpolation()
    {
        for (int i = 0; i < nbKeys; i++)
        {
            if (i < nbKeys - 1)
            {
                interpolatePositions[i] = interpolate(allValues[i], allValues[i + 1]);
            }
            else
            {
                interpolatePositions[i] = interpolate(allValues[i], allValues[0]);
            }
        }

    }

    float[] interpolate(float[] a, float[] b)
    {
        float[] x = new float[a.Length];

        for (int i = 0; i < a.Length; i++)
        {
            x[i] = b[i] - a[i];
            if (wrongMode) x[i] = (a[i] - b[i])/2;
        }
        return x;
    }

    // Update is called once per frame
    void Update () {


        if (Input.GetKeyDown(KeyCode.R))
        {

            for (int i = 0; i < nbKeys; i++)
            {
                allValues[i] = randomizeValues(defaultValues.Length);
            }

            calculInterpolation();

            GameObject newTwister = GameObject.Instantiate(twister);
            newTwister.transform.localScale = new Vector3(scale, scale, scale);
            NA_AnimateSynthTwister anim = newTwister.GetComponent<NA_AnimateSynthTwister>();
            anim.SetValuesAndInterpolation(allValues, interpolatePositions, nbFrames, nbKeys);

            NA_Simple_CurveMaker cmk = newTwister.GetComponent<NA_Simple_CurveMaker>();
            cmk.StartCurveMaker(nbPoints, nbCircles,amplitude,material);


        }
    }


    float[] randomizeValues(int lg)
    {

        float[] f = new float[lg];

        for (int i = 0; i < lg; i++)
        {
            f[i] = Random.Range(0.0f, 1.0f);
        }

        return f;

    }

}

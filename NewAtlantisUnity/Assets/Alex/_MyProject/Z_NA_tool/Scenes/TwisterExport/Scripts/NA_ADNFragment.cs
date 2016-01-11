using UnityEngine;
using System.Collections;

public class NA_ADNFragment : MonoBehaviour {
    public Material adenine, thymine, guanine, cytosine;

    GameObject fragmentA, fragmentB;
	// Use this for initialization
	void Start () {


        fragmentA = transform.Find("AdnFrag").gameObject;
        fragmentB = transform.Find("AdnFrag2").gameObject;

        int rdInt = (int)Random.Range(0, 2);
        if (rdInt == 0) {
            AdenineGuanine();
        } else
        {
            ThymineCytosine();
        }

	}

    void AdenineGuanine()
    {
        int rdInt = (int)Random.Range(0, 2);
        if (rdInt == 0)
        {
            fragmentA.GetComponent<MeshRenderer>().material = Material.Instantiate(adenine);
            fragmentB.GetComponent<MeshRenderer>().material = Material.Instantiate(guanine);

        }
        else
        {
            fragmentB.GetComponent<MeshRenderer>().material = Material.Instantiate(adenine);
            fragmentA.GetComponent<MeshRenderer>().material = Material.Instantiate(guanine);


        }

    }

        void ThymineCytosine()
    {
        int rdInt = (int)Random.Range(0, 2);
        if (rdInt == 0)
        {
            fragmentA.GetComponent<MeshRenderer>().material = Material.Instantiate(cytosine);
            fragmentB.GetComponent<MeshRenderer>().material = Material.Instantiate(thymine);

        }
        else
        {
            fragmentB.GetComponent<MeshRenderer>().material = Material.Instantiate(cytosine);
            fragmentA.GetComponent<MeshRenderer>().material = Material.Instantiate(thymine);

        }

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Linq;
using System;

public class NAPlayImpactOnCollide : MonoBehaviour {

	public int synthID = 1;

	private float vitesseNormale;
	private float distance2Cam;
	private float[] densities = new float[] {700f        , 2500f       , 1000f      , 2500f       , 8000f        };
	private float[] youngs    = new float[] {10000000000f, 50000000000f, 2000000000f, 70000000000f, 100000000000f};

	public float wood  = 0;//UnityEngine.Random.value;
	public float stone = 0;//UnityEngine.Random.value;
	public float plastic = 0;//UnityEngine.Random.value;
	public float glass = 0;//UnityEngine.Random.value;
	public float metal = 1;

	public float size = 1;

	public float curv  = 0;
	public float stiff = 0;
	public float thick = 0;
	public float width = 0;


	private static bool bInit = false;
	private static void Init()
	{
		if (!bInit)
		{
			bInit = true;
			OSCHandler.Instance.Init ();
			OSCHandler.Instance.CreateClient("Unity2Max", IPAddress.Parse("255.255.255.255"), 7474);
			OSCHandler.Instance.CreateServer("Max2Unity", 7475);
			int count = 20;
			OSCHandler.Instance.SendMessageToClient ("Unity2Max", "Nobj",count);
		}
	}
	// Use this for initialization
	void Start () 
	{
		Init();


	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void updateDensityYoung(float wood, float stone, float plastic, float glass, float metal, out float density, out float young)
	{
		// Init of material data
		float[] material = new float[5];
		material [0] = wood;
		material [1] = stone;
		material [2] = plastic;
		material [3] = glass;
		material [4] = metal;

		float somme = 0f;

		for (int k=0; k<5; k++) 
		{
			somme += material[k];
		}

		density = 0f;
		young   = 0f;

		for (int k=0; k<5; k++) 
		{
			density += material[k] / somme * densities[k];
			young   += material[k] / somme * youngs[k];
		}
	}

	void OnCollisionEnter (Collision collision)
	{// A l'entrée en collision on crée un bruit d'impact

		// On a besoin de la vitesse normale aux points d'impact 
		Vector3 relVelocity = collision.relativeVelocity;
		Vector3 normale     = collision.contacts[0].normal.normalized;

		vitesseNormale = Mathf.Abs( Vector3.Dot(relVelocity,normale) );

		// On a besoin de la position de l'objet (distance & azimuth) pour spatialiser le son d'impact 
		distance2Cam = Vector3.Distance(Camera.main.transform.position, transform.position);

		Vector3 heading = transform.position - Camera.main.transform.position;
		float x = Vector3.Dot (heading, Camera.main.transform.right);
		float y = Vector3.Dot (heading, Camera.main.transform.forward);

		float azimuth = Mathf.Atan2 (y, x); 

		//playImpactSound (vitesseNormale, distance2Cam, azimuth);
		playImpactSound (vitesseNormale, 1, azimuth);
	}	




	public void playImpactSound(float vNorm, float distance2Cam, float azimuth)
	{
		Debug.Log("playImpactSound " + synthID);
		// Params de l'objet impacté
		/*
		float width = GetComponent<SynthParamsOnlyImpacts>().width  ;
		float curv  = GetComponent<SynthParamsOnlyImpacts>().curv   ;
		float stiff = GetComponent<SynthParamsOnlyImpacts>().stiff  ;
		float thick = GetComponent<SynthParamsOnlyImpacts>().thick  ;
		float size  = GetComponent<SynthParamsOnlyImpacts>().size   ;
		float wood  = GetComponent<SynthParamsOnlyImpacts>().Wood ;
		float stone = GetComponent<SynthParamsOnlyImpacts>().Stone ;
		float plast = GetComponent<SynthParamsOnlyImpacts>().Plastic ;
		float glass = GetComponent<SynthParamsOnlyImpacts>().Glass ;
		float metal = GetComponent<SynthParamsOnlyImpacts>().Metal ;
		float mass  = GetComponent<SynthParamsOnlyImpacts>().mass   ;
		float young = GetComponent<SynthParamsOnlyImpacts>().young  ;
		*/


		//to do : guess from object






		float mass  = 1;
		float young = 0;
		float density = 0;

		updateDensityYoung(wood, stone, plastic, glass, metal, out density, out young);

		//JT : density is not used ?


		//		// On a besoin du point d'impact exprimé dans le repère normalisé lié à la face impactée 
		//		Vector3    ImpactPoint         = collision.contacts[0].point;                          ; // coordonnées de l'impact dans le référentiel global
		//		Vector3    ImpactPointRelatif  = collision.collider.transform.InverseTransformPoint(ImpactPoint) ; // coordonnées de l'impact dans le référentiel de l'objet impacté
		//		MeshFilter mf                  = (MeshFilter)collision.collider.gameObject.GetComponent("MeshFilter");
		//		Mesh       m                   = mf.mesh;
		//		float      ImpactPointRelatifX = 0.5f + (float)ImpactPointRelatif[0]/m.bounds.size.x ; // Dans MAXMSP l'origine d'une plaque est située dans son coin inférieur gauche
		//		float      ImpactPointRelatifY = 0.5f + (float)ImpactPointRelatif[2]/m.bounds.size.z ;

		float      ImpactPointRelatifX = 0.23f;      // Dans MAXMSP l'origine d'une plaque est située dans son coin inférieur gauche
		float      ImpactPointRelatifY = 0.37f;

		List<object> msg2Max = new List<object>();

		msg2Max.Add ("ENTER");
		msg2Max.Add (width);
		msg2Max.Add (curv);
		msg2Max.Add (stiff);
		msg2Max.Add (thick);;
		msg2Max.Add (size);
		msg2Max.Add (wood);
		msg2Max.Add (stone);
		msg2Max.Add (plastic);
		msg2Max.Add (glass);
		msg2Max.Add (metal);
		msg2Max.Add (mass);
		msg2Max.Add (young);
		msg2Max.Add (vNorm);
		msg2Max.Add (distance2Cam);
		msg2Max.Add (azimuth);
		msg2Max.Add(((float)(Math.Truncate((double)ImpactPointRelatifX * 100.0) / 100.0))) ;
		msg2Max.Add(((float)(Math.Truncate((double)ImpactPointRelatifY * 100.0) / 100.0))) ;

		int N = msg2Max.Count;

		msg2Max.Add (synthID);
		msg2Max.Add (N);

		if (vNorm >= 0f) 
			OSCHandler.Instance.SendMessageToClient("Unity2Max", "fromUnity", msg2Max) ;

	}
}

using UnityEngine;
using System.Collections;

// Genere des rotations aleatoire de la camera autour d'une position
public class NA_CameraRandomRotateAroundCam : NACamera{

    //Camera
    public GameObject myCamera;
	//le point autour duquel tourne la camera
	public GameObject focusPoint;

	//Vitesse de rotation
	public float rotationSpeed = 1;


	////////// POSITION
	/// 

	//Valeur minimale de la position en Y de la camera ( voir plus bas son utilite )
	public float minimalYPositionValue = 0;

	
	//Valeurs utilisees pour instancier une position aleatoire 
	//exemple : xGap = 3 // on calcule une valeur aléatoire qui oscillera entre -xGap et xGap <==> -3 et 3
	
	public float xGap = 3;
	public float yGap = 3;
	public float zGap = 3;

	
	////////// ROTATION
	/// 

	//Valeur minimale de rotation
	public float minimalRotation = 0.5f;
	//Valeur maximale de rotation
	public float maximalRotation = 2;


	//Axe de rotation de la caméra
	Vector3 axis = new Vector3 (0, 0, 0);
	

	// Valeurs de rotation en x, y et z
	public float xRotation = 1;
	public float yRotation = 1;
	public float zRotation = 1;


	////////// VARIABLES TEMPORELLES
	/// 


	//Vitesse pour une revolution 
	public int revolutionSpeed = 4;

	//Variables temporelles
	int time=0;
	int revolution = 360; // revolution = cycle // quand time = revolution le cycle est abouti, 
	// on redemarre avec init()
	bool over=true;

   // bool activate = false;

	void Start () {

		init ();
	
	}
	//Les valeurs de positions et de rotation de la caméra sont calculées ici
	void init(){
	

		revolution = (int)Random.Range (50, 800) / revolutionSpeed;
		rotationSpeed = Random.Range (0.02f, 0.3f);
		// position aléatoire autour de l'objet
		Vector3 rdV = new Vector3 (Random.Range (-xGap, xGap),Random.Range (-yGap, yGap), Random.Range (-zGap, zGap));
	
        Vector3 cameraPosition = focusPoint.transform.position + rdV;

        int nbIteration = 10;
        int i = 0;
        while(cameraPosition.y < minimalYPositionValue && i < nbIteration)
        {
            rdV = new Vector3(Random.Range(-xGap, xGap), Random.Range(-yGap, yGap), Random.Range(-zGap, zGap));

            cameraPosition = focusPoint.transform.position + rdV;

            i++;
        }

        myCamera.transform.position = cameraPosition;

		//myCamera.transform.rotation = startPoint.transform.rotation;

		// on redémarre le temps
		time = 0;

		float rotationValue = Random.Range (minimalRotation, maximalRotation);
		axis = new Vector3 (xRotation * rotationValue , yRotation * rotationValue, zRotation * rotationValue);

		int checkRotation = (int)Random.Range (0, 10);
		if (checkRotation % 2 == 1)
			rotationSpeed *= -1;

		over = false;
	}
	// Update is called once per frame
	void Update () {


        //if (!activate) return;

		//La caméra ne peut pas descendre plus bas qu'un certain seuil
		// ( objectif : empecher a la camera de tomber sous le sol )
		//if (myCamera.transform.position.y < minimalYPositionValue && time > revolution) //fix jonathan (time>revolution)
		//	init ();
		//jonathan Fix
		//float d = (myCamera.transform.position - focusPoint.transform.position).magnitude;
		//if (d<0.1f)
		//	init();

	if (!over) {

			time++;
			// le temps a atteint sa revolution , on reinstancie la position et la rotation de la camera
			if(time>revolution){
				over=true;
				init ();
			}

			myCamera.transform.RotateAround(focusPoint.transform.position,axis,rotationSpeed);
			myCamera.transform.LookAt(focusPoint.transform.position);
		}
	}

 
}

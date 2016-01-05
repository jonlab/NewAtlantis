using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DesignAvatar : MonoBehaviour {

	WriteFile writeFile;

	public GameObject oreilles;
	 float tailleOreille = 1;
	public GameObject yeux;
	float tailleYeux = 1;
	public GameObject bouche;
	float tailleBouche = 1;

	public Material skin;

	public GameObject redUi,greenUi,blueUi,earsUi,mouthUi, eyesUi;
	Slider redValueSlider, greenValueSlider, blueValueSlider,earsSlider, mouthSlider, eyesSlider;

	// Use this for initialization
	void Start () {
	
		redValueSlider = redUi.GetComponent<Slider> ();
		greenValueSlider = greenUi.GetComponent<Slider> ();
		blueValueSlider = blueUi.GetComponent<Slider> ();

		earsSlider = earsUi.GetComponent<Slider> ();
		mouthSlider = mouthUi.GetComponent<Slider> ();
		eyesSlider = eyesUi.GetComponent<Slider> ();

		earsSlider.value = 1;
		mouthSlider.value = 1;
		eyesSlider.value = 1;

        loadValues();
		
	}

    void updateValues()
    {
        tailleOreille = earsSlider.value;
        tailleYeux = eyesSlider.value;
        tailleBouche = mouthSlider.value;

        oreilles.transform.localScale = new Vector3(tailleOreille, tailleOreille, tailleOreille);
        yeux.transform.localScale = new Vector3(tailleYeux, tailleYeux, tailleYeux);
        bouche.transform.localScale = new Vector3(tailleBouche, tailleBouche, tailleBouche);


        float redV = redValueSlider.value;
        float greenV = greenValueSlider.value;
        float blueV = blueValueSlider.value;

        skin.color = new Color(redV, greenV, blueV);

    }

    public void loadValues()
    {

        writeFile = (WriteFile)GetComponent(typeof(WriteFile));
        writeFile.loadFile();


        //updateValues();
        
   

    }


    // Update is called once per frame
    void Update () {


        updateValues();




    }

    public void setAvatarValues(float r , float g, float b, float tO, float tY, float tB){

        redValueSlider = redUi.GetComponent<Slider>();
        greenValueSlider = greenUi.GetComponent<Slider>();
        blueValueSlider = blueUi.GetComponent<Slider>();

        earsSlider = earsUi.GetComponent<Slider>();
        mouthSlider = mouthUi.GetComponent<Slider>();
        eyesSlider = eyesUi.GetComponent<Slider>();


        redValueSlider.value = r;
		greenValueSlider.value = g;
		blueValueSlider.value = b;

		earsSlider.value = tO;
		eyesSlider.value = tY;
		mouthSlider.value = tB;


	}

	void OnGUI(){

		float xb = 50;
		float yb = 50;
		float wb = 100;
		float hb = 30;

		if (GUI.Button(new Rect( xb,yb,wb,hb ) , " Save " )) {

			writeFile.editFile(redValueSlider.value, greenValueSlider.value, blueValueSlider.value, tailleYeux, tailleOreille, tailleBouche); 

		}


	}

}

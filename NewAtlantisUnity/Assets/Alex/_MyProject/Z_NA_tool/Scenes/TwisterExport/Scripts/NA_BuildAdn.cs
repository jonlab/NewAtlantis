using UnityEngine;
using System.Collections;

public class NA_BuildAdn : MonoBehaviour {

    public GameObject adnPiece;

    public int nbPieces = 10;

    public float ySpeed = 0.5f;
	// Use this for initialization
	void Start () {
	
        for(int i = 0; i < nbPieces; i++)
        {
           GameObject newPiece = GameObject.Instantiate(adnPiece);
            newPiece.transform.position = transform.position + new Vector3(0, ySpeed * transform.localScale.y, 0) * i;
            newPiece.transform.parent = transform;
            newPiece.transform.Rotate(new Vector3(0,i*360/(float)nbPieces,0));
            newPiece.transform.localScale = new Vector3(1, 1, 1);
        }



	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

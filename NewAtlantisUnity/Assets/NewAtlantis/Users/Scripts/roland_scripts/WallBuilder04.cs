using UnityEngine;
using System.Collections;

// Pour la scène "WallBuider01"
// Ce script instancie un ensemble de cubes espacés en colonnes à partir de la base 
// on joue un son à chaque instanciation
// lors de l'instanciation au démarrage (méthode Awake) le Rigidbody par défaut is not kinematic
// on essaye ensuite de lui redonner un comportement physique ou inversement

public class WallBuilder04 : MonoBehaviour{

public Rigidbody brick2_soundCollision;
public bool bascule1;
//public Rigidbody mabrique;
	
    void Awake() { // méthode awake
		 						 
		for(int i=1;i<10;i++) {
			for(int j=1;j<10;j++){
				for(int k=1;k<10;k++){
			
            //Instantiate(brick1, new Vector3(i * 2.0F, 0, 0), Quaternion.identity); // instanciation simplifiée sans nom de variable
			Rigidbody mabrique1 = Instantiate(brick2_soundCollision, new Vector3(i * 2.0F, k, j * 2.0F), Quaternion.identity) as Rigidbody; // instanciation avec nom de variable

			mabrique1.transform.parent = gameObject.transform;
			mabrique1.name = "cellule "+ i +" "+ j +" "+ k ;
			mabrique1.isKinematic = false;
					
					}
				}
			}
        }

}
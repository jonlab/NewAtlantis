using UnityEngine;
using System.Collections;

public class ChangeMaterialOnMouseOver : MonoBehaviour {

	public Material newMat;
	Material init;
	MeshRenderer mr;

	SelectorScript selector;


	void Start () {

		GameObject s = GameObject.Find ("Selector");
		selector = (SelectorScript)s.GetComponent (typeof(SelectorScript));

		mr = GetComponent<MeshRenderer> ();
	
		init = mr.material;
	}


	void OnMouseEnter(){

		mr.material = newMat;


	}

	void OnMouseDown(){

		if (selector.returnSelected () == null) {
			selector.setSelected (this.gameObject);
		}

	}

	public void displaySelected(){
		mr.material = newMat;
	}

	public void displayUnselected(){
		mr.material = init;
	}

	void OnMouseExit(){

		mr.material = init;


	}




}

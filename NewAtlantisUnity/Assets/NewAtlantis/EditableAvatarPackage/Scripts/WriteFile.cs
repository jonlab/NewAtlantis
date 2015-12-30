using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;

public class WriteFile : MonoBehaviour {

	string  fileName = "MyFile.txt";


	DesignAvatar designScript;


	void Start()
	{

	
	}

	public void loadFile(){

		designScript = (DesignAvatar)GetComponent (typeof(DesignAvatar));


		if (!File.Exists(fileName))
        {

            return;

        }



		string[] lines = File.ReadAllLines (fileName);
		List<float> values = new List<float> ();
		
		if (designScript == null)
			print ("is nulll");
		
		
		for (int i = 1; i < lines.Length; i+=2) {

			values.Add(float.Parse(lines[i])); 

		}

		designScript.setAvatarValues(values [0], values [1], values [2], values [3], values [4], values [5]);
        
	}


	public void editFile(float r,float g,float b, float tailleYeux, float tailleOreilles , float tailleBouche ){

#if UNITY_WEBPLAYER
#else
		var sr = File.CreateText(fileName);
		sr.WriteLine ("_");
		sr.WriteLine (r);
		sr.WriteLine ("_");
		sr.WriteLine (g);
		sr.WriteLine ("_");
		sr.WriteLine (b);

		sr.WriteLine ("_");
		sr.WriteLine (tailleOreilles);

		sr.WriteLine ("_");
		sr.WriteLine (tailleYeux);
		sr.WriteLine ("_");
		sr.WriteLine (tailleBouche);

		sr.Close();

#endif
	}


}

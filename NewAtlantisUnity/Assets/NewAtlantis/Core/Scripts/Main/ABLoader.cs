using UnityEngine;
using System.Collections;

public class ABLoader : MonoBehaviour {

	public string BundleName ="";
	WWW www = null;
	// Use this for initialization
	void Start () 
	{
		Load ("file://" + BundleName);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (www == null)
			return;
		if (www.isDone) 
		{
			Debug.Log ("loaded");
			AssetBundle assetBundle = www.assetBundle;
			//Obj = assetBundle.Load();
			//Object[] objects = assetBundle.LoadAll();
			//assetBundle.mainAsset
			//Debug.Log("count=" + objects.Length);
			//foreach (Object o in objects)
			//{
			//	Debug.Log ("loaded object from bundle : " + o.name);
			//	GameObject go = GameObject.Instantiate(o) as GameObject;
			//}

			GameObject go = GameObject.Instantiate(assetBundle.mainAsset) as GameObject;

			//go.transform.position = new Vector3(Random.value*20-10, 0, Random.value*20-10);

			// Unload the AssetBundles compressed contents to conserve memory
			assetBundle.Unload(false);
			www.Dispose();
			www = null;
		}
	}


	void Load(string url)
	{
		www = new WWW (url);

	}
}

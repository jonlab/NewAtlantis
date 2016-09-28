// C# Example
// Builds an asset bundle from the selected objects in the project view.
// Once compiled go to "Menu" -> "Assets" and select one of the choices
// to build the Asset Bundle

using UnityEngine;
using UnityEditor;

public class ExportAssetBundles 
{ 
	//[MenuItem("Assets/Build AssetBundle From Selection - Track dependencies")] 
	[MenuItem("Assets/Create a New Atlantis Asset Bundle from your prefab")] 
	static void ExportResource () 
	{ 
		// Bring up save panel 
		string path = EditorUtility.SaveFilePanel ("Save Resource", "", "New Resource", "unity3d"); 
		if (path.Length != 0) 
		{ 
			// Build the resource file from the active selection. 
			//Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets); 
			//removed on 16 April 2016 (Unity 5.4)
			//BuildPipeline.BuildAssetBundle(Selection.activeObject, selection, path, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets); Selection.objects = selection; 
			AssetBundleBuild[] builds = new AssetBundleBuild[1];
			string folder = System.IO.Path.GetDirectoryName(path);
			string file = System.IO.Path.GetFileName(path);
			builds[0] = new AssetBundleBuild();
			builds[0].assetBundleName = file;
			builds[0].assetNames = new string[1];
			builds[0].assetNames[0] = EditorUtility.GetAssetPath(Selection.activeObject);
			//BuildPipeline.BuildAssetBundles(

			BuildPipeline.BuildAssetBundles(folder, builds, BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets, BuildTarget.StandaloneWindows64);
		}
	} 
	/*[MenuItem("Assets/Build AssetBundle From Selection - No dependency tracking")] 
	static void ExportResourceNoTrack () 
	{ 
		// Bring up save panel 
		string path = EditorUtility.SaveFilePanel ("Save Resource", "", "New Resource", "unity3d"); 
		if (path.Length != 0) 
		{ 
			// Build the resource file from the active selection. 
			BuildPipeline.BuildAssetBundle(Selection.activeObject, Selection.objects, path); 
		} 
	} 
	*/
} 
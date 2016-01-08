using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using UnityEngine;
using UnityEditor;


#region Mesh Property Types
class ZGONMeshProperty
{
	public string Name;
}

class ZGONMeshBoolProperty : ZGONMeshProperty
{	
	public bool Value;
}

class ZGONMeshFloatProperty : ZGONMeshProperty
{
	public float Value;
}

class ZGONMeshColorProperty : ZGONMeshProperty
{
	public Color Value;
}

class ZGONMeshStringProperty : ZGONMeshProperty
{
	public string Value;
}

class ZGONMeshTextureProperty : ZGONMeshProperty
{
	public Texture Value;
	public string Parameter;
}

class ZGONMeshShaderProperty : ZGONMeshProperty
{
	public string Value;
}

class ZGONAssetProperties
{
	// Object Properties
	public const string objTangentFromUv2Name 		= "ObjTangentFromUv2";
	public const string objScaleName 				= "ObjScale";
	
	// Texture Properties
	public const string texNormalName 				= "TexNormal";
	public const string texOcclusionName 			= "TexOcclusion";
	public const string texDetailNormalName 		= "TexDetailNormal";
	public const string texDiffuseName 				= "TexDiffuse";
	public const string texBodyNormalName 			= "TexBodyNormal";
	
	// Material Properties
	public const string matShaderName				= "MatShader";
	public const string matName						= "MatName";
	public const string matMainColorName			= "MatMainColor";
    public const string matSpecColorName			= "matSpecColor";
    public const string matShineName				= "MatShine";
    public const string matBodyNormalPowerName		= "MatBodyNormalPower";
    public const string matNormalPowerName			= "MatNormalPower";
    public const string matDetailNormalPowerName	= "MatDetailNormalPower";
    public const string matDetailTexEnabledName		= "MatDetailTexEnabled";
    public const string matDetailTexOffsetName		= "MatDetailTexOffset";
    public const string matDetailTexScaleName		= "MatDetailTexScale";	
}
#endregion

class ZGONAssetPostProcessor : AssetPostprocessor
{
	const string _brandingPostfix = "ZGON";
	const string _sharedFolderName = "Shared";
	const string _matDirName = "Materials";
	const string _prefabDirName = "Prefabs";
	const string _meshDirName = "Meshes";
	const string _texDirName = "Textures";
	
	// Directory and file info
	DirectoryInfo _assetDirectoryInfo;
	string _assetName;
	string _meshsetRootDir;
	string _meshsetName;
	string _rootFolderName;
    string _sharedDir;
    string _meshsetSharedDirName;
    string _meshsetSharedDir;
	
	XmlDocument _meshsetXml = new XmlDocument();
	string _lastMeshsetName = String.Empty;
	List<ZGONMeshProperty> _meshProps = new List<ZGONMeshProperty>();
	
	void OnPreprocessModel()
	{		
		 _rootFolderName = "Assets/" + _brandingPostfix + "Assets/";
		
		if (!assetPath.Contains(_brandingPostfix + "Assets"))
			return;
		

		// Setup directory infos
		_assetDirectoryInfo = new DirectoryInfo(assetPath);
		_assetName = Path.GetFileNameWithoutExtension(_assetDirectoryInfo.Name);
		_meshsetName = _assetDirectoryInfo.Parent.Parent.Name;
		_meshsetRootDir = _rootFolderName + _meshsetName + "/";
        _sharedDir = FindDirectoryInProject(_sharedFolderName, true);

        _meshsetSharedDirName = (_meshsetName.Split(new char[] { '_' }))[0] + "_" + _sharedFolderName;
        _meshsetSharedDir = FindDirectoryInProject(_meshsetSharedDirName, true);
		
		// Read the properties from the XML file
		ParseXMLData();
		
		// Disable auto generated materials
		ModelImporter importer = (ModelImporter)assetImporter;
		importer.generateMaterials = ModelImporterGenerateMaterials.None;
		importer.swapUVChannels = false;
		importer.generateAnimations = ModelImporterGenerateAnimations.None;
		importer.tangentImportMode = ModelImporterTangentSpaceMode.None;
		importer.addCollider = true;
		ZGONMeshFloatProperty objScale = (ZGONMeshFloatProperty)SearchMeshProps(ZGONAssetProperties.objScaleName);
		importer.globalScale = objScale.Value;
	}
	
	void OnPostprocessModel(GameObject gameObject)
	{			
		if (!assetPath.Contains(_brandingPostfix + "Assets"))
			return;
		
		// Remove and disable auto generated materials, assign the one from the xml file
		SetupMaterial(gameObject);		
//		SetupPrefab(gameObject, newObjMat);
		
		// Recalculate the tangents using UV2
		ZGONMeshBoolProperty objTangentFromUv2 = (ZGONMeshBoolProperty)SearchMeshProps(ZGONAssetProperties.objTangentFromUv2Name);
		if (objTangentFromUv2.Value)
		{
			try
			{
				gameObject.GetComponent<MeshFilter>().sharedMesh = CalcTangentsFromUV2(gameObject.GetComponent<MeshFilter>().sharedMesh);
			}
			catch (System.Exception)
			{
				Debug.LogError(_assetName + " : Has invalid UV2 coordinates, cannot recalculate tangents.");
			}
		}
		
	}
	
	ZGONMeshProperty SearchMeshProps(string searchValue)
	{
		foreach (ZGONMeshProperty meshProp in _meshProps)
		{
			if (meshProp.Name == searchValue)
				return meshProp;
		}
		
		return null;
	}
	
	void ParseXMLData()
	{		
		if (_meshsetName != _lastMeshsetName)
		{
			_meshsetXml.Load(_meshsetRootDir + "/" + _meshsetName + ".xml");
			_lastMeshsetName = _meshsetName;
		}
		
		// Find the imported mesh in the XML file
		XmlNodeList nodeList = _meshsetXml.GetElementsByTagName(_assetName);
		IEnumerator enumNodeList = nodeList[0].GetEnumerator();
		
		while (enumNodeList.MoveNext())
		{
			XmlNode currentNode = (XmlNode) enumNodeList.Current;
			XmlNode attribTypeNode = currentNode.Attributes.GetNamedItem("type");
			
			string typeValue = attribTypeNode.Value;
			
			if (typeValue == "string")
			{
				ZGONMeshStringProperty meshProp = new ZGONMeshStringProperty();
				meshProp.Name = currentNode.Name;
				meshProp.Value = currentNode.Attributes.GetNamedItem("value").Value;
				_meshProps.Add(meshProp);
			}
			else if (typeValue == "bool")
			{
				ZGONMeshBoolProperty meshProp = new ZGONMeshBoolProperty();
				meshProp.Name = currentNode.Name;
				meshProp.Value = Convert.ToBoolean(currentNode.Attributes.GetNamedItem("value").Value);
				_meshProps.Add(meshProp);
			}
			else if (typeValue == "float")
			{
				ZGONMeshFloatProperty meshProp = new ZGONMeshFloatProperty();
				meshProp.Name = currentNode.Name;
				meshProp.Value = Convert.ToSingle(currentNode.Attributes.GetNamedItem("value").Value);
				_meshProps.Add(meshProp);
			}
			else if (typeValue == "color")
			{
				ZGONMeshColorProperty meshProp = new ZGONMeshColorProperty();
				meshProp.Name = currentNode.Name;
				Color colorValue = new Color();
				colorValue.r = Convert.ToSingle(currentNode.Attributes.GetNamedItem("r").Value);
				colorValue.g = Convert.ToSingle(currentNode.Attributes.GetNamedItem("g").Value);
				colorValue.b = Convert.ToSingle(currentNode.Attributes.GetNamedItem("b").Value);
				meshProp.Value = colorValue;
				_meshProps.Add(meshProp);
			}
			else if (typeValue == "texture")
			{
				ZGONMeshTextureProperty meshProp = new ZGONMeshTextureProperty();
				meshProp.Name = currentNode.Name;
				meshProp.Parameter = currentNode.Attributes.GetNamedItem("parameter").Value;
				
				string texPath = null;
                List<string> foundTextures = new List<string>();
                string[] fileExtensions = new string[3] { "*.psd", "*.png", "*.tga" };
				FindFilesInProject(fileExtensions, _meshsetRootDir, ref foundTextures);
                FindFilesInProject(fileExtensions, _sharedDir, ref foundTextures);
                FindFilesInProject(fileExtensions, _meshsetSharedDir, ref foundTextures);

                for (int i = 0; i < foundTextures.Count; i++)
                {
                    foundTextures[i] = CleanFilePathString(foundTextures[i]);
                }

				foreach (string foundTex in foundTextures)
				{
					string texName = currentNode.Attributes.GetNamedItem("value").Value;
					
					if (texName == string.Empty)
					{
						texPath = "none";
						continue;
					}
					
					if (foundTex.Contains(texName))
						texPath = foundTex;
				}
				
				if (texPath != null)
				{
					meshProp.Value = (Texture)AssetDatabase.LoadAssetAtPath(texPath, typeof(Texture));
					_meshProps.Add(meshProp);
				}
				else if (texPath == "none")
					Debug.Log(meshProp.Parameter + " : Has an empty texure reference.");
				else
					Debug.LogError("Did not find texture in project.");
			}
			else if (typeValue == "shader")
			{
				ZGONMeshShaderProperty meshProp = new ZGONMeshShaderProperty();
				meshProp.Name = currentNode.Name;
				meshProp.Value = currentNode.Attributes.GetNamedItem("value").Value;
				_meshProps.Add(meshProp);
			}
		}		
	}
	
	void SetupPrefab(GameObject gameObject, Material material)
	{
		string prefabPath = _meshsetRootDir + "/" + _prefabDirName + "/" + gameObject.name + ".prefab";
		UnityEngine.Object prefab = EditorUtility.CreateEmptyPrefab(prefabPath);
		EditorUtility.ReplacePrefab(gameObject, prefab);
	}
	
	Material SetupMaterial(GameObject gameObject)
	{	
		ZGONMeshStringProperty mat = (ZGONMeshStringProperty)SearchMeshProps(ZGONAssetProperties.matName);
		string matPath = _meshsetRootDir + _matDirName + "/" + mat.Value + ".mat";
		
		// Check to see if the material already exists
		Material oldMat = (Material)AssetDatabase.LoadAssetAtPath(matPath, typeof(Material));
		if (oldMat == null)
		{
			// Create a new material from props in XML file
			ZGONMeshShaderProperty matShader = (ZGONMeshShaderProperty)SearchMeshProps(ZGONAssetProperties.matShaderName);
			Material newMaterial = new Material(Shader.Find(matShader.Value));
			
			foreach (ZGONMeshProperty meshProp in _meshProps)
			{
				if (meshProp is ZGONMeshTextureProperty)
				{
					ZGONMeshTextureProperty texProp = (ZGONMeshTextureProperty)meshProp;
					newMaterial.SetTexture(texProp.Parameter, texProp.Value);
				}
				
				if (meshProp is ZGONMeshFloatProperty)
				{
					ZGONMeshFloatProperty floatProp = (ZGONMeshFloatProperty)meshProp;
					switch (floatProp.Name)
					{
					case "MatShine":
						newMaterial.SetFloat("_Shininess", floatProp.Value);
						break;
					case "MatBodyOcclusionPower":
						newMaterial.SetFloat("_BodyOcclusionPower", floatProp.Value);
						break;
					case "MatBodyNormalPower":
						newMaterial.SetFloat("_BodyBumpPower", floatProp.Value);
						break;
					case "MatNormalPower":
						newMaterial.SetFloat("_BumpPower", floatProp.Value);
						break;
					case "MatDetailNormalPower":
						newMaterial.SetFloat("_BumpDetailPower", floatProp.Value);
						break;				
					case "MatDetailTexOffset":
						newMaterial.SetFloat("_DetailTexOffset", floatProp.Value);
						break;
					case "MatDetailTexScale":
						newMaterial.SetFloat("_DetailTexScale", floatProp.Value);
						break;
					case "MatDetailTexPower":
						newMaterial.SetFloat("_DetailTexPower", floatProp.Value);
						break;
					case "UvOffsetY":
						newMaterial.SetFloat("_AtlasOffsetY", floatProp.Value);
						break;
					case "UvOffsetX":
						newMaterial.SetFloat("_AtlasOffsetX", floatProp.Value);
						break;
					case "UvScaleY":
						newMaterial.SetFloat("_AtlasScaleY", floatProp.Value);
						break;
					case "UvScaleX":
						newMaterial.SetFloat("_AtlasScaleX", floatProp.Value);
						break;						
					}
				}
			}

			// Create and set the new material
			AssetDatabase.CreateAsset(newMaterial, matPath);
			gameObject.GetComponent<MeshRenderer>().material = newMaterial;
			return newMaterial;
		}
		else
			gameObject.GetComponent<MeshRenderer>().material = oldMat;
			return oldMat;
	}
	
    public static Mesh CalcTangentsFromUV2(Mesh mesh)
    {
        int triangleCount = mesh.triangles.Length;
        int vertexCount = mesh.vertices.Length;

        Vector3[] tan1 = new Vector3[vertexCount];
        Vector3[] tan2 = new Vector3[vertexCount];

        Vector4[] tangents = new Vector4[vertexCount];

        for(long a = 0; a < triangleCount; a+=3)
        {
			if (a + 1 > mesh.triangles.Length)
				throw new System.Exception();
			
            long i1 = mesh.triangles[a+0];
            long i2 = mesh.triangles[a+1];
            long i3 = mesh.triangles[a+2];

            Vector3 v1 = mesh.vertices[i1];
            Vector3 v2 = mesh.vertices[i2];
            Vector3 v3 = mesh.vertices[i3];

            Vector2 w1 = mesh.uv2[i1];
            Vector2 w2 = mesh.uv2[i2];
            Vector2 w3 = mesh.uv2[i3];

            float x1 = v2.x - v1.x;
            float x2 = v3.x - v1.x;
            float y1 = v2.y - v1.y;
            float y2 = v3.y - v1.y;
            float z1 = v2.z - v1.z;
            float z2 = v3.z - v1.z;

            float s1 = w2.x - w1.x;
            float s2 = w3.x - w1.x;
            float t1 = w2.y - w1.y;
            float t2 = w3.y - w1.y;

            float r = 1.0f / (s1 * t2 - s2 * t1);

            Vector3 sdir = new Vector3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);
            Vector3 tdir = new Vector3((s1 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r, (s1 * z2 - s2 * z1) * r);

            tan1[i1] += sdir;
            tan1[i2] += sdir;
            tan1[i3] += sdir;

            tan2[i1] += tdir;
            tan2[i2] += tdir;
            tan2[i3] += tdir;
        }


        for (long a = 0; a < vertexCount; ++a)
        {
            Vector3 n = mesh.normals[a];
            Vector3 t = tan1[a];

            Vector3 tmp = (t - n * Vector3.Dot(n, t)).normalized;
            tangents[a] = new Vector4(tmp.x, tmp.y, tmp.z);

            tangents[a].w = (Vector3.Dot(Vector3.Cross(n, t), tan2[a]) < 0.0f) ? -1.0f : 1.0f;
        }

        mesh.tangents = tangents;
		return mesh;
    }
	
#region Directory / File Helper Methods	
	string FindDirectoryInProject(string dirName, bool matchWholeWord)
	{
		string[] foundDirs = Directory.GetDirectories(_rootFolderName);

        if (!matchWholeWord)
        {
            foreach (string dir in foundDirs)
            {
                if (dir.Contains(dirName))
                    return dir;
            }
        }
        else
        {
            foreach (string dir in foundDirs)
            {
                string[] splitDirNames = dir.Split(new char[] { '/' });
                string lastDirName = splitDirNames[splitDirNames.Length - 1];
                if (dirName == lastDirName)
                    return dir;
            }
        }
		
		return null;
	}
	
	string FindFileInProject(string fileName)
	{	
		string[] foundFiles = Directory.GetFiles(_rootFolderName, "*" + Path.GetExtension(fileName), SearchOption.AllDirectories);
		foreach (string file in foundFiles)
		{
			if (file.Contains(fileName))
				return file;
		}
		
		return null;
	}
	
	string FindFileInProject(string fileName, string path)
	{
		string[] foundFiles = Directory.GetFiles(path, "*" + Path.GetExtension(fileName), SearchOption.AllDirectories);
		foreach (string file in foundFiles)
		{
			if (file.Contains(fileName))
				return file;
		}
		
		return null;
	}
	
	List<string> FindFilesInProject(string[] fileExtensions)
	{
		List<string> fileList = new List<string>();
		foreach (string fileExt in fileExtensions)
		{
			string[] foundFiles = Directory.GetFiles(_rootFolderName, "*" + fileExt, SearchOption.AllDirectories);
			foreach (string fileName in foundFiles)
				fileList.Add(fileName);
		}
		
		if (fileList.Count != 0)
			return fileList;
		else
			return null;
	}
	
	List<string> FindFilesInProject(string[] fileExtensions, string path)
	{
		List<string> fileList = new List<string>();
		foreach (string fileExt in fileExtensions)
		{
			string[] foundFiles = Directory.GetFiles(path, fileExt, SearchOption.AllDirectories);
			foreach (string fileName in foundFiles)
				fileList.Add(fileName);
		}
		
		if (fileList.Count != 0)
			return fileList;
		else
			return null;
	}

    void FindFilesInProject(string[] fileExtensions, string path, ref List<string> foundFilesList)
    {
        foreach (string fileExt in fileExtensions)
        {
            string[] foundFiles = Directory.GetFiles(path, fileExt, SearchOption.AllDirectories);
            foreach (string fileName in foundFiles)
                foundFilesList.Add(fileName);
        }
    }

    string CleanFilePathString(string filePath)
    {
        return filePath.Replace("\\", "/");
    }
	
    //string ConvertAbsolutePath(string absPath)
    //{
    //    // Walk the path up until we hit the Assets folder
    //    DirectoryInfo dirInfo = new DirectoryInfo(absPath);
    //    string relativePath = dirInfo.Name;
    //    while (!_rootFolderName.Contains(dirInfo.Parent.Name))
    //    {
    //        relativePath = dirInfo.Parent.Name + _dirSlashes + relativePath;
    //        dirInfo = dirInfo.Parent;
    //    }
		
    //    relativePath = _rootFolderName + relativePath;
    //    return relativePath;
    //}
#endregion	
}


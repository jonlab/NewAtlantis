using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NA_Simple_CurveMaker : MonoBehaviour
{


    public Material material;

    int nbPoints, nbCircles;
    float[] radiusValues;
    int amplitude = 25;

    float width, height;


    List<Vector3> vertices;
    List<int> faces;

    MeshFilter mf;
    MeshRenderer mr;

    Mesh m;

    public float yIteration = 1.5f;
    // Use this for initialization
    void Start()
    {
        initComponents();
        BuildAllCircles();
        AssignMesh();
    }

    public void StartCurveMaker(int nbP, int nbC, int ampli,Material mat)
    {
        nbPoints = nbP;
        nbCircles = nbC;
        amplitude = ampli;
        material = mat;
    }

    public void SetRadiusValues(float[] f)
    {
        radiusValues = f;
    }
    void initComponents()
    {

        MeshRenderer mr = GetComponent<MeshRenderer>();
        mf = GetComponent<MeshFilter>();

        if (mf == null) mf = transform.gameObject.AddComponent<MeshFilter>();
        if (mr == null) mr = transform.gameObject.AddComponent<MeshRenderer>();

        mr.material = material;


        m = new Mesh();

        vertices = new List<Vector3>();
        faces = new List<int>();

        radiusValues = new float[nbCircles];

        for(int i = 0; i < nbCircles; i++)
        {
            radiusValues[i] = 0.5f;

        }
    }



    // Update is called once per frame
    void Update()
    {
        UpdateMeshPositions();
        AssignMesh();

   
    }

    // WHEN VERTICES AND TRIANGLES ARE READY, BUILD THE MESH OBJECT :°)
    void AssignMesh()
    {
 
        m.vertices = vertices.ToArray();
        m.triangles = faces.ToArray();
        m.RecalculateBounds();
        m.RecalculateNormals();

        mf.mesh = m;
    }

    public void BuildAllCircles()
    {

        Vector3 currentPos = new Vector3(0, 0, 0);


        for (int i = 0; i < nbCircles; i++)
        {
            BuildCircle(currentPos, width + ((radiusValues[i] - 1.0f) * amplitude), i);
            currentPos += new Vector3(0, yIteration, 0);
        }

        CloseMesh();

    }


    // BUILD A FACE WITH 3 INDEX
    void BuildFace(int a, int b, int c)
    {
        faces.Add(a);
        faces.Add(b);
        faces.Add(c);
    }

    // CONSTRUIRE UN CERCLE
    void BuildCircle(Vector3 position, float radius, int circleIndex)
    {

        bool isFirstCircle = vertices.Count == 0;

        float angle = 0;
        Vector3 centerPos = new Vector3(0, 0, 0);

        int indexStart = vertices.Count;
        int currentPoint = 0;


        float circleDeformAmplitude = 25;

        while (currentPoint < nbPoints)
        {

            // CALCUL DU CERCLE
            float x = position.x + Mathf.Cos(angle) * radius;
            float y = position.y;
            float z = position.z + Mathf.Sin(angle) * radius;



            // AJOUT DES POSITIONS
            vertices.Add(position + new Vector3(x, y, z));

            // AJOUT DES FACES
            if (!isFirstCircle)
            {
                int currentIndex = currentPoint + indexStart;
                if (currentPoint < nbPoints - 1)
                {
                    BuildFace(currentIndex, currentIndex + 1, currentIndex - nbPoints);
                    BuildFace(currentIndex + 1, currentIndex - nbPoints + 1, currentIndex - nbPoints);
                }
                else {
                    // RELIER LA FIN AU DEBUT
                    BuildFace(currentIndex, indexStart - nbPoints, currentIndex - nbPoints);
                    BuildFace(indexStart - nbPoints, currentIndex, indexStart);
                }
            }

            angle += Mathf.PI * 2 / nbPoints;
            centerPos += position + new Vector3(x, y, z) + transform.position;
            currentPoint++;

        }

        centerPos /= nbPoints;


    }

    void UpdateMeshPositions()
    {
        Vector3 currentPos = new Vector3(0, 0, 0);

        for (int i = 0; i < nbCircles; i++)
        {
            UpdateCirclePositions(currentPos, width + ((radiusValues[i] - 1.0f) * amplitude),i);
            currentPos += new Vector3(0, 1, 0);
        }

    }

    void UpdateCirclePositions(Vector3 position, float radius,int index)
    {
        // bool isFirstCircle = vertices.Count == 0;

        float angle = 0;

        int indexStart = vertices.Count;
        int currentPoint = 0;




        float circleDeformAmplitude = 25;
        while (currentPoint < nbPoints)
        {

            // CALCUL DU CERCLE
            float x = position.x + Mathf.Cos(angle) * radius;
            float y = position.y;
            float z = position.z + Mathf.Sin(angle) * radius;

            vertices[index*nbPoints + currentPoint] = position + new Vector3(x, y, z);

            currentPoint++;
            angle += Mathf.PI * 2 / nbPoints;
        }

    }

















    Vector3 getFirstCenterPos()
    {

        Vector3 center = new Vector3(0, 0, 0);

        int currentPoint = 0;

        while (currentPoint < nbPoints)
        {
            center += vertices[currentPoint];
            currentPoint++;
        }

        return center / nbPoints;
    }

    Vector3 getLastCenterPos(int indexStart)
    {

        Vector3 center = new Vector3(0, 0, 0);

        int currentPoint = 0;

        while (currentPoint < nbPoints)
        {
            int currentIndex = vertices.Count - nbPoints + currentPoint - indexStart;
            Vector3 correctPosition = vertices[currentIndex];
            center += correctPosition;
            currentPoint++;
        }

        return center / nbPoints;
    }



    void CloseMesh()
    {

        int removeLastAndFirst = 1;
        int indexStart = 0;

    
            indexStart++;
            removeLastAndFirst++;

            Vector3 firstCenter = getFirstCenterPos();
            vertices.Add(firstCenter);

            for (int i = 0; i < nbPoints; i++)
            {
                int a = i + 1;
                if (i == nbPoints - 1)
                    a = 0;
                BuildFace(i, a, vertices.Count - 1);
            }

        


            Vector3 lastCenter = getLastCenterPos(indexStart);
            vertices.Add(lastCenter);

            int indexMax = vertices.Count - removeLastAndFirst;

            for (int i = indexMax - nbPoints; i < indexMax; i++)
            {

                if (i == indexMax - 1) BuildFace(i + 1 - nbPoints, i, vertices.Count - 1);
                if (i != indexMax - 1) BuildFace(i + 1, i, vertices.Count - 1);

            }

        


    }




}

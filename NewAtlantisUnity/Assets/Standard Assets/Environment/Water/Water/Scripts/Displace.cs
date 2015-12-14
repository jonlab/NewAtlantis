<<<<<<< HEAD
using System;
using UnityEngine;

namespace UnityStandardAssets.Water
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(WaterBase))]
    public class Displace : MonoBehaviour
    {
        public void Awake()
        {
            if (enabled)
            {
                OnEnable();
            }
            else
            {
                OnDisable();
            }
        }


        public void OnEnable()
        {
            Shader.EnableKeyword("WATER_VERTEX_DISPLACEMENT_ON");
            Shader.DisableKeyword("WATER_VERTEX_DISPLACEMENT_OFF");
        }


        public void OnDisable()
        {
            Shader.EnableKeyword("WATER_VERTEX_DISPLACEMENT_OFF");
            Shader.DisableKeyword("WATER_VERTEX_DISPLACEMENT_ON");
        }
    }
=======
using System;
using UnityEngine;

namespace UnityStandardAssets.Water
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(WaterBase))]
    public class Displace : MonoBehaviour
    {
        public void Awake()
        {
            if (enabled)
            {
                OnEnable();
            }
            else
            {
                OnDisable();
            }
        }


        public void OnEnable()
        {
            Shader.EnableKeyword("WATER_VERTEX_DISPLACEMENT_ON");
            Shader.DisableKeyword("WATER_VERTEX_DISPLACEMENT_OFF");
        }


        public void OnDisable()
        {
            Shader.EnableKeyword("WATER_VERTEX_DISPLACEMENT_OFF");
            Shader.DisableKeyword("WATER_VERTEX_DISPLACEMENT_ON");
        }
    }
>>>>>>> cc58b2cb32f6563ea23f0550281efd5fb4b5637f
}
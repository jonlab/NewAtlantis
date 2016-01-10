Shader "Custom/ReflectiveTransparent" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_SpecColor ("Specular Color", Color) = (0.5,0.5,0.5,1)
	_Shininess ("Shininess", Range (0.01, 1)) = 0.078125
	_ReflectColor ("Reflection Color", Color) = (1,1,1,0.5)
	_MainTex ("Base (RGB) RefStrength (A)", 2D) = "white" {} 
	_Cube ("Reflection Cubemap", Cube) = "_Skybox" { TexGen CubeReflect }
	_Fresnel("Fresnel Coef.", Range (0.01, 4)) = 0.35
	_FresnelMin("Fresnel Min", Range (0.01, 1)) = 0.1
}
SubShader {
	LOD 200
	Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
	
CGPROGRAM
#pragma surface surf BlinnPhong alpha

sampler2D _MainTex;
samplerCUBE _Cube;

float4 _Color;
float4 _ReflectColor;
float _Shininess;


struct Input {
	float2 uv_MainTex;
	float3 worldRefl;
	float3 viewDir;
};

//fresnel factor
half _FresnelMin;
float _Fresnel;
float fresnel(float VdotN)
{
 return _FresnelMin + (1.0-_FresnelMin)*pow(1 - VdotN, _Fresnel); 
}

void surf (Input IN, inout SurfaceOutput o) {
	half4 tex = tex2D(_MainTex, IN.uv_MainTex);
	half4 c = tex * _Color;
	o.Albedo = c.rgb;
	
	half4 reflcol = texCUBE (_Cube, IN.worldRefl) * fresnel(dot(normalize(IN.viewDir),o.Normal));
	reflcol.a = 0+reflcol.r;
	//reflcol.a = 1;
	//reflcol *= tex.a;
	o.Emission = reflcol.r * _ReflectColor.r;
	o.Alpha = reflcol.a * _ReflectColor.a;
	//o.Alpha = _ReflectColor.a;
	o.Gloss = tex.a*4;
	o.Specular = _Shininess;
}
ENDCG
}
	
FallBack "Transparent/VertexLit"
} 

Shader "Custom/3dSpecular" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_SpecColor ("SpecColor", Color ) = (1,1,1,1)
		_Shininess ("Shininess", Float ) = 10
	}
	SubShader {
		Tags { "LightMode"="ForwardBase" }

		Pass{
		CGPROGRAM
		
		#pragma vertex vert
		#pragma fragment frag
		
		uniform float4 _Color;
		uniform float4 _SpecColor;
		uniform float _Shininess;
		
		uniform float4 _LightColor0;
		
		struct vertexInput{
		
		float4 vertex : POSITION;
		float3 normal : NORMAL;
		
		};
		
		struct vertexOutput{
		float4 pos : SV_POSITION;
		float4 posWorld : TEXCOORD0;
		float3 normalDir : TEXCOORD1;
		
		};
		
		vertexOutput vert(vertexInput v){
		vertexOutput o;
		
		o.posWorld = mul(_Object2World, v.vertex);
		o.normalDir = normalize( mul(float4(v.normal,0.0), _World2Object).xyz);
		
	
	
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		return o;
		
		}
		
		float4 frag(vertexOutput i) : COLOR 
		{
		float3 normalDirection = i.normalDir;
		float3 viewDirection = normalize( _WorldSpaceCameraPos.xyz - i.posWorld.xyz );
		float3 lightDirection;
		float atten = 1.0;
		
		//lighting
		lightDirection = normalize(_WorldSpaceLightPos0.xyz);
		float3 diffuseReflection = atten * _LightColor0.xyz * max( 0.0 , dot( normalDirection , lightDirection ) );
		float3 specularReflection = atten * _LightColor0.xyz * _SpecColor.rgb * max(0.0 , dot(normalDirection , lightDirection ) ) * pow( max(0.0, dot(reflect( -lightDirection , normalDirection ) , viewDirection) ), _Shininess );
		float3 lightFinal = diffuseReflection + specularReflection + UNITY_LIGHTMODEL_AMBIENT;
		
		return float4(lightFinal * _Color.rgb, 1.0);
		}
		
		
	
	
		ENDCG
	} 

}

}

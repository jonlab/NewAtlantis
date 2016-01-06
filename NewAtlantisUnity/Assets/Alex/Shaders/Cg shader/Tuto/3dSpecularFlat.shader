Shader "Custom/3dSpecularFlat" {
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
		float4 col : COLOR;
		
		};
		
		vertexOutput vert(vertexInput v){
		vertexOutput o;
		
		float3 normalDirection = normalize( mul(float4(v.normal , 0.0 ) , _World2Object ).xyz);
		float3 viewDirection = normalize(float3( float4( _WorldSpaceCameraPos.xyz , 1.0 ) -
		mul(UNITY_MATRIX_MVP, v.vertex).xyz) );
		float3 lightDirection;
		float atten = 1.0;
		//lightDirection = normalize( _WorldSpaceLightPos0.xyz);
		
		o.col = float4(viewDirection,1.0);
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		return o;
		
		}
		
		float4 frag(vertexOutput i) : COLOR 
		{
		
		return i.col;
		}
		
		
	
	
		ENDCG
	} 

}

}

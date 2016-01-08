Shader "Custom/2dAmbient" {
	Properties{
		_Color("Color", Color) = (1.0,1.0,1.0)
	}
	
	SubShader{
		Pass{
		Tags { "LightMode" = "ForwardBase" }
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		
		//user defined variables
		uniform float4 _Color;
		
		//unity defined variableds
		uniform float4 _LightColor0;
		//Unity 3 definition
		//float4x4 _Object2World;
		//float4x4 _World2Object;
		//float4 _WorldSpaceLightPos0;
		
		//base Input structs
		struct vertexInput{
		float4 vertex : POSITION;
		float3 normal : NORMAL;
		
		
		};
		struct vertexOutput{
		float4 pos : SV_POSITION;
		//float4 col : TEX_COORD6
		float4 col : COLOR;
		
		};
		
		vertexOutput vert(vertexInput v){
			vertexOutput o;
			float3 normalDirection = normalize(mul(float4(v.normal,0.0), _World2Object)).xyz;
			float3 lightDirection;
			float atten=1.0;
			lightDirection = normalize(_WorldSpaceLightPos0.xyz);
			//float3 diffuseReflection = max(0.0,dot(normalDirection,lightDirection));
			
			float3 diffuseReflection = atten * _LightColor0.xyz * _Color.rgb * max(0.0,dot(normalDirection,lightDirection));
			float3 lightFinal = diffuseReflection + UNITY_LIGHTMODEL_AMBIENT.xyz;
									
		 	o.col = float4(lightFinal * _Color.rgb, 1.0);
			o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
			return o;
		}
		
		float4 frag(vertexOutput i) : COLOR{
		
		return i.col;
		}
		
		ENDCG
		}	
	
	}
	
	
	//FallBack "Diffuse"
}

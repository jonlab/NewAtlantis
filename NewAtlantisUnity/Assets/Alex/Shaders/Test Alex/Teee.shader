Shader "Custom/Teee" {

	Properties{
		//NO PROPERTIES
	}
	
	SubShader{
		Pass{
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag

		//base Input structs
		struct vertexInput{
		float4 vertex : POSITION;
		float3 normal : NORMAL;
		
		};
		
		struct vertexOutput{
		float4 pos : SV_POSITION;
		float4 col : COLOR;
		float4 posWorld : TEXCOORD0;
        float3 normalDir : TEXCOORD1;
		};
		
		vertexOutput vert(vertexInput v){
			vertexOutput o;
		 	o.col = float4(v.normal, 1.0);
			o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
			
			o.posWorld = mul(_Object2World, v.vertex);
            o.normalDir = normalize( mul( float4 ( v.normal, 0.0 ), _World2Object).xyz );
            
			return o;
		}
		
		fixed4 frag(vertexOutput i) : COLOR{
		
	
		
		return fixed4(_WorldSpaceCameraPos.xyz ,i.normalDir.x);
		
		}
		
		ENDCG
		}	
	
	}
	


}

Shader "Cg shader using discard" {


	Properties{
		_Discard("DiscardValue", Float) = 0
	}



		SubShader{

			Pass{
			Cull Off // turn off triangle culling, alternatives are:
					 // Cull Back (or nothing): cull only back faces 
					 // Cull Front : cull only front faces

			CGPROGRAM

	#pragma vertex vert  
	#pragma fragment frag 

			uniform float _Discard;

		struct vertexInput {
			float4 vertex : POSITION;
		};
		struct vertexOutput {
			float4 pos : SV_POSITION;
			float4 posInObjectCoords : TEXCOORD0;
			float4 col : COLOR;
		};

		vertexOutput vert(vertexInput input)
		{
			vertexOutput output;

			output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
			output.posInObjectCoords = input.vertex;

			return output;
		}

		float4 frag(vertexOutput input) : COLOR
		{
			if (input.posInObjectCoords.y > _Discard)
			{
				discard; // drop the fragment if y coordinate > 0
			}
		return input.posInObjectCoords;
		}

			ENDCG

		}
	}
}

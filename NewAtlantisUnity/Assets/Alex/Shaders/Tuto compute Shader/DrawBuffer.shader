
Shader "Custom/DrawBuffer" 
{
	// one shader can get several subshaders
	SubShader 
	{	// one subshader has to get at least one pass
		Pass 
		{
			// Don’t render polygons facing away from the viewer (default).
			Cull back

			CGPROGRAM
			
			#include "UnityCG.cginc"
			
			// les pragmas sont des instructions au compilateur
			
			// les #pragma permettent à chaque compilateur d'offrir
			// des fonctionnalités propres aux ordinateurs et aux système d'exploitation
			// tout en conservant une compatibilité globale avec le langage (C , C++, cG , etc).
			
			#pragma target 5.0
			#pragma vertex vert
			#pragma fragment frag
			
			//vertex structure
			struct Vert
			{
				float4 position;
				float3 normal;
			};

			// BUFFER DE LA STRUCTURE PRECEDEMMENT DEFINIE "VERT"
			uniform StructuredBuffer<Vert> _Buffer;

			//vertex to pixel
			struct v2f 
			{
				float4 pos : SV_POSITION;
				float3 col : Color;
			};

			v2f vert(uint id : SV_VertexID)
			{
				Vert vert = _Buffer[id];

				v2f OUT;
				OUT.pos = mul(UNITY_MATRIX_MVP, float4(vert.position.xyz, 1));
				
				OUT.col = dot(float3(0,1,0), vert.normal) * 0.5 + 0.5;
				
				return OUT;
			}

			float4 frag(v2f IN) : COLOR
			{
				return float4(IN.col,1);
			}

			ENDCG

		}
	}
}
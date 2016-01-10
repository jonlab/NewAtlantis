Shader "Custom/A-ColorSimpleBlend" {
	Properties{
		//NO PROPERTIES
		_MovePos("MovePos",Vector) = (1.0,1.0,1.0,1.0)
		_LerpValue("LerpValue",Float) = 1
		_Opacity("Opacity", Range(0.0, 1.0)) = 0.75
	}

		
		SubShader{
		Tags{ "Queue" = "Transparent" }

			Pass{

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		/*
		Cull Front // first pass renders only back faces 
				   // (the "inside")
		ZWrite Off // don't write to depth buffer 
				   // in order not to occlude other objects
		Blend SrcAlpha OneMinusSrcAlpha // use alpha blending
		*/

			CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag

	uniform float4 _MovePos;
	uniform float _LerpValue;
	uniform float _Opacity;
		//base Input structs
	struct vertexInput {
		float4 vertex : POSITION;
		float3 normal : NORMAL;

	};

	struct vertexOutput {
		float4 pos : SV_POSITION;
		float4 col : COLOR;
	};

	vertexOutput vert(vertexInput v) {
		vertexOutput o;
		o.col = reflect(float4(abs(v.normal), 1.0) , _MovePos);
		o.pos = mul(UNITY_MATRIX_MVP, fmod(v.vertex , _LerpValue) );

		return o;
	}

	float4 frag(vertexOutput i) : COLOR{
		return float4(i.col.r,i.col.g,i.col.b, _Opacity);
		//return i.col;
	}

		ENDCG
	}

	}
		



		SubShader{
		Tags{ "Queue" = "Transparent" }

		Pass{

		Cull Back // second pass renders only front faces 
				  // (the "outside")
		ZWrite Off // don't write to depth buffer 
				   // in order not to occlude other objects
		Blend SrcAlpha OneMinusSrcAlpha // use alpha blending


		CGPROGRAM
#pragma vertex vert
#pragma fragment frag

	uniform float4 _MovePos;
	uniform float _LerpValue;
	uniform float _Opacity;
	//base Input structs
	struct vertexInput {
		float4 vertex : POSITION;
		float3 normal : NORMAL;

	};

	struct vertexOutput {
		float4 pos : SV_POSITION;
		float4 col : COLOR;
	};

	vertexOutput vert(vertexInput v) {
		vertexOutput o;
		o.col = reflect(float4(abs(v.normal), 1.0) , _MovePos);
		o.pos = mul(UNITY_MATRIX_MVP, fmod(v.vertex , _LerpValue));

		return o;
	}

	float4 frag(vertexOutput i) : COLOR{
		float4 final = float4(i.col.r,i.col.g,i.col.b,i.col.a  );
		//float a = i.col.r;
		return final;
	}

		ENDCG
	}

	}


}
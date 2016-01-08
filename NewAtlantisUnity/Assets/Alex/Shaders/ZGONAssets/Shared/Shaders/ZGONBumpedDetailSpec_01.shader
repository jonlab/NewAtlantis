Shader "ZGON : Bumped Detail Spec 01" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_TrimColor ("Trim Color", Color) = (1,1,1,1)
		_VertexMaskColor ("Vertex Mask Color", Color) = (0.5,0.5,0.5,1)
		_SpecColor ("Specular Color", Color) = (1.0, 1.0, 1.0, 1)
		_TrimSpecColor ("Trim Specular Color", Color) = (0.5, 0.5, 0.5, 1)
		_MainSpecColor ("Main Specular Color", Color) = (0.5, 0.5, 0.5, 1)		
		_Shininess ("Main Shininess", Range (0.01, 0.15)) = 0.078125
		_TrimShininess ("Trim Shininess", Range (0.01, 0.15)) = 0.078125
		_BodyOcclusionPower ("Body Occlusion Power", Range(0.0, 3.0)) = 1.0
		_BodyOcclusionColor ("Body Occlusion Color", Color) = (0,0,0,1)
		_BodyBumpPower ("Body Normalmap Power", Range(0.0, 5.0)) = 1.0
		_BumpPower ("Normalmap Power", Range(0.0, 3.0)) = 1.0
		_BumpDetailPower ("Detail Normalmap Power", Range(0.0, 4.0)) = 1.0	
		_DetailTexEnable ("Detail Texture Enable", Range(0.0, 1.0)) = 0.0	
		_DetailTexOffset ("Detail Texture Offset", Range(0.0, 1.0)) = 1.0	
		_DetailTexScale ("Detail Texture Scale", Range(1.0, 10.0)) = 2.0	
		_DetailTexPower ("Detail Texture Power", Range(1.0, 4.0)) = 1.5	
		_DetailTexColor ("Detail Texture Color", Color) = (0,0,0,1)
		_AtlasOffsetX ("Atlas Offset X", Float) = 0.0
		_AtlasOffsetY ("Atlas Offset Y", Float) = 0.0
		_AtlasScaleX ("Atlas Scale X", Float) = 1.0
		_AtlasScaleY ("Atlas Scale Y", Float) = 1.0
		_MainTex ("Base (RGB)", 2D) = "white" {}		
		_BumpMap ("Normalmap", 2D) = "bump" {}
		_BumpDetailTex ("Bump Detail (RGB)", 2D) = "bump" {}
		_BodyBumpTex ("Body Normalmap (RGB)", 2D) = "bump" {}
		_BodyOcclusionTex ("Body Occlusion (RGB)", 2D) = "white" {}
		_BumpTrimTex ("Trim Normalmap (RGB)", 2D) = "white" {}
		_DiffuseTrimTex ("Trim Diffuse (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf BlinnPhong
		#pragma target 3.0
		#include "ZGONShaderOps.cginc"

		sampler2D _MainTex;
		sampler2D _BumpMap;
		sampler2D _BumpDetailTex;
		sampler2D _BodyBumpTex;
		sampler2D _BodyOcclusionTex;
		sampler2D _BumpTrimTex;
		sampler2D _DiffuseTrimTex;
		half3 _Color;
		half3 _TrimColor;	
		half3 _TrimSpecColor;
		half3 _MainSpecColor;
		half3 _VertexMaskColor;
		half3 _BodyOcclusionColor;
		float _BodyOcclusionPower;
		float _BumpPower;
		float _BodyBumpPower;
		float _BumpDetailPower;
		float _DetailTexEnable;
		float _DetailTexOffset;
		float _DetailTexScale;
		float _DetailTexPower;
		half3 _DetailTexColor;
		half _Shininess;
		half _TrimShininess;
		float _AtlasOffsetX;
		float _AtlasOffsetY;
		float _AtlasScaleX;
		float _AtlasScaleY;

		struct Input {
			float2 uv_MainTex;
			float2 uv2_BodyBumpTex;
			float4 color : COLOR;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			float3 mainTex 				= tex2D(_MainTex, IN.uv_MainTex);
			float3 trimDifTex			= tex2D(_DiffuseTrimTex, IN.uv_MainTex);
			float3 trimBumpTex			= UnpackNormal(tex2D(_BumpTrimTex, IN.uv_MainTex));
			float2 atlasUv				= (IN.uv2_BodyBumpTex * float2(_AtlasScaleX, _AtlasScaleY)) + float2(_AtlasOffsetX, _AtlasOffsetY);
			float bodyOcc 				= (tex2D(_BodyOcclusionTex, atlasUv));			
			float3 bodyBump 			= UnpackNormal(tex2D(_BodyBumpTex, atlasUv));
			float3 bumpMap 				= UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));
			float3 bumpDetail 			= UnpackNormal(tex2D(_BumpDetailTex, IN.uv2_BodyBumpTex * _DetailTexScale));
			float3 detailTex			= tex2D(_BumpDetailTex, (IN.uv2_BodyBumpTex + _DetailTexOffset) * _DetailTexScale).rrr + 
										tex2D(_BumpDetailTex, (IN.uv2_BodyBumpTex + _DetailTexOffset) * _DetailTexScale).ggg;			
										
			half trimMask 				= (half)IN.color.r;
			half vertexColorMask 		= (half)IN.color.g;
			
			// Blend the base and trim textures
			half3 vertexColor 			= lerp(half3(1, 1, 1), _VertexMaskColor, vertexColorMask);
			float3 mainTrimDiff 		= lerp((mainTex * _Color), (trimDifTex * _TrimColor), trimMask) * vertexColor;		
			float3 mainTrimBump 		= lerp(bumpMap, trimBumpTex, trimMask);
			half3 mainTrimSpecColor 	= lerp(_MainSpecColor, _TrimSpecColor, trimMask);
			
			if (_DetailTexEnable > 0.5) {
				o.Albedo = CombineColorDetail(
				CombineOcclusion(mainTrimDiff, bodyOcc, _BodyOcclusionPower, _BodyOcclusionColor),
				detailTex, _DetailTexPower, _DetailTexColor);			
			}
			else
				o.Albedo = CombineOcclusion(mainTrimDiff, bodyOcc, _BodyOcclusionPower, _BodyOcclusionColor);
				
			o.Gloss = mainTrimDiff.g;	
			_SpecColor = _SpecColor * half4(mainTrimSpecColor, 1);	
			o.Specular = lerp(_Shininess, _TrimShininess, trimMask);
			
			if (_BodyBumpPower <= 1.0) {
				bodyBump = lerp(float3(0.5, 0.5, 1), bodyBump, _BodyBumpPower);
			}
		
			bodyBump = CombineNormalmap(bodyBump, bodyBump, _BodyBumpPower);
			
			if (_BumpPower != 0.0 && _BumpDetailPower != 0.0)		
				o.Normal = CombineNormalmap(CombineNormalmap(bodyBump, mainTrimBump, _BumpPower), bumpDetail, _BumpDetailPower);
			else if (_BumpPower == 0.0 && _BumpDetailPower != 0.0) 
				o.Normal = CombineNormalmap(bodyBump, bumpDetail, _BumpDetailPower);
			else if (_BumpPower != 0.0 && _BumpDetailPower == 0.0) 
				o.Normal = CombineNormalmap(bodyBump, mainTrimBump, _BumpPower);
			else
				o.Normal = bodyBump;
		}
		
		ENDCG
	}
		
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf BlinnPhong
		#pragma target 2.0
		#include "ZGONShaderOps.cginc"

		sampler2D _MainTex;
		sampler2D _BumpMap;	
		sampler2D _BodyBumpTex;
		sampler2D _BodyOcclusionTex;
		sampler2D _BumpTrimTex;
		sampler2D _DiffuseTrimTex;
		float _BumpPower;
		float _AtlasOffsetX;
		float _AtlasOffsetY;
		float _AtlasScaleX;
		float _AtlasScaleY;
		half3 _Color;
		half3 _TrimColor;
		half3 _VertexMaskColor;

		struct Input {
			float2 uv_MainTex;
			float2 uv2_BodyBumpTex;
			float4 color : COLOR;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			float3 mainTex 				= tex2D(_MainTex, IN.uv_MainTex);
			float3 trimDifTex			= tex2D(_DiffuseTrimTex, IN.uv_MainTex);
			float3 trimBumpTex			= UnpackNormal(tex2D(_BumpTrimTex, IN.uv_MainTex));
			float2 atlasUv				= (IN.uv2_BodyBumpTex * float2(_AtlasScaleX, _AtlasScaleY)) + float2(_AtlasOffsetX, _AtlasOffsetY);
			float bodyOcc 				= (tex2D(_BodyOcclusionTex, atlasUv));			
			float3 bodyBump 			= UnpackNormal(tex2D(_BodyBumpTex, atlasUv));
			float3 bumpMap 				= UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));
										
			half trimMask 				= (half)IN.color.r;
			half3 mainTrimVertexColor 	= lerp(_Color, _VertexMaskColor, trimMask);
						
			// Blend the base and trim textures
			mainTex 					= mainTex * _Color;
			trimDifTex 					= trimDifTex * _TrimColor;
			float3 mainTrimDiff 		= lerp(mainTex, trimDifTex, trimMask);
			float3 mainTrimBump 		= lerp(bumpMap, trimBumpTex, trimMask);
			
			o.Albedo = mainTrimDiff * bodyOcc;
			o.Normal = CombineNormalmap(bodyBump, mainTrimBump, _BumpPower);
		}

		ENDCG		
	} 
	
	Fallback "VertexLit"
}


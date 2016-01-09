#ifndef ZGON_SHADEROPS_INCLUDED
#define ZGON_SHADEROPS_INCLUDED

float3 CombineNormalmap(float3 normalA, float3 normalB, float normalPower) {
	float3 OUT;
	OUT = normalA + (normalB * float3(normalPower, normalPower, 0.0));
	
	OUT = normalize(OUT);
	
	return OUT;
}

float3 CombineColor(float3 colorA, float3 colorB) {
	float3 OUT;
	OUT = colorA * colorB;
	
	return OUT;
}

float3 CombineOcclusion(float3 colorA, float3 occlusion, float occPower, half3 occColor) {
	float3 OUT;
	float3 diff;

	diff = clamp((float3(1,1,1) - occlusion) * occPower, (0, 0, 0), (1, 1, 1));
	OUT = colorA * lerp(float3(1, 1, 1), occColor, diff);
	
	return OUT;
}

float3 ClampUnit(float3 colorA) {
	float3 OUT;
	OUT = clamp(colorA, float3(0, 0, 0), float3(1, 1, 1));
	
	return OUT;
}

float3 ApplyColorContrast(float3 colorA, float contrast) {
	float3 OUT;
	OUT = ((colorA - 0.5f) * max(contrast, 0)) + 0.025f;
	
	return OUT;
}

float3 CombineColorDetail(float3 colorA, float3 detail, float detailPower, half3 detailColor) {
	float3 OUT;
	float3 diff;
	float3 contrast;
	
	contrast = ClampUnit(ApplyColorContrast(detail, detailPower));
	diff = float3(1, 1, 1) - contrast;
	OUT = colorA * lerp(float3(1, 1, 1), detailColor, (diff * detailPower));
	
	return OUT;
}

#endif
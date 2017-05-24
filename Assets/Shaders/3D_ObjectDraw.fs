#version 110

varying vec2 TexCoord;
varying vec3 WorldPos;
varying vec3 CameraPos;

uniform sampler2D Surface;
uniform sampler2D Height;
uniform sampler2D Normal;
uniform sampler2D Cloud;
uniform vec3 EyePos;
uniform float Time;

/** 雾 **/


float GetDepthFogDensity(){
	const float LOG2 = 1.442695;
	float fogFactor = 1.0-clamp(CameraPos.z / 160.0,0.0,1.0);
	fogFactor = exp2( -fogFactor *  fogFactor * fogFactor * LOG2 );
    fogFactor = clamp(fogFactor - 0.5, 0.0, 1.0) * 2.0;
	
	return 1.0-fogFactor;
}

/*
float GetHeightFogDensity(){
	return clamp(WorldPos.y / 100.0,0.0,1.0);
}
*/


vec4 CalcFog(vec4 color,float density){
	const vec4 fogColor = vec4(0.0);
	return mix(fogColor,color,density);
}

vec2 ParallaxUvDelta()
{
    float h = texture2D(Height, TexCoord).r;
    vec3 viewDir = (normalize(EyePos - WorldPos));
	const float _ParallaxScale = 0.0025;
    vec2 delta = viewDir.xy / viewDir.z * h * _ParallaxScale;
    return delta;
}


void main(){
	vec4 color = texture2D(Surface,TexCoord - ParallaxUvDelta());
	gl_FragColor = color;
	
}

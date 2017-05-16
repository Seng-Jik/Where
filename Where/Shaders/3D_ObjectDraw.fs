#version 110

varying vec2 TexCoord;
varying vec3 WorldPos;
varying vec3 CameraPos;

uniform sampler2D Tex1;
uniform sampler2D Tex2;
uniform sampler2D Tex3;

/** 雾 **/
float GetDepthFogDensity(){
	return 1.0-clamp(CameraPos.z / 80.0,0.0,1.0);
}

float GetHeightFogDensity(){
	return clamp(WorldPos.y / 100.0,0.0,1.0);
}

vec4 CalcFog(vec4 color,float density){
	const vec4 fogColor = vec4(1.0);
	return mix(fogColor,color,density);
}

void main(){
	vec4 color = vec4(sin(TexCoord.x*256.0),cos(512.0*TexCoord.y),1.0,1.0);
	gl_FragColor = CalcFog(color,GetHeightFogDensity() * GetDepthFogDensity());
}

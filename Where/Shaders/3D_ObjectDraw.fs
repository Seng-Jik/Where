#version 110

varying vec2 TexCoord;
varying vec3 WorldPos;
varying vec3 CameraPos;

uniform sampler2D Tex1;
uniform sampler2D Tex2;
uniform sampler2D Tex3;



void main(){
	float densityA = clamp(WorldPos.y / 100.0,0.0,1.0);
	float densityB = 1.0-clamp(CameraPos.z / 80.0,0.0,1.0);
	float density = densityA * densityB;
	vec4 fogColor = vec4(1.0);
	vec4 color = vec4(sin(TexCoord.x*256.0),cos(512.0*TexCoord.y),1.0,1.0);
	
	vec4 foged = mix(fogColor,color,density);
	
	gl_FragColor = foged;//mix(foged,color,0.05);
}

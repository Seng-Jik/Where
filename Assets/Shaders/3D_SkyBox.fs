#version 110

varying vec3 WorldPos;
varying vec3 CameraPos;
varying vec2 TexCoord;
uniform float Time;
uniform sampler2D Perlin;

/** 云系数计算 **/
float Cloud(){
	float color = texture2D(Perlin,TexCoord + vec2(Time / 4000.0,0.0)).a;
	color = clamp(color,0.5,1.0) - 0.5;
	return color * 1.5;
}

/** 云光线衰减 **/
float CloudSub(){
	//云的厚度
	float height = texture2D(Perlin,TexCoord/2.0 + vec2(Time / 8000.0,0.0)).a;
	return 0.0;
}

/** 天空底色计算 **/
vec3 SkyColorA(){
	return vec3(0.4, 0.7, 1.0);
}

vec3 SkyColorB(){
	return vec3(0.2, 0.4, 0.6);
}

vec3 CalcSkyColor(vec2 sunCoord){
	float d = distance(TexCoord,sunCoord);
	return mix(SkyColorA(),SkyColorB(),d);
}

void main(){
	vec3 color = CalcSkyColor(vec2(0.5,0.5));
	
	float cloudDensity = Cloud();
	color += cloudDensity * vec3(1.0);
	
	gl_FragColor = vec4(color,1.0);
}

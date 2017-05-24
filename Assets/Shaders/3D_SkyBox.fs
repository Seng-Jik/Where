#version 110

varying vec3 WorldPos;
varying vec3 CameraPos;
varying vec2 TexCoord;
uniform float Time;
uniform sampler2D Perlin;
uniform vec3 EyePos;


/** 云 **/
float Shape(float color){	//加强云层形状
	return pow(sin(color * 3.1415926),3.0);
}

float Cloud(vec2 cloudCoord){	//计算云层系数
	vec2 cA = cloudCoord + vec2(Time / 2000.0,0.0);
	vec2 cB = cloudCoord + vec2(Time / 4000.0,0.0);
	vec2 cC = cloudCoord + vec2(Time / 8000.0,0.0);
	vec2 cD = cloudCoord + vec2(Time / 16000.0,0.0);
	float colorA = (texture2D(Perlin,cA).a);
	float colorB = (texture2D(Perlin,cB).a);
	float colorC = (texture2D(Perlin,cC).a);
	float colorD = (texture2D(Perlin,cD).a);
	float color = (colorA + colorB + colorC + colorD) / 4.0;

	color = clamp(color,0.5,1.0) - 0.5;
	return Shape(color * 2.0);
}

/** 天空底色 **/
vec3 SkyColorA(){	//亮色
	return vec3(0.4, 0.7, 1.0);
}

vec3 SkyColorB(){	//暗色
	return vec3(0.2, 0.4, 0.6);
}

vec3 CalcSkyColor(vec2 sunCoord){	//计算颜色，传入发光中心
	float d = distance(TexCoord,sunCoord);
	return mix(SkyColorA(),SkyColorB(),d);
}

void main(){
	//天空
	vec3 color = CalcSkyColor(vec2(0.0));
	
	//云
	float cloudDens = Cloud(TexCoord);
	color = mix(color,vec3(1.0),cloudDens);
	
	
	gl_FragColor = vec4(color,1.0);
}

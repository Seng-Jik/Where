#version 110

varying vec2 TexCoord;
varying vec3 WorldPos;
varying vec3 CameraPos;

uniform sampler2D Surface;
uniform sampler2D Height;
uniform sampler2D NormalMap;
uniform sampler2D Cloud;
uniform vec3 Normal;
uniform mat3 TBNMatrix;
uniform vec3 EyePos;
uniform float Time;

/** 视差贴图 **/
vec2 ParallaxUvDelta()
{
    float h = texture2D(Height, TexCoord).r;
    vec3 viewDir = (normalize(-EyePos - WorldPos));
	const float _ParallaxScale = 0.0025;
    vec2 delta = viewDir.xy / viewDir.z * h * _ParallaxScale;
    return delta;
}

/** 计算最终贴图坐标 **/
vec2 CalcFinalCoord()
{
	return TexCoord - ParallaxUvDelta();
}

/** 计算法线 **/
vec3 CalcFinalNormal(vec2 finalCoord)
{
	vec3 normal = 2.0 * texture2D(NormalMap,finalCoord).xyz - vec3(1.0);
	return normalize(TBNMatrix * normal);
}

/** 雾 **/
//深度雾的浓度
float GetDepthFogDensity(){
	const float LOG2 = 1.442695;
	float fogFactor = 1.0-clamp(CameraPos.z / 160.0,0.0,1.0);
	fogFactor = exp2( -fogFactor *  fogFactor * fogFactor * LOG2 );
    fogFactor = clamp(fogFactor - 0.5, 0.0, 1.0) * 2.0;
	
	return 1.0-fogFactor;
}

//计算雾的颜色
vec4 CalcFog(vec4 color,float density){
	const vec4 fogColor = vec4(0.0);
	return mix(fogColor,color,density);
}

/** 玩家的光照 **/
vec3 PlayerLighting(vec4 diffColor,vec3 normal,float diffFactor)
{
	float dis = distance(EyePos,WorldPos);
	float lightMul = 1.0-clamp(dis,0.0,50.0)/50.0;

	vec3 L = normalize(EyePos - WorldPos);
	float diff = 1.0-abs(dot(normal , L));
	vec3 color = lightMul * diff * diffFactor * diffColor.rgb;
	return vec3(color);
}

/** 太阳光照 **/


void main(){
	vec2 texCoord = CalcFinalCoord();
	vec4 color = texture2D(Surface,texCoord);
	vec3 normal = CalcFinalNormal(texCoord);

	color = vec4(PlayerLighting(color,normal,1.0),1.0);
	gl_FragColor = color;
}

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

uniform vec3 SkyColor;

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

/** 天空光照 **/
vec3 SkyLighting(vec3 diffColor,vec3 normal,float diffFactor)
{
	vec3 dir = WorldPos - vec3(5000.0,200.0,5000.0);

	vec3 L = normalize(dir);
	L.z = -L.z;
	float diff = max(0.001,dot(normal , L));
	vec3 color = diff * diffFactor * diffColor * 0.25;
	color += 0.75 * diffColor * SkyColor;
	return vec3(color);
}

/** 玩家的光照 **/
vec3 PlayerLighting(vec3 diffColor,vec3 normal,float diffFactor)
{
	vec3 dir = WorldPos - EyePos;
	float dis = distance(WorldPos,EyePos);
	float lightMul = 1.0-clamp(dis,0.0,100.0)/100.0;

	vec3 L = normalize(dir);
	float diff = clamp(1.5*(1.0-abs(dot(normal , L))),0.0,1.0) * 0.25;
	vec3 color = lightMul * lightMul * diff * diffFactor * diffColor;
	return vec3(color);
}

/** 太阳光照 **/


void main(){
	vec2 texCoord = CalcFinalCoord();
	vec3 diffColor = texture2D(Surface,texCoord).rgb;
	vec3 normal = CalcFinalNormal(texCoord);

	vec3 color = SkyLighting(diffColor,normal,1.0);
	//color += PlayerLighting(diffColor,normal,1.0);
	gl_FragColor = vec4(color,1.0);
}

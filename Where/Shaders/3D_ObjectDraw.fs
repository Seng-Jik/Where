#version 110

varying vec2 TexCoord;

uniform sampler2D Tex1;
uniform sampler2D Tex2;
uniform sampler2D Tex3;

void main(){
	gl_FragColor = vec4(sin(TexCoord.x*256.0),cos(512.0*TexCoord.y),1.0,1.0);
}

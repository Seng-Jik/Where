#version 110

attribute vec4 Vertex;
attribute vec2 TexCoordInp;
uniform mat4 Camera;


varying vec3 WorldPos;
varying vec2 TexCoord;
varying vec3 CameraPos;

void main(){
	WorldPos = Vertex.xyz;
	TexCoord = TexCoordInp;
	gl_Position = Camera * Vertex;
	CameraPos = gl_Position.xyz;
}

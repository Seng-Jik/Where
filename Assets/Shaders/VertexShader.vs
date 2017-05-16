#version 110

attribute vec4 Vertex;
attribute vec2 TexCoordInput;
uniform mat4 Camera;

varying vec2 TexCoord;
varying vec3 WorldPos;
varying vec3 CameraPos;

void main(){
	TexCoord = TexCoordInput;
	WorldPos = Vertex.xyz;
	gl_Position = Camera * Vertex;
	CameraPos = gl_Position.xyz;
}

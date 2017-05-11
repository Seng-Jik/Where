#version 110

attribute vec4 Vertex;
attribute vec2 TexCoordInput;
uniform mat4 Camera;

varying vec2 TexCoord;

void main(){
	TexCoord = TexCoordInput;
	gl_Position = Camera * Vertex;
}

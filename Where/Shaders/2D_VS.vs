#version 110

attribute vec4 Vertex;
uniform mat4 Camera;

void main(){
	gl_Position = Camera * Vertex;
}

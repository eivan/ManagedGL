#version 330

in vec4 position;
in vec3 normal;
in vec4 color;

out Vertex
{
    vec3 normal;
	vec4 color;
}  vertex;

uniform mat4 mvp;

void main(void)
{
    gl_Position = position;
    vertex.normal = normal;
	vertex.color = vec4(normal, 1);//color;
}
#version 330

in vec4 position;
in vec3 normal;
in vec3 tangent;
in vec3 bitangent;
in vec4 color;

out Vertex
{
    vec3 normal;
	vec3 tangent;
	vec3 bitangent;
	vec4 color;
}  vertex;

uniform mat4 mvp;

void main(void)
{
    gl_Position = position;
    vertex.normal = normal;
	vertex.tangent = tangent;
	vertex.bitangent = bitangent;
	vertex.color = color;
}
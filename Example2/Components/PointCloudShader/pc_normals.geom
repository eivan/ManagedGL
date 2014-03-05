#version 330

layout (points) in;
layout (line_strip) out;
//layout (points) out;
layout (max_vertices = 2) out;

in Vertex
{
    vec3 normal;
	vec4 color;
}  vertex[];

out Vertex
{
	vec4 color;
}  frag_in;

// Uniform to hold the model-view-projection matrix
uniform mat4 mvp;

// Uniform to store the length of the visualized normals
uniform float normal_length = 1.0;

void main(void)
{
	gl_Position = mvp * gl_in[0].gl_Position;
	frag_in.color = vertex[0].color;
	EmitVertex();

	gl_Position = mvp * (gl_in[0].gl_Position +
						 vec4(vertex[0].normal * normal_length, 0.0));
	frag_in.color = vertex[0].color;
	EmitVertex();

	EndPrimitive();
}
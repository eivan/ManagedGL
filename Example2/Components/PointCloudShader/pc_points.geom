#version 330

layout (points) in;
layout (points) out;
layout (max_vertices = 1) out;

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
uniform mat3 mvpit;

void main(void)
{
	gl_Position = mvp * gl_in[0].gl_Position;
	gl_PointSize = min(max(50.0/gl_Position.w, 2), 50);
	//frag_in.normal = mvpit * vertex[0].normal;
	frag_in.color = vertex[0].color;
	EmitVertex();

	EndPrimitive();
}
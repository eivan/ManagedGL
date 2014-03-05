#version 330 compatibility

in Vertex
{
	vec4 color;
}  frag_in;

out vec4 fs_out_col;

void main()
{
    fs_out_col = frag_in.color;
}
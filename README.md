ManagedGL
=========

A framework for rapid OpenGL graphics software development for .NET/Mono.

Easy vertex attributes definition
---------------------------------

```c#
[StructLayout(LayoutKind.Explicit, Pack = 1)]
public struct VertexPositionColor : IVertex
{
    [VertexElement(0, "vs_in_pos"), FieldOffset(0)]
    public Vector3 position;

    [VertexElement(1, "vs_in_col"), FieldOffset(12)]
    public Vector3 color;

    public VertexPositionColor(Vector3 position, Vector3 color)
    {
        this.position = position;
        this.color = color;
    }
}
```

Easy VBO/VAO creation
---------------------

```c#
VertexPositionColor[] vertices = ...;
vao = vertices.ToVertexArray();
```
or
```c#
vao = Factory<VertexPositionColor>.CreateVertexArray(vertices);
```

Creating shader programs
------------------------

Using the factory, your defined vertex attributes are bound automatically to the shader.

```c#
shader = Factory<VertexPositionColor>.CreateShaderProgram(
	new VertexShaders.PositionColor(),
	new FragmentShaders.Color());
```

or

```c#
public class NormalsProgram : ShaderProgram
{
	private Uniform modelViewProjectionMatrix;
	private Uniform normal_length;

	public NormalsProgram()
		: base()
	{
		const string path = "Components/PointCloudShader/";
		Attach(new Shader(ShaderType.GeometryShader, File.ReadAllText(path + "pc_normals.geom")));
		Attach(new Shader(ShaderType.VertexShader, File.ReadAllText(path + "pc_normals.vert")));
		Attach(new Shader(ShaderType.FragmentShader, File.ReadAllText(path + "pc_normals.frag")));

		Factory<VertexPositionNormalColor>.BindVertexAttributeLocations(this);
		
		Link();

		modelViewProjectionMatrix = GetUniform("mvp");
		normal_length = GetUniform("normal_length");
	}
// ...
```

More
----

Scene, Camera, Component classes and more!

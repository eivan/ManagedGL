using ManagedGL.Shaders;
using ManagedGL.Vertices;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace ManagedGL
{
    /// <summary>
    /// Egy egyszerű Factory létrehozása azt kihasználva, 
    /// hogy c#-ban a generic osztályok különböző paraméterekkel
    /// egymástól diszjunktan kezelik a statikus mezőket
    /// 
    /// Elvárás, hogy bizonyos származtatott osztályokból egyetlen létezzen,
    /// erről különben futásidejű hibával értesülünk. (lásd .Single())
    /// </summary>
    /// <typeparam name="T">Vertexformátum</typeparam>
    public class Factory<V> where V : struct, IVertex
    {
        private static readonly FieldInfo[] fields;
        public static readonly Type VertexType = typeof(V);
        public static readonly int VertexOffset = Marshal.SizeOf(VertexType);
        public static readonly VertexAttributePointer[] VertexAttributes;

        static Factory()
        {
            fields = VertexType.GetFields();

            VertexAttributes =
            (from f in fields
             let vertexElement = f.GetCustomAttribute<VertexElementAttribute>()
             let fieldOffset = f.GetCustomAttribute<FieldOffsetAttribute>()
             let fieldType = vertexAttribTypeConversion(f.FieldType)
             select new VertexAttributePointer(
                 vertexElement.UsageIndex,
                 VertexOffset,
                 fieldOffset.Value,
                 fieldType.size,
                 vertexElement.ShaderAttributeName ?? f.Name,
                 vertexElement.Normalized,
                 fieldType.type)).ToArray();
        }

        #region VAO-s

        struct attribType{
            public attribType(VertexAttribPointerType type, int size){
                this.size = size;
                this.type = type;
            }
            public VertexAttribPointerType type;
            public int size;
        }
        
        private static attribType vertexAttribTypeConversion(Type fieldType)
        {
            if(fieldType.IsByRef)
                throw new InvalidOperationException("Vertex field type cannot bea reference type");
            
            if(fieldType.IsEquivalentTo(typeof(float)))
                return new attribType(VertexAttribPointerType.Float, 1);

            if(fieldType.IsEquivalentTo(typeof(byte)))
                return new attribType(VertexAttribPointerType.Byte, 1);

            if (fieldType.IsEquivalentTo(typeof(Vector2)))
                return new attribType(VertexAttribPointerType.Float, 2);

            if (fieldType.IsEquivalentTo(typeof(Vector3)))
                return new attribType(VertexAttribPointerType.Float, 3);

            if (fieldType.IsEquivalentTo(typeof(Vector4)))
                return new attribType(VertexAttribPointerType.Float, 4);

            if (fieldType.IsEquivalentTo(typeof(Vector2d)))
                return new attribType(VertexAttribPointerType.Double, 2);

            if (fieldType.IsEquivalentTo(typeof(Vector3d)))
                return new attribType(VertexAttribPointerType.Double, 3);

            if (fieldType.IsEquivalentTo(typeof(Vector4d)))
                return new attribType(VertexAttribPointerType.Double, 4);

            if (fieldType.IsEquivalentTo(typeof(Vector2h)))
                return new attribType(VertexAttribPointerType.HalfFloat, 2);

            if (fieldType.IsEquivalentTo(typeof(Vector3h)))
                return new attribType(VertexAttribPointerType.HalfFloat, 3);
            
            if (fieldType.IsEquivalentTo(typeof(Vector4h)))
                return new attribType(VertexAttribPointerType.HalfFloat, 4);


            throw new NotImplementedException("Vertex field type (" + fieldType.ToString() + ") not supported!");
        }

        public static VertexArray<V> CreateVertexArray(params V[] vertices)
        {
            return new VertexArray<V>(vertices);
        }

        #endregion

        public static Shaders.ShaderProgram CreateShaderProgram(params Shaders.Shader[] shaders)
        {
            var result = new ShaderProgram();

            foreach(var shader in shaders)
            {
                result.Attach(shader);
            }

            BindVertexAttributeLocations(result);

            result.Link();

            return result;
        }

        public static void BindVertexAttributeLocations(ShaderProgram program)
        {
            foreach (var attribP in Factory<V>.VertexAttributes)
                attribP.BindAttribLocation(program);
        }
    }
}

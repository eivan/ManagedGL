using ManagedGL.Vertices;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagedGL.Helpers
{
    public class Geometries
    {
        public static Vector3[] Ellipsoid(uint uiStacks, uint uiSlices, float fA, float fB, float fC)
        {
            const float Pi = (float)Math.PI;

	        float tStep = (Pi) / (float)uiSlices;
	        float sStep = (Pi) / (float)uiStacks;

            var list = new List<Vector3>();

	        for(float t = -Pi/2; t <= (Pi/2)+.0001; t += tStep)
		        for(float s = -Pi; s <= Pi+.0001; s += sStep)
		        {
                    list.Add(new Vector3(
                        fA * (float)Math.Cos(t) * (float)Math.Cos(s),
                        fB * (float)Math.Cos(t) * (float)Math.Sin(s),
                        fC * (float)Math.Sin(t)));
			        list.Add(new Vector3(
                        fA * (float)Math.Cos(t+tStep) * (float)Math.Cos(s), 
                        fB * (float)Math.Cos(t+tStep) * (float)Math.Sin(s), 
                        fC * (float)Math.Sin(t+tStep)));
		        }

            return list.ToArray();
        }

        public static void Ellipsoid(uint uiStacks, uint uiSlices, float fA, float fB, float fC, out VertexPositionNormal[] vertices)
        {
            const float Pi = (float)Math.PI;

            float tStep = (Pi) / (float)uiSlices;
            float sStep = (Pi) / (float)uiStacks;

            var list = new List<VertexPositionNormal>();

            for (float t = -Pi / 2; t <= (Pi / 2) + .0001; t += tStep)
                for (float s = -Pi; s <= Pi + .0001; s += sStep)
                {
                    list.Add(new VertexPositionNormal()
                    {
                        position = new Vector3(
                            fA * (float)Math.Cos(t) * (float)Math.Cos(s),
                            fB * (float)Math.Cos(t) * (float)Math.Sin(s),
                            fC * (float)Math.Sin(t)),
                        normal = new Vector3(
                            fA * (float)Math.Cos(t) * (float)Math.Cos(s),
                            fB * (float)Math.Cos(t) * (float)Math.Sin(s),
                            fC * (float)Math.Sin(t)).Normalized()
                    });

                    list.Add(new VertexPositionNormal()
                    {
                        position = new Vector3(
                        fA * (float)Math.Cos(t + tStep) * (float)Math.Cos(s),
                        fB * (float)Math.Cos(t + tStep) * (float)Math.Sin(s),
                        fC * (float)Math.Sin(t + tStep)),
                        normal = new Vector3(
                        fA * (float)Math.Cos(t + tStep) * (float)Math.Cos(s),
                        fB * (float)Math.Cos(t + tStep) * (float)Math.Sin(s),
                        fC * (float)Math.Sin(t + tStep)).Normalized()
                    });
                }

            vertices = list.ToArray();
        }

        public static Vector3[] Sphere(uint slices = 8, float radius = 1.0f)
        {
            return Ellipsoid(slices, slices, radius, radius, radius);
        }
    }
}

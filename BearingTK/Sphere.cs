using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LearnOpenTK.Common;

using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;

namespace BearingTK
{
    public class Sphere
    {
        private readonly List<float> _vertices = new List<float>();
        private readonly List<uint> _indices = new List<uint>();

        private int _vertexArrayObject;
        private int _vertexBufferObject;
        private int _elementBufferObject;
        private Texture _diftexture;
        private Texture _spectexture;


        public Sphere(float radius, uint stackCount,uint sectorCount)
        {
            float x, y, z, xy;                              // vertex position
            float nx, ny, nz, lengthInv = 1.0f / radius;    // vertex normal
            float s, t;                                     // vertex texCoord

            float sectorStep = 2 * MathF.PI / sectorCount;
            float stackStep = MathF.PI / stackCount;
            float sectorAngle, stackAngle;

            for (int i = 0; i <= stackCount; ++i)
            {
                stackAngle = MathF.PI / 2 - i * stackStep;        // starting from pi/2 to -pi/2
                xy = radius * MathF.Cos(stackAngle);             // r * cos(u)
                z = radius * MathF.Sin(stackAngle);              // r * sin(u)

                // add (sectorCount+1) vertices per stack
                // the first and last vertices have same position and normal, but different tex coords
                for (int j = 0; j <= sectorCount; ++j)
                {
                    sectorAngle = j * sectorStep;           // starting from 0 to 2pi

                    // vertex position (x, y, z)
                    x = xy * MathF.Cos(sectorAngle);             // r * cos(u) * cos(v)
                    y = xy * MathF.Sin(sectorAngle);             // r * cos(u) * sin(v)
                    _vertices.Add(x);
                    _vertices.Add(y);
                    _vertices.Add(z);

                    // normalized vertex normal (nx, ny, nz)
                    nx = x * lengthInv;
                    ny = y * lengthInv;
                    nz = z * lengthInv;
                    _vertices.Add(nx);
                    _vertices.Add(ny);
                    _vertices.Add(nz);

                    // vertex tex coord (s, t) range between [0, 1]
                    s = 1 - (float)j / sectorCount;
                    t = (float)i / stackCount;
                    _vertices.Add(s);
                    _vertices.Add(t);
                }
            }


            uint k1, k2;
            for (uint i = 0; i < stackCount; ++i)
            {
                k1 = i * (sectorCount + 1);     // beginning of current stack
                k2 = k1 + sectorCount + 1;      // beginning of next stack

                for (uint j = 0; j < sectorCount; ++j, ++k1, ++k2)
                {
                    // 2 triangles per sector excluding first and last stacks
                    // k1 => k2 => k1+1
                    if (i != 0)
                    {
                        _indices.Add(k1);
                        _indices.Add(k2);
                        _indices.Add(k1 + 1);
                    }

                    // k1+1 => k2 => k2+1
                    if (i != (stackCount - 1))
                    {
                        _indices.Add(k1 + 1);
                        _indices.Add(k2);
                        _indices.Add(k2 + 1);
                    }

                }
            }


        }

        public float[] GetVertics()
        {
            return _vertices.ToArray();
        }

        public uint[] GetIndices()
        {
            return _indices.ToArray();
        }

        public void VBOandVAO()
        {
            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.ToArray().Length * sizeof(float), _vertices.ToArray(), BufferUsageHint.StaticDraw);

            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);
        }

        public void ElemBuf()
        {
            _elementBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.ToArray().Length * sizeof(uint), _indices.ToArray(), BufferUsageHint.StaticDraw);
        }

        public void texture(string texturePath, string specTexturePath)
        {
            _diftexture = Texture.LoadFromFile(texturePath);
            _spectexture = Texture.LoadFromFile(specTexturePath);
            _diftexture.Use(TextureUnit.Texture0);
            _spectexture.Use(TextureUnit.Texture1);
        }


        public void Pointers(Shader shader)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BindVertexArray(_vertexArrayObject);

            GL.VertexAttribPointer(shader.GetAttribLocation("aPosition"), 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            GL.EnableVertexAttribArray(shader.GetAttribLocation("aPosition"));

            GL.VertexAttribPointer(shader.GetAttribLocation("aTexCoord"), 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
            GL.EnableVertexAttribArray(shader.GetAttribLocation("aTexCoord"));

            
            GL.VertexAttribPointer(shader.GetAttribLocation("aNormal"), 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(shader.GetAttribLocation("aNormal"));
        }

        public void Bind()
        {
            GL.BindVertexArray(_vertexArrayObject);

            _diftexture.Use(TextureUnit.Texture0);
            _spectexture.Use(TextureUnit.Texture1);

        }

        public void Draw()
        {
            GL.BindVertexArray(_vertexArrayObject);

            GL.DrawElements(PrimitiveType.Triangles, _indices.ToArray().Length, DrawElementsType.UnsignedInt, 0);
        }
    }

    


}

using LearnOpenTK.Common;
using OpenTK.Graphics.OpenGL4; 

namespace BearingTK
{
    public class Circle
    {

        private readonly List<float> _vertices = new List<float>();
        private readonly List<uint> _indices = new List<uint>();

        private int _vertexArrayObject;
        private int _vertexBufferObject;
        private int _elementBufferObject;
        private Texture _diftexture;
        private Texture _spectexture;

        public Circle(float minradius, float maxradius, uint sectorCount, float thickness)
        {
            float x, y, z;

            for (uint i = 0; i < sectorCount * 4 ; i++)
            {
                x = (i % 4 < 2 ? maxradius : minradius) * MathF.Cos(        MathF.Round(i / 2, 0, MidpointRounding.ToZero) / (sectorCount * 2)  * 2 * MathF.PI          );
                y = (i % 4 < 2 ? maxradius : minradius) * MathF.Sin(        MathF.Round(i / 2, 0, MidpointRounding.ToZero) / (sectorCount * 2) * 2 * MathF.PI        );
                z = thickness / 2 * MathF.Pow(-1, i);

                _vertices.Add(x); _vertices.Add(y); _vertices.Add(z);

                _vertices.Add(0f); _vertices.Add(0f); _vertices.Add(MathF.Pow(-1, i));

                // Texture

                _vertices.Add(MathF.Round(i / 4, 0, MidpointRounding.ToZero) / sectorCount);

                _vertices.Add((i % 4 < 2) ? 1f:0f);



                _vertices.Add(x); _vertices.Add(y); _vertices.Add(z);

                _vertices.Add(x * (i % 4 < 2 ? 1 : -1) ); _vertices.Add(y * (i % 4 < 2 ? 1 : -1)); _vertices.Add(0);

                //Texture

                _vertices.Add( MathF.Round(i / 4, 0, MidpointRounding.ToZero) / sectorCount);

                //_vertices.Add(1f / (2 * sectorCount) * MathF.Round(i / 2, 0, MidpointRounding.ToZero));

                _vertices.Add( (i + 1) % 2);
            }

            for (uint i = 0; i < 4; i++)
            {
                x = (i % 4 < 2 ? maxradius : minradius) * MathF.Cos(MathF.Round(i / 2, 0, MidpointRounding.ToZero) / (sectorCount * 2) * 2 * MathF.PI);
                y = (i % 4 < 2 ? maxradius : minradius) * MathF.Sin(MathF.Round(i / 2, 0, MidpointRounding.ToZero) / (sectorCount * 2) * 2 * MathF.PI);
                z = thickness / 2 * MathF.Pow(-1, i);

                _vertices.Add(x); _vertices.Add(y); _vertices.Add(z);

                _vertices.Add(0f); _vertices.Add(0f); _vertices.Add(MathF.Pow(-1, i));

                // Texture

                _vertices.Add(1f);

                _vertices.Add((i % 4 < 2) ? 1f : 0f);



                _vertices.Add(x); _vertices.Add(y); _vertices.Add(z);

                _vertices.Add(x * (i % 4 < 2 ? 1 : -1)); _vertices.Add(y * (i % 4 < 2 ? 1 : -1)); _vertices.Add(0);

                //Texture

                _vertices.Add(1f);

                //_vertices.Add(1f / (2 * sectorCount) * MathF.Round(i / 2, 0, MidpointRounding.ToZero));

                _vertices.Add((i + 1) % 2);
            }



            for (uint i = 0, tmp = sectorCount * 8; i < sectorCount * 4; i++)  //// Top and Bottom
            {
                _indices.Add(2 * i);
                _indices.Add((2 * i + 4) );
                _indices.Add((2 * i + 8) );
            }

            for (uint i = 0, tmp = sectorCount * 8; i < sectorCount ; i++)  //// 
            {
                _indices.Add((1 + 8 * i) );
                _indices.Add((3 + 8 * i) );
                _indices.Add((9 + 8 * i) );

                _indices.Add((3 + 8 * i) );
                _indices.Add((9 + 8 * i) );
                _indices.Add((11 + 8 * i) );

                _indices.Add((5 + 8 * i));
                _indices.Add((7 + 8 * i));
                _indices.Add((13 + 8 * i) );

                _indices.Add((7 + 8 * i));
                _indices.Add((13 + 8 * i) );
                _indices.Add((15 + 8 * i) );

            }




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
            GL.BindVertexArray(_vertexArrayObject);
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
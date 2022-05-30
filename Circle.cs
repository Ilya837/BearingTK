using LearnOpenTK.Common;
using OpenTK.Graphics.OpenGL4;
using 

namespace BearingTK
{
    public class Circle
    {

        private readonly List<float> _vertices = new List<float>();
        private readonly List<uint> _indices = new List<uint>();

        private int _vertexArrayObject;
        private int _vertexBufferObject;
        private int _elementBufferObject;
        private Texture _texture;

        public Circle(float minradius, float maxradius, uint sectorCount)
        {
            float x,y,z;

            for (uint i = 0; i< sectorCount; i++)
            {
                x = (i%2 == 0 ? maxradius : minradius) * 
                _vertices.Add((float) );
            }
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using LearnOpenTK.Common;
using OpenTK.Mathematics;

namespace BearingTK
{
    public class WindowTK : GameWindow
    {
        private Shader _shader;
        private Camera _camera;
        Sphere sphere;
        Circle circle1;
        Circle circle2;
        private double _grad1 = 0;
        private double _grad2 = 0;
        private double _grad3 = 0;
        private double _povorot = 0;
        private Vector2 _lastPos;
        private bool _firstMove = true;


        private readonly Vector3[] Positions =
        {
            new Vector3(-3f, 0f, 0f),
            new Vector3(3f, 0f, 0f),
            new Vector3(0f, 3f, 0f),
            new Vector3(0f, -3f, 0f),
            new Vector3(-1.5f *(float) Math.Sqrt(2), 1.5f * (float) Math.Sqrt(2), 0f),
            new Vector3(-1.5f * (float) Math.Sqrt(2), -1.5f * (float) Math.Sqrt(2), 0f),
            new Vector3(1.5f * (float) Math.Sqrt(2), -1.5f * (float) Math.Sqrt(2), 0f),
            new Vector3(1.5f * (float) Math.Sqrt(2), 1.5f * (float) Math.Sqrt(2), 0f)
         };

        

        public WindowTK(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings):
            base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();


            GL.ClearColor(0f, 0f, 0f, 1.0f);

            GL.Enable(EnableCap.DepthTest);

            _shader = new Shader("../../../Shader/shader.vert", "../../../Shader/shader.frag");

            
            _shader.SetInt("material.diffuse", 0);
            _shader.SetInt("material.specular", 1);
            _shader.SetFloat("material.shininess", 4f);

            _shader.SetVector3("light.direction", new Vector3(50,50,100));
            _shader.SetVector3("light.ambient",  new Vector3(0.07f));
            _shader.SetVector3("light.diffuse",  new Vector3(0.7f));
            _shader.SetVector3("light.specular", new Vector3(1f, 1f, 1f));


            sphere = new Sphere(1f, 50, 50);

            sphere.VBOandVAO();

            sphere.ElemBuf();

            sphere.Pointers(_shader);

            sphere.texture("D:/Texture6.jpg", "D:/NormalMap3.png");

            circle1 = new Circle(4, 5, 100, 2);

            circle1.VBOandVAO();

            circle1.ElemBuf();

            circle1.Pointers(_shader);

            circle1.texture("D:/Texture6.jpg", "D:/NormalMap3.png");

            circle2 = new Circle(1, 2, 100, 2);

            circle2.VBOandVAO();

            circle2.ElemBuf();

            circle2.Pointers(_shader);

            circle2.texture("D:/Texture6.jpg", "D:/NormalMap3.png");

            
            _camera = new Camera(Vector3.UnitZ * 3, Size.X / (float)Size.Y);

            CursorGrabbed = true;
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            

            _shader.Use();

            sphere.Bind();



            var model = Matrix4.Identity;

            

            _shader.SetMatrix4("view", _camera.GetViewMatrix());
            _shader.SetMatrix4("projection", _camera.GetProjectionMatrix());

            
            _shader.SetVector3("viewPos", _camera.Position);

            for (int i = 0; i < 8; i++)
            {
                model = Matrix4.Identity * Matrix4.CreateRotationX(MathHelper.DegreesToRadians(90)) *
                  Matrix4.CreateRotationZ(MathHelper.DegreesToRadians((float)(-_grad3))) * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians((float)(_povorot))) * Matrix4.CreateTranslation(Positions[i]) *
                  Matrix4.CreateRotationZ((float)MathHelper.DegreesToRadians(_grad3));

                _shader.SetMatrix4("model", model);

                sphere.Draw();
            }

            

            circle1.Bind();

            _shader.SetMatrix4("model", Matrix4.Identity * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians((float)(_grad1))));

            circle1.Draw();

            circle2.Bind();

            _shader.SetMatrix4("model", Matrix4.Identity * Matrix4.CreateRotationZ(MathHelper.DegreesToRadians((float)(_grad2))));

            circle2.Draw();


           
            SwapBuffers();
        }
        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            if (!IsFocused) // Check to see if the window is focused
            {
                return;
            }

            var input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }


            const float cameraSpeed = 2f;
            const float sensitivity = 0.2f;

            double grad = 100 * args.Time;

            if (input.IsKeyDown(Keys.W))
            {
                _camera.Position += _camera.Front * cameraSpeed * (float)args.Time; // Forward
            }

            if (input.IsKeyDown(Keys.S))
            {
                _camera.Position -= _camera.Front * cameraSpeed * (float)args.Time; // Backwards
            }
            if (input.IsKeyDown(Keys.A))
            {
                _camera.Position -= _camera.Right * cameraSpeed * (float)args.Time; // Left
            }
            if (input.IsKeyDown(Keys.D))
            {
                _camera.Position += _camera.Right * cameraSpeed * (float)args.Time; // Right
            }
            if (input.IsKeyDown(Keys.Space))
            {
                _camera.Position += _camera.Up * cameraSpeed * (float)args.Time; // Up
            }
            if (input.IsKeyDown(Keys.LeftShift))
            {
                _camera.Position -= _camera.Up * cameraSpeed * (float)args.Time; // Down
            }
            if (input.IsKeyDown(Keys.Left)){
                
                _grad1 += grad  ;
                _grad3 += ((grad / 360) * 2 * 4) / (2 * 2) * 360;
                _povorot +=  ((grad / 360) * 2 * 4) / (2 * 1) * 360;
            }
            if (input.IsKeyDown(Keys.Right))
            {
                
                _grad1 -= grad;
                _grad3 -= ((grad / 360) * 2 * 4) / (2 * 2) * 360;
                _povorot -= ((grad / 360) * 2 * 4) / (2 * 1) * 360;
            }
            if (input.IsKeyDown(Keys.Up))
            {
                
                _grad2 += grad;
                _grad3 += ((grad / 360) * 2 * 2) / (2 * 4) * 360;
                _povorot -= ((grad / 360) * 2 * 2) / (2 * 1) * 360;
            }
            if (input.IsKeyDown(Keys.Down))
            {
                
                _grad2 -= grad;
                _grad3 -= ((grad / 360) * 2 * 2) / (2 * 4) * 360;
                _povorot += ((grad / 360) * 2 * 2) / (2 * 1) * 360;
            }

            // Get the mouse state
            var mouse = MouseState;

            if (_firstMove) // This bool variable is initially set to true.
            {
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _firstMove = false;
            }
            else
            {
                // Calculate the offset of the mouse position
                var deltaX = mouse.X - _lastPos.X;
                var deltaY = mouse.Y - _lastPos.Y;
                _lastPos = new Vector2(mouse.X, mouse.Y);

                // Apply the camera pitch and yaw (we clamp the pitch in the camera class)
                _camera.Yaw += deltaX * sensitivity;
                _camera.Pitch -= deltaY * sensitivity; // Reversed since y-coordinates range from bottom to top
            }
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, Size.X, Size.Y);
        }

    }
}

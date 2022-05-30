using System;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Mathematics;

namespace BearingTK
{
    class Program
    {
        static void Main()
        {
            NativeWindowSettings nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(1024,1024),
                Title = "Bearing",
                Flags = ContextFlags.Default,
            };

            using (var window = new WindowTK(GameWindowSettings.Default, nativeWindowSettings))
            {
                window.Run();
            } 


        }
    }
}

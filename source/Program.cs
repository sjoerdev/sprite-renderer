using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;

namespace Project;

class Program
{
    static void Main()
    {
        Window window = new Window();
        window.Run();
    }
}

class Window : GameWindow
{
    Shader shader;

    static NativeWindowSettings windowSettings = new NativeWindowSettings()
    {
        Title = "opengl sprite renderer",
        APIVersion = new Version(3, 3),
        ClientSize = new Vector2i(1280, 720)
    };

    public Window() : base(GameWindowSettings.Default, windowSettings) { }

    protected override void OnLoad()
    {
        base.OnLoad();
        shader = new Shader("shaders/vert.glsl", "shaders/frag.glsl");
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);

        shader.RenderSprite("sprites/idle.png", 8, 8);

        Context.SwapBuffers();
    }
}
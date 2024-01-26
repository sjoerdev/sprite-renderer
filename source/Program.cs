using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.Common;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.GraphicsLibraryFramework;

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
    float timer = 0;
    Vector2 position;

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

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);
        VSync = VSyncMode.On;

        // start input
        var mouse = MouseState;
        var input = KeyboardState;
        if (!IsFocused) return;

        float speed = 2;
        if (input.IsKeyDown(Keys.W)) position.Y += speed * (float)args.Time;
        if (input.IsKeyDown(Keys.S)) position.Y -= speed * (float)args.Time;
        if (input.IsKeyDown(Keys.D)) position.X += speed * (float)args.Time;
        if (input.IsKeyDown(Keys.A)) position.X -= speed * (float)args.Time;
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);
        timer += (float)args.Time * 4;

        if (MathF.Floor(timer) % 2 != 0) shader.RenderSprite("sprites/idle.png", Size, position);
        else shader.RenderSprite("sprites/walk.png", Size, position);

        Context.SwapBuffers();
    }
}
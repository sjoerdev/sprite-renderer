using OpenTK.Graphics.OpenGL;

namespace Project;

public class Shader
{
    public int pgrm;
    public int vbo;
    public int vao;

    public Shader(string vertPath, string fragPath)
    {
        // read shaders
        string vertCode = File.ReadAllText(vertPath);
        string fragCode = File.ReadAllText(fragPath);

        // compile vert
        int vert = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vert, vertCode);
        GL.CompileShader(vert);
        GL.GetShader(vert, ShaderParameter.CompileStatus, out int vStatus);
        if (vStatus != 1) throw new Exception("Vertex shader failed to compile: " + GL.GetShaderInfoLog(vert));

        // compile frag
        int frag = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(frag, fragCode);
        GL.CompileShader(frag);
        GL.GetShader(frag, ShaderParameter.CompileStatus, out int fStatus);
        if (fStatus != 1) throw new Exception("Fragment shader failed to compile: " + GL.GetShaderInfoLog(frag));

        // create main shader program
        pgrm = GL.CreateProgram();
        GL.AttachShader(pgrm, vert);
        GL.AttachShader(pgrm, frag);
        GL.LinkProgram(pgrm);
        GL.DetachShader(pgrm, vert);
        GL.DetachShader(pgrm, frag);

        // delete shaders
        GL.DeleteShader(vert);
        GL.DeleteShader(frag);

        // define vertices
        float[] vertices = new float[]
        {
            -1f, 1f, 0f,
            1f, 1f, 0f,
            -1f, -1f, 0f,
            1f, 1f, 0f,
            1f, -1f, 0f,
            -1f, -1f, 0f,
        };

        // create vbo
        vbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

        // create vao
        vao = GL.GenVertexArray();
        GL.BindVertexArray(vao);
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);
        GL.BindVertexArray(0);
    }

    public void Render(int width, int height)
    {
        GL.Viewport(0, 0, width, height);
        GL.UseProgram(pgrm);
        GL.Clear(ClearBufferMask.ColorBufferBit);
        GL.BindVertexArray(vao);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
    }
}
using OpenTK.Graphics.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;
using System.Runtime.InteropServices;
using OpenTK.Mathematics;

namespace Project;

public class Shader
{
    public int program;
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
        program = GL.CreateProgram();
        GL.AttachShader(program, vert);
        GL.AttachShader(program, frag);
        GL.LinkProgram(program);
        GL.DetachShader(program, vert);
        GL.DetachShader(program, frag);

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

        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
    }

    public void RenderSprite(string path, Vector2 resolution)
    {
        GL.UseProgram(program);
        GL.Clear(ClearBufferMask.ColorBufferBit);

        int texture = LoadTexture(path);

        SetVector2("resolution", resolution);

        // Bind texture before setting uniforms
        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2D, texture);
        GL.Uniform1(GL.GetUniformLocation(program, "tex"), 0);

        GL.BindVertexArray(vao);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
    }

    public int LoadTexture(string path)
    {
        int textureId;

        Image<Rgba32> image = Image.Load<Rgba32>(path);
        var pixelMemoryGroup = image.GetPixelMemoryGroup();

        GL.GenTextures(1, out textureId);
        GL.BindTexture(TextureTarget.Texture2D, textureId);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToBorder);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToBorder);

        /// Load pixel data into the texture
        IntPtr pixelData = Marshal.UnsafeAddrOfPinnedArrayElement(pixelMemoryGroup[0].ToArray(), 0);
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, pixelData);

        // Unbind the texture
        GL.BindTexture(TextureTarget.Texture2D, 0);

        return textureId;
    }

    public void SetFloat(string name, float value)
    {
        GL.Uniform1(GL.GetUniformLocation(program, name), value);
    }

    public void SetInt(string name, int value)
    {
        GL.Uniform1(GL.GetUniformLocation(program, name), value);
    }

    public void SetBool(string name, bool value)
    {
        GL.Uniform1(GL.GetUniformLocation(program, name), value ? 1 : 0);
    }

    public void SetVector2(string name, Vector2 value)
    {
        GL.Uniform2(GL.GetUniformLocation(program, name), value.X, value.Y);
    }

    public void SetVector3(string name, Vector3 value)
    {
        GL.Uniform3(GL.GetUniformLocation(program, name), value.X, value.Y, value.Z);
    }
}
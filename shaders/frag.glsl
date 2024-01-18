#version 330 core

in vec2 ndc;
out vec4 fragColor;

uniform sampler2D tex;

void main()
{
    fragColor = texture(tex, -ndc * 0.5 + 0.5);
}
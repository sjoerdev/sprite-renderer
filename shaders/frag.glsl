#version 330 core

in vec2 ndc;
out vec4 fragColor;

uniform sampler2D tex;
uniform vec2 resolution;

float scale = 0.5; // scale as a factor of vertical pixels

void main()
{
    float aspectX = resolution.x / resolution.y;
    float aspectY = resolution.y / resolution.x;
    vec2 uv = (-vec2(ndc.x * aspectX, ndc.y * aspectY) / scale) + 0.5;

    fragColor = texture(tex, uv);
}
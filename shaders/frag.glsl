#version 330 core

in vec2 ndc;
out vec4 fragColor;

uniform sampler2D tex;
uniform vec2 resolution;
uniform vec2 size;
uniform vec2 position;

int pixelScale = 16;

void main()
{
    float uvScale = resolution.y / size.y;

    float spriteAspect = size.y / size.x;
    float resAspect = resolution.x / resolution.y;
    
    vec2 uv = (-vec2(ndc.x * resAspect * spriteAspect, ndc.y) * uvScale / pixelScale) + 0.5;

    fragColor = texture(tex, uv + position);
}
#version 330

in vec2 fragTexCoord;
in vec4 fragColor;
out vec4 finalColor;

uniform sampler2D texture0;
uniform vec3 palette[37];

const float MaxIndex = 36.0;

void main()
{
    vec2 size = vec2(textureSize(texture0, 0));
    vec2 uv = clamp(fragTexCoord, vec2(0.0), vec2(1.0));
    uv = (floor(uv * size) + 0.5) / size;

    int index = int(round(texture(texture0, uv).r * 255.0));
    index = clamp(index, 0, int(MaxIndex));
    finalColor = vec4(palette[index], 1.0);
}

#version 330

in vec2 fragTexCoord;
in vec4 fragColor;
out vec4 finalColor;

uniform sampler2D texture0;
uniform vec3 palette[37];
uniform float softened;

const float MaxIndex = 36.0;

vec3 paletteColor(vec2 uv)
{
    vec2 size = vec2(textureSize(texture0, 0));
    vec2 snapped = (floor(clamp(uv, vec2(0.0), vec2(1.0)) * size) + 0.5) / size;
    int index = int(round(texture(texture0, snapped).r * 255.0));
    index = clamp(index, 0, int(MaxIndex));
    return palette[index];
}

void main()
{
    vec2 size = vec2(textureSize(texture0, 0));
    vec2 uv = clamp(fragTexCoord, vec2(0.0), vec2(1.0));
    vec2 tex = uv * size - 0.5;
    vec2 f = fract(tex);
    vec2 texel = 1.0 / size;
    vec2 origin = (floor(tex) + 0.5) / size;

    if (softened < 0.5)
    {
        finalColor = vec4(paletteColor(uv), 1.0);
        return;
    }

    vec3 c00 = paletteColor(origin);
    vec3 c10 = paletteColor(origin + vec2(texel.x, 0.0));
    vec3 c01 = paletteColor(origin + vec2(0.0, texel.y));
    vec3 c11 = paletteColor(origin + texel);
    // Blend palette colors across the cell so the upscale is soft, while the
    // heat values underneath stay the discrete scatter.
    finalColor = vec4(mix(mix(c00, c10, f.x), mix(c01, c11, f.x), f.y), 1.0);
}

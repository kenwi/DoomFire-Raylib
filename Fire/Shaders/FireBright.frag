#version 330

in vec2 fragTexCoord;
in vec4 fragColor;
out vec4 finalColor;

uniform sampler2D texture0;

void main()
{
    vec3 color = texture(texture0, fragTexCoord).rgb;
    // Green tracks heat: the yellow core blooms, and orange leaves a softer halo.
    float hot = smoothstep(0.18, 0.75, color.g);
    finalColor = vec4(color * hot, 1.0);
}

#version 330 core
out vec4 FragColor;
in vec2 vTexCoord;
uniform sampler2D uTexture0;
void main()
{
    FragColor = texture(uTexture0, vTexCoord);
}
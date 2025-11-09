#version 330 core

// Color de salida final
out vec4 FragColor;

// Coordenada de textura de entrada (desde el vertex shader)
in vec2 vTexCoord;

// La textura (sampler) que le pasamos desde C#
uniform sampler2D uTexture0;

void main()
{
    // Busca el color en la textura usando la coordenada
    // Exactamente igual que en el proyecto 2D
    FragColor = texture(uTexture0, vTexCoord);
}
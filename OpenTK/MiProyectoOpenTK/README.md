# Guía de Texturizado en OpenTK

Esta guía explica el proceso detallado para aplicar texturas a objetos 3D usando OpenTK y OpenGL.

## Índice

- [Conceptos Básicos](#conceptos-básicos)
- [Proceso de Texturizado](#proceso-de-texturizado)
  - [1. Configuración de Vértices](#1-configuración-de-vértices)
  - [2. Implementación de Shaders](#2-implementación-de-shaders)
  - [3. Carga de Texturas](#3-carga-de-texturas)
  - [4. Renderizado](#4-renderizado)
  - [5. Configuración del Proyecto](#5-configuración-del-proyecto)

## Conceptos Básicos

El texturizado en OpenGL requiere la coordinación de 5 componentes principales:

1. **Vértices**: Definen la geometría y las coordenadas UV
2. **Shaders**: Procesan los vértices y aplican la textura
3. **Textura**: La imagen que se aplicará al objeto
4. **Renderizado**: El proceso de dibujo
5. **Configuración**: Ajustes del proyecto .NET

## Proceso de Texturizado

### 1. Configuración de Vértices

Los vértices deben incluir tanto la posición (X,Y,Z) como las coordenadas de textura (U,V).

```csharp
private readonly float[] _vertices =
{
    // Posición (X,Y,Z)      // Textura (U,V)
     0.5f,  0.5f, 0.0f,     1.0f, 1.0f,  // Superior derecha
     0.5f, -0.5f, 0.0f,     1.0f, 0.0f,  // Inferior derecha
    -0.5f, -0.5f, 0.0f,     0.0f, 0.0f,  // Inferior izquierda
    -0.5f,  0.5f, 0.0f,     0.0f, 1.0f   // Superior izquierda
};
```

Configuración de los atributos de vértice:

```csharp
int stride = 5 * sizeof(float);

// Posición (Atributo 0)
GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, stride, 0);
GL.EnableVertexAttribArray(0);

// Coordenadas UV (Atributo 1)
GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, stride, 3 * sizeof(float));
GL.EnableVertexAttribArray(1);
```

### 2. Implementación de Shaders

#### Vertex Shader (shader.vert)

```glsl
#version 330 core

layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec2 aTexCoord;

out vec2 vTexCoord;

void main(void)
{
    vTexCoord = aTexCoord;
    gl_Position = vec4(aPosition, 1.0);
}
```

#### Fragment Shader (shader.frag)

```glsl
#version 330 core

out vec4 FragColor;
in vec2 vTexCoord;
uniform sampler2D uTexture0;

void main()
{
    FragColor = texture(uTexture0, vTexCoord);
}
```

### 3. Carga de Texturas

Puntos clave para la carga de texturas:

- Usar `ImageSharp` para cargar la imagen
- Voltear la imagen verticalmente
- Generar y configurar la textura en OpenGL

### 4. Renderizado

Secuencia de renderizado en `OnRenderFrame`:

```csharp
protected override void OnRenderFrame(FrameEventArgs e)
{
    base.OnRenderFrame(e);
    GL.Clear(ClearBufferMask.ColorBufferBit);

    _shader.Use();                           // Activar shader
    _texture.Use(TextureUnit.Texture0);      // Activar textura
    _shader.SetInt("uTexture0", 0);          // Configurar sampler

    GL.BindVertexArray(_vertexArrayObject);  // Enlazar VAO
    GL.DrawElements(PrimitiveType.Triangles, _indices.Length,
                   DrawElementsType.UnsignedInt, 0);

    SwapBuffers();
}
```

### 5. Configuración del Proyecto

Añade esto a tu archivo `.csproj`:

```xml
<ItemGroup>
    <None Update="textura.jpg">
        <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
    <None Update="shader.vert">
        <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
    <None Update="shader.frag">
        <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
</ItemGroup>
```

## Solución de Problemas Comunes

- **Objeto Negro**: Verifica que la textura se cargó correctamente
- **Textura No Visible**: Confirma que las coordenadas UV están correctas
- **FileNotFoundException**: Revisa la configuración del `.csproj`
- **Imagen Invertida**: Asegúrate de voltear la imagen verticalmente

## Referencias Útiles

- [Documentación de OpenTK](https://opentk.net/learn/index.html)
- [LearnOpenGL - Texturas](https://learnopengl.com/Getting-started/Textures)
- [OpenGL Wiki - Texture](https://www.khronos.org/opengl/wiki/Texture)

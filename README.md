# Tareas - Videojuegos CUCosta (Rommel-DA)

[![.NET Build](https://github.com/HALOSEL117/Tareas-Videojuegos-CUCosta.Rommel-DA/actions/workflows/dotnet.yml/badge.svg)](https://github.com/HALOSEL117/Tareas-Videojuegos-CUCosta.Rommel-DA/actions/workflows/dotnet.yml)

Repositorio con ejercicios y proyectos de programación (C# / .NET) para la materia.

## Contenido

- Varias carpetas con proyectos de consola y formularios (por ejemplo `Bancojido`, `Condicionalpmayor`, `ContactosEmpresax`, etc.).
- Proyectos de gráficos 3D con OpenTK:
  - **ProyectoCuadrado2D**: Implementación de un cuadrado 2D con texturas usando shaders.
  - **ProyectoCubo3D**: Implementación de un cubo 3D con texturas y rotación.

## Requisitos

- .NET SDK 9.0 (o compatible) instalado: https://dotnet.microsoft.com/
- OpenTK (se instalará automáticamente al restaurar las dependencias del proyecto)
- Tarjeta gráfica compatible con OpenGL 4.0 o superior

## Cómo compilar y ejecutar

Desde la raíz del repositorio puedes compilar todos los proyectos:

```powershell
dotnet build
```

Para ejecutar un proyecto concreto, usa `dotnet run` indicando la ruta del proyecto. Ejemplo:

```powershell
dotnet run --project Bancojido/Bancojido.csproj
```

## Proyectos OpenTK

### Ejecutar el Proyecto Cubo 3D

1. Navega al directorio del proyecto:

```powershell
cd OpenTK/MiProyectoOpenTK/ProyectoCubo3D
```

2. Restaura las dependencias:

```powershell
dotnet restore
```

3. Ejecuta el proyecto:

```powershell
dotnet run
```

### Controles del Cubo 3D

- Usa las flechas del teclado para rotar el cubo
- Presiona ESC para cerrar la ventana

### Características

- Renderizado 3D con OpenGL
- Implementación de shaders personalizados (vertex y fragment)
- Sistema de texturas
- Transformaciones 3D (rotación)
- Matrices de proyección y vista

## Notas

- Ya se agregó un archivo `.gitignore` para excluir `bin/` y `obj/` y evitar subir artefactos de compilación.
- Las texturas deben estar en el directorio de salida del proyecto para que se carguen correctamente.

## Licencia

Este repositorio no contiene una licencia explícita

# Comparador de Algoritmos de Ordenamiento (C# Multithreading)

Proyecto Final: Algoritmia y Base de Datos.

Este proyecto implementa y compara el rendimiento de tres algoritmos de ordenamiento clásicos (Bubble Sort, Merge Sort, Quick Sort) utilizando procesamiento paralelo (Tasks) en C#.

El programa incluye una métrica de tiempo real llamada "Ticks de Reloj" (Clock Ticks) y además reporta milisegundos (ms) para facilitar el análisis. Cuenta con dos modos de prueba diseñados para simular escenarios de "Mejor Caso" y "Peor Caso".

**Características**

- **Multihilo:** Utiliza `Task.Run` para ejecutar simultáneamente los 3 algoritmos y comparar sus tiempos en paralelo.
- **Generación de Datos:**
  - **Modo Aleatorio (Estándar):** Genera una lista de N números aleatorios dentro de un rango específico. Ideal para pruebas generales.
  - **Modo "Cruel" (Stress Test):** Genera una lista invertida (ordenada de mayor a menor). Este modo busca romper la eficiencia de algoritmos con pivotes fijos.
- **Interfaz de Consola:** Visualización limpia de los datos de entrada y reporte por algoritmo (ticks y ms).

## Estructura

```
Algoritmos/
  ├─ Algoritmos.csproj
  └─ Program.cs
```

## Instrucciones de Uso

El código viene configurado por defecto en **Modo Aleatorio**. Ahora `Program.cs` acepta argumentos opcionales para correr benchmarks desde línea de comandos:

Uso básico (PowerShell):

```
cd "c:\Users\HALOSEL117\Documents\Repositorios Github\Tareas-Videojuegos-CUCosta.Rommel-DA\Algoritmos"
dotnet build .\Algoritmos.csproj
dotnet run --project .\Algoritmos.csproj -- <cantidad> <modo> <maxValor>
```

Parámetros:

- `<cantidad>`: Número de elementos a generar (ej: `10000`).
- `<modo>`: `aleatorio` (por defecto) o `cruel` (lista invertida).
- `<maxValor>`: (opcional) máximo valor para números aleatorios.

Nuevas opciones útiles para automatización:

- `--headless`: Ejecuta sin interacción ni dibujo en consola (útil para scripts automatizados).
- `--run <N>`: Identificador de la ejecución (enteros). Útil cuando se repite la misma configuración varias veces.
- `--out <path>`: Archivo CSV donde el programa escribirá resultados estructurados (`Size,Mode,Run,Algorithm,Ticks,Ms`). Si el archivo no existe, el programa lo creará con encabezado UTF-8 BOM y añadirá filas.
- `--headless`: Ejecuta sin interacción ni dibujo en consola (útil para scripts automatizados).
- `--run <N>`: Identificador de la ejecución (enteros). Útil cuando se repite la misma configuración varias veces (externamente).
- `--repeat <N>`: Ejecuta internamente N repeticiones (el programa ejecuta las 3 rutinas N veces y produce filas `Run=1..N`).
- `--out <path>`: Archivo CSV donde el programa escribirá resultados estructurados (`Size,Mode,Run,Algorithm,Ticks,Ms`). Si el archivo no existe, el programa lo creará con encabezado UTF-8 BOM y añadirá filas.

Adicionalmente, a partir de esta versión el programa soporta opciones para controlar el modo de ejecución (paralelo o secuencial) y captura métricas de memoria y profundidad de recursión:

- `--sequential`: Ejecuta los algoritmos uno a uno (secuencial). Recomendado cuando deseas mediciones de memoria privada (`PrivateDelta`) no solapadas.
- `--parallel`: Ejecuta los algoritmos en paralelo (por defecto) usando `Task.Run` (mayor concurrencia, pero las medidas de memoria privada pueden solaparse).

Métricas CSV completas: El CSV generado ahora contiene las siguientes columnas:

Size,Mode,Run,Algorithm,Ticks,Ms,ManagedBefore,ManagedAfter,ManagedDelta,PrivateBefore,PrivateAfter,PrivateDelta,MaxRecursionDepth

Nota sobre memoria privada: `PrivateBefore/PrivateAfter` usan `Process.PrivateMemorySize64` del proceso actual. Si se ejecutan los algoritmos en paralelo estas lecturas pueden reflejar la memoria total del proceso y no la memoria exclusiva de cada algoritmo; por eso `--sequential` es la opción recomendada para análisis de memoria.

Ejemplo:

```
dotnet run --project .\Algoritmos.csproj -- 10000 cruel 99999

Ejemplo para salida estructurada (escribir en `benchmarks.csv`):

```

dotnet run --project .\Algoritmos.csproj -- 1000 aleatorio --run 1 --headless --out benchmarks.csv

```

```

### Modos: Normal vs Cruel

1. Modo Normal (Aleatorio)

- Configuración: `GenerarNumerosAleatorios` (por defecto).
- Resultado esperado: Quick Sort suele ser el más rápido; Merge sigue, y Bubble es muy lento en listas grandes.

2. Modo Cruel (Lista Invertida)

- Configuración: `cruel` como segundo argumento o pasar `cruel` en el script.
- Qué sucede: Quick Sort (con pivote fijo) puede degradar su rendimiento a O(n^2). Merge Sort se mantiene estable.

## Tabla de Complejidad (Big-O)

| Algoritmo       | Mejor Caso | Promedio   | Peor Caso  | Observación                                                   |
| :-------------- | :--------- | :--------- | :--------- | :------------------------------------------------------------ |
| **Bubble Sort** | O(n)       | O(n^2)     | O(n^2)     | Solo fines educativos.                                        |
| **Merge Sort**  | O(n log n) | O(n log n) | O(n log n) | Estable, usa memoria auxiliar.                                |
| **Quick Sort**  | O(n log n) | O(n log n) | O(n^2)     | Rápido en promedio; peor con pivote fijo en listas ordenadas. |

## Benchmarks automáticos

He añadido un script PowerShell `run_benchmarks.ps1` (en la carpeta `Algoritmos`) que ejecuta el programa para varias combinaciones de tamaños y modos, repite cada prueba varias veces y guarda los resultados en un archivo CSV (`benchmarks.csv`).

Uso del script (PowerShell):

```
cd .\Algoritmos
.\run_benchmarks.ps1 -Sizes 1000 5000 10000 -Modes aleatorio cruel -Repeats 3 -OutFile benchmarks.csv

Si quieres que cada ejecución del script use modo secuencial (no paralelo), llama al programa directamente con `--sequential` o modifica `run_benchmarks.ps1` para pasar `--sequential` a `dotnet run`.

Ejemplo (ejecución secuencial manual):

dotnet run --project .\Algoritmos.csproj -- 1000 aleatorio --repeat 3 --headless --out '.\benchmarks\benchmarks.csv' --sequential
```

El script compila el proyecto (si es necesario), ejecuta las pruebas y genera un CSV con columnas: `Size,Mode,Run,Algorithm,Ticks,Ms`.

## Archivar resultados y `.gitignore`

Los archivos CSV de resultados son artefactos generados (no forman parte del código fuente). Este repositorio ya incluye una entrada en `.gitignore` para evitar añadirlos al control de versiones: `Algoritmos/benchmarks*.csv` y la carpeta `Algoritmos/benchmarks/` están ignoradas.

Recomendaciones para conservación y trazabilidad:

- Generar archivos con timestamp para no sobrescribir ejecuciones previas. Ejemplo (PowerShell):

```
$ts = Get-Date -Format "yyyyMMdd_HHmmss"
.\run_benchmarks.ps1 -Sizes 1000 5000 10000 -Modes aleatorio cruel -Repeats 3 -OutFile "benchmarks_$ts.csv"
```

- Opcional: mover automáticamente a un subdirectorio `benchmarks/archive` para centralizar históricos:

```
New-Item -ItemType Directory -Path .\benchmarks\archive -Force
Move-Item .\benchmarks*.csv .\benchmarks\archive\
```

- Si deseas mantener sólo el último resultado, borra los CSV antiguos antes de ejecutar:

```
Remove-Item .\benchmarks*.csv -Force
.\run_benchmarks.ps1 -Sizes 1000 5000 10000 -Modes aleatorio cruel -Repeats 3 -OutFile benchmarks.csv

También hay un script auxiliar para limpiar la carpeta de archivos archivados si quieres vaciar `benchmarks/archive`:

PowerShell:
```

powershell -NoProfile -ExecutionPolicy Bypass -File .\scripts\clean_archive.ps1

```

```

Nota: `run_benchmarks.ps1` puede recibir el parámetro `-OutFile` para indicar el archivo de salida; si el archivo existe se añadirán filas. Para evitar problemas de codificación con herramientas Windows, el script crea el CSV con encabezado en UTF‑8 + BOM.

## Conclusiones

Tras experimentar con estos métodos de ordenamiento, los resultados demuestran que ningún algoritmo es perfecto para todas las situaciones:

1. **Quick Sort:** Es el ganador indiscutible en velocidad con grandes volúmenes de datos aleatorios.
2. **Bubble Sort:** Ineficiente para grandes datos, pero en listas muy pequeñas puede ser competitivo debido a su baja sobrecarga.
3. **Merge Sort:** Muy confiable y estable en tiempo; su desventaja es el mayor uso de memoria.

## Contribuciones

- Si quieres mejorar el README, añadir más algoritmos (HeapSort, TimSort) o variantes paralelas, abre un pull request.

## Licencia

- Código de ejemplo sin licencia explícita. Añade la licencia que prefieras si planeas compartir o publicar.

---

Desarrollado en C# .NET

Si quieres, puedo también:

- traducir el README al inglés,
- ampliar el script para producir gráficos (CSV -> PNG),
- o añadir más algoritmos y métricas (uso de memoria, recursión máxima).

---

**Registro de la versión / commit**

- Última actualización: `2025-11-24 16:41:00` (commit `07baac0`)
- Tag: `v1.0-20251124_1641`

_Nota:_ Este tag corresponde a la versión que incluye:

- Refactorización completa a clases (`ISorter`, `SorterBase`, `ResultMetrics`, `CsvWriter`, `BenchmarkRunner`).
- Nuevas métricas: memoria administrada (antes/después/delta), memoria privada (antes/después/delta), profundidad máxima de recursión.
- Soporte para ejecución secuencial (`--sequential`) y paralela (`--parallel`).
- Escritura directa de resultados en CSV con encabezado UTF-8 BOM y esquema completo.
- Scripts automatizados: `run_benchmarks.ps1`, `summarize_benchmarks.ps1`, `scripts/clean_archive.ps1`.
- Documentación ampliada y ejemplos de uso.

_Revisa el historial de commits y tags para trazabilidad completa._

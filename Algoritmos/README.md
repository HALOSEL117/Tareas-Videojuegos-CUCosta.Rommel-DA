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
├─ Program.cs
├─ README.md
├─ run_benchmarks.ps1
├─ summarize_benchmarks.ps1
├─ benchmarks/
│   ├─ bench_test_par.csv
│   ├─ bench_test_seq.csv
│   └─ archive/
├─ Classes/
│   ├─ IO/
│   │   └─ CsvWriter.cs
│   ├─ Models/
│   │   └─ ResultMetrics.cs
│   ├─ Runner/
│   │   └─ BenchmarkRunner.cs
│   └─ Sorting/
│       ├─ BubbleSorter.cs
│       ├─ ISorter.cs
│       ├─ MergeSorter.cs
│       ├─ QuickSorter.cs
│       └─ SorterBase.cs
├─ scripts/
│   └─ clean_archive.ps1
```

## Instrucciones de Uso

El código está listo para ejecutarse en modo interactivo o automatizado. Puedes usarlo desde la consola, scripts PowerShell, o integrarlo en pipelines de pruebas.

### Ejecución manual (PowerShell)

```powershell
cd "c:\Users\HALOSEL117\Documents\Repositorios Github\Tareas-Videojuegos-CUCosta.Rommel-DA\Algoritmos"
dotnet build .\Algoritmos.csproj
dotnet run --project .\Algoritmos.csproj -- <cantidad> <modo> <maxValor> [opciones]
```

**Parámetros principales:**

- `<cantidad>`: Número de elementos a generar (ej: `10000`).
- `<modo>`: `aleatorio` (por defecto) o `cruel` (lista invertida).
- `<maxValor>`: (opcional) máximo valor para números aleatorios.

**Opciones avanzadas:**

- `--headless`: Ejecuta sin interacción ni dibujo en consola (ideal para scripts y benchmarks).
- `--run <N>`: Identificador de la ejecución (útil para distinguir repeticiones externas).
- `--repeat <N>`: Ejecuta internamente N repeticiones (produce filas `Run=1..N`).
- `--out <path>`: Archivo CSV donde se guardan los resultados. Si no existe, se crea con encabezado UTF-8 BOM.
- `--sequential`: Fuerza ejecución secuencial (uno por uno, útil para medición precisa de memoria).
- `--parallel`: Ejecuta en paralelo (por defecto, más rápido pero las métricas de memoria pueden solaparse).

**Ejemplo básico:**

```powershell
dotnet run --project .\Algoritmos.csproj -- 10000 aleatorio --headless --out benchmarks.csv
```

**Ejemplo con repeticiones y modo cruel:**

```powershell
dotnet run --project .\Algoritmos.csproj -- 5000 cruel --repeat 5 --headless --out benchmarks.csv
```

**Ejemplo secuencial para análisis de memoria:**

```powershell
dotnet run --project .\Algoritmos.csproj -- 1000 aleatorio --repeat 3 --headless --out benchmarks.csv --sequential
```

### Ejecución automatizada con scripts

Para ejecutar múltiples combinaciones y archivar resultados automáticamente, usa los scripts incluidos:

- `run_benchmarks.ps1`: Ejecuta el programa en varios tamaños, modos y repeticiones, guarda resultados en CSV y archiva históricos.
- `summarize_benchmarks.ps1`: Resume los resultados y exporta un CSV con promedios y desviaciones estándar.
- `scripts/clean_archive.ps1`: Limpia la carpeta de archivos archivados.

**Ejemplo de uso del script principal:**

```powershell
cd .\Algoritmos
./run_benchmarks.ps1 -Sizes 1000 5000 10000 -Modes aleatorio cruel -Repeats 3 -OutFile benchmarks.csv
```

**Ejemplo para limpiar archivos archivados:**

```powershell
cd .\Algoritmos
./scripts/clean_archive.ps1
```

**Métricas CSV completas:**

El CSV generado contiene las siguientes columnas:

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

#### 1. Modo Normal (Aleatorio)

- **Configuración:** Usar `aleatorio` como argumento (o por defecto).
- **Comportamiento:** Genera una lista de números aleatorios. Simula el caso promedio para los algoritmos.
- **Resultados típicos:**
  - Quick Sort es el más rápido en la mayoría de los casos.
  - Merge Sort es consistente y cercano a Quick Sort.
  - Bubble Sort es muy lento para listas grandes, pero útil para comparar en listas pequeñas.

#### 2. Modo Cruel (Lista Invertida)

- **Configuración:** Usar `cruel` como argumento (ejemplo: `dotnet run -- 10000 cruel`).
- **Comportamiento:** Genera una lista ordenada de mayor a menor (peor caso para algunos algoritmos).
- **Resultados típicos:**
  - Quick Sort (con pivote fijo) puede degradar su rendimiento a O(n²), mostrando tiempos mucho mayores.
  - Merge Sort mantiene su rendimiento estable (O(n log n)), ideal para comparar robustez.
  - Bubble Sort sigue siendo el más lento, pero el impacto es menos notorio en listas pequeñas.

**Recomendación:** Usa ambos modos para comparar robustez y eficiencia. El modo "cruel" es útil para evidenciar debilidades en algoritmos con pivote fijo y para validar la estabilidad de Merge Sort.

## Tabla de Complejidad (Big-O)


| Algoritmo       | Mejor Caso | Promedio   | Peor Caso  | Memoria           | Recursión Máxima      | Observación                                                   |
| :-------------- | :--------- | :--------- | :--------- | :----------------- | :--------------------- | :------------------------------------------------------------ |
| **Bubble Sort** | O(n)       | O(n^2)     | O(n^2)     | O(1)               | 0 (iterativo)          | Solo fines educativos.                                        |
| **Merge Sort**  | O(n log n) | O(n log n) | O(n log n) | O(n)               | log₂(n)                | Estable, usa memoria auxiliar y recursión.                    |
| **Quick Sort**  | O(n log n) | O(n log n) | O(n^2)     | O(log n)           | n (peor caso)          | Rápido en promedio; peor con pivote fijo en listas ordenadas. |

**Cómo se mide la memoria y recursión máxima en este proyecto:**

- **Memoria administrada (Managed):** Se mide con `GC.GetTotalMemory(false)` antes y después de cada algoritmo. El delta (`ManagedDelta`) refleja el cambio en memoria .NET gestionada.
- **Memoria privada (Private):** Se mide con `Process.PrivateMemorySize64` antes y después de cada algoritmo. El delta (`PrivateDelta`) refleja el cambio en memoria total usada por el proceso (puede solaparse en modo paralelo).
- **Recursión máxima:** Se mide en los algoritmos recursivos (Merge y Quick) contando la profundidad máxima alcanzada durante la ejecución (`MaxRecursionDepth`). Bubble Sort es iterativo, por lo que su valor es 0.


---
**Ejemplo cotidiano para entender la medición de memoria:**

Imagina que tienes una caja donde guardas manzanas (memoria administrada) y otra caja donde guardas todo lo que usas en la cocina (memoria privada). Antes de ordenar las manzanas, cuentas cuántas hay en la caja. Después de terminar, vuelves a contar. La diferencia te dice cuántas manzanas usaste o moviste durante el proceso.

En el caso de la memoria privada, es como pesar toda la cocina antes y después de cocinar: si usaste más ollas, cuchillos o ingredientes, el peso total cambia. Si cocinas varias recetas al mismo tiempo (modo paralelo), el peso puede mezclarse y no saber exactamente cuánto usó cada receta.

La recursión máxima sería como saber cuántas veces tuviste que abrir cajas dentro de cajas para encontrar la manzana más pequeña. Si solo usas una caja (iterativo), el número es cero. Si tienes que abrir muchas cajas dentro de otras (recursivo), ese número crece.

Así, el programa mide la memoria y la recursión como si estuviera contando y pesando objetos cotidianos antes y después de ordenar, para saber cuánto realmente se usó en cada algoritmo.

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

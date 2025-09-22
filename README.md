# PGE-T6

Aplicación WPF (.NET 8) con una interfaz basada en pestañas (TabControl) para 11 ejercicios. Ejercicios implementados: Conversor de Temperatura (E1), Dibujo Libre (E2), Registro de Gastos (E3), Visualización de Datos (E4), Editor de Imágenes (E5), Conversor de Divisas (E6). Todos con code-behind y validación regional para Argentina (es-AR).

## Estructura

```
PGE-T6/
├── App.xaml
├── MainWindow.xaml
├── Views/
│   ├── Ej01View.xaml
│   ├── Ej01View.xaml.cs
│   ├── Ej02View.xaml
│   ├── Ej02View.xaml.cs
│   ├── Ej03View.xaml
│   ├── Ej03View.xaml.cs
│   ├── Ej04View.xaml
│   ├── Ej04View.xaml.cs
│   ├── Ej05View.xaml
│   ├── Ej05View.xaml.cs
│   ├── Ej06View.xaml
│   ├── Ej06View.xaml.cs
│   ├── Ej07View.xaml (placeholder)
│   ├── Ej08View.xaml (placeholder)
│   ├── Ej09View.xaml (placeholder)
│   ├── Ej10View.xaml (placeholder)
│   └── Ej11View.xaml (placeholder)
├── Themes/
│   └── Styles.xaml
└── Assets/
```

## Ejercicio 1: Conversor de Temperatura

- Entrada: Celsius (acepta coma decimal siguiendo cultura es-AR)
- Conversión: F = (C × 9/5) + 32
- Validaciones:
  - Campo vacío
  - Texto no numérico
  - Menor al cero absoluto (−273,15 °C)
- Resultado formateado con 2 decimales: "77,00 °F" (según cultura)

## Ejecutar

```
dotnet build
dotnet run
```

## Casos de prueba (E1)

| Entrada | Resultado Esperado | Descripción |
|---|---|---|
| 25 | 77,00 °F | Temperatura ambiente |
| 0 | 32,00 °F | Punto de congelación |
| -10 | 14,00 °F | Temperatura negativa |
| 36,5 | 97,70 °F | Temperatura corporal |
| "" | Error | Campo vacío |
| "abc" | Error | Texto no numérico |
| -300 | Error | Menor al cero absoluto |

## Ejercicio 2: Dibujo Libre en Canvas

- Superficie de dibujo: `Canvas` con eventos `MouseDown`, `MouseMove`, `MouseUp`.
- Trazo: segmentos `Line` conectando posiciones sucesivas del ratón.
- Propiedades del trazo:
  - Color seleccionable (`ComboBox`): Negro, Rojo, Verde, Azul, Naranja, Violeta.
  - Grosor ajustable (`Slider`) de 1 a 12 px.
- Botón "Limpiar" para borrar el lienzo.

## Ejercicio 3: Registro de Gastos con Conversión

- Entrada de gasto: descripción y valor en ARS (cultura es-AR).
- Listado: DataGrid con columnas Descripción, Valor (ARS) y Fecha.
- Conversión de moneda:
  - Selección de moneda objetivo: USD, EUR, BRL, CLP.
  - Tipo de cambio manual (ARS por 1 unidad de la moneda objetivo).
  - Cálculo: totalARS / tipoCambio → total en moneda objetivo.
- Validaciones: campos vacíos, valor/tipo de cambio no numérico o ≤ 0.
- Mensajes de error y total convertido mostrado en la UI.

## Ejercicio 4: Visualización de Datos (Barras)

- Canvas para graficar barras de ventas mensuales con ejes X/Y.
- Ticks y líneas de cuadrícula horizontales con escala "bonita" automática.
- Selector de dataset (Ventas 2023, 2024, 2025) y botón Actualizar.
- Etiquetas de mes en el eje X e indicadores de valor sobre cada barra.
- Texto inferior con métricas: máximo y total del dataset.

## Ejercicio 5: Editor de Imágenes

- Carga de imágenes: `OpenFileDialog` para seleccionar archivos (JPG, PNG, BMP, GIF).
- Visualización: `Image` control con `ScrollViewer` para imágenes grandes.
- Edición de tamaño: `Slider` de 10% a 200% con redimensionado proporcional.
- Ajuste de brillo: `Slider` de -100 a +100 con manipulación de píxeles RGBA.
- Funciones: Aplicar cambios, guardar imagen (`SaveFileDialog`) y restaurar original.
- Formatos de guardado: PNG, JPEG, BMP según extensión seleccionada.

## Ejercicio 6: Conversor de Divisas

- Entrada: valor numérico con validación (cultura es-AR) y selector de moneda base.
- Monedas soportadas: ARS, USD, EUR, BRL, CLP con tipos de cambio predefinidos.
- Conversión automática: al cambiar valor o moneda base, se actualizan todas las conversiones.
- Visualización: tarjetas con scroll mostrando símbolo, nombre, código y valor convertido.
- Interfaz: botón de conversión manual y actualización en tiempo real.


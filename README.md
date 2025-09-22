# PGE-T6

Aplicación WPF (.NET 8) con una interfaz basada en pestañas (TabControl) para 11 ejercicios. El Ejercicio 1 (Conversor de Temperatura) está implementado con code-behind y validación regional para Argentina (es-AR). El Ejercicio 2 (Dibujo Libre) permite trazar líneas sobre un Canvas con selección de color y grosor.

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
│   ├── Ej05View.xaml (placeholder)
│   ├── Ej06View.xaml (placeholder)
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


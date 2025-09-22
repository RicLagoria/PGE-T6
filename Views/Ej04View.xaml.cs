using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PGE_T6.Views
{
    public partial class Ej04View : UserControl
    {
        private readonly CultureInfo _cultureArgentina = new CultureInfo("es-AR");
        private readonly Dictionary<string, double[]> _datosPorDataset = new Dictionary<string, double[]>
        {
            { "2024", new double[] { 120, 150, 90, 180, 220, 160, 130, 140, 200, 210, 170, 190 } },
            { "2025", new double[] { 100, 130, 110, 160, 190, 200, 220, 210, 240, 230, 250, 260 } }
        };

        private readonly string[] _meses = new[] { "E", "F", "M", "A", "M", "J", "J", "A", "S", "O", "N", "D" };

        public Ej04View()
        {
            InitializeComponent();
            Loaded += (_, __) => DibujarGrafico();
            SizeChanged += (_, __) => DibujarGrafico();
        }

        private void BtnActualizar_Click(object sender, RoutedEventArgs e)
        {
            DibujarGrafico();
        }

        private void CmbDataset_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DibujarGrafico();
        }

        private string DatasetSeleccionado()
        {
            if (cmbDataset.SelectedItem is ComboBoxItem item && item.Tag is string tag)
                return tag;
            return "2024";
        }

        private void DibujarGrafico()
        {
            cnvChart.Children.Clear();

            var datos = _datosPorDataset[DatasetSeleccionado()];
            if (datos == null || datos.Length == 0)
                return;

            double width = cnvChart.ActualWidth;
            double height = cnvChart.ActualHeight;

            if (width <= 0 || height <= 0)
            {
                // Estimar tamaño si aún no midió
                width = 600;
                height = 320;
            }

            // Márgenes
            const double marginLeft = 40;
            const double marginBottom = 30;
            const double marginTop = 10;
            const double marginRight = 10;

            double plotWidth = width - marginLeft - marginRight;
            double plotHeight = height - marginTop - marginBottom;

            // Ejes
            var ejeX = new Line { X1 = marginLeft, Y1 = marginTop + plotHeight, X2 = marginLeft + plotWidth, Y2 = marginTop + plotHeight, Stroke = Brushes.Gray };
            var ejeY = new Line { X1 = marginLeft, Y1 = marginTop, X2 = marginLeft, Y2 = marginTop + plotHeight, Stroke = Brushes.Gray };
            cnvChart.Children.Add(ejeX);
            cnvChart.Children.Add(ejeY);

            // Escala
            double maxValor = Math.Max(1, datos.Max());
            double barWidth = plotWidth / datos.Length * 0.6;
            double slotWidth = plotWidth / datos.Length;

            // Barras y etiquetas
            for (int i = 0; i < datos.Length; i++)
            {
                double valor = datos[i];
                double barHeight = (valor / maxValor) * plotHeight;
                double x = marginLeft + (i * slotWidth) + (slotWidth - barWidth) / 2.0;
                double y = marginTop + plotHeight - barHeight;

                var rect = new Rectangle
                {
                    Width = barWidth,
                    Height = barHeight,
                    Fill = new SolidColorBrush(Color.FromRgb(94, 129, 172))
                };
                Canvas.SetLeft(rect, x);
                Canvas.SetTop(rect, y);
                cnvChart.Children.Add(rect);

                var labelMes = new TextBlock { Text = _meses[i], Foreground = Brushes.Black };
                Canvas.SetLeft(labelMes, marginLeft + (i * slotWidth) + slotWidth / 2.0 - 6);
                Canvas.SetTop(labelMes, marginTop + plotHeight + 4);
                cnvChart.Children.Add(labelMes);

                var labelValor = new TextBlock { Text = valor.ToString("N0", _cultureArgentina), Foreground = Brushes.Black, FontSize = 10 };
                Canvas.SetLeft(labelValor, x + barWidth / 2.0 - 10);
                Canvas.SetTop(labelValor, y - 14);
                cnvChart.Children.Add(labelValor);
            }

            double total = datos.Sum();
            txtInfo.Text = $"Dataset: {DatasetSeleccionado()}  •  Máximo: {maxValor:N0}  •  Total: {total:N0}";
        }
    }
}

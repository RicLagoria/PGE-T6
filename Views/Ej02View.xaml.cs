using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PGE_T6.Views
{
    public partial class Ej02View : UserControl
    {
        private bool _estaDibujando;
        private Point _ultimoPunto;

        public Ej02View()
        {
            InitializeComponent();
            SizeChanged += AjustarTamañoCanvas;
        }

        private void AjustarTamañoCanvas(object sender, SizeChangedEventArgs e)
        {
            // Ajusta el Canvas al tamaño disponible del borde contenedor
            if (cnvDibujo.Parent is FrameworkElement contenedor)
            {
                cnvDibujo.Width = contenedor.ActualWidth;
                cnvDibujo.Height = contenedor.ActualHeight;
            }
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                _estaDibujando = true;
                _ultimoPunto = e.GetPosition(cnvDibujo);
                cnvDibujo.CaptureMouse();
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_estaDibujando) return;

            Point actual = e.GetPosition(cnvDibujo);
            var linea = new Line
            {
                X1 = _ultimoPunto.X,
                Y1 = _ultimoPunto.Y,
                X2 = actual.X,
                Y2 = actual.Y,
                Stroke = new SolidColorBrush(ObtenerColorSeleccionado()),
                StrokeThickness = sldGrosor.Value,
                StrokeStartLineCap = PenLineCap.Round,
                StrokeEndLineCap = PenLineCap.Round
            };

            cnvDibujo.Children.Add(linea);
            _ultimoPunto = actual;
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_estaDibujando)
            {
                _estaDibujando = false;
                cnvDibujo.ReleaseMouseCapture();
            }
        }

        private void BtnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            cnvDibujo.Children.Clear();
        }

        private Color ObtenerColorSeleccionado()
        {
            if (cmbColor.SelectedItem is ComboBoxItem item && item.Tag is string colorName)
            {
                try
                {
                    var color = (Color)ColorConverter.ConvertFromString(colorName);
                    return color;
                }
                catch
                {
                    return Colors.Black;
                }
            }

            return Colors.Black;
        }
    }
}



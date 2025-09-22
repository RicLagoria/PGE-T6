using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace PGE_T6.Views
{
    public partial class Ej05View : UserControl
    {
        private BitmapSource? _imagenOriginal;
        private BitmapSource? _imagenActual;
        private string? _rutaImagenOriginal;

        public Ej05View()
        {
            InitializeComponent();
            sldTamaño.ValueChanged += (s, e) => ActualizarInfoSlider();
            sldBrillo.ValueChanged += (s, e) => ActualizarInfoSlider();
        }

        private void ActualizarInfoSlider()
        {
            if (txtInfoImagen != null)
            {
                txtInfoImagen.Text = $"Tamaño: {sldTamaño.Value:F0}% | Brillo: {sldBrillo.Value:F0}";
            }
        }

        private void BtnCargar_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png;*.bmp;*.gif|Todos los archivos|*.*",
                Title = "Seleccionar imagen"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    _rutaImagenOriginal = openFileDialog.FileName;
                    _imagenOriginal = new BitmapImage(new Uri(_rutaImagenOriginal));
                    _imagenActual = _imagenOriginal;
                    
                    imgVisualizacion.Source = _imagenActual;
                    ActualizarInfoImagen();
                    LimpiarInfo();
                    
                    // Resetear sliders
                    sldTamaño.Value = 100;
                    sldBrillo.Value = 0;
                }
                catch (Exception ex)
                {
                    txtInfo.Text = $"Error al cargar la imagen: {ex.Message}";
                }
            }
        }

        private void BtnAplicar_Click(object sender, RoutedEventArgs e)
        {
            if (_imagenOriginal == null)
            {
                txtInfo.Text = "Primero debe cargar una imagen.";
                return;
            }

            try
            {
                var imagenModificada = AplicarModificaciones(_imagenOriginal);
                _imagenActual = imagenModificada;
                imgVisualizacion.Source = _imagenActual;
                ActualizarInfoImagen();
                LimpiarInfo();
            }
            catch (Exception ex)
            {
                txtInfo.Text = $"Error al aplicar modificaciones: {ex.Message}";
            }
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (_imagenActual == null)
            {
                txtInfo.Text = "No hay imagen para guardar.";
                return;
            }

            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Archivo PNG|*.png|Archivo JPEG|*.jpg|Archivo BMP|*.bmp|Todos los archivos|*.*",
                Title = "Guardar imagen",
                DefaultExt = "png"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    GuardarImagen(_imagenActual, saveFileDialog.FileName);
                    txtInfo.Text = "Imagen guardada exitosamente.";
                }
                catch (Exception ex)
                {
                    txtInfo.Text = $"Error al guardar: {ex.Message}";
                }
            }
        }

        private void BtnRestaurar_Click(object sender, RoutedEventArgs e)
        {
            if (_imagenOriginal == null)
            {
                txtInfo.Text = "No hay imagen original para restaurar.";
                return;
            }

            _imagenActual = _imagenOriginal;
            imgVisualizacion.Source = _imagenActual;
            sldTamaño.Value = 100;
            sldBrillo.Value = 0;
            ActualizarInfoImagen();
            LimpiarInfo();
        }

        private BitmapSource AplicarModificaciones(BitmapSource imagenOriginal)
        {
            var factorTamaño = sldTamaño.Value / 100.0;
            var factorBrillo = sldBrillo.Value / 100.0;

            // Redimensionar si es necesario
            BitmapSource imagenRedimensionada = imagenOriginal;
            if (Math.Abs(factorTamaño - 1.0) > 0.01)
            {
                var nuevoAncho = (int)(imagenOriginal.PixelWidth * factorTamaño);
                var nuevoAlto = (int)(imagenOriginal.PixelHeight * factorTamaño);
                
                var scaleTransform = new ScaleTransform(factorTamaño, factorTamaño);
                var transformedBitmap = new TransformedBitmap(imagenOriginal, scaleTransform);
                imagenRedimensionada = transformedBitmap;
            }

            // Aplicar brillo si es necesario
            if (Math.Abs(factorBrillo) > 0.01)
            {
                return AplicarBrillo(imagenRedimensionada, factorBrillo);
            }

            return imagenRedimensionada;
        }

        private BitmapSource AplicarBrillo(BitmapSource imagen, double factorBrillo)
        {
            var pixels = new byte[imagen.PixelHeight * imagen.PixelWidth * 4];
            imagen.CopyPixels(pixels, imagen.PixelWidth * 4, 0);

            for (int i = 0; i < pixels.Length; i += 4)
            {
                // BGRA format
                var blue = (byte)Math.Max(0, Math.Min(255, pixels[i] + (factorBrillo * 255)));
                var green = (byte)Math.Max(0, Math.Min(255, pixels[i + 1] + (factorBrillo * 255)));
                var red = (byte)Math.Max(0, Math.Min(255, pixels[i + 2] + (factorBrillo * 255)));
                
                pixels[i] = blue;
                pixels[i + 1] = green;
                pixels[i + 2] = red;
                // Alpha channel (pixels[i + 3]) remains unchanged
            }

            return BitmapSource.Create(
                imagen.PixelWidth, imagen.PixelHeight,
                imagen.DpiX, imagen.DpiY,
                PixelFormats.Bgra32, null, pixels, imagen.PixelWidth * 4);
        }

        private void GuardarImagen(BitmapSource imagen, string ruta)
        {
            using (var fileStream = new FileStream(ruta, FileMode.Create))
            {
                BitmapEncoder encoder = Path.GetExtension(ruta).ToLower() switch
                {
                    ".jpg" or ".jpeg" => new JpegBitmapEncoder(),
                    ".bmp" => new BmpBitmapEncoder(),
                    _ => new PngBitmapEncoder()
                };

                encoder.Frames.Add(BitmapFrame.Create(imagen));
                encoder.Save(fileStream);
            }
        }

        private void ActualizarInfoImagen()
        {
            if (_imagenActual != null && txtInfoImagen != null)
            {
                var tamaño = sldTamaño.Value;
                var brillo = sldBrillo.Value;
                txtInfoImagen.Text = $"Imagen: {_imagenActual.PixelWidth}x{_imagenActual.PixelHeight} | Tamaño: {tamaño:F0}% | Brillo: {brillo:F0}";
            }
        }

        private void LimpiarInfo()
        {
            if (txtInfo != null)
            {
                txtInfo.Text = string.Empty;
            }
        }
    }
}

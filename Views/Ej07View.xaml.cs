using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PGE_T6.Views
{
    public partial class Ej07View : UserControl
    {
        private readonly CultureInfo _cultureArgentina = new CultureInfo("es-AR");
        private readonly Random _random = new Random();
        private int _numeroSecreto;
        private int _intentos;
        private bool _juegoTerminado;

        public Ej07View()
        {
            InitializeComponent();
            IniciarNuevoJuego();
        }

        private void IniciarNuevoJuego()
        {
            _numeroSecreto = _random.Next(1, 101); // 1 a 100 inclusive
            _intentos = 0;
            _juegoTerminado = false;

            // Limpiar interfaz
            txtNumero.Text = string.Empty;
            txtMensaje.Text = string.Empty;
            txtError.Text = string.Empty;
            txtIntentos.Text = string.Empty;
            
            borderMensaje.Visibility = Visibility.Collapsed;
            btnNuevoJuego.Visibility = Visibility.Collapsed;
            
            // Habilitar controles
            txtNumero.IsEnabled = true;
            btnAdivinar.IsEnabled = true;
            txtNumero.Focus();
        }

        private void BtnAdivinar_Click(object sender, RoutedEventArgs e)
        {
            txtError.Text = string.Empty;
            borderMensaje.Visibility = Visibility.Collapsed;

            var texto = txtNumero.Text?.Trim();
            if (string.IsNullOrWhiteSpace(texto))
            {
                txtError.Text = "Ingresa un número.";
                return;
            }

            if (!int.TryParse(texto, NumberStyles.Integer, _cultureArgentina, out var numero))
            {
                txtError.Text = "Ingresa un número válido.";
                return;
            }

            if (numero < 1 || numero > 100)
            {
                txtError.Text = "El número debe estar entre 1 y 100.";
                return;
            }

            _intentos++;
            txtIntentos.Text = $"Intentos: {_intentos}";

            // Comparar con el número secreto
            if (numero == _numeroSecreto)
            {
                // ¡Adivinó!
                _juegoTerminado = true;
                txtMensaje.Text = $"¡Felicitaciones! 🎉\nAdivinaste el número {_numeroSecreto} en {_intentos} intento{(_intentos == 1 ? "" : "s")}.";
                
                // Cambiar color del mensaje a verde
                borderMensaje.Background = new SolidColorBrush(Color.FromRgb(240, 248, 255));
                txtMensaje.Foreground = new SolidColorBrush(Color.FromRgb(0, 128, 0));
                
                borderMensaje.Visibility = Visibility.Visible;
                btnNuevoJuego.Visibility = Visibility.Visible;
                
                // Deshabilitar controles
                txtNumero.IsEnabled = false;
                btnAdivinar.IsEnabled = false;
            }
            else if (numero < _numeroSecreto)
            {
                // El número es mayor
                txtMensaje.Text = $"El número es mayor que {numero}. 📈";
                txtMensaje.Foreground = new SolidColorBrush(Color.FromRgb(255, 140, 0));
                borderMensaje.Background = new SolidColorBrush(Color.FromRgb(255, 248, 240));
                borderMensaje.Visibility = Visibility.Visible;
            }
            else
            {
                // El número es menor
                txtMensaje.Text = $"El número es menor que {numero}. 📉";
                txtMensaje.Foreground = new SolidColorBrush(Color.FromRgb(255, 69, 0));
                borderMensaje.Background = new SolidColorBrush(Color.FromRgb(255, 248, 240));
                borderMensaje.Visibility = Visibility.Visible;
            }

            // Limpiar el TextBox para el siguiente intento
            txtNumero.Text = string.Empty;
            txtNumero.Focus();
        }

        private void BtnNuevoJuego_Click(object sender, RoutedEventArgs e)
        {
            IniciarNuevoJuego();
        }

        private void TxtNumero_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter && !_juegoTerminado)
            {
                BtnAdivinar_Click(sender, new RoutedEventArgs());
            }
        }
    }
}

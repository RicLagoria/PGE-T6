using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace PGE_T6.Views
{
    public partial class Ej08View : UserControl
    {
        private readonly CultureInfo _cultureArgentina = new CultureInfo("es-AR");

        public Ej08View()
        {
            InitializeComponent();
            LimpiarCampos();
        }

        private void BtnCalcular_Click(object sender, RoutedEventArgs e)
        {
            txtError.Text = string.Empty;
            
            // Validar monto de la cuenta
            if (!ValidarMonto())
                return;

            // Obtener porcentaje de propina
            if (!ObtenerPorcentajePropina(out double porcentaje))
                return;

            // Calcular propina y total
            var monto = double.Parse(txtMonto.Text, NumberStyles.Float, _cultureArgentina);
            var propina = monto * (porcentaje / 100.0);
            var total = monto + propina;

            // Mostrar resultados
            MostrarResultados(monto, porcentaje, propina, total);
        }

        private bool ValidarMonto()
        {
            var texto = txtMonto.Text?.Trim();
            if (string.IsNullOrWhiteSpace(texto))
            {
                txtError.Text = "Ingresa el monto de la cuenta.";
                return false;
            }

            if (!double.TryParse(texto, NumberStyles.Float, _cultureArgentina, out var monto))
            {
                txtError.Text = "Ingresa un monto válido (usa coma para decimales).";
                return false;
            }

            if (monto <= 0)
            {
                txtError.Text = "El monto debe ser mayor a cero.";
                return false;
            }

            if (monto > 999999.99)
            {
                txtError.Text = "El monto es demasiado alto (máximo: $999.999,99).";
                return false;
            }

            return true;
        }

        private bool ObtenerPorcentajePropina(out double porcentaje)
        {
            porcentaje = 0;

            // Verificar si hay porcentaje personalizado
            var textoPersonalizado = txtPorcentajePersonalizado.Text?.Trim();
            if (!string.IsNullOrWhiteSpace(textoPersonalizado))
            {
                if (!double.TryParse(textoPersonalizado, NumberStyles.Float, _cultureArgentina, out porcentaje))
                {
                    txtError.Text = "El porcentaje personalizado debe ser un número válido.";
                    return false;
                }

                if (porcentaje < 0 || porcentaje > 100)
                {
                    txtError.Text = "El porcentaje debe estar entre 0% y 100%.";
                    return false;
                }

                return true;
            }

            // Usar porcentaje del ComboBox
            var itemSeleccionado = cmbPorcentaje.SelectedItem as ComboBoxItem;
            if (itemSeleccionado?.Content is string contenido)
            {
                var porcentajeTexto = contenido.Replace("%", "");
                if (double.TryParse(porcentajeTexto, NumberStyles.Float, _cultureArgentina, out porcentaje))
                {
                    return true;
                }
            }

            txtError.Text = "Selecciona un porcentaje de propina válido.";
            return false;
        }

        private void MostrarResultados(double monto, double porcentaje, double propina, double total)
        {
            txtMontoCuenta.Text = $"${monto:N2}".Replace(".", ",");
            txtPorcentajeUsado.Text = $"{porcentaje:F1}%";
            txtPropina.Text = $"${propina:N2}".Replace(".", ",");
            txtTotal.Text = $"${total:N2}".Replace(".", ",");
        }

        private void BtnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            LimpiarCampos();
        }

        private void LimpiarCampos()
        {
            txtMonto.Text = string.Empty;
            cmbPorcentaje.SelectedIndex = 1; // 15% por defecto
            txtPorcentajePersonalizado.Text = string.Empty;
            txtError.Text = string.Empty;

            // Limpiar resultados
            txtMontoCuenta.Text = "—";
            txtPorcentajeUsado.Text = "—";
            txtPropina.Text = "—";
            txtTotal.Text = "—";

            txtMonto.Focus();
        }

        private void TxtMonto_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                BtnCalcular_Click(sender, new RoutedEventArgs());
            }
        }
    }
}

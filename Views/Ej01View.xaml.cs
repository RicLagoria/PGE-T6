using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace PGE_T6.Views
{
    public partial class Ej01View : UserControl
    {
        private readonly CultureInfo _cultureArgentina = new CultureInfo("es-AR");
        private const double CeroAbsolutoCelsius = -273.15;

        public Ej01View()
        {
            InitializeComponent();
        }

        private void BtnConvertir_Click(object sender, RoutedEventArgs e)
        {
            txtError.Text = string.Empty;
            txtResultado.Text = "—";

            var texto = txtCelsius.Text?.Trim();
            if (string.IsNullOrWhiteSpace(texto))
            {
                txtError.Text = "El campo está vacío.";
                return;
            }

            if (!double.TryParse(texto, NumberStyles.Float, _cultureArgentina, out var celsius))
            {
                txtError.Text = "Ingrese un número válido (use coma para decimales).";
                return;
            }

            if (celsius < CeroAbsolutoCelsius)
            {
                txtError.Text = "El valor es menor al cero absoluto (-273,15 °C).";
                return;
            }

            var fahrenheit = (celsius * 9.0 / 5.0) + 32.0;
            txtResultado.Text = $"{fahrenheit:0.00} °F";
        }

        private void BtnLimpiar_Click(object sender, RoutedEventArgs e)
        {
            txtCelsius.Text = string.Empty;
            txtResultado.Text = "—";
            txtError.Text = string.Empty;
            txtCelsius.Focus();
        }
    }
}



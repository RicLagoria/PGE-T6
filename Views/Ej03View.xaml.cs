using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PGE_T6.Views
{
    public partial class Ej03View : UserControl
    {
        private readonly CultureInfo _cultureArgentina = new CultureInfo("es-AR");
        private readonly ObservableCollection<Gasto> _gastos;

        public Ej03View()
        {
            InitializeComponent();
            _gastos = new ObservableCollection<Gasto>();
            dgGastos.ItemsSource = _gastos;
            _gastos.CollectionChanged += (s, e) => ActualizarTotalARS();
            ActualizarTotalARS();
        }

        private void BtnAgregar_Click(object sender, RoutedEventArgs e)
        {
            txtError.Text = string.Empty;

            var descripcion = txtDescripcion.Text?.Trim();
            if (string.IsNullOrWhiteSpace(descripcion))
            {
                txtError.Text = "La descripción no puede estar vacía.";
                return;
            }

            var valorTexto = txtValor.Text?.Trim();
            if (string.IsNullOrWhiteSpace(valorTexto))
            {
                txtError.Text = "El valor no puede estar vacío.";
                return;
            }

            if (!decimal.TryParse(valorTexto, NumberStyles.Currency | NumberStyles.AllowDecimalPoint, _cultureArgentina, out var valor))
            {
                txtError.Text = "Ingrese un valor numérico válido (use coma para decimales).";
                return;
            }

            if (valor <= 0)
            {
                txtError.Text = "El valor debe ser mayor a cero.";
                return;
            }

            var gasto = new Gasto
            {
                Descripcion = descripcion,
                ValorARS = valor,
                Fecha = DateTime.Now
            };

            _gastos.Add(gasto);

            // Limpiar campos
            txtDescripcion.Text = string.Empty;
            txtValor.Text = string.Empty;
            txtDescripcion.Focus();

            ActualizarTotalARS();
        }

        private void BtnConvertir_Click(object sender, RoutedEventArgs e)
        {
            if (!_gastos.Any())
            {
                txtTotalConvertido.Text = "No hay gastos para convertir.";
                return;
            }

            var tipoCambioTexto = txtTipoCambio.Text?.Trim();
            if (string.IsNullOrWhiteSpace(tipoCambioTexto))
            {
                txtTotalConvertido.Text = "Ingrese un tipo de cambio válido.";
                return;
            }

            if (!decimal.TryParse(tipoCambioTexto, NumberStyles.Float, _cultureArgentina, out var tipoCambio))
            {
                txtTotalConvertido.Text = "Tipo de cambio inválido.";
                return;
            }

            if (tipoCambio <= 0)
            {
                txtTotalConvertido.Text = "El tipo de cambio debe ser mayor a cero.";
                return;
            }

            var totalARS = _gastos.Sum(g => g.ValorARS);
            var totalConvertido = totalARS / tipoCambio;

            var monedaSeleccionada = "USD";
            if (cmbMoneda.SelectedItem is ComboBoxItem item && item.Tag is string moneda)
            {
                monedaSeleccionada = moneda;
            }

            var simboloMoneda = ObtenerSimboloMoneda(monedaSeleccionada);
            txtTotalConvertido.Text = $"Total: {totalARS:C} ARS = {totalConvertido:F2} {simboloMoneda}";
        }

        private void ActualizarTotalARS()
        {
            var total = _gastos.Sum(g => g.ValorARS);
            if (txtTotalARS != null)
            {
                txtTotalARS.Text = total.ToString("C", _cultureArgentina);
            }
        }

        private string ObtenerSimboloMoneda(string codigoMoneda)
        {
            return codigoMoneda switch
            {
                "USD" => "$",
                "EUR" => "€",
                "BRL" => "R$",
                "CLP" => "$",
                _ => codigoMoneda
            };
        }
    }

    public class Gasto
    {
        public string Descripcion { get; set; } = string.Empty;
        public decimal ValorARS { get; set; }
        public DateTime Fecha { get; set; }
    }
}

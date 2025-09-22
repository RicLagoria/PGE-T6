using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PGE_T6.Views
{
    public partial class Ej06View : UserControl
    {
        private readonly CultureInfo _cultureArgentina = new CultureInfo("es-AR");
        private readonly Dictionary<string, MonedaInfo> _monedas;
        private readonly ObservableCollection<ConversionResult> _conversiones;

        public Ej06View()
        {
            InitializeComponent();
            
            // Inicializar tipos de cambio (valores aproximados para demostración)
            _monedas = new Dictionary<string, MonedaInfo>
            {
                { "ARS", new MonedaInfo("ARS", "Peso Argentino", "$", 1.0) },
                { "USD", new MonedaInfo("USD", "Dólar Estadounidense", "$", 1000.0) }, // 1 USD = 1000 ARS
                { "EUR", new MonedaInfo("EUR", "Euro", "€", 1100.0) }, // 1 EUR = 1100 ARS
                { "BRL", new MonedaInfo("BRL", "Real Brasileño", "R$", 200.0) }, // 1 BRL = 200 ARS
                { "CLP", new MonedaInfo("CLP", "Peso Chileno", "$", 1.2) } // 1 CLP = 1.2 ARS
            };

            _conversiones = new ObservableCollection<ConversionResult>();
            icConversiones.ItemsSource = _conversiones;

            // Conversión inicial
            txtValor.TextChanged += (s, e) => ConvertirMonedas();
            cmbMonedaBase.SelectionChanged += (s, e) => ConvertirMonedas();
            
            Loaded += (s, e) => ConvertirMonedas();
        }

        private void BtnConvertir_Click(object sender, RoutedEventArgs e)
        {
            ConvertirMonedas();
        }

        private void ConvertirMonedas()
        {
            if (!IsLoaded) return;

            LimpiarError();

            if (!decimal.TryParse(txtValor.Text?.Trim(), NumberStyles.Float, _cultureArgentina, out var valor))
            {
                MostrarError("Ingrese un valor numérico válido.");
                return;
            }

            if (valor < 0)
            {
                MostrarError("El valor debe ser positivo.");
                return;
            }

            var monedaBase = ObtenerMonedaBase();
            if (!_monedas.TryGetValue(monedaBase, out var infoBase))
            {
                MostrarError("Moneda base no válida.");
                return;
            }

            _conversiones.Clear();

            // Convertir a todas las monedas excepto la base
            foreach (var moneda in _monedas.Values.Where(m => m.Codigo != monedaBase))
            {
                var valorConvertido = ConvertirValor(valor, infoBase, moneda);
                _conversiones.Add(new ConversionResult
                {
                    Codigo = moneda.Codigo,
                    Nombre = moneda.Nombre,
                    Simbolo = moneda.Simbolo,
                    Valor = valorConvertido
                });
            }

            // Ordenar por código de moneda
            var conversionesOrdenadas = _conversiones.OrderBy(c => c.Codigo).ToList();
            _conversiones.Clear();
            foreach (var item in conversionesOrdenadas)
            {
                _conversiones.Add(item);
            }
        }

        private decimal ConvertirValor(decimal valor, MonedaInfo desde, MonedaInfo hacia)
        {
            // Convertir a ARS primero, luego a la moneda destino
            var valorEnARS = valor * (decimal)desde.TipoCambioAR;
            return valorEnARS / (decimal)hacia.TipoCambioAR;
        }

        private string ObtenerMonedaBase()
        {
            if (cmbMonedaBase.SelectedItem is ComboBoxItem item && item.Tag is string codigo)
                return codigo;
            return "ARS";
        }

        private void MostrarError(string mensaje)
        {
            if (txtError != null)
            {
                txtError.Text = mensaje;
            }
        }

        private void LimpiarError()
        {
            if (txtError != null)
            {
                txtError.Text = string.Empty;
            }
        }
    }

    public class MonedaInfo
    {
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Simbolo { get; set; }
        public double TipoCambioAR { get; set; } // Cuántos ARS equivalen a 1 unidad de esta moneda

        public MonedaInfo(string codigo, string nombre, string simbolo, double tipoCambioAR)
        {
            Codigo = codigo;
            Nombre = nombre;
            Simbolo = simbolo;
            TipoCambioAR = tipoCambioAR;
        }
    }

    public class ConversionResult
    {
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Simbolo { get; set; } = string.Empty;
        public decimal Valor { get; set; }
    }
}

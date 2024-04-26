using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TicketCine
{
    public partial class Asiento : Window
    {
        private int pelicula;
        private int horario;
        private List<int> asientosSeleccionados = new List<int>();

        public Asiento(int idPelicula, int idHorario)
        {
            pelicula = idPelicula;
            horario = idHorario;
            InitializeComponent();
            CargarAsientos();
        }

        private void CargarAsientos()
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 12; col++)
                {
                    Button btn = new Button
                    {
                        Content = (row * 12 + col + 1).ToString(),
                        Margin = new Thickness(5)
                    };
                    btn.Click += Asiento_Click;
                    Grid.SetRow(btn, row);
                    Grid.SetColumn(btn, col);
                    (FindName("GridAsientos") as Grid)?.Children.Add(btn);
                }
            }
        }

        private void Asiento_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            int asientoNum = int.Parse(clickedButton.Content.ToString());

            if (asientosSeleccionados.Contains(asientoNum))
            {
                asientosSeleccionados.Remove(asientoNum);
                clickedButton.Background = null; // Cambia a color por defecto
            }
            else
            {
                asientosSeleccionados.Add(asientoNum);
                clickedButton.Background = new SolidColorBrush(Colors.Green); // Cambia a verde al seleccionar
            }
        }

        private void UsuarioNuevo_Click(object sender, RoutedEventArgs e)
        {
            Usuario window = new Usuario(pelicula, horario);
            window.Show();
        }

        private void UsuarioExistente_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("presionaste este boton");
        }
    }
}

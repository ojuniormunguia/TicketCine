using Npgsql;
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
        private HashSet<int> asientosOcupados = new HashSet<int>();  // Hacer global dentro de la clase
        private string cadenaConexion = "Server=hansken.db.elephantsql.com;Port=5432;Database=ikegunyj;User Id=ikegunyj;Password=PjDjGMbve9rwF5eP4fMGm5M59yzCpExq;";

        public Asiento(int idPelicula, int idHorario)
        {
            pelicula = idPelicula;
            horario = idHorario;
            InitializeComponent();
            CargarAsientos();
        }

        private void CargarAsientos()
        {
            asientosOcupados = ObtenerAsientosOcupados();  // Carga los asientos ocupados en la variable de instancia

            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 12; col++)
                {
                    int asientoNum = row * 12 + col + 1;
                    Button btn = new Button
                    {
                        Content = asientoNum.ToString(),
                        Margin = new Thickness(5),
                        Background = asientosOcupados.Contains(asientoNum) ? new SolidColorBrush(Colors.Red) : null
                    };
                    btn.Click += Asiento_Click;
                    Grid.SetRow(btn, row);
                    Grid.SetColumn(btn, col);
                    (FindName("GridAsientos") as Grid)?.Children.Add(btn);
                }
            }
        }

        private HashSet<int> ObtenerAsientosOcupados()
        {
            HashSet<int> ocupados = new HashSet<int>();
            using (NpgsqlConnection connection = new NpgsqlConnection(cadenaConexion))
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand("SELECT Asiento FROM reservaciones WHERE IDHorario = @IDHorario", connection);
                command.Parameters.AddWithValue("@IDHorario", horario);
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ocupados.Add(reader.GetInt32(0));
                    }
                }
            }
            return ocupados;
        }

        private void Asiento_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            int asientoNum = int.Parse(clickedButton.Content.ToString());
            if (!asientosOcupados.Contains(asientoNum))  // Ahora accesible aquí
            {
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

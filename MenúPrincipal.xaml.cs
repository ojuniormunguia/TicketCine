using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// <summary>
    /// Lógica de interacción para MenúPrincipal.xaml
    /// </summary>
    public partial class MenúPrincipal : Window
    {
        string cadenaConexion = "Server=hansken.db.elephantsql.com;Port=5432;Database=ikegunyj;User Id=ikegunyj;Password=PjDjGMbve9rwF5eP4fMGm5M59yzCpExq;";
        public MenúPrincipal()
        {
            InitializeComponent();
            CargarPeliculas();
            this.Closing += ThisWindow_Closing;
        }

        private void ThisWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        public class Pelicula
        {
            public string NombrePelicula { get; set; }
            public int IdPelicula { get; set; }
            public int DuracionPelicula { get; set; }
            public string GeneroPelicula { get; set; }
            public string DirectorPelicula { get; set; }
        }
        private DataTable ConseguirPeliculas()
        {
            string consulta = "SELECT * FROM \"public\".\"peliculas\"";
            DataTable dt = new DataTable();
            try
            {
                using (var conexion = new NpgsqlConnection(cadenaConexion))
                {
                    conexion.Open();
                    using (var cmd = new NpgsqlCommand(consulta, conexion))
                    {
                        NpgsqlDataReader reader = cmd.ExecuteReader();
                        dt.Load(reader);
                    }
                }
                return dt;
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                MessageBox.Show($"error: {ex.Message}");
                return dt;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error: {ex.Message}");
                MessageBox.Show($"error: {ex.Message}");
                return dt;
            }
        }
        private void CargarPeliculas()
        {
            List<Pelicula> peliculas = new List<Pelicula>();
            DataTable dt = ConseguirPeliculas();
            foreach (DataRow row in dt.Rows)
            {
                peliculas.Add(new Pelicula()
                {
                    IdPelicula = Convert.ToInt32(row["idpelicula"]),
                    NombrePelicula = row["pelicula"].ToString(),
                    DuracionPelicula = Convert.ToInt32(row["duracion"]),
                    GeneroPelicula = row["genero"].ToString(),
                    DirectorPelicula = row["director"].ToString(),
                });
            }

            ListViewPeliculas.ItemsSource = peliculas;
        }
        private void ListViewPeliculas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListViewPeliculas.SelectedItem is Pelicula peliculaSeleccionada)
            {
                Horarios horarios = new Horarios(peliculaSeleccionada.IdPelicula);
                horarios.Show();
                this.Hide();
            }
        }

        private void Click_Usuarios(object sender, RoutedEventArgs e)
        {
            MostrarTodosUsuarios mostrarTodosUsuarios = new MostrarTodosUsuarios();
            mostrarTodosUsuarios.Show();
            this.Hide();
        }
    }
}

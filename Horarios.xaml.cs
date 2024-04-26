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
    public partial class Horarios : Window
    {
        public int IdPelicula { get; private set; }
        string cadenaConexion = "Server=hansken.db.elephantsql.com;Port=5432;Database=ikegunyj;User Id=ikegunyj;Password=PjDjGMbve9rwF5eP4fMGm5M59yzCpExq;";

        public Horarios(int idPelicula)
        {
            InitializeComponent();
            IdPelicula = idPelicula;
            CargarHorarios();
            this.Closing += ThisWindow_Closing;
        }
        private void ThisWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void CargarHorarios()
        {
            List<Horario> horarios = new List<Horario>();
            string consulta = "SELECT idhorario ,hora, fecha, sala, formato FROM horarios WHERE IDPelicula = @IdPelicula";

            using (var conexion = new NpgsqlConnection(cadenaConexion))
            {
                conexion.Open();
                using (var cmd = new NpgsqlCommand(consulta, conexion))
                {
                    cmd.Parameters.AddWithValue("@IdPelicula", IdPelicula);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            
                            horarios.Add(new Horario()
                            {
                                idHorario = reader.GetInt32(0),
                                Hora = reader.GetTimeSpan(1).ToString(),
                                Fecha = reader.GetDateTime(2).ToString("yyyy-MM-dd"),
                                Sala = reader.GetString(3),
                                Formato = reader.GetString(4)
                            });
                        }
                    }
                }
            }

            ListViewHorarios.ItemsSource = horarios;
        }
        private void click_regresar(object sender, RoutedEventArgs e)
        {
            MenúPrincipal menúPrincipal = new MenúPrincipal();
            menúPrincipal.Show();
            this.Hide();
        }

        private void ListViewHorarios_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListViewHorarios.SelectedItem is Horario selectedHorario)
            {
                Asiento ventanaAsientos = new Asiento(IdPelicula, selectedHorario.idHorario);
                ventanaAsientos.Show();
                this.Hide();
            }
        }

    }

    public class Horario
    {
        public int idHorario { get; set; }
        public string Hora { get; set; }
        public string Fecha { get; set; }
        public string Sala { get; set; }
        public string Formato { get; set; }
    }
    
}

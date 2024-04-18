﻿using Npgsql;
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
        }

        private void CargarHorarios()
        {
            List<Horario> horarios = new List<Horario>();
            string consulta = "SELECT Hora, Fecha, Sala, Formato FROM horarios WHERE IDPelicula = @IdPelicula";

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
                                Hora = reader.GetTimeSpan(0).ToString(),
                                Fecha = reader.GetDateTime(1).ToString("yyyy-MM-dd"),
                                Sala = reader.GetString(2),
                                Formato = reader.GetString(3)
                            });
                        }
                    }
                }
            }

            ListViewHorarios.ItemsSource = horarios;
        }
    }

    public class Horario
    {
        public string Hora { get; set; }
        public string Fecha { get; set; }
        public string Sala { get; set; }
        public string Formato { get; set; }
    }
}
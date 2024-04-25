using Npgsql;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace TicketCine
{
    public partial class MostrarTodosUsuarios : Window
    {
        private string cadenaConexion = "Server=hansken.db.elephantsql.com;Port=5432;Database=ikegunyj;User Id=ikegunyj;Password=PjDjGMbve9rwF5eP4fMGm5M59yzCpExq;";

        public MostrarTodosUsuarios()
        {
            InitializeComponent();
            CargarUsuarios();
            this.Closing += ThisWindow_Closing;
        }

        private void ThisWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void CargarUsuarios()
        {
            List<User> listaUsuarios = new List<User>();
            string consulta = "SELECT usuario, nombre, contrasena FROM userbase";

            try
            {
                using (var conexion = new NpgsqlConnection(cadenaConexion))
                {
                    conexion.Open();
                    using (var cmd = new NpgsqlCommand(consulta, conexion))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                listaUsuarios.Add(new User
                                {
                                    Usuario = reader.GetString(0),
                                    Nombre = reader.GetString(1),
                                    Contrasena = reader.GetString(2)
                                });
                            }
                        }
                    }
                }
                listViewUsuarios.ItemsSource = listaUsuarios;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading users: " + ex.Message);
            }
        }
    }

    public class User
    {
        public string Usuario { get; set; }
        public string Nombre { get; set; }
        public string Contrasena { get; set; }
    }

}

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
    public partial class Usuario : Window
    {
        private string cadenaConexion = "Server=hansken.db.elephantsql.com;Port=5432;Database=ikegunyj;User Id=ikegunyj;Password=PjDjGMbve9rwF5eP4fMGm5M59yzCpExq;";

        public Usuario(int idpelicula, int idhorario)
        {
            InitializeComponent();
            cmbGenero.ItemsSource = new string[] { "Masculino", "Femenino", "Otro" }; // Asumiendo opciones de género
            this.Closing += ThisWindow_Closing;
        }
        private void ThisWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void Button_SaveClick(object sender, RoutedEventArgs e)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(cadenaConexion))
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand();
                NpgsqlTransaction transaction = connection.BeginTransaction();
                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    // Inserción en la tabla de clientes
                    command.CommandText = "INSERT INTO cliente (Nombre, Genero, Edad, Correo, Telefono) VALUES (@Nombre, @Genero, @Edad, @Correo, @Telefono) RETURNING IDCliente";
                    command.Parameters.AddWithValue("@Nombre", txtNombre.Text);
                    command.Parameters.AddWithValue("@Genero", cmbGenero.SelectedItem.ToString());
                    command.Parameters.AddWithValue("@Edad", int.Parse(txtEdad.Text));
                    command.Parameters.AddWithValue("@Correo", txtCorreo.Text);
                    command.Parameters.AddWithValue("@Telefono", txtTelefono.Text);

                    // Ejecución y captura del ID del cliente nuevo
                    object result = command.ExecuteScalar();
                    int idCliente = (result != null) ? Convert.ToInt32(result) : 0;

                    if (idCliente == 0)
                    {
                        throw new Exception("No se pudo obtener el ID del cliente.");
                    }

                    if (chkAgregarTarjetaPlus.IsChecked == true)
                    {
                        // Limpia los parámetros para el nuevo comando
                        command.Parameters.Clear();
                        command.CommandText = "INSERT INTO tarjeta_plus (IDCliente, Puntos, Creacion, Vencimiento) VALUES (@IDCliente, 10, @Creacion, @Vencimiento)";
                        command.Parameters.AddWithValue("@IDCliente", idCliente);
                        command.Parameters.AddWithValue("@Creacion", DateTime.Now);
                        command.Parameters.AddWithValue("@Vencimiento", DateTime.Now.AddYears(1));

                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    MessageBox.Show("Cliente guardado correctamente.");

                    // Aquí llamamos a la función que maneja las reservaciones y tickets
                    CreateReservationsAndTickets(idCliente);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al guardar cliente: " + ex.Message);
                    transaction.Rollback();
                }
            }
        }

        private void CreateReservationsAndTickets(int idCliente)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(cadenaConexion))
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand();
                NpgsqlTransaction transaction = connection.BeginTransaction();
                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    // Inicia la transacción
                    command.CommandText = "BEGIN;";
                    command.ExecuteNonQuery();

                    // Insertar reservaciones
                    command.CommandText = @"
                INSERT INTO reservaciones (IDHorario, Asiento)
                VALUES (1, 12), (1, 13)
                RETURNING IDReservacion";
                    List<int> reservationIds = new List<int>();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            reservationIds.Add(reader.GetInt32(0));
                        }
                    }

                    // Crear y mostrar IDTickets
                    StringBuilder sb = new StringBuilder();
                    foreach (var reservationId in reservationIds)
                    {
                        command.Parameters.Clear();
                        command.CommandText = @"
                    INSERT INTO ticket (IDTicket, IDCliente, IDReservacion)
                    VALUES (md5(random()::text || clock_timestamp()::text)::char(16), @idCliente, @idReservacion)
                    RETURNING IDTicket";
                        command.Parameters.AddWithValue("@idCliente", idCliente);
                        command.Parameters.AddWithValue("@idReservacion", reservationId);
                        var ticketId = command.ExecuteScalar().ToString();
                        sb.AppendLine(ticketId);
                    }

                    // Finalizar transacción
                    command.CommandText = "COMMIT;";
                    command.ExecuteNonQuery();

                    MessageBox.Show("Reservaciones y tickets creados correctamente.\nTickets IDs:\n" + sb.ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al crear reservaciones y tickets: " + ex.Message);
                    transaction.Rollback();
                }
            }
        }



    }
}

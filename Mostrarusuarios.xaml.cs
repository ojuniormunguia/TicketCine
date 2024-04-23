private void CargarUsuarios()
{
    List<Usuario> listaUsuarios = new List<Usuario>();
    string consulta = "SELECT id_usuario, nombre_usuario, contrasena FROM userbase";
    
    using (var conexion = new NpgsqlConnection(cadenaConexion))
    {
        conexion.Open();
        using (var cmd = new NpgsqlCommand(consulta, conexion))
        {
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    listaUsuarios.Add(new Usuario()
                    {
                        IdUsuario = reader.GetInt32(0),
                        NombreUsuario = reader.GetString(1),
                        Contrasena = reader.GetString(2)
                    });
                }
            }
        }
    }
    listViewUsuarios.ItemsSource = listaUsuarios; // sería el nombre de la ListView o DataGrid
}

private void CrearUsuario(Usuario usuario)
{
    string consulta = "INSERT INTO userbase (nombre_usuario, contrasena) VALUES (@NombreUsuario, crypt(@Contrasena))"; //agregar otros datos que vayan en la bd

    using (var conexion = new NpgsqlConnection(cadenaConexion))
    {
        conexion.Open();
        using (var cmd = new NpgsqlCommand(consulta, conexion))
        {
            cmd.Parameters.AddWithValue("@NombreUsuario", usuario.NombreUsuario);
            cmd.Parameters.AddWithValue("@Contrasena", usuario.Contrasena); // La contraseña ya debe estar hasheada antes de llamar a este método
            cmd.ExecuteNonQuery();
        }
    }

    // Recargar la lista de usuarios para reflejar el cambio
    CargarUsuarios();
}

private void ActualizarUsuario(Usuario usuario)
{
    string consulta = "UPDATE userbase SET nombre_usuario = @NombreUsuario, contrasena = crypt(@Contrasena) WHERE id_usuario = @IdUsuario";

    using (var conexion = new NpgsqlConnection(cadenaConexion))
    {
        conexion.Open();
        using (var cmd = new NpgsqlCommand(consulta, conexion))
        {
            cmd.Parameters.AddWithValue("@IdUsuario", usuario.IdUsuario);
            cmd.Parameters.AddWithValue("@NombreUsuario", usuario.NombreUsuario);
            cmd.Parameters.AddWithValue("@Contrasena", usuario.Contrasena); 
            cmd.ExecuteNonQuery();
        }
    }

    // Recargar la lista de usuarios para reflejar el cambio
    CargarUsuarios();
}

private void EliminarUsuario(int idUsuario)
{
    string consulta = "DELETE FROM userbase WHERE id_usuario = @IdUsuario";

    using (var conexion = new NpgsqlConnection(cadenaConexion))
    {
        conexion.Open();
        using (var cmd = new NpgsqlCommand(consulta, conexion))
        {
            cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
            cmd.ExecuteNonQuery();
        }
    }

    // Recargar la lista de usuarios para reflejar el cambio
    CargarUsuarios();
}

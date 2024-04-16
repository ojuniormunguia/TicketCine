using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Security;
using System.Diagnostics.Eventing.Reader;
using Npgsql;

namespace TicketCine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string cadenaConexion = "Server=hansken.db.elephantsql.com;Port=5432;Database=ikegunyj;User Id=ikegunyj;Password=PjDjGMbve9rwF5eP4fMGm5M59yzCpExq;";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void iniciarSesion(object sender, RoutedEventArgs e)
        {
            string usuario = txtUsuario.Text;
            AutenticarUsuario(usuario, ContrasenaEncriptada());

        }
        private bool AutenticarUsuario(string usuario, string contrasena)
        {
            bool autenticado = false;
            string consulta = "SELECT contrasena = crypt(@contrasena, contrasena) FROM userbase WHERE usuario = @usuario";

            try
            {
                using (var conexion = new NpgsqlConnection(cadenaConexion))
                {
                    conexion.Open();
                    using (var cmd = new NpgsqlCommand(consulta, conexion))
                    {
                        cmd.Parameters.Add(new NpgsqlParameter("@usuario", NpgsqlTypes.NpgsqlDbType.Varchar) { Value = usuario });
                        cmd.Parameters.Add(new NpgsqlParameter("@contrasena", NpgsqlTypes.NpgsqlDbType.Text) { Value = contrasena });

                        autenticado = Convert.ToBoolean(cmd.ExecuteScalar());

                        if (autenticado)
                        {
                            MessageBox.Show("Inició sesión.");
                        }
                        else
                        {
                            MessageBox.Show("Usuario o contraseña incorrecta.");
                        }
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                MessageBox.Show($"error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error: {ex.Message}");
                MessageBox.Show($"error: {ex.Message}");
            }
            return autenticado;
        }
        private string ContrasenaEncriptada()
        {
            SecureString securePassword = txtPass.SecurePassword;

            IntPtr ptr = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(securePassword);
            try
            {
                return System.Runtime.InteropServices.Marshal.PtrToStringBSTR(ptr);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeBSTR(ptr);
            }
        }
        
    }

}
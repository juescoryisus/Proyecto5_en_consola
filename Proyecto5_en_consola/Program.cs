using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace Proyecto5_en_consola
{
    internal class Program
    {
        private static string connectionString = "Server=DESKTOP-OG56094\\SQLEXPRESS;Database=Entidades; integrated security=true";

        static void Main(string[] args)
        {
            if (Login())
            {
                MostrarMenu();
            }
            else
            {
                Console.WriteLine("Usuario o contraseña incorrectos. Cerrando aplicación...");
            }
        }

        // Método para el inicio de sesión
        static bool Login()
        {
            Console.Write("Ingrese usuario: ");
            string usuario = Console.ReadLine();

            Console.Write("Ingrese contraseña: ");
            string password = LeerPassword();

            string passwordHash = EncriptarSHA256(password);

            // Simulación de usuario y contraseña encriptada
            string usuarioCorrecto = "jesus";
            string passwordCorrectoHash = EncriptarSHA256("1234"); //Contraseña: 1234

            return usuario == usuarioCorrecto && passwordHash == passwordCorrectoHash;
        }

        // Método para mostrar el menú principal
        static void MostrarMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Menú CRUD ===");
                Console.WriteLine("1. Escuela");
                Console.WriteLine("2. Dirección General");
                Console.WriteLine("3. Administración");
                Console.WriteLine("4. Salir");
                Console.Write("Elija una opción: ");

                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        MenuEscuela();
                        break;
                    case "2":
                        MenuDireccionGeneral();
                        break;
                    case "3":
                        MenuAdministracion();
                        break;
                    case "4":
                        Console.WriteLine("Saliendo del programa...");
                        return;
                    default:
                        Console.WriteLine("Opción inválida. Intente nuevamente.");
                        break;
                }
                Console.WriteLine("Presione cualquier tecla para continuar...");
                Console.ReadKey();
            }
        }

        // ===== CRUD Escuela =====
        static void MenuEscuela()
        {
            Console.Clear();
            Console.WriteLine("=== Escuela ===");
            Console.WriteLine("1. Agregar Escuela");
            Console.WriteLine("2. Mostrar Escuelas");
            Console.WriteLine("3. Eliminar Escuela");
            Console.Write("Elija una opción: ");
            string opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    AgregarEscuela();
                    break;
                case "2":
                    MostrarTabla("Escuela");
                    break;
                case "3":
                    EliminarRegistro("Escuela", "idEscuela");
                    break;
            }
        }

        static void AgregarEscuela()
        {
            Console.Write("Guardias: ");
            string guardias = Console.ReadLine();

            Console.Write("Maestros: ");
            string maestros = Console.ReadLine();

            Console.Write("Directivos: ");
            string directivos = Console.ReadLine();

            Console.Write("Clases: ");
            string clases = Console.ReadLine();

            EjecutarConsulta("INSERT INTO Escuela (Guardias, Maestros, Directivos, Clases) VALUES (@param1, @param2, @param3, @param4)",
                guardias, maestros, directivos, clases);
        }

        // ===== CRUD Dirección General =====
        static void MenuDireccionGeneral()
        {
            Console.Clear();
            Console.WriteLine("=== Dirección General ===");
            Console.WriteLine("1. Agregar Dirección General");
            Console.WriteLine("2. Mostrar Direcciones Generales");
            Console.WriteLine("3. Eliminar Dirección General");
            Console.Write("Elija una opción: ");
            string opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    AgregarDireccionGeneral();
                    break;
                case "2":
                    MostrarTabla("DireccionGeneral");
                    break;
                case "3":
                    EliminarRegistro("DireccionGeneral", "idDireccionGeneral");
                    break;
            }
        }

        static void AgregarDireccionGeneral()
        {
            Console.Write("Secretaria: ");
            string secretaria = Console.ReadLine();

            Console.Write("Correo Electrónico: ");
            string correo = Console.ReadLine();

            Console.Write("Teléfono: ");
            string telefono = Console.ReadLine();

            EjecutarConsulta("INSERT INTO DireccionGeneral (Secretaria, correoElectronico, telefono) VALUES (@param1, @param2, @param3)",
                secretaria, correo, telefono);
        }

        // ===== CRUD Administración =====
        static void MenuAdministracion()
        {
            Console.Clear();
            Console.WriteLine("=== Administración ===");
            Console.WriteLine("1. Agregar Administración");
            Console.WriteLine("2. Mostrar Administraciones");
            Console.WriteLine("3. Eliminar Administración");
            Console.Write("Elija una opción: ");
            string opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    AgregarAdministracion();
                    break;
                case "2":
                    MostrarTabla("Administracion");
                    break;
                case "3":
                    EliminarRegistro("Administracion", "idAdministracion");
                    break;
            }
        }

        static void AgregarAdministracion()
        {
            Console.Write("Nombre del encargado: ");
            string encargado = Console.ReadLine();

            Console.Write("Teléfono: ");
            string telefono = Console.ReadLine();

            Console.Write("Fecha (yyyy-MM-dd): ");
            string fecha = Console.ReadLine();

            Console.Write("Nombre de usuario: ");
            string usuario = Console.ReadLine();

            EjecutarConsulta("INSERT INTO Administracion (nombreEncargado, telefono, fecha, nombre_de_Usuario) VALUES (@param1, @param2, @param3, @param4)",
                encargado, telefono, fecha, usuario);
        }

        // ===== Funciones Comunes =====
        static void EjecutarConsulta(string query, params string[] parametros)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);

                for (int i = 0; i < parametros.Length; i++)
                {
                    cmd.Parameters.AddWithValue($"@param{i + 1}", parametros[i]);
                }

                cmd.ExecuteNonQuery();
                Console.WriteLine("Operación completada.");
            }
        }

        static void MostrarTabla(string tabla)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand($"SELECT * FROM {tabla}", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.Write($"{reader.GetName(i)}: {reader[i]} | ");
                    }
                    Console.WriteLine();
                }
            }
        }

        static void EliminarRegistro(string tabla, string idColumna)
        {
            Console.Write($"Ingrese el ID del registro a eliminar en {tabla}: ");
            string id = Console.ReadLine();

            EjecutarConsulta($"DELETE FROM {tabla} WHERE {idColumna} = @param1", id);
        }

        // ===== Utilidades =====
        static string EncriptarSHA256(string texto)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(texto));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        static string LeerPassword()
        {
            string password = "";
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(intercept: true);
                if (key.Key != ConsoleKey.Enter)
                {
                    password += key.KeyChar;
                    Console.Write("*");
                }
            } while (key.Key != ConsoleKey.Enter);
            Console.WriteLine();
            return password;
        }

    }
}

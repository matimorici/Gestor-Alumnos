using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using GestorAlumnos.Models;

namespace GestorAlumnos {
    public static class GestorArchivos {
        // Método para validar el formato del email
        public static bool EsEmailValido(string email) {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            // Expresión regular para validar el formato del email
            string patronEmail = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, patronEmail);
        }

        // Método para solicitar y validar que un input no sea null o vacío
        public static string SolicitarInput(string mensaje) {
            string input;
            do
            {
                Console.Write(mensaje);
                input = Console.ReadLine() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("La entrada no puede estar vacía. Por favor, inténtalo nuevamente.");
                }
            } while (string.IsNullOrWhiteSpace(input));

            return input;
        }

        // Método para solicitar y validar un email
        public static string SolicitarEmail(string mensaje) {
            string email;
            do {
                Console.Write(mensaje);
                email = Console.ReadLine() ?? string.Empty;
                if (!EsEmailValido(email))
                {
                    Console.WriteLine("El email ingresado no es válido. Por favor, inténtalo nuevamente.");
                }
            } while (!EsEmailValido(email));

            return email;
        }

        public static string ObtenerRutaArchivo(string nombreArchivo)
        {
            string rutaCarpeta = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Archivos"
            );

            return Path.Combine(rutaCarpeta, nombreArchivo);
        }

        public static bool VerificarExistenciaArchivo(string rutaCompleta)
        {
            if (!File.Exists(rutaCompleta))
            {
                Console.Write("El archivo no existe. Presiona cualquier tecla para continuar...");
                Console.ReadLine();
                return false;
            }
            return true;
        }

        public static Alumno PedirDatosAlumno(List<Alumno>? alumnos) {
            // Crear una nueva instancia de Alumno
            Alumno alumno = new Alumno();

            // Solicitar y validar el legajo del alumno
            alumno.Legajo = SolicitarInput("Legajo: ");
            // Verificar si el legajo ya existe
            while (alumnos != null && alumnos.Any(a => a.Legajo == alumno.Legajo)) {
                Console.WriteLine("El legajo ya existe. Por favor, ingresa un legajo diferente.");
                alumno.Legajo = SolicitarInput("Legajo: ");
            }

            // Solicitar y validar el apellido del alumno
            alumno.Apellido = SolicitarInput("Apellido: ");

            // Solicitar y validar los nombres del alumno
            alumno.Nombres = SolicitarInput("Nombres: ");

            // Solicitar y validar el numero de documento del alumno
            alumno.NumeroDocumento = SolicitarInput("Número de Documento: ");

            // Solicitar y validar el email del alumno
            alumno.Email = SolicitarEmail("Email: ");

            // Solicitar y validar el teléfono del alumno
            alumno.Telefono = SolicitarInput("Teléfono: ");

            return alumno;
        }

        public static void CrearArchivo() {
            Console.Clear();

            Console.WriteLine("=== CREAR NUEVO ARCHIVO ===");

            //? Validar que no exista otro archivo con el mismo nombre y extensión?
            string nombreArchivo = SolicitarInput("Ingresa el nombre del archivo a crear (sin extensión): ");

            string formatoDestino = SolicitarInput("Ingresa el formato de destino (1- TXT, 2- CSV, 3- JSON, 4-XML): ");
            while (formatoDestino != "1" && formatoDestino != "2" && formatoDestino != "3" && formatoDestino != "4") {
                Console.WriteLine("Formato no válido. Por favor, ingresa 1, 2, 3 o 4.");
                formatoDestino = SolicitarInput("Ingresa el formato de destino (1- TXT, 2- CSV, 3- JSON, 4-XML): ");
            }

            string extension = string.Empty;

            // Procesar la opción seleccionada
            switch (formatoDestino) {
                case "1":
                    extension = "txt";
                    break;
                case "2":
                    extension = "csv";
                    break;
                case "3":
                    extension = "json";
                    break;
                case "4":
                    extension = "xml";
                    break;
            }
            
            // Obtenemos la ruta completa de la carpeta donde se guardan los archivos
            string rutaBuscarCarpeta = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Archivos"
            );

            // Armamos la ruta completa del archivo a buscar
            string rutaBuscarCompleta = Path.Combine(
                rutaBuscarCarpeta,
                nombreArchivo + "." + extension
            );

            // Verificamos si el archivo ya existe
            if (File.Exists(rutaBuscarCompleta)) {
                Console.Write("\nYa existe un archivo con ese nombre y esa extension. Presiona cualquier tecla para volver al menú...");
                Console.ReadKey();
                return;
            }

            Console.Write("Ingrese la cantidad de alumnos a registrar: ");
            int cantidadAlumnos;
            // Validar que la cantidad ingresada sea un número válido mayor que cero
            while (!int.TryParse(Console.ReadLine() ?? string.Empty, out cantidadAlumnos) || cantidadAlumnos <= 0) {
                Console.WriteLine("Por favor, ingresa un número válido mayor que cero.");
            }

            // Crear una lista para almacenar los alumnos
            List<Alumno> alumnos = new List<Alumno>();

            for (int i = 0; i < cantidadAlumnos; i++) {
                Console.WriteLine($"\n--- Ingresa datos del alumno {i + 1} de {cantidadAlumnos} ---");

                var alumno = PedirDatosAlumno(alumnos);

                alumnos.Add(alumno);
            }

            // Definimos el nombre de la carpeta donde se guardarán los archivos
            string carpetaArchivos = "Archivos";

            // Obtenemos la ruta completa de la carpeta
            string rutaCarpeta = Path.Combine(
                Directory.GetCurrentDirectory(),
                carpetaArchivos
            );

            // Si la carpeta no existe, la creamos
            if (!Directory.Exists(rutaCarpeta)) {
                Directory.CreateDirectory(rutaCarpeta);
            }

            // Armamos el nombre completo del archivo con su extensión
            string nombreCompleto = nombreArchivo + "." + extension;

            // Armamos la ruta completa del archivo dentro de la carpeta Archivos
            string rutaCompleta = Path.Combine(
                rutaCarpeta,
                nombreCompleto
            );

            GuardarArchivo(rutaCompleta, alumnos, extension);

            // Una vez que el archivo fue creado correctamente, mostramos un mensaje
            Console.WriteLine("Archivo creado correctamente.");
            Console.WriteLine("Ruta del archivo: ");
            Console.WriteLine(rutaCompleta);

            // Pausa para que el usuario pueda leer el mensaje antes de volver al menú
            Console.Write("Presione una tecla para volver al menú...");
            Console.ReadLine();
            
        }

        public static void GuardarArchivo(string rutaCompleta, List<Alumno> alumnos, string extension) {
            using (StreamWriter writer = new StreamWriter(rutaCompleta)) {
                if (extension == "txt") {
                    foreach (var alumno in alumnos) {
                        writer.WriteLine($"{alumno.Legajo}|{alumno.Apellido}|{alumno.Nombres}|{alumno.NumeroDocumento}|{alumno.Email}|{alumno.Telefono}");
                    }
                } else if (extension == "csv") {
                    writer.WriteLine("Legajo,Apellido,Nombres,NumeroDocumento,Email,Telefono"); // Encabezado
                    foreach (var alumno in alumnos) {
                        writer.WriteLine($"{alumno.Legajo},{alumno.Apellido},{alumno.Nombres},{alumno.NumeroDocumento},{alumno.Email},{alumno.Telefono}");
                    }
                } else if (extension == "json") {
                    string json = JsonSerializer.Serialize(alumnos, new JsonSerializerOptions { WriteIndented = true });
                    writer.Write(json);
                } else if (extension == "xml") {
                    var xml = new XElement("Alumnos",
                        alumnos.Select(alumno =>
                            new XElement("Alumno",
                                new XElement("Legajo", alumno.Legajo),
                                new XElement("Apellido", alumno.Apellido),
                                new XElement("Nombres", alumno.Nombres),
                                new XElement("NumeroDocumento", alumno.NumeroDocumento),
                                new XElement("Email", alumno.Email),
                                new XElement("Telefono", alumno.Telefono)
                            )));
                    writer.Write(xml);
                }
            }
        }

        public static List<Alumno> ParsearArchivo(string rutaCompleta, string extension)
        {
            string[] contenido = File.ReadAllLines(rutaCompleta);

            List<Alumno> alumnos = new List<Alumno>();

            if (extension == ".txt") {
                foreach (var linea in contenido) {
                    string[] partes = linea.Split('|');
                    Alumno a = new Alumno {
                        Legajo = partes[0],
                        Apellido = partes[1],
                        Nombres = partes[2],
                        NumeroDocumento = partes[3],
                        Email = partes[4],
                        Telefono = partes[5]
                    };
                    alumnos.Add(a);
                }
            } else if (extension == ".csv") {
                for (int i = 1; i < contenido.Length; i++) {
                    string linea = contenido[i];
                    string[] partes = linea.Split(',');
                    Alumno a = new Alumno {
                        Legajo = partes[0],
                        Apellido = partes[1],
                        Nombres = partes[2],
                        NumeroDocumento = partes[3],
                        Email = partes[4],
                        Telefono = partes[5]
                    };
                    alumnos.Add(a);
                }
            } else if (extension == ".json") {
                string jsonCompleto = File.ReadAllText(rutaCompleta);
                alumnos = JsonSerializer.Deserialize<List<Alumno>>(jsonCompleto) ?? new List<Alumno>();
            } else if (extension == ".xml") {
                XDocument xmlDoc = XDocument.Load(rutaCompleta);
                alumnos = xmlDoc.Descendants("Alumno")
                .Select(x => new Alumno {
                    Legajo = x.Element("Legajo")?.Value ?? string.Empty,
                    Apellido = x.Element("Apellido")?.Value ?? string.Empty,
                    Nombres = x.Element("Nombres")?.Value ?? string.Empty,
                    NumeroDocumento = x.Element("NumeroDocumento")?.Value ?? string.Empty,
                    Email = x.Element("Email")?.Value ?? string.Empty,
                    Telefono = x.Element("Telefono")?.Value ?? string.Empty
                }).ToList();
            } else {
                Console.WriteLine("Formato no soportado.");
            }

            return alumnos;
        }
            
        public static void LeerArchivo() {
            Console.Clear();

            Console.WriteLine("=== LEER ARCHIVO ===");

            string nombreArchivo = SolicitarInput("Ingresa el nombre del archivo a leer (CON extensión): ");

            // Obtenemos la ruta completa de la carpeta
            string rutaCompleta = ObtenerRutaArchivo(nombreArchivo);

            // Verificamos si el archivo existe
            if (!VerificarExistenciaArchivo(rutaCompleta))
            {
                return;
            }

            // Obtener la extensión del archivo
            string extension = Path.GetExtension(nombreArchivo).ToLower();

            Console.WriteLine($"\n--- Contenido del archivo {nombreArchivo} ---\n");
            
            // Cargar los datos del archivo
            var alumnos = ParsearArchivo(rutaCompleta, extension);

            Console.WriteLine("\n--- ALUMNOS ---");

            if (alumnos.Count == 0) {
                Console.WriteLine("No hay alumnos para mostrar.");
            }
            else {
                Console.WriteLine(new string('=', 120));
                Console.WriteLine("| {0,-10} | {1,-15} | {2,-16} | {3,-20} | {4,-25} | {5,-15} |", "Legajo", "Apellido", "Nombres", "Número Documento", "Email", "Teléfono");
                Console.WriteLine(new string('=', 120));

                // Mostrar los datos de los alumnos en formato tabular
                foreach (var a in alumnos) {
                    Console.WriteLine("| {0,-10} | {1,-15} | {2,-16} | {3,-20} | {4,-25} | {5,-15} |", a.Legajo, a.Apellido, a.Nombres, a.NumeroDocumento, a.Email, a.Telefono);
                }

                Console.WriteLine(new string('=', 120));
            }

            // Pausa para que el usuario pueda leer el contenido antes de volver al menú
            Console.Write("\nPresione una tecla para volver al menú...");
            Console.ReadLine();
        }

        public static void AgregarAlumno(List<Alumno> alumnos) {
            Console.WriteLine("\n=== AGREGAR NUEVO ALUMNO ===");

            // Ingreso de datos del nuevo alumno
            var alumno = PedirDatosAlumno(alumnos);

            // Agregar el nuevo alumno a la lista
            alumnos.Add(alumno);
            Console.WriteLine("\nAlumno agregado correctamente.");
            Console.Write("\nPresione una tecla para continuar...");
            Console.ReadKey();
            Console.Clear();
        }

        public static void ModificarAlumno(List<Alumno> alumnos) {
            Console.WriteLine("\n=== MODIFICAR ALUMNO ===");

            // Solicitar el legajo del alumno a modificar
            string legajo = SolicitarInput("\n1Ingresa el legajo del alumno a modificar: ");

            // Buscar el alumno por legajo
            var alumno = alumnos.FirstOrDefault(a => a.Legajo == legajo);
            if (alumno == null) {
                Console.WriteLine("\nNo se encontró un alumno con ese legajo.");
            } else {
                Console.WriteLine($"\nApreta enter para mantener el valor actual o ingresa un nuevo valor para modificar los datos del alumno con legajo {alumno.Legajo}.");

                // Solicitar nuevos datos
                Console.Write($"Apellido ({alumno.Apellido}): ");
                string nuevoApellido = Console.ReadLine() ?? string.Empty;
                alumno.Apellido = string.IsNullOrWhiteSpace(nuevoApellido) ? alumno.Apellido : nuevoApellido;

                Console.Write($"Nombres ({alumno.Nombres}): ");
                string nuevosNombres = Console.ReadLine() ?? string.Empty;
                alumno.Nombres = string.IsNullOrWhiteSpace(nuevosNombres) ? alumno.Nombres : nuevosNombres;

                Console.Write($"Número de Documento ({alumno.NumeroDocumento}): ");
                string nuevoDocumento = Console.ReadLine() ?? string.Empty;
                alumno.NumeroDocumento = string.IsNullOrWhiteSpace(nuevoDocumento) ? alumno.NumeroDocumento : nuevoDocumento;

                Console.Write($"Email ({alumno.Email}): ");
                string nuevoEmail = Console.ReadLine() ?? string.Empty;
                alumno.Email = string.IsNullOrWhiteSpace(nuevoEmail) ? alumno.Email : nuevoEmail;
                while (!EsEmailValido(alumno.Email)) {
                    Console.Write("El email ingresado no es válido. Por favor, ingresa un email válido: ");
                    nuevoEmail = Console.ReadLine() ?? string.Empty;
                    alumno.Email = string.IsNullOrWhiteSpace(nuevoEmail) ? alumno.Email : nuevoEmail;
                }

                Console.Write($"Teléfono ({alumno.Telefono}): ");
                string nuevoTelefono = Console.ReadLine() ?? string.Empty;
                alumno.Telefono = string.IsNullOrWhiteSpace(nuevoTelefono) ? alumno.Telefono : nuevoTelefono;

                Console.WriteLine("\nAlumno modificado correctamente.");

            }
            Console.Write("\nPresione una tecla para continuar...");
            Console.ReadKey();
            Console.Clear();
        }

        public static void EliminarAlumno(List<Alumno> alumnos) {
            Console.WriteLine("\n=== ELIMINAR ALUMNO ===");

            // Solicitar el legajo del alumno a eliminar
            string legajo = SolicitarInput("\nIngresa el legajo del alumno a eliminar: ");

            // Buscar el alumno por legajo
            var alumno = alumnos.FirstOrDefault(a => a.Legajo == legajo);

            if (alumno == null) {
                Console.WriteLine("\nNo se encontró un alumno con ese legajo.");
                return;
            } else {
                // Eliminar el alumno de la lista
                Console.WriteLine("\n-- Información del alumno a eliminar --");
                Console.WriteLine($"Legajo: {alumno.Legajo}");
                Console.WriteLine($"Apellido: {alumno.Apellido}");
                Console.WriteLine($"Nombres: {alumno.Nombres}");
                Console.WriteLine($"Número de Documento: {alumno.NumeroDocumento}");
                Console.WriteLine($"Email: {alumno.Email}");
                Console.WriteLine($"Teléfono: {alumno.Telefono}");

                string respuesta = SolicitarInput("\n¿Estás seguro que deseas eliminar al alumno? (CONFIRMAR/CANCELAR): ");
                while (respuesta != "CONFIRMAR" && respuesta != "CANCELAR")
                {
                    Console.WriteLine("Respuesta no válida.");
                    respuesta = SolicitarInput("¿Estás seguro que deseas eliminar al alumno? (CONFIRMAR/CANCELAR): ");
                }

                if (respuesta == "CONFIRMAR") {
                    alumnos.Remove(alumno);
                    Console.WriteLine("\nAlumno eliminado correctamente.");
                } else {
                    Console.WriteLine("\nOperación cancelada. El alumno no fue eliminado.");
                    return;
                }

                Console.Write("\nPresione una tecla para continuar...");
                Console.ReadKey();
            }
            

        }

        public static void ModificarArchivo() {
            // Implementar la lógica para modificar un archivo existente
            Console.Clear();
            Console.WriteLine("=== MODIFICAR ARCHIVO ===");
            
            Console.Write("Ingresa el nombre del archivo a modificar (CON extensión): ");
            string nombreArchivo = Console.ReadLine() ?? string.Empty;

            // Obtenemos la ruta completa de la carpeta
            string rutaCompleta = ObtenerRutaArchivo(nombreArchivo);

            // Verificamos si el archivo existe
            if (!VerificarExistenciaArchivo(rutaCompleta)) {
                return;
            }

            // Obtener la extensión del archivo
            string extension = Path.GetExtension(nombreArchivo).ToLower();

            // Cargar los datos del archivo
            var alumnos = GestorArchivos.ParsearArchivo(rutaCompleta, extension);

            bool salirModificacion = false;

            while (!salirModificacion) {
                // Mostrar las opciones del menú
                Console.WriteLine("\n=== OPCIONES DE MODIFICACIÓN ===");
                Console.WriteLine("1. Agregar nuevo alumno");
                Console.WriteLine("2. Modificar alumno existente (por legajo)");
                Console.WriteLine("3. Eliminar alumno (por legajo)");
                Console.WriteLine("4. Guardar y salir");
                Console.WriteLine("5. Cancelar sin guardar");

                // Leer la opción del usuario
                Console.Write("Selecciona una opción: ");
                string opcion = Console.ReadLine() ?? string.Empty;
                while (opcion != "1" && opcion != "2" && opcion != "3" && opcion != "4" && opcion != "5") {
                    Console.Write("Por favor, ingresa una opción válida: ");
                    opcion = Console.ReadLine() ?? string.Empty;
                }

                // Procesar la opción seleccionada
                switch (opcion) {
                    case "1":
                        AgregarAlumno(alumnos);
                        break;
                    case "2":
                        ModificarAlumno(alumnos);
                        break;
                    case "3":
                        EliminarAlumno(alumnos);
                        break;
                    case "4":
                        // Generar un nombre de archivo de respaldo con marca de tiempo
                        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                        string backupRuta = rutaCompleta + $"_{timestamp}.bak";

                        // Crear el archivo de respaldo
                        File.Copy(rutaCompleta, backupRuta);

                        // Guardar los cambios en el archivo original
                        GuardarArchivo(rutaCompleta, alumnos, extension.TrimStart('.'));
                        Console.WriteLine("\nCambios guardados correctamente.");
                        salirModificacion = true;
                        break;
                    case "5":
                        Console.WriteLine("\nModificaciones canceladas. No se guardaron los cambios.");
                        salirModificacion = true;
                        break;
                }

                Console.Write("Presione una tecla para volver al menú...");
                Console.ReadKey();
            }
        }

        public static void EliminarArchivo() {
            Console.Clear();

            Console.WriteLine("=== ELIMINAR ARCHIVO ===");

            string nombreArchivo = SolicitarInput("Ingresa el nombre del archivo a eliminar (con extensión): ");

            // Obtenemos la ruta completa de la carpeta
            string rutaCompleta = ObtenerRutaArchivo(nombreArchivo);

            // Verificamos si el archivo existe
            if (!VerificarExistenciaArchivo(rutaCompleta)) {
                return;
            }

            // La clase FileInfo permite acceder a propiedades como nombre, tamaño y fechas del archivo
            FileInfo infoArchivo = new FileInfo(rutaCompleta);

            Console.Write("\n-- Información del archivo --");
            Console.WriteLine($"Nombre: {infoArchivo.Name}");
            Console.WriteLine($"Tamaño: {infoArchivo.Length / 1024.0:F2} KB");
            Console.WriteLine($"Fecha de creación: {infoArchivo.CreationTime}");
            Console.WriteLine($"Última modificación: {infoArchivo.LastWriteTime}");

            string respuesta;

            Console.Write("\n¿Estás seguro que deseas eliminar este archivo? (CONFIRMAR/CANCELAR): ");
            respuesta = Console.ReadLine() ?? string.Empty;

            while (respuesta != "CONFIRMAR" && respuesta != "CANCELAR")
            {
                Console.WriteLine("Respuesta no válida. Por favor, ingresa CONFIRMAR o CANCELAR.");
                Console.Write("¿Estás seguro que deseas eliminar este archivo? (CONFIRMAR/CANCELAR): ");
                respuesta = Console.ReadLine() ?? string.Empty;
            }

            if (respuesta == "CONFIRMAR")
            {
                File.Delete(rutaCompleta);
                Console.WriteLine("Archivo eliminado correctamente.");
            }
            else
            {
                Console.WriteLine("Operación cancelada. El archivo no fue eliminado.");
            }


            // Pausa para que el usuario pueda leer el mensaje antes de volver al menú
            Console.Write("Presione una tecla para volver al menú...");
            Console.ReadLine();
        
        }
            
    }


}

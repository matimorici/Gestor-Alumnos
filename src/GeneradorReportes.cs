using System;

namespace GestorAlumnos {
    public static class GeneradorReportes{

        public static void CrearReporteCorteControl() {
            Console.Clear();
            Console.WriteLine("=== CREAR REPORTE CON CORTE DE CONTROL DE UN NIVEL ===");

            string nombreArchivo = GestorArchivos.SolicitarInput("Ingresa el nombre del archivo de interes (CON extensión): ");

            // Obtenemos la ruta completa de la carpeta
            string rutaCarpeta = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Archivos"
            );

            // Armamos la ruta completa del archivo dentro de la carpeta Archivos
            string rutaCompleta = GestorArchivos.ObtenerRutaArchivo(nombreArchivo);
            if (!GestorArchivos.VerificarExistenciaArchivo(rutaCompleta))
            {
                return;
            }

            // Obtener la extensión del archivo
            string extension = Path.GetExtension(nombreArchivo).ToLower();

            // Parsear el archivo para obtener la lista de alumnos
            var alumnos = GestorArchivos.ParsearArchivo(rutaCompleta, extension);

            // Validar que contenga datos
            if (alumnos.Count == 0) {
                Console.WriteLine("El archivo no contiene datos de alumnos.");
            } else {
                alumnos = alumnos
                    .OrderBy(a => a.Apellido)
                    .ToList();

                // Realizar el corte de control de un nivel (por Apellido)
                var grupos = alumnos
                    .GroupBy(a => a.Apellido);

                // Mostrar el reporte en consola
                Console.WriteLine(new string('=', 40));
                Console.WriteLine("REPORTE DE ALUMNOS POR APELLIDO".PadRight(10));
                Console.WriteLine("Fecha: " + DateTime.Now.ToString("G"));
                Console.WriteLine(new string('=', 40));
                foreach (var grupo in grupos) {
                    Console.WriteLine($"APELLIDO: {grupo.Key}");
                    Console.WriteLine(new string('-', 40));
                    foreach (var alumno in grupo) {
                        Console.WriteLine($"Legajo: {alumno.Legajo}");
                        Console.WriteLine($"Nombres: {alumno.Nombres}");
                        Console.WriteLine($"Documento: {alumno.NumeroDocumento}");
                        Console.WriteLine($"Email: {alumno.Email}");
                        Console.WriteLine($"Teléfono: {alumno.Telefono}\n");
                    }
                    Console.WriteLine($"→ Subtotal '{grupo.Key}': {grupo.Count()} alumno(s)\n");
                }
                Console.WriteLine(new string('=', 40));
                Console.WriteLine("RESUMEN GENERAL".PadRight(15));
                Console.WriteLine(new string('=', 40));
                Console.WriteLine($"Total de Apellidos diferentes: {grupos.Count()}");
                Console.WriteLine($"Total de alumnos registrados: {alumnos.Count}");
                Console.WriteLine(new string('=', 40));

                // Preguntar si desea guardar el reporte en un archivo de texto
                string guardarOpcion;

                Console.WriteLine("\nDesea guardar este reporte en un archivo de texto? (CONFIRMAR/CANCELAR): ");
                guardarOpcion = Console.ReadLine() ?? string.Empty;
                while (guardarOpcion != "CONFIRMAR" && guardarOpcion != "CANCELAR") {
                    Console.WriteLine("Opción no válida. Por favor ingresa 'CONFIRMAR' para Sí o 'CANCELAR' para No: ");
                    guardarOpcion = Console.ReadLine() ?? string.Empty;
                }

                if (guardarOpcion == "CONFIRMAR") {
                    // Obtener el nombre del archivo sin extensión
                    string nombreArchivoSinExtension = Path.GetFileNameWithoutExtension(nombreArchivo);

                    // Generar el nombre del archivo del reporte
                    string nombreArchivoReporte = "Reporte_" + nombreArchivoSinExtension + "_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".txt";
                    string rutaArchivoReporte = GestorArchivos.ObtenerRutaArchivo(nombreArchivoReporte);

                    //! No se como parametrizar este write.WriteLine con el Console.WriteLine de arriba para no repetir codigo
                    // Guardar el reporte en el archivo de texto
                    using (StreamWriter writer = new StreamWriter(rutaArchivoReporte)) {
                        writer.WriteLine(new string('=', 40));
                        writer.WriteLine("REPORTE DE ALUMNOS POR APELLIDO".PadRight(10));
                        writer.WriteLine("Fecha: " + DateTime.Now.ToString("G"));
                        writer.WriteLine(new string('=', 40));
                        foreach (var grupo in grupos) {
                            writer.WriteLine($"APELLIDO: {grupo.Key}");
                            writer.WriteLine(new string('-', 40));
                            foreach (var alumno in grupo) {
                                writer.WriteLine($"Legajo: {alumno.Legajo}");
                                writer.WriteLine($"Nombres: {alumno.Nombres}");
                                writer.WriteLine($"Documento: {alumno.NumeroDocumento}");
                                writer.WriteLine($"Email: {alumno.Email}");
                                writer.WriteLine($"Teléfono: {alumno.Telefono}");
                            }
                            writer.WriteLine($"\n→ Subtotal '{grupo.Key}': {grupo.Count()} alumno(s)\n");
                        }
                        writer.WriteLine(new string('=', 40));
                        writer.WriteLine("RESUMEN GENERAL".PadRight(15));
                        writer.WriteLine(new string('=', 40));
                        writer.WriteLine($"Total de Apellidos diferentes: {grupos.Count()}");
                        writer.WriteLine($"Total de alumnos registrados: {alumnos.Count}");
                        writer.WriteLine(new string('=', 40));
                    }

                    // Confirmación de guardado
                    Console.WriteLine($"Reporte guardado exitosamente en: {rutaArchivoReporte}");
                }

            }

            Console.WriteLine("\nPresiona cualquier tecla para volver al menú...");
            Console.ReadLine();
        }
    }

}
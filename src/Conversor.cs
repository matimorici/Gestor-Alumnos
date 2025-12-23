using System;

namespace GestorAlumnos {
    public static class Conversor{
        public static void ConvertirFormatos() {
            Console.Clear();
            Console.WriteLine("=== CONVERTIR FORMATOS ===");

            string nombreArchivo = GestorArchivos.SolicitarInput("Ingresa el nombre del archivo a convertir (CON extensión): ");

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

            // Declarar la variables fuera del while para que esté disponible en todo el método
            string rutaCompletaNueva = string.Empty;
            string extensionNueva = string.Empty;
            string nombreArchivoNuevo = string.Empty;

            bool bandera = false;

            while(bandera == false) {
                Console.WriteLine($"\nFormato actual del archivo: {extension}");
                Console.WriteLine("Formatos disponibles para convertir:");
                switch (extension) {
                    case ".txt":
                        Console.WriteLine("1. CSV (.csv)");
                        Console.WriteLine("2. JSON (.json)");
                        Console.WriteLine("3. XML (.xml)");
                        break;
                    case ".json":
                        Console.WriteLine("1. TXT (.txt)");
                        Console.WriteLine("2. CSV (.csv)");
                        Console.WriteLine("3. XML (.xml)");
                        break;
                    case ".csv":
                        Console.WriteLine("1. TXT (.txt)");
                        Console.WriteLine("2. JSON (.json)");
                        Console.WriteLine("3. XML (.xml)");
                        break;
                    case ".xml":
                        Console.WriteLine("1. TXT (.txt)");
                        Console.WriteLine("2. CSV (.csv)");
                        Console.WriteLine("3. JSON (.json)");
                        break;
                }

                string opExtension;

                Console.Write("Selecciona el formato al que deseas convertir (ingresa el número): ");
                opExtension = Console.ReadLine() ?? string.Empty;
                while (opExtension != "1" && opExtension != "2" && opExtension != "3") {
                    Console.WriteLine("\nOpción no válida.");
                    Console.Write("Selecciona el formato al que deseas convertir (ingresa el número): ");
                    opExtension = Console.ReadLine() ?? string.Empty;
                }

                // Determinar la nueva extensión basada en la opción seleccionada
                if (extension == ".txt"){
                    switch (opExtension) {
                        case "1":
                            extensionNueva = "csv";
                            break;
                        case "2":
                            extensionNueva = "json";
                            break;
                        case "3":
                            extensionNueva = "xml";
                            break;
                    }
                } else if (extension == ".json") {
                    switch (opExtension) {
                        case "1":
                            extensionNueva = "txt";
                            break;
                        case "2":
                            extensionNueva = "csv";
                            break;
                        case "3":
                            extensionNueva = "xml";
                            break;
                    }
                } else if (extension == ".csv") {
                    switch (opExtension) {
                        case "1":
                            extensionNueva = "txt";
                            break;
                        case "2":
                            extensionNueva = "json";
                            break;
                        case "3":
                            extensionNueva = "xml";
                            break;
                    }
                } else if (extension == ".xml") {
                    switch (opExtension) {
                        case "1":
                            extensionNueva = "txt";
                            break;
                        case "2":
                            extensionNueva = "csv";
                            break;
                        case "3":
                            extensionNueva = "json";
                            break;
                    }
                }

                // Solicitar el nuevo nombre para el archivo convertido
                nombreArchivoNuevo = GestorArchivos.SolicitarInput("\nIngresa el NUEVO nombre del archivo convertido (SIN extensión): ");
                
                // Armamos la ruta completa del nuevo archivo dentro de la carpeta Archivos
                rutaCompletaNueva = Path.Combine(rutaCarpeta, nombreArchivoNuevo + "." + extensionNueva);

                // Verificamos si el archivo ya existe
                if (File.Exists(rutaCompletaNueva)) {
                    Console.Write("\nYa existe un archivo con ese nombre y esa extension...");
                    Console.ReadKey();
                } else {
                    bandera = true; 
                }
            } 

            // Cargamos los datos del archivo original
            var alumnos = GestorArchivos.ParsearArchivo(rutaCompleta, extension);

            // Guardamos los datos en el nuevo formato
            GestorArchivos.GuardarArchivo(rutaCompletaNueva, alumnos, extensionNueva);

            Console.WriteLine("\nArchivo convertido y guardado exitosamente.");
            Console.WriteLine($"Archivo origen: {nombreArchivo} ({alumnos.Count} registros)");
            Console.WriteLine($"Archivo destino: {nombreArchivoNuevo + "." + extensionNueva} ({alumnos.Count} registros)");
            Console.WriteLine("\nPresiona cualquier tecla para volver al menú...");
            Console.ReadLine();
        }
    }

}
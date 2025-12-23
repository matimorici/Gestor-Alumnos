using System;

namespace GestorAlumnos
{
    public static class Menu {
        public static void MostrarMenu() {

            // Variable para controlar la salida del menú
            bool salir = false;

            while (!salir) {

                // Limpiar la consola
                Console.Clear();
                
                // Mostrar las opciones del menú
                Console.WriteLine("=== GESTOR DE ARCHIVOS DE TEXTO ===");
                Console.WriteLine("1. Crear nuevo archivo");
                Console.WriteLine("2. Leer archivo existente");
                Console.WriteLine("3. Modificar archivo existente");
                Console.WriteLine("4. Eliminar archivo");
                Console.WriteLine("5. Convertir entre formatos");
                Console.WriteLine("6. Crear Reporte con Corte de control de un nivel");
                Console.WriteLine("0. Salir");

                // Leer la opción del usuario
                Console.Write("Selecciona una opción: ");
                string opcion = Console.ReadLine() ?? string.Empty;
                while (opcion != "1" && opcion != "2" && opcion != "3" && opcion != "4" && opcion != "5" && opcion != "6" && opcion != "0") {
                    Console.Write("Por favor, ingresa una opción válida: ");
                    opcion = Console.ReadLine() ?? string.Empty;
                }

                // Procesar la opción seleccionada
                switch (opcion) {
                    case "1":
                        GestorArchivos.CrearArchivo();
                        break;
                    case "2":
                        GestorArchivos.LeerArchivo();
                        break;
                    case "3":
                        GestorArchivos.ModificarArchivo();
                        break;
                    case "4":
                        GestorArchivos.EliminarArchivo();
                        break;
                    case "5":
                        Conversor.ConvertirFormatos();
                        break;
                    case "6":
                        GeneradorReportes.CrearReporteCorteControl();
                        break;
                    case "0":
                        salir = true;
                        break;
                }


            }
            
        }

    }

}
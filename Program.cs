using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

class Program
{
    //Mostrar -> Esperar -> LLamar -> Mostrar
    static async Task Main(string[] args)
    {
        // URL base del servidor Flask; se debe actualizar con la IP real en la red local
        string baseUrl = "http://127.0.0.1:5000";

        // Bucle principal: muestra el menú y espera la selección del usuario
        while (true)
        {
            MostrarMenu();
            var opcion = Console.ReadLine();

    
            using (var client = new HttpClient())
            {
                switch (opcion)
                {
                    case "1":
                        await ObtenerTareas(client, baseUrl); // Ver todas las tareas
                        break;
                    case "2":
                        await CrearTarea(client, baseUrl); // Crear una nueva tarea
                        break;
                    case "3":
                        await ObtenerTareaPorId(client, baseUrl); // Obtener tarea por ID
                        break;
                    case "4":
                        await ActualizarTarea(client, baseUrl); // Actualizar tarea por ID
                        break;
                    case "5":
                        await EliminarTarea(client, baseUrl); // Eliminar tarea por ID
                        break;
                    case "6":
                        Console.WriteLine("Saliendo...");
                        return; //rompe 
                    default:
                        Console.WriteLine("Opción inválida.");
                        break;
                }
            }
        }
    }

    // Funcion para imprimir el menu al ser llamado por el main
    static void MostrarMenu()
    {
        Console.WriteLine("\n===== MENÚ CRUD =====");
        Console.WriteLine("1. Ver todas las tareas");
        Console.WriteLine("2. Crear tarea");
        Console.WriteLine("3. Obtener tarea por ID");
        Console.WriteLine("4. Actualizar tarea");
        Console.WriteLine("5. Eliminar tarea");
        Console.WriteLine("6. Salir");
        Console.Write("Seleccione opción: ");
    }

    // Funcion para el GET de todas las tareas
    static async Task ObtenerTareas(HttpClient client, string baseUrl)
    {
        try
        {
            HttpResponseMessage response = await client.GetAsync($"{baseUrl}/tareas");
            string respuesta = await response.Content.ReadAsStringAsync();
            Console.WriteLine("\nTareas:");
            Console.WriteLine(respuesta);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    // Funcion POST solicita y valida 3 datos
    static async Task CrearTarea(HttpClient client, string baseUrl)
    {
        Console.Write("Título de la tarea: ");
        string titulo = Console.ReadLine();

        Console.Write("¿Está completada? (true/false): ");
        if (!bool.TryParse(Console.ReadLine(), out bool completada))
        {
            Console.WriteLine("Entrada inválida."); 
            return;
        }

        Console.Write("Fecha (DD-MM-YYYY): ");
        string fechaInput = Console.ReadLine();
        if (!DateTime.TryParseExact(fechaInput, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime fecha))
        {
            Console.WriteLine("Formato inválido.");
            return;
        }

        // Convierte la fecha a formato YYYY-MM-DD para el servidor
        string fechaStr = fecha.ToString("yyyy-MM-dd");

        // Serializa los datos de la tarea a JSON
        string json = JsonSerializer.Serialize(new { titulo, completada, fecha = fechaStr });
        var contenido = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            HttpResponseMessage response = await client.PostAsync($"{baseUrl}/tarea", contenido);
            string respuesta = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Respuesta: {respuesta}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    // Funcion GET para ID y valida que no sea nulo
    static async Task ObtenerTareaPorId(HttpClient client, string baseUrl)
    {
        Console.Write("ID de la tarea: ");
        string id = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(id))
        {
            Console.WriteLine("ID inválido.");
            return;
        }

        try
        {
            HttpResponseMessage response = await client.GetAsync($"{baseUrl}/tarea/{id}");
            string respuesta = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Tarea ID {id}: {respuesta}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    // Solicita ID  valida y actualiza titulo, estado, fecha
    static async Task ActualizarTarea(HttpClient client, string baseUrl)
    {
        Console.Write("ID a actualizar: ");
        string id = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(id))
        {
            Console.WriteLine("ID inválido.");
            return;
        }

        Console.Write("Nuevo título: ");
        string titulo = Console.ReadLine();

        Console.Write("¿Está completada? (true/false): ");
        if (!bool.TryParse(Console.ReadLine(), out bool completada))
        {
            Console.WriteLine("Entrada inválida.");
            return;
        }

        Console.Write("Nueva fecha (DD-MM-YYYY): ");
        string fechaInput = Console.ReadLine();
        if (!DateTime.TryParseExact(fechaInput, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime fecha))
        {
            Console.WriteLine("Formato inválido.");
            return;
        }
        string fechaStr = fecha.ToString("yyyy-MM-dd");

        // Serializa los nuevos datos de la tarea a JSON
        string json = JsonSerializer.Serialize(new { titulo, completada, fecha = fechaStr });
        var contenido = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            HttpResponseMessage response = await client.PutAsync($"{baseUrl}/tarea/{id}", contenido);
            string respuesta = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Respuesta: {respuesta}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    // Permite eliminar una tarea especificando su ID
    static async Task EliminarTarea(HttpClient client, string baseUrl)
    {
        Console.Write("ID a eliminar: ");
        string id = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(id))
        {
            Console.WriteLine("ID inválido.");
            return;
        }

        try
        {
            HttpResponseMessage response = await client.DeleteAsync($"{baseUrl}/tarea/{id}");
            string respuesta = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Respuesta: {respuesta}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}

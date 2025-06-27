using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

class Program
{
    static readonly HttpClient client = new HttpClient();
    static readonly string baseUrl = "http://localhost:5000";
    static async Task Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("\n===== MENÚ CRUD =====");
            Console.WriteLine("1. Ver todas las tareas");
            Console.WriteLine("2. Crear tarea");
            Console.WriteLine("3. Obtener tarea por ID");
            Console.WriteLine("4. Actualizar tarea");
            Console.WriteLine("5. Eliminar tarea");
            Console.WriteLine("6. Salir");
            Console.Write("Seleccione opción: ");
            var opcion = Console.ReadLine();

            switch (opcion)
            {
                case "1":
                    await ObtenerTareas();
                    break;
                case "2":
                    await CrearTarea();
                    break;
                case "3":
                    await ObtenerTareaPorId();
                    break;
                case "4":
                    await ActualizarTarea();
                    break;
                case "5":
                    await EliminarTarea();
                    break;
                case "6":
                    Console.WriteLine("Saliendo...");
                    return;
                default:
                    Console.WriteLine("Opción inválida.");
                    break;
            }
        }
    }

    static async Task ObtenerTareas()
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

    static async Task CrearTarea()
    {
        Console.Write("Título de la tarea: ");
        string titulo = Console.ReadLine();

        Console.Write("¿Está completada? (true/false): ");
        if (!bool.TryParse(Console.ReadLine(), out bool completada))
        {
            Console.WriteLine("Entrada inválida, debe ser true o false.");
            return;
        }

        Console.Write("Fecha (DD-MM-YYYY): ");
        string fechaInput = Console.ReadLine();
        if (!DateTime.TryParseExact(fechaInput, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime fecha))
        {
            Console.WriteLine("Formato de fecha inválido. Use DD-MM-YYYY.");
            return;
        }
        string fechaStr = fecha.ToString("yyyy-MM-dd");

        var nuevaTarea = new
        {
            titulo,
            completada,
            fecha = fechaStr
        };

        string json = JsonSerializer.Serialize(nuevaTarea);
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

    static async Task ObtenerTareaPorId()
    {
        Console.Write("Ingrese el ID de la tarea: ");
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
            Console.WriteLine($"Tarea ID {id}:");
            Console.WriteLine(respuesta);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static async Task ActualizarTarea()
    {
        Console.Write("ID de la tarea a actualizar: ");
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
            Console.WriteLine("Entrada inválida, debe ser true o false.");
            return;
        }

        Console.Write("Nueva fecha (DD-MM-YYYY): ");
        string fechaInput = Console.ReadLine();
        if (!DateTime.TryParseExact(fechaInput, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime fecha))
        {
            Console.WriteLine("Formato de fecha inválido. Use DD-MM-YYYY.");
            return;
        }
        string fechaStr = fecha.ToString("yyyy-MM-dd");

        var tareaActualizada = new
        {
            titulo,
            completada,
            fecha = fechaStr
        };

        string json = JsonSerializer.Serialize(tareaActualizada);
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

    static async Task EliminarTarea()
    {
        Console.Write("ID de la tarea a eliminar: ");
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

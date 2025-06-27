# Cliente CRUD C# para API Flask

Este proyecto es una aplicación de consola en C# que permite consumir una API REST (por ejemplo, hecha en Flask) para gestionar tareas mediante operaciones CRUD (Crear, Leer, Actualizar, Eliminar).

## Características

- Ver todas las tareas (`GET /tareas`)
- Crear una tarea (`POST /tarea`)
- Obtener tarea por ID (`GET /tarea/{id}`)
- Actualizar tarea (`PUT /tarea/{id}`)
- Eliminar tarea (`DELETE /tarea/{id}`)

## Requisitos

- .NET SDK 9.0 o superior
- Tener la API Flask corriendo en `http://localhost:5000`

## Uso

1. Clona o descarga este repositorio.
2. Abre una terminal en la carpeta del proyecto.
3. Ejecuta el cliente con:

   ```sh
   dotnet run
   ```

4. Sigue el menú interactivo para realizar operaciones CRUD.

## Notas

- El campo `fecha` debe ingresarse como `DD-MM-YYYY` y será convertido automáticamente al formato `YYYY-MM-DD` requerido por la API.
- Asegúrate de que el servidor Flask esté activo antes de usar

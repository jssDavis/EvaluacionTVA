# Proyecto ASP.NET MVC - Evaluación - Gestión de Ventas. Candidato: Jesús David Anastacio Vázquez.

## Descripción

Este proyecto es una aplicación web de gestión de ventas utilizando el framework ASP.NET MVC con .NET Framework. 

## Requisitos

Para ejecutar este proyecto, necesitas:
- Visual Studio con soporte para ASP.NET MVC.
- .NET Framework.
- Una base de datos Oracle con las tablas y vistas necesarias.
- Los scripts se encuentran en el repositorio

## Estructura del Proyecto

El proyecto contiene los siguientes elementos clave:
- **Modelos**: Representan las tablas y vistas de la base de datos (Clientes, Productos, Ventas, Detalle de Ventas).
- **Controladores**: Manejan la lógica de negocio y consultas a la base de datos.
- **Vistas**: Presentan la interfaz gráfica al usuario, como la lista de ventas y los detalles.

## Configuración

1. Configura la conexión a la base de datos Oracle en el archivo `Web.config` en la sección `<connectionStrings>`.
   
   Ejemplo de configuración:

   ```xml
   <connectionStrings>
       <add name="OracleDbContext" 
            connectionString="Data Source=localhost;User Id=tu_usuario;Password=tu_contraseña;" 
            providerName="Oracle.ManagedDataAccess.Client" />
   </connectionStrings>

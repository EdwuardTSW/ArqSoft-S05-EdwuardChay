# CitasApp

CitasApp es una aplicacion web para consultar informacion basica de un sistema de citas medicas. Permite organizar los datos principales relacionados con pacientes, medicos y citas dentro de una estructura sencilla.

## Arquitectura

El proyecto utiliza el patron MVC, separando la aplicacion en modelos, vistas y controladores. Los modelos representan las entidades principales del sistema, las vistas muestran la informacion al usuario y los controladores coordinan las solicitudes y respuestas.

Tambien se aplica el patron Repository mediante interfaces y clases concretas. Esto separa el acceso a datos de la logica de los controladores, permitiendo que la informacion almacenada en archivos JSON pueda consultarse de forma ordenada y mantenible.

## Descripcion general

La aplicacion esta organizada por capas simples, lo que facilita entender la responsabilidad de cada parte del sistema. Los datos se encuentran en archivos JSON dentro de la carpeta `data`, mientras que los repositorios se encargan de leerlos y entregarlos a los controladores para ser mostrados en las vistas.

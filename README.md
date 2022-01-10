# Pasos para crear la app con conexion a una BD
> Este proyecto es para dejar medianamente claros los pasos para utilizar bd en las apps

## Para MySql

> Se utiliza dotnet ef 

Para crear contexto y modelos de las tablas de una BD existente:
- `dotnet ef dbcontext scaffold {connectionstring} "Pomelo.EntityFrameworkCore.MySql"`
  - En Connecitionstring lo q va es algo similar a `"Server=localhost;User=root;Password=1234;Database=ef"`
  - Ese comando si todo sale bien te crea todo ese en el directorio actual
  - Requiere la herramienta **dotnet-ef** y los paquetes **Pomelo.EntityFrameworkCore.MySql** **Microsoft.EntityFrameworkCore.Design**


Para generar controladores de modelos y context ya creados se utiliza:
- `dotnet-aspnet-codegenerator controller [-tfm net60] -m <modelo> -name <ControllerName> -dc <contexto de DB> [-outDir Controllers/] -async -api `
  - Crea el controlador en el directorio actual o en la outDir especificada
  - Requiere la tool **dotnet-aspnet-codegenerator** y el paquete **Microsoft.VisualStudio.Web.CodeGeneration.Design**

### Configuracion
Lugo de esto hay q crear el ConnectionString en *appsettings.json* que quedaria similar a:

    "ConnectionStrings": {
        "DDatabase": "server=localhost;user=root;password=12345678;database=testDB"
    },

Seguidamente se configra el servicio para **DI** en *Program.cs*

    //Añadir al principio dl todo la linea signte
    using Microsoft.EntityFrameworkCore;


    var connectionString = builder.Configuration.GetConnectionString("DDatabase");

    var serverVersion = new MySqlServerVersion(new Version(8, 0, 27));

    builder.Services.AddDbContext<DBContext>(opt =>
        opt.UseMySql(connectionString, serverVersion));

**IMPORTANTE** Al final se debe eliminar la funcion OnConfiguring() de la clase *Contexto* pues ya se configuro por otra via

Luego se llama en la clase q se requiera a traves de DI el Context por el Constructor

## Para MongoDB

> No se aun como hacer scafolding pero marco los paquetes necesarios

Paquetes necesarios:
- MongoDB.Bson
- MongoDB.Driver

### Configuracion
Se crean los modelos manualmente con cuidado de marcar al atributo identificador de la siguiente manera:

    // Agregar estas lineas al inicio del todo
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

Luego se crea el Contexto o Settings de la BD con una clase similar a esta:

    public class BlogDatabaseSettings
    {
        public string? ConnectionString { get; set; }
        public string? DatabaseName { get; set; }
        public string? CollectionName { get; set; }
    }
    
En el *appsettings.json* se configura las opciones de conexion asi:

    "DatabaseS": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "dbName",
    "CollectionName": "collection"
    },

Por ahora hay q crear un DBSettings y una entrada en *appsettings.json* por cada colleccion a utilizar

Luego se configura el servicio q consume la DB, por ejemplo BlogService en la carpeta Services

    // Esto va dentro de la clase BlogService q contine todo el CRUD ademas
    // Poner estas 2 linea arriba del todo
    using MongoDB.Driver;
    using Microsoft.Extensions.Options;
    
    private readonly IMongoCollection<Blog> _blogCollection;

        public BlogService(IOptions<BlogDatabaseSettings> settings)
        {
            var mongoClient = new MongoClient(
            settings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                settings.Value.DatabaseName);

            _blogCollection = mongoDatabase.GetCollection<Blog>(
                settings.Value.CollectionName);
        }

Lugo se configura el servicio en Program.cs

    builder.Services.Configure<Models.BlogDatabaseSettings>(
    builder.Configuration.GetSection("DatabaseS"));

    builder.Services.AddSingleton<Services.BlogService>();
var builder = DistributedApplication.CreateBuilder(args);

var sqlServer = builder
    .AddSqlServer("sqlserver")   
    .WithLifetime(ContainerLifetime.Persistent);

var db = sqlServer
    .AddDatabase("database");

builder.AddProject<Projects.Fide_Blazor_Server>("app")
    .WithEnvironment("ConnectionString", "Integrated Security=SSPI;Pooling=true;MultipleActiveResultSets=true;Data Source=sqlserver;Initial Catalog=database")
    .WithReference(db)
    .WaitFor(db);

builder.Build().Run();

var builder = DistributedApplication.CreateBuilder(args);

var sqlServer = builder
    .AddSqlServer("sqlserver")   
    .WithLifetime(ContainerLifetime.Persistent);

var db = sqlServer
    .AddDatabase("database");

builder.AddProject<Projects.Fide_Blazor_Server>("app")
       .WithReference(db)
       .WaitFor(db);

builder.Build().Run();

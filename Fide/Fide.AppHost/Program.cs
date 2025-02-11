var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder
    .AddPostgres("postgres")
    .WithLifetime(ContainerLifetime.Persistent);

var db = postgres
    .AddDatabase("database");

builder.AddProject<Projects.Fide_Blazor_Server>("app")
    .WithReference(db)
    .WaitFor(db);

builder.Build().Run();

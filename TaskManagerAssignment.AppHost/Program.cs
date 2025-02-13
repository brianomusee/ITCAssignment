var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.TaskManagerAPI>("taskmanagerapi");

builder.Build().Run();

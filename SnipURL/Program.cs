using SnipURL.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.RegisterApplicationServices();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapGet("/", () => 
"""
  ____        _       _    _ ____  _     
 / ___| _ __ (_)_ __ | |  | |  _ \| |    
 \___ \| '_ \| | '_ \| |  | | |_) | |    
  ___) | | | | | |_) | |__| |  _ <| |___ 
 |____/|_| |_|_| .__/ \____/|_| \_\_____|
               |_|   
"""
  );
app.MapControllers();

app.Run();

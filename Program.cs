using System.Text.Json.Serialization;
using HHPW_BackEnd;
using HHPW_BackEnd.Models;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("http://localhost:3000",
                                "http://localhost:7042")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
        });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// allows passing datetimes without time zone data 
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// allows our api endpoints to access the database through Entity Framework Core
builder.Services.AddNpgsql<HHPWDbContext>(builder.Configuration["HHPWDbConnectionString"]);

// Set the JSON serializer options
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
var app = builder.Build();

app.UseCors();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

//Check User
app.MapGet("/checkuser/{uid}", (HHPWDbContext db, string Uid) =>
{
    var userExist = db.Employee.Where(x => x.Uid == Uid).ToList();
    if (userExist == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(userExist);
});
//Get all Employees
app.MapGet("/employees", (HHPWDbContext db) =>
{
    return db.Employee.ToList();
});
//Add a new employee 
app.MapPost("/employees", (HHPWDbContext db, Employee employee) =>
{
    db.Employee.Add(employee);
    db.SaveChanges();
    return Results.Ok(employee);
});
//Delete an Employee 
app.MapDelete("/employees/{id}", (HHPWDbContext db, int id) =>
{
    var employee = db.Employee.Find(id);
    if (employee == null)
    {
        return Results.NotFound(id);
    }

    db.Employee.Remove(employee);
    db.SaveChanges();
    return Results.Ok();
});
//Update an Employee
app.MapPut("/employees/{id}", (HHPWDbContext db, int id, Employee updatedEmployee) =>
{
    var employee = db.Employee.Find(id);
    if (employee == null)
    {
        return Results.NotFound(id);
    }

    employee.Name = updatedEmployee.Name;

    db.Employee.Update(employee);
    db.SaveChanges();

    return Results.Ok();
});
//Get All Orders
app.MapGet("/orders", (HHPWDbContext db) =>
{
    return db.Orders.ToList();
});
//Get Order by ID
app.MapGet("/orders/{id}", (HHPWDbContext db, int id) =>
{
    var order = db.Orders.Find(id);
    if (order == null)
    {
        return Results.NotFound(id);
    }

    return Results.Ok(order);
});
//Adding an Order
app.MapPost("/orders", (HHPWDbContext db, Orders newOrder) =>
{
    db.Orders.Add(newOrder);
    db.SaveChanges();
    return Results.Created($"/orders/{newOrder.Id}", newOrder);
});
//Deleting an Order
app.MapDelete("/orders/{id}", (HHPWDbContext db, int id) =>
{
    var order = db.Orders.Find(id);
    if (order == null)
    {
        return Results.NotFound(id);
    }

    db.Orders.Remove(order);
    db.SaveChanges();

    return Results.NoContent();
});
//Updating an Order
app.MapPut("/orders/{id}", (HHPWDbContext db, int id, Orders updatedOrder) =>
{
    var order = db.Orders.Find(id);
    if (order == null)
    {
        return Results.NotFound(id);
    }

    order.Name = updatedOrder.Name;
    order.Status = updatedOrder.Status;

    db.Orders.Update(order);
    db.SaveChanges();

    return Results.Ok(order);
});
//Get All Order Status
app.MapGet("/orderstatus", (HHPWDbContext db) =>
{
    return db.Status.ToList();
});
// Adding an order Status
app.MapPost("/orderstatus", (HHPWDbContext db, OrderStatus newStatus) =>
{
    db.Status.Add(newStatus);
    db.SaveChanges();
    return Results.Created($"/orderstatus/{newStatus.Id}", newStatus);
});
//Deleting an Order Status
app.MapDelete("/orderstatus/{id}", (HHPWDbContext db, int id) =>
{
    var status = db.Status.Find(id);
    if (status == null)
    {
        return Results.NotFound(id);
    }

    db.Status.Remove(status);
    db.SaveChanges();

    return Results.NoContent();
});
//adding a status to an order
app.MapPost("/api/OrderStatus", (int OrderId, int OrderStatusId, HHPWDbContext db) =>
{
    var order = db.Orders.Include(o => o.Status).FirstOrDefault(o => o.Id == OrderId);

    if (order == null)
    {
        return Results.NotFound();
    }

    var statusToAdd = db.Status.FirstOrDefault(s => s.Id == OrderStatusId);

    if (statusToAdd == null)
    {
        return Results.NotFound();
    }

    order.Status.Add(statusToAdd);
    db.SaveChanges();

    return Results.NoContent();
});




app.Run();

using System.Text.Json.Serialization;
using HHPW_BackEnd;
using HHPW_BackEnd.Models;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("http://localhost:3000",
                                "http://localhost:7120")
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
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
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
app.MapPut("/orders/{id}", (HHPWDbContext db, int id, Orders updatedOrder ) =>
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
app.MapPut("/orders/{id}/close", (HHPWDbContext db, int id, Orders closedOrder) =>
{
    var order = db.Orders.Find(id);
    if (order == null)
    {
        return Results.NotFound(id);
    }

    int closedStatusId = 2;
    order.StatusId = closedStatusId;

    if (closedOrder.PaymentTypesId != null)
    {
        order.PaymentTypesId = closedOrder.PaymentTypesId;
    }

    if (closedOrder.Tip != null)
    {
        order.Tip = closedOrder.Tip;
    }

    if (closedOrder.Review != null)
    {
        order.Review = closedOrder.Review;
    }

    try
    {
        db.Orders.Update(order);
        db.SaveChanges();
        return Results.Ok(order);
    }
    catch (DbUpdateException)
    {
        return Results.NoContent();
    }
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
app.MapPost("/api/OrderStatus/{OrderId}", (int OrderId, int OrderStatusId, HHPWDbContext db) =>
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
// Get all Products 
app.MapGet("/products", (HHPWDbContext db) =>
{
    return db.Products.ToList();
});

// Get Product by ID
app.MapGet("/products/{id}", (HHPWDbContext db, int id) =>
{
    var product = db.Products.Find(id);
    if (product == null)
    {
        return Results.NotFound(id);
    }

    return Results.Ok(product);
});

// Adding a Product
app.MapPost("/products", (HHPWDbContext db, Product newProduct) =>
{
    db.Products.Add(newProduct);
    db.SaveChanges();
    return Results.Created($"/products/{newProduct.Id}", newProduct);
});

// Deleting a Product
app.MapDelete("/products/{id}", (HHPWDbContext db, int id) =>
{
    var product = db.Products.Find(id);
    if (product == null)
    {
        return Results.NotFound(id);
    }

    db.Products.Remove(product);
    db.SaveChanges();

    return Results.NoContent();
});

// Updating a Product
app.MapPut("/products/{id}", (HHPWDbContext db, int id, Product updatedProduct) =>
{
    var product = db.Products.Find(id);
    if (product == null)
    {
        return Results.NotFound(id);
    }

    product.Name = updatedProduct.Name;
    product.Price = updatedProduct.Price;
    product.Description =  updatedProduct.Description;

    db.Products.Update(product);
    db.SaveChanges();

    return Results.Ok(product);
});

//Adding Product to an Order
app.MapPost("/ProductOrders/{ProductId}/{OrderId}", (int ProductId, int OrderId, HHPWDbContext db) =>

{
    var orderToAdd = db.Orders.FirstOrDefault(o => o.Id == OrderId);
    var productToAdd = db.Products.FirstOrDefault(p => p.Id == ProductId);

    if (orderToAdd == null)
    {
        return Results.NotFound();
    }

    if (orderToAdd.Products == null)
    {
        orderToAdd.Products = new List<Product>();
    }

    orderToAdd.Products.Add(productToAdd);
    db.SaveChanges();

    return Results.Created($"orders/{orderToAdd.Id}", orderToAdd);
});
// delete product from an order
app.MapDelete("/ProductOrders/{ProductId}/{OrderId}", (int ProductId, int OrderId, HHPWDbContext db) =>
{
    var order = db.Orders
        .Include(o => o.Products) // Ensure that the Products are included in the query
        .FirstOrDefault(o => o.Id == OrderId);

    if (order == null)
    {
        return Results.NotFound();
    }

    var product = order.Products.FirstOrDefault(p => p.Id == ProductId);

    if (product == null)
    {
        return Results.NotFound(); // Product not found in the order.
    }

    order.Products.Remove(product);
    db.SaveChanges();

    return Results.NoContent(); // Product successfully removed from the order.
});

// Get an order's products
app.MapGet("/ordersProducts/{orderId}", (HHPWDbContext db, int orderId) =>
{
    var orderProducts = db.Orders
        .Where(x => x.Id == orderId)
        .Include(x => x.Products)
        .FirstOrDefault();

    if (orderProducts != null)
    {
        var items = orderProducts.Products.ToList();
        return Results.Ok(items);
    }
    else
    {
        return Results.NotFound();
    }
});

// Get all Payments
app.MapGet("/payments", (HHPWDbContext db) =>
{
    return db.PaymentType.ToList();
});

// Adding a Payment
app.MapPost("/payments", (HHPWDbContext db, PaymentType newPaymentType) =>
{
    db.PaymentType.Add(newPaymentType);
    db.SaveChanges();
    return Results.Created($"/paymenttype/{newPaymentType.Id}", newPaymentType);
});

app.MapPost("/OrderPaymentTypes/{OrderId}/{PaymentTypeId}", (int OrderId, int PaymentTypeId, HHPWDbContext db) =>
{
    var orderToAdd = db.Orders.FirstOrDefault(o => o.Id == OrderId);
    var paymentTypeToAdd = db.PaymentType.FirstOrDefault(pt => pt.Id == PaymentTypeId);

    if (orderToAdd == null || paymentTypeToAdd == null)
    {
        return Results.NotFound();
    }

    if (orderToAdd.PaymentTypes == null)
    {
        orderToAdd.PaymentTypes = new List<PaymentType>();
    }

    orderToAdd.PaymentTypes.Add(paymentTypeToAdd);
    db.SaveChanges();

    return Results.Created($"orders/{orderToAdd.Id}", orderToAdd);
});

app.MapGet("/orderPayments/{orderId}", (HHPWDbContext db, int orderId) =>
{
    var order = db.Orders
        .Where(x => x.Id == orderId)
        .Include(x => x.PaymentTypes) // Include the OrderPayments navigation property
        .FirstOrDefault();

    if (order != null)
    {
        var orderPayments = order.PaymentTypes.ToList();
        return Results.Ok(orderPayments);
    }
    else
    {
        return Results.NotFound();
    }
});


// Deleting a Payment
app.MapDelete("/payments/{id}", (HHPWDbContext db, int id) =>
{
    var payment = db.PaymentType.Find(id);
    if (payment == null)
    {
        return Results.NotFound(id);
    }

    db.PaymentType.Remove(payment);
    db.SaveChanges();

    return Results.NoContent();
});

app.Run();

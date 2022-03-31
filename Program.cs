// начальные данные
List<Shoping> products = new List<Shoping>
{
    new() { Id = Guid.NewGuid().ToString(), Name = "Milk", Quantity = 1, Comment = "1%" },
    new() { Id = Guid.NewGuid().ToString(), Name = "Bread", Quantity = 2, Comment = "light and dark" },
    new() { Id = Guid.NewGuid().ToString(), Name = "Salt",Quantity = 1, Comment = "" }
};

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/api/products", () => products);

app.MapGet("/api/products/{id}", (string id) =>
{
    // получаем продукт по id
    Shoping? product = products.FirstOrDefault(p => p.Id == id);
    // если не найден, отправляем статусный код и сообщение об ошибке
    if (product == null) return Results.NotFound(new { message = "Product not found!" });

    // если пользователь найден, отправляем его
    return Results.Json(product);
});

app.MapDelete("/api/products/{id}", (string id) =>
{
    // получаем продукт по id
    Shoping? product = products.FirstOrDefault(p => p.Id == id);

    // если не найден, отправляем статусный код и сообщение об ошибке
    if (product == null) return Results.NotFound(new { message = "Product not found!" });

    // если пользователь найден, удаляем его
    products.Remove(product);
    return Results.Json(product);
});

app.MapPost("/api/products", (Shoping product) => {

    // устанавливаем id для нового продукта
    product.Id = Guid.NewGuid().ToString();
    // добавляем продукт в список
    products.Add(product);
    return product;
});

app.MapPut("/api/products", (Shoping productData) => {

    // получаем продукт по id
    var product = products.FirstOrDefault(p => p.Id == productData.Id);
    // если не найден, отправляем статусный код и сообщение об ошибке
    if (product == null) return Results.NotFound(new { message = "Product not found!" });
    // если продукта найден, изменяем его данные и отправляем обратно клиенту

    product.Name = productData.Name;
    product.Quantity = productData.Quantity;
    product.Comment = productData.Comment;
    return Results.Json(product);
});

app.Run();

public class Shoping
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public int Quantity { get; set; }
    public string Comment { get; set; } = "";
}
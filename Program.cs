using APIMinima;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

var todoItems = app.MapGroup("/todoitems");

todoItems.MapGet("/", GetAllTodos);
todoItems.MapGet("/complete", GetCompleteTodos);
todoItems.MapGet("/{id}", GetTodo);
//todoItems.MapPost("/", CreateTodo);
//todoItems.MapPut("/{id}", UpdateTodo);
//todoItems.MapDelete("/{id}", DeleteTodo);

app.Run();

static async Task<IResult> GetAllTodos(TodoDb db)
{
    return TypedResults.Ok(await db.Todos.ToArrayAsync());
}


static async Task<IResult> GetCompleteTodos(TodoDb db)
{
    return TypedResults.Ok(await db.Todos.Where(t => t.IsComplete).ToArrayAsync());
}

static async Task<IResult> GetTodo(int id, TodoDb db)
{
    return await db.Todos.FindAsync(id)
         is Todo todo
         ? TypedResults.Ok()
         : TypedResults.NotFound();
}



//todoItems.MapGet("/todoitems", async (TodoDb db) =>
//    await db.Todos.ToListAsync());

//todoItems.MapGet("/complete", async (TodoDb db) =>
//    await db.Todos.Where(t => t.IsComplete).ToListAsync());

//todoItems.MapGet("/{id}", async (int id, TodoDb db) =>
//           await db.Todos.FindAsync(id)
//           is Todo todo
//           ? Results.Ok(todo)
//           : Results.NotFound()
//           );

//todoItems.MapPost("/", async (Todo todo, TodoDb db) =>
//{
//    db.Todos.Add(todo);
//    await db.SaveChangesAsync();
//    return Results.Created($"todoitems/{todo.Id}", todo);
//});

//todoItems.MapPut("/{id}", async (int id, Todo inputTodo, TodoDb db) =>
//{
//    var todo = await db.Todos.FindAsync(id);
//    if (todo == null) return Results.NotFound();

//    todo.Name = inputTodo.Name;
//    todo.IsComplete = inputTodo.IsComplete;

//    await db.SaveChangesAsync();
//    return Results.NoContent();
//}
//);


//todoItems.MapDelete("/{id}", async (int id, TodoDb db) =>
//{
//    if (await db.Todos.FindAsync(id) is Todo todo)
//    {
//        db.Todos.Remove(todo);
//        await db.SaveChangesAsync();
//        return Results.Ok(todo);
//    }
//    return Results.NotFound();
//}
//);

//app.Run();

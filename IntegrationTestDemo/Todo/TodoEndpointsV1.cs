using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTestDemo.Todo
{
    public static class TodoEndpointsV1
    {
        public static RouteGroupBuilder MapTodosApiV1(this RouteGroupBuilder group)
        {
            group.MapGet("/", GetAllTodos);
            group.MapGet("/{id}", GetTodo);
            group.MapGet("/complete", GetTodoComplete);
            group.MapPost("/", CreateTodo);
            // .AddEndpointFilter(async (efiContext, next) =>
            // {
            //     var param = efiContext.GetArgument<Todo>(0);

            //     var validationErrors = Utilities.IsValid(param);

            //     if (validationErrors.Any())
            //     {
            //         return Results.ValidationProblem(validationErrors);
            //     }

            //     return await next(efiContext);
            // });

            group.MapPut("/{id}", UpdateTodo);
            group.MapDelete("/{id}", DeleteTodo);

            return group;
        }

        // get all todos
        // <snippet_1>
        public static async Task<Ok<List<Todo>>> GetAllTodos(TodoDbContext database)
        {
            var todos = await database.Todos.ToListAsync();
            return TypedResults.Ok(todos);
        }
        // </snippet_1>

        // get todo by id
        public static async Task<Results<Ok<Todo>, NotFound>> GetTodo(int id, TodoDbContext database)
        {
            var todo = await database.Todos.FindAsync(id);

            if (todo != null)
            {
                return TypedResults.Ok(todo);
            }

            return TypedResults.NotFound();
        }

        public static async Task<Results<Ok<List<Todo>>, NotFound>> GetTodoComplete(TodoDbContext database)
        {
            var todos = await database.Todos.Where(t => t.IsComplete).ToListAsync();

            if (todos != null)
            {
                return TypedResults.Ok(todos);
            }

            return TypedResults.NotFound();
        }


        // create todo
        public static async Task<Created<Todo>> CreateTodo(Todo todo, TodoDbContext database)
        {
            await database.Todos.AddAsync(todo);
            await database.SaveChangesAsync();

            return TypedResults.Created($"/todoitems/v1/{todo.Id}", todo);
        }

        // update todo
        public static async Task<Results<Created<Todo>, NotFound>> UpdateTodo(int id, Todo inputTodo, TodoDbContext database)
        {
            var existingTodo = await database.Todos.FindAsync(id);

            if (existingTodo != null)
            {
                existingTodo.Name = inputTodo.Name;
                existingTodo.IsComplete = inputTodo.IsComplete;

                await database.SaveChangesAsync();

                return TypedResults.Created($"/todoitems/v1/{existingTodo.Id}", existingTodo);
            }

            return TypedResults.NotFound();
        }

        // delete todo
        public static async Task<Results<NoContent, NotFound>> DeleteTodo(int id, TodoDbContext database)
        {
            var todo = await database.Todos.FindAsync(id);

            if (todo != null)
            {
                database.Todos.Remove(todo);
                await database.SaveChangesAsync();

                return TypedResults.NoContent();
            }

            return TypedResults.NotFound();
        }
    }
}

﻿using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.Json;

namespace IntegrationTestDemo.User
{
    public static class UserEndpointsV1
    {
        public static RouteGroupBuilder MapUsersApiV1(this RouteGroupBuilder group)
        {
            group.MapGet("/", GetAll);
            group.MapGet("/{id}", GetById);
            group.MapPost("/", Create);
            group.MapPut("/{id}", Update);
            group.MapDelete("/{id}", Delete);

            return group;
        }

        // get all
        // <snippet_1>
        public static async Task<Ok<List<UserEntity>>> GetAll(UserDbContext database, HttpClient httpClient)
        {
            //var options = new JsonSerializerOptions
            //{
            //    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            //    // etc.
            //};
            string url = $"https://jsonplaceholder.typicode.com/users";
            try
            {
                var response = await httpClient.GetAsync(url);
                // var result = await httpClient.GetFromJsonAsync<List<UserEntity>>(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var users = JsonSerializer.Deserialize<List<UserEntity>>(content);
                    return TypedResults.Ok(users);
                }

                return TypedResults.Ok(new List<UserEntity>());

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        // </snippet_1>

        // get by id
        public static async Task<Results<Ok<UserEntity>, NotFound>> GetById(int id, UserDbContext database)
        {
            var user = await database.Users.FindAsync(id);

            if (user != null)
            {
                return TypedResults.Ok(user);
            }

            return TypedResults.NotFound();
        }

        // create
        public static async Task<Created<UserEntity>> Create(UserEntity user, UserDbContext database, HttpClient httpClient)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                // etc.
            };
            string url = $"https://jsonplaceholder.typicode.com/users";
            var response = await httpClient.PostAsJsonAsync(url, user);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<UserEntity>(jsonString, options);
                return TypedResults.Created($"/useritems/v1/{result.Id}", result);
            }

            await database.Users.AddAsync(user);
            await database.SaveChangesAsync();

            return TypedResults.Created($"/useritems/v1/{user.Id}", user);
        }

        // update
        public static async Task<Results<Created<UserEntity>, NotFound>> Update(int id, UserEntity user, UserDbContext database, HttpClient httpClient)
        {


            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                // etc.
            };
            string url = $"https://jsonplaceholder.typicode.com/users/{id}";
            var response = await httpClient.PutAsJsonAsync(url, user);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<UserEntity>(jsonString, options);
                return TypedResults.Created($"/useritems/v1/{result.Id}", result);
            }

            var existingTodo = await database.Users.FindAsync(id);

            if (existingTodo != null)
            {
                existingTodo.Name = user.Name;
                existingTodo.Email = user.Email;

                await database.SaveChangesAsync();

                return TypedResults.Created($"/useritems/v1/{existingTodo.Id}", existingTodo);
            }

            return TypedResults.NotFound();
        }

        // delete user
        public static async Task<Results<NoContent, NotFound>> Delete(int id, UserDbContext database, HttpClient httpClient)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                // etc.
            };
            string url = $"https://jsonplaceholder.typicode.com/users/{id}";
            var response = await httpClient.DeleteAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return TypedResults.NoContent();
            }

            var user = await database.Users.FindAsync(id);

            if (user != null)
            {
                database.Users.Remove(user);
                await database.SaveChangesAsync();

                return TypedResults.NoContent();
            }

            return TypedResults.NotFound();
        }
    }
}

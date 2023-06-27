

using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;



public class RestService : IRestService
{
    private readonly string baseUrl = "http://jsonplaceholder.typicode.com/";
    public async Task<T> Get<T>(RestRequest request) where T : class
    {
        var client = new RestClient(baseUrl);

        var response = await client.ExecuteAsync<T>(request);

        if (response.ErrorException != null)
            throw new Exception();

        return response.Data;
    }
}

public interface IRestService
{
    Task<T> Get<T>(RestRequest request) where T : class;
}
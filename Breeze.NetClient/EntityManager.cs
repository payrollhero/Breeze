﻿using Breeze.Metadata;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Breeze.NetClient {
  public class EntityManager {

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceName">"http://localhost:9000/"</param>
    public EntityManager(String serviceName) {
      _client = new HttpClient();
      _client.BaseAddress = new Uri(serviceName);

      // Add an Accept header for JSON format.
      _client.DefaultRequestHeaders.Accept.Add(
          new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public String ServiceName { get; private set; }
    HttpClient _client;
    

    public async Task<Object> FetchMetadata() {
      
      try {

        var response = await _client.GetAsync("Metadata");
        response.EnsureSuccessStatusCode(); // Throw on error code.

        var metadata = await response.Content.ReadAsStringAsync();
        var metadataStore = new MetadataStore();
        var metadataProcessor = new CsdlMetadataProcessor(metadataStore, metadata);

        return metadata;
      } catch (Newtonsoft.Json.JsonException jEx) {
        // This exception indicates a problem deserializing the request body.
        throw;
      } catch (HttpRequestException ex) {
        throw;
      } finally {
        
      }
    }
  

    /// <summary>
    /// 
    /// </summary>
    /// <param name="webApiQuery">"api/products"</param>
    public async Task<Object> ExecuteQuery(string resourcePath) {
      
      try {

        var response = await _client.GetAsync(resourcePath);
        response.EnsureSuccessStatusCode(); // Throw on error code.

        var result = await response.Content.ReadAsStringAsync();
        var x = JsonConvert.DeserializeObject(result);
        var a = (JArray)x;
        return a;
      } catch (Newtonsoft.Json.JsonException jEx) {
        // This exception indicates a problem deserializing the request body.
        throw;
      } catch (HttpRequestException ex) {
        throw;
      } finally {
        
      }
    }

    public async Task<IEnumerable<T>> ExecuteQuery<T>(EntityQuery<T> query) {

      try {
        var resourcePath = query.GetResourcePath();
        var response = await _client.GetAsync(resourcePath);
        response.EnsureSuccessStatusCode(); // Throw on error code.

        var result = await response.Content.ReadAsStringAsync();
        var x = JsonConvert.DeserializeObject<IEnumerable<T>>(result);
        
        return x;
      } catch (Newtonsoft.Json.JsonException jEx) {
        // This exception indicates a problem deserializing the request body.
        throw;
      } catch (HttpRequestException ex) {
        throw;
      } finally {

      }
    }
  }
}
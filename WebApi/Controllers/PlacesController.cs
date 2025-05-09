/// <summary>
/// Controller for managing places and related operations.
/// </summary>
/// <remarks>
/// This static controller provides endpoints for adding, retrieving, and searching for places.
/// It uses the PlaceService to handle business logic and data operations.
/// The endpoints are grouped under the "/places" route and require authorization for write operations.
/// </remarks>
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.DTOs.Requests;
using WebApi.Models.DTOs.Responses;
using WebApi.Services;

public static class PlacesController
{
    /// <summary>
    /// Maps all place-related endpoints to the application.
    /// </summary>
    /// <param name="app">The WebApplication instance to which endpoints are mapped.</param>
    public static void MapPlacesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/places")
                       .WithTags("Places Management");
        
        group.MapPost("/", AddPlace)
             .RequireAuthorization()
             .DisableAntiforgery();
        
        group.MapGet("/{id}", GetPlace);
        group.MapGet("/nearby", GetNearbyPlaces);
    }

    /// <summary>
    /// Adds a new place to the system.
    /// </summary>
    /// <param name="request">The place data to be added.</param>
    /// <param name="placeService">The service handling place operations.</param>
    /// <returns>An IResult containing the operation result.</returns>
    private static async Task<IResult> AddPlace(
        [FromBody] PlaceRequest request,
        [FromServices] PlaceService placeService)
    {
        var result = await placeService.AddPlace(request);
        return Results.Ok(result);
    }

    /// <summary>
    /// Retrieves a specific place by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the place.</param>
    /// <param name="placeService">The service handling place operations.</param>
    /// <returns>An IResult containing the place or a not found response.</returns>
    private static async Task<IResult> GetPlace(
        Guid id,
        [FromServices] PlaceService placeService)
    {
        var result = await placeService.GetPlace(id);
        return result is null ? Results.NotFound() : Results.Ok(result);
    }

    /// <summary>
    /// Retrieves places near a specified location within a given radius.
    /// </summary>
    /// <param name="request">The request containing location and search parameters.</param>
    /// <param name="placeService">The service handling place operations.</param>
    /// <returns>An IResult containing the list of nearby places.</returns>
    private static async Task<IResult> GetNearbyPlaces(
        [FromQuery] NearbyPlacesRequest request,
        [FromServices] PlaceService placeService)
    {
        var result = await placeService.GetNearbyPlaces(request);
        return Results.Ok(result);
    }
}
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.DTOs.Requests;
using WebApi.Models.DTOs.Responses;
using WebApi.Services;

public static class PlacesController
{
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

    private static async Task<IResult> AddPlace(
        [FromBody] PlaceRequest request,
        [FromServices] PlaceService placeService)
    {
        var result = await placeService.AddPlace(request);
        return Results.Ok(result);
    }

    private static async Task<IResult> GetPlace(
        Guid id,
        [FromServices] PlaceService placeService)
    {
        var result = await placeService.GetPlace(id);
        return result is null ? Results.NotFound() : Results.Ok(result);
    }

    private static async Task<IResult> GetNearbyPlaces(
        [FromQuery] NearbyPlacesRequest request,
        [FromServices] PlaceService placeService)
    {
        var result = await placeService.GetNearbyPlaces(request);
        return Results.Ok(result);
    }
}
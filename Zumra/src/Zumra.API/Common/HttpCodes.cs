namespace Zumra.API.Common;

public static class HttpCodes
{
    public const int BadRequest = StatusCodes.Status400BadRequest;
    public const int NotFound = StatusCodes.Status404NotFound;
    public const int ServerError = StatusCodes.Status500InternalServerError;
}
using Microsoft.AspNetCore.Mvc;
using Paintball.Abstractions.Enums;
using Paintball.Abstractions.Exceptions;
using ErrorCodes = Paintball.Abstractions.Enums.ErrorCodes;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace PaintballResults.Api.Middleware
{
    public class GlobalExceptionHandlerMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)

        {
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(context, e);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception e)
        {
            ErrorCodes errorCodes;
            string message;
            var statusCode = StatusCodes.Status400BadRequest;

            var exceptionType = e.GetType();

            if (exceptionType == typeof(InvalidFileException))
            {
                message = e.Message;
                errorCodes = ErrorCodes.InvalidFileFormat;
            }
            else if (exceptionType == typeof(DuplicatedRecordsException))
            {
                message = e.Message;
                errorCodes = ErrorCodes.DuplicatedItem;
            }
            else if (exceptionType == typeof(InvalidGameResultException))
            {
                message = e.Message;
                errorCodes = ErrorCodes.InvalidGameResult;
            }
            else if (exceptionType == typeof(InvalidRecordException))
            {
                message = e.Message;
                errorCodes = ErrorCodes.InvalidRecordFormat;
            }
            else if (exceptionType == typeof(NotAbleToParseException))
            {
                message = e.Message;
                errorCodes = ErrorCodes.NotAbleToParse;
            }

            else if (exceptionType == typeof(GameResultNotFoundException))
            {
                message = e.Message;
                errorCodes = ErrorCodes.GameResultNotFound;
                statusCode = StatusCodes.Status404NotFound;
            }
            else if (exceptionType == typeof(GameResultsNotImportedException))
            {
                message = e.Message;
                errorCodes = ErrorCodes.GameResultNotFound;
                statusCode = StatusCodes.Status404NotFound;
            }
            else if (exceptionType == typeof(FileHasNoDataException))
            {
                message = e.Message;
                errorCodes = ErrorCodes.InvalidRecordFormat;
                statusCode = StatusCodes.Status404NotFound;
            }
            else if (exceptionType == typeof(StreamIsNullOrEmptyException))
            {
                message = e.Message;
                errorCodes = ErrorCodes.OmegaFail;
                statusCode = StatusCodes.Status409Conflict;
            }
            else if (exceptionType == typeof(InvalidDataStringException))
            {
                message = e.Message;
                errorCodes = ErrorCodes.InvalidFileFormat;
                statusCode = StatusCodes.Status409Conflict;
            }

            else
            {
                //todo: Umbenennen
                message = "Omegafail";
                errorCodes = ErrorCodes.OmegaFail;
            }

            ProblemDetails exceptionResult = new()
            {
                Status = (int)errorCodes,
                Detail = message
            };

            var exceptionResponse = JsonSerializer.Serialize(exceptionResult);

            context.Response.ContentType = "application/json";

            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsync(exceptionResponse);
        }
    }
}
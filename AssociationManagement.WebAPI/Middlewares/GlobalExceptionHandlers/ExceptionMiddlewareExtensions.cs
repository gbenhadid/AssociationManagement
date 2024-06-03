﻿using Microsoft.AspNetCore.Diagnostics;
using AssociationManagement.Tools.Exceptions.Abstractions;
using AssociationManagement.Tools.Logging;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace AssociationManagement.WebAPI.Middlewares.GlobalExceptionHandler {
    public static class ExceptionMiddlewareExtensions {
        public static void ConfigureExceptionHandler(this WebApplication app, ILoggerManager logger) {
            app.UseExceptionHandler(appError => {
                appError.Run(async context => {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if(contextFeature != null) {
                        context.Response.StatusCode = contextFeature.Error switch {
                            ValidationException => StatusCodes.Status400BadRequest,
                            NotFoundException => StatusCodes.Status404NotFound,
                            BadRequestException => StatusCodes.Status400BadRequest,
                            AlreadyExistsException => StatusCodes.Status409Conflict,
                            ForbiddenException => StatusCodes.Status403Forbidden,
                            ConflictException => StatusCodes.Status409Conflict,
                            UnauthorizedException => StatusCodes.Status401Unauthorized,
                            _ => StatusCodes.Status500InternalServerError
                        };
                        logger.LogError($"Something went wrong: {contextFeature.Error}");

                        await context.Response.WriteAsync(new ErrorDetails() {
                            StatusCode = context.Response.StatusCode,
                            Message = contextFeature.Error.Message,
                        }.ToString());

                    }
                });
            });
        }
    }
}

using Microsoft.AspNetCore.Http;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using BolosDoJacquin.Domain.Exceptions; // Para ele conhecer a sua Exception

namespace BolosDoJacquin.Middlewares;

public class ErrorMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            // Deixa o pedido passar e seguir o fluxo normal (Controller -> Service -> etc)
            await _next(context);
        }
        catch (Exception ex)
        {
            Console.WriteLine("=== ERRO INTERNO ===");
            Console.WriteLine(ex.ToString());
            Console.WriteLine("====================");
            await TratarErroAsync(context, ex);
        }
    }

    private static Task TratarErroAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";

        // Se o erro for a NOSSA regra de negócio (DomainException)
        if (ex is DomainException)
        {
            context.Response.StatusCode = 400; // 400: Bad Request (A culpa é de quem mandou o dado errado)
            var resultado = JsonSerializer.Serialize(new 
            {   erro = ex.Message,
                codigo = ((DomainException)ex).Codigo
            });
            return context.Response.WriteAsync(resultado);
        }

        if (ex is NotFoundException)
        {
            context.Response.StatusCode = 404;
            var resultado = JsonSerializer.Serialize(new { erro = ex.Message });
            return context.Response.WriteAsync(resultado);
        }

        if (ex is ForbiddenException)
        {
            context.Response.StatusCode = 403;
            var resultado = JsonSerializer.Serialize(new { erro = ex.Message });
            return context.Response.WriteAsync(resultado);
        }

        if (ex is ConflictException)
        {
            context.Response.StatusCode = 409; // 409 significa Conflito!
            var resultado = System.Text.Json.JsonSerializer.Serialize(new { erro = ex.Message });
            return context.Response.WriteAsync(resultado);
        }

        // Se for um bug técnico, banco fora do ar, erro de código, etc.
        context.Response.StatusCode = 500; // 500: Internal Server Error (A culpa é nossa)
        var erroServidor = JsonSerializer.Serialize(new { erro = "Ocorreu um erro interno no servidor. Tente novamente mais tarde." });
        return context.Response.WriteAsync(erroServidor);
    }
}
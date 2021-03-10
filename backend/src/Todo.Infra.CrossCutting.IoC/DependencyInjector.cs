using Todo.Application.Interfaces;
using Todo.Application.Services;
using Todo.Domain.CommandHandlers;
using Todo.Domain.Commands.BoardCommands;
using Todo.Domain.Interfaces;
using Todo.Domain.Mediator;
using Todo.Infra.CrossCutting.Bus;
using Todo.Infra.Data;
using Todo.Infra.Data.Accessor;
using Todo.Infra.Data.Repositories;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Todo.Infra.CrossCutting.IoC
{
    public static class DependencyInjector
    {
        public static void Register(this IServiceCollection services)
        {
            // Application
            services.AddScoped<IBoardAppService, BoardAppService>();
            services.AddScoped<IEmailSenderService, EmailSenderService>();

            // Domain Bus (Mediator)
            services.AddScoped<IMediatorHandler, InMemoryBus>();

            // Domain (Handler)
            services.AddScoped<IRequestHandler<AddBoardCommand, ValidationResult>, BoardCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateBoardTitleCommand, ValidationResult>, BoardCommandHandler>();
            services.AddScoped<IRequestHandler<AddColumnToBoardCommand, ValidationResult>, BoardCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateColumnPositionInBoardCommand, ValidationResult>, BoardCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateColumnTitleCommand, ValidationResult>, BoardCommandHandler>();
            services.AddScoped<IRequestHandler<AddCardToColumnCommand, ValidationResult>, BoardCommandHandler>();
            services.AddScoped<IRequestHandler<UpdateCardPriorityInColumnCommand, ValidationResult>, BoardCommandHandler>();
            services.AddScoped<IRequestHandler<MoveCardBetweenColumnsCommand, ValidationResult>, BoardCommandHandler>();

            // Infra - Data
            services.AddScoped<IBoardRepository, BoardRepository>();
            services.AddScoped<TodoContext>();

            // Accessor
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUserAccessor, HttpContextInfoAccessor>();
        }
    }
}

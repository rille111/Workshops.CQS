using System;
using System.Threading.Tasks;
using Cqs.Domain.Models;
using Cqs.Infrastructure;
using Cqs.Infrastructure.PubSub;

namespace Cqs.Domain.Commands
{
    /// <summary>
    /// Why separate all commands like this? Because of ... separation!
    /// 
    /// Plus, we declare our intent and can map commands to actual business operations! 'Data' right now is just a simple class, but you got all flexibility here!
    /// </summary>
    public class CreateTodoCommand : ICommand
    {
        public Todo Data { get; set; }
        public dynamic Output { get; set; }
    }

    public class UpdateTodoCommand : ICommand
    {
        public Todo Data { get; set; }
        public dynamic Output { get; set; }
    }

    public class DeleteTodoCommand : ICommand
    {
        public int Id { get; set; }
        public dynamic Output { get; set; }
    }

    public class WeirdTodoCommand : ICommand
    {
        public int Id { get; set; }
        public dynamic Output { get; set; }
    }

    /// <summary>
    /// * Handles CreateTodoCommand commands. 
    /// * This implementation works with the generic repository, and therefore is very useful and reusable.
    /// * You can change this implementation to something else if you wish, but probably there is no need because of the generic repository. 
    ///     It probably makes more sense to switch that repository, in some IoC engine.
    /// * Why handle the commands in different handlers? Because of .. separation!
    ///     If they are their own classes - we can attach decorators, logging etc on each handler separately!
    /// </summary>
    public class CreateTodoCommandHandler : ICommandHandler<CreateTodoCommand>
    {
        private readonly IRepository<Todo> _repo;

        public CreateTodoCommandHandler(IRepository<Todo> repo)
        {
            this._repo = repo;
        }

        public async Task Handle(CreateTodoCommand command)
        {
            await Task.Run( async () =>
            {
                this._repo.Create(command.Data);
                command.Output = command.Data.Id;
            });
        }
    }

    public class ChainedCreateTodoCommandHandler : ICommandHandler<CreateTodoCommand>
    {
        private readonly IRepository<Todo> _repo;
        private IPubSub _pubSub;

        public ChainedCreateTodoCommandHandler(IPubSub pubSub)
        {
            this._pubSub = pubSub;
        }

        public async Task Handle(CreateTodoCommand command)
        {
            await Task.Run( () =>
            {
                _pubSub.Publish("ChainedCreateTodoCommandHandler", "Chained handler just handled a task!");
            });
        }
    }


    public class UpdateTodoCommandHandler : ICommandHandler<UpdateTodoCommand>
    {
        private readonly IRepository<Todo> _repo;

        public UpdateTodoCommandHandler(IRepository<Todo> repo)
        {
            this._repo = repo;
        }

        public async Task Handle(UpdateTodoCommand command)
        {
            await Task.Run(async () =>
            {
                this._repo.Update(command.Data);
            });
        }
    }

    public class DeleteTodoCommandHandler : ICommandHandler<DeleteTodoCommand>
    {
        private readonly IRepository<Todo> _repo;

        public DeleteTodoCommandHandler(IRepository<Todo> repo)
        {
            this._repo = repo;
        }

        public async Task Handle(DeleteTodoCommand command)
        {
            await Task.Run(async () =>
            {
                var item = this._repo.ItemBy(p => p.Id == command.Id);
                command.Output = item.Id;
                this._repo.Delete(item);
            });
        }
    }
}

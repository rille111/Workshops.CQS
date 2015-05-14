using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cqs.Domain.Commands
{
	public class ???CommandHandler : ICommandHandler<???Command>
	{
		private readonly IRepository<???> _repo;

		public CreateTodoCommandHandler(IRepository<???> repo)
		{
			this._repo = repo;
		}

		public async Task Handle(???Command command)
		{
			await Task.Run(() =>
			{
				this._repo.???(command.Data);
				command.Output = command.Data.Id;
			});
		}
	}
}
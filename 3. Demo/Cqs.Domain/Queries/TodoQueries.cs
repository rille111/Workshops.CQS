using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cqs.Domain.Models;
using Cqs.Infrastructure;

namespace Cqs.Domain.Queries
{
    // Grouping all related queries and their handlers for finding 'To do' here.

    /// <summary>
    /// The idea here is that this query can be used in any project, even at the bottom for example as Mvc model.
    /// This is possible because all projects work with the shared .Domain and the .Infrastructure projects.
    /// </summary>
    public class FindTodosByCategoryQuery : IQuery<Todo[]>
    {
        public string Category { get; set; }
        public bool? Done { get; set; }
    }

    public class FindTodosByCategoryQueryHandler : IQueryHandler<FindTodosByCategoryQuery, Todo[]>
    {
        private readonly IRepository<Todo> _repo;

        /// <summary>
        /// Ctor. The Repo is hopefully injected with IoC. So don't forget to set it up!
        /// </summary>
        public FindTodosByCategoryQueryHandler(IRepository<Todo> repo )
        {
            this._repo = repo;
        }

        /// <summary>
        /// Handle the query and produce a result from the repository.
        /// </summary>
        public async Task<Todo[]> Handle(FindTodosByCategoryQuery query)
        {
            return await Task.Run(() =>
            {
                var itemz = _repo.ItemsWhere(p => p.Category == query.Category);

                if (query.Done.HasValue)
                    itemz = itemz.Where(p => query.Done.HasValue && p.Done == query.Done);

                return itemz.ToArray();
            });
        }
    }
}

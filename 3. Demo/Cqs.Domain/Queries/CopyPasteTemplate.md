using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cqs.Domain.Queries
{
    public class ???Query : IQuery<???[]>
    {
        public string ?? { get; set; }
    }

    public class ???QueryHandler : IQueryHandler<???Query, ???[]>
    {
        private readonly IRepository<???> _repo;

        public ???QueryHandler(IRepository<???> repo )
        {
            this._repo = repo;
        }

        public async Task<???[]> Handle(???Query query)
        {
            return await Task.Run(() =>
            {
                var itemz = _repo.ItemsWhere(p => p.?? == query.??);
                return itemz.ToArray();
            });
        }
    }
}

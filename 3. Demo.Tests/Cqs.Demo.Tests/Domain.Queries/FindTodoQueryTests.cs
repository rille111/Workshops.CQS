using System;
using System.Linq;
using Cqs.Backend.Tests._TestUtils;
using Cqs.Domain.Models;
using Cqs.Domain.Queries;
using Cqs.Infrastructure;
using Cqs.Persistence.RuntimeCache;
using NUnit.Framework;
using SimpleInjector;

namespace Cqs.Backend.Tests.Domain.Queries
{
    [TestFixture]
    public class FindTodoQueryTests
    {
        private Container iocContainer;

        [TestFixtureSetUp]
        public void SetupClass()
        {
            iocContainer = new Container();
            iocContainer.Register<IRepository<Todo>>(() =>
            {
                var repo = new CachedRepository<Todo>();
                foreach (var todo in Stubs.CreateTodos())
                    repo.Create(todo);
                return repo;
            });
        }

        [SetUp]
        public void SetupEachTest()
        {
            EmptyRepository();
        }

        [Test]
        public void Handle_should_resolve_and_not_produce_exception()
        {
            // Arrange
            var handler = iocContainer.GetInstance<FindTodosByCategoryQueryHandler>();
            var query = new FindTodosByCategoryQuery();

            // Act & Assert
            handler.Handle(query);
        }


        [Test]
        public void FindTodosByCategoryQuery_should_find_stuff()
        {
            // Arrange
            var handler = iocContainer.GetInstance<FindTodosByCategoryQueryHandler>();
            var query = new FindTodosByCategoryQuery();
            query.Category = "Shopping";

            // Act
            var task = handler.Handle(query);
            
            // Assert
            Assert.That(task.Result.Count(), Is.EqualTo(2));
        }

        // Helpers

        private static void EmptyRepository()
        {
            IRepository<Todo> repo = new CachedRepository<Todo>();

            var ids = repo.Items.Select(p => p.Id).ToList();
            foreach (var id in ids)
            {
                repo.Delete(repo.ItemBy(p => p.Id == id));
            }
        }
    }
}

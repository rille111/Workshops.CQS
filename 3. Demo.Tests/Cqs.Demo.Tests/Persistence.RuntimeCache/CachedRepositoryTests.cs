using System;
using System.Linq;
using Cqs.Backend.Tests._TestUtils;
using Cqs.Domain.Models;
using Cqs.Infrastructure;
using Cqs.Persistence.RuntimeCache;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using SimpleInjector;
using SimpleInjector.Extensions;

namespace Cqs.Backend.Tests.Persistence.RuntimeCache
{
    [TestFixture]
    public class CachedRepositoryTests
    {
        private Container iocContainer;

        [TestFixtureSetUp]
        public void SetupClass()
        {
            iocContainer = new Container();
            iocContainer.RegisterOpenGeneric(typeof(IRepository<>), typeof(CachedRepository<>));
        }

        [SetUp]
        public void SetupEachTest()
        {
            EmptyRepository();
        }

        [Test]
        public void When_adding_stuff_they_should_be_available()
        {
            // Arrange
            var repo = iocContainer .GetInstance<IRepository<Todo>>();
            var todos = Stubs.CreateTodos();
            var todoCount = todos.Count();

            // Act
            foreach (var todo in todos)
                repo.Create(todo);
            var items = repo.ItemsWhere(p => true).ToList();
            
            // Assert
            Assert.That(
                items.Count, Is.EqualTo(todoCount));
        }

        [Test]
        public void When_updating_something_it_should_update()
        {
            // Arrange
            var repo = iocContainer .GetInstance<IRepository<Todo>>();
            foreach (var todo in Stubs.CreateTodos())
                repo.Create(todo);

            // Act
            var modelToUpdate = repo.Items.First();
            modelToUpdate.Caption = "Buy more milk";
            repo.Update(modelToUpdate);
            var items = repo.ItemsWhere(p => p.Caption == "Buy more milk").ToList();

            // Assert
            Assert.That(
                items.Count, Is.EqualTo(1));
        }

        [Test]
        public void When_deleting_something_it_should_be_removed_from_repo()
        {
            // Arrange
            var repo = iocContainer .GetInstance<IRepository<Todo>>();
            foreach (var todo in Stubs.CreateTodos())
                repo.Create(todo);
            var countBeforeDelete = repo.Items.Count();

            // Act
            var id = repo.Items.First().Id;
            repo.Delete(repo.ItemBy(p=> p.Id == id));
            var countAfterDelete = repo.Items.Count();

            // Assert
            Assert.That(countBeforeDelete, Is.GreaterThan(countAfterDelete));
        }

        // Helpers

        private void EmptyRepository()
        {
            var repo = iocContainer.GetInstance<IRepository<Todo>>();

            var ids = repo.Items.Select(p => p.Id).ToList();
            foreach (var id in ids)
            {
                repo.Delete(repo.ItemBy(p => p.Id == id));
            }
        }

    }
}

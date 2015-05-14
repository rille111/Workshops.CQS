using Cqs.Domain.Models;
using Cqs.Domain.Queries;
using Cqs.Infrastructure;
using Cqs.Infrastructure.Implementations;
using Cqs.Persistence.RuntimeCache;
using Moq;
using NUnit.Framework;
using SimpleInjector;
using SimpleInjector.Extensions;

namespace Cqs.Backend.Tests.Infrastructure.Decorators
{
    [TestFixture]
    public class LoggingHandlerDecoratorsTests
    {
        private Container iocContainer;

        [TestFixtureSetUp]
        public void ClassSetUp()
        {
            iocContainer = new Container();
        }

        [Test]
        public void LogDecorator_should_be_used_when_using_handlers()
        {
            // Arrange
            var mockedLogger = new Mock<ILogger>(MockBehavior.Strict);
            mockedLogger.Setup(p =>
                p.Write(
                It.IsAny<LoggerLevel>(),
                It.IsAny<string>(),
                It.IsAny<object[]>()
                ));

            var mockedRepo = new Mock<CachedRepository<Todo>>(MockBehavior.Loose);
            var domainAssembly = typeof(FindTodosByCategoryQueryHandler).Assembly;
            var query = new FindTodosByCategoryQuery {Category = "Etc"};
            iocContainer.RegisterManyForOpenGeneric(typeof(IQueryHandler<,>), domainAssembly);
            iocContainer.RegisterManyForOpenGeneric(typeof(ICommandHandler<>), domainAssembly);

            iocContainer.Register<ILogger>(() => mockedLogger.Object, Lifestyle.Singleton);
            iocContainer.Register<IRepository<Todo>>(() => mockedRepo.Object);
            iocContainer.RegisterDecorator(typeof(IQueryHandler<,>), typeof(LoggingQueryHandlerDecorator<,>));

            // Act
            var qhandler = iocContainer.GetInstance<IQueryHandler<FindTodosByCategoryQuery, Todo[]>>();
            qhandler.Handle(query);

            // Assert
            Assert.That(
                qhandler.GetType().Name,
                Is.EqualTo(typeof(LoggingQueryHandlerDecorator<,>).Name)
                );
            mockedLogger.VerifyAll();
        }
    }
}

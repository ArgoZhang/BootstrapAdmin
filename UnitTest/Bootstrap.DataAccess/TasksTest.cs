using System.Linq;
using Xunit;

namespace Bootstrap.DataAccess
{
    public class TasksTest : IClassFixture<BootstrapAdminStartup>
    {
        [Fact]
        public void Retrieves_Ok()
        {
            var t = new Task();
            Assert.Equal(Enumerable.Empty<Task>(), t.Retrieves());
        }
    }
}

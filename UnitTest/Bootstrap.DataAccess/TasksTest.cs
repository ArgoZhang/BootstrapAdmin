using System.Linq;
using Xunit;

namespace Bootstrap.DataAccess
{
    [Collection("SQLServerContext")]
    public class TasksTest
    {
        [Fact]
        public void Retrieves_Ok()
        {
            Assert.Equal(Enumerable.Empty<Task>(), TaskHelper.Retrieves());
        }
    }
}

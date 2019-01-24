using Xunit;

namespace Bootstrap.Admin.Api.SQLite
{
    [Collection("SQLiteContext")]
    public class NewTest : Api.NewTest
    {
        public NewTest(SQLiteBAWebHost factory) : base(factory) { }
    }
}

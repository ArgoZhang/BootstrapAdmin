using Xunit;

namespace Bootstrap.Admin.Api.MySql
{
    [Collection("MySqlContext")]
    public class ProfilesTest : Api.ProfilesTest
    {
        public ProfilesTest(MySqlBAWebHost factory) : base(factory) { }
    }
}

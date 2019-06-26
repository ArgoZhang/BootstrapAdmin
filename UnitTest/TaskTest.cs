using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace UnitTest
{
    public class TaskTest
    {
        private ITestOutputHelper _helper;

        public TaskTest(ITestOutputHelper helper) => _helper = helper;

        [Fact]
        public async void Task_Ok()
        {
            var cts = new CancellationTokenSource(3000);
            var sw = Stopwatch.StartNew();
            try
            {
                // await Task.Delay(3000, cts.Token);

                await TestAsync(cts.Token);
            }
            catch (Exception) { }
            finally
            {
                sw.Stop();
            }
            _helper.WriteLine(sw.ElapsedMilliseconds.ToString());

            //var waiter = new CancellationTokenSource(1000);
            //var task = TestAsync(waiter.Token);
            //task.Wait(waiter.Token);
            ////try
            ////{
            ////    task.Wait(2000, waiter.Token);
            ////}
            ////catch (Exception) { }
            //sw.Stop();
        }

        private Task TestAsync(CancellationToken token) => Task.Delay(2000, token);
    }
}

using System;
using System.IO;

namespace UniTest
{
    public static class TestHelper
    {
        /// <summary>
        /// 获得当前工程解决方案目录
        /// </summary>
        /// <returns></returns>
        public static string RetrieveSolutionPath()
        {
            var dirSeparator = Path.DirectorySeparatorChar;
            var paths = AppContext.BaseDirectory.SpanSplit($"{dirSeparator}.vs{dirSeparator}");
            return paths.Count > 1 ? paths[0] : Path.Combine(AppContext.BaseDirectory, $"..{dirSeparator}..{dirSeparator}..{dirSeparator}..{dirSeparator}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public static string RetrievePath(string folder)
        {
            var soluFolder = RetrieveSolutionPath();
            return Path.Combine(soluFolder, folder);
        }
    }
}

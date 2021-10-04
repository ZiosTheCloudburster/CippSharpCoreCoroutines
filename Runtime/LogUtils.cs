namespace CippSharp.Core.Coroutines
{
    public static class LogUtils
    {
        /// <summary>
        /// Retrieve a more contextual name for logs, based on type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string LogName(System.Type type)
        {
            return $"[{type.Name}]: ";
        }
    }
}
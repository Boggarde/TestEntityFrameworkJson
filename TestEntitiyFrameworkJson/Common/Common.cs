namespace TestEntityFrameworkJson.Common
{
    public static class TimeProvider
    {
        static TimeProvider()
        {
            Reset();
        }

        /// <summary>Property for getting current time delegate.</summary>
        public static Func<DateTime> Now { get; set; }

        /// <summary>Reset current time delegate to current time.</summary>
        public static void Reset()
        {
            Now = () => DateTime.Now;
        }


        private static long lastTime;
        private static object _timeLock = new object();
        //TODO check if this function is valid in web farm environment.
        /// <summary>Get current (cached time). It is always returning unique time, which is at least 1 tick different to all other returns.</summary>
        /// <returns>Return unique current time. If 2 calls return same time, next call is moved by 1 tick.</returns>
        public static DateTime GetCurrentUniversalTime()
        {
            var obj = (object)_timeLock;
            System.Threading.Monitor.Enter(obj);
            try
            {
                // prevent concurrent access to ensure uniqueness 
                var result = DateTime.Now.ToUniversalTime();
                if (result.Ticks <= lastTime)
                {
                    result = new DateTime(lastTime + 1, DateTimeKind.Utc);

                }
                lastTime = result.Ticks;
                return result;
            }
            finally
            {
                System.Threading.Monitor.Exit(obj);
            }
        }

    }
}

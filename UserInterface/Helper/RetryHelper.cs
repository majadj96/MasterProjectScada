using System;
using System.Threading.Tasks;

namespace UserInterface.Helper
{
    public static class RetryHelper
    {
        public static Task Retry(Action action)
            => Retry(action, 3, TimeSpan.FromMilliseconds(500));

        public static Task Retry(Action action, int retryCount)
            => Retry(action, retryCount, TimeSpan.FromMilliseconds(500));

        public static async Task Retry(Action action, int retryCount, TimeSpan delay, Action afterFailure = null)
        {
            int currentCount = 0;

            while (currentCount < retryCount)
            {
                try
                {
                    action();
                    break;
                }
                catch (Exception ex)
                {
                    currentCount++;
                    if (currentCount == retryCount)
                    {
                        throw;
                    }
                    Console.WriteLine($"Failed to execute action! Retry count {currentCount}");
                    await Task.Delay(delay).ConfigureAwait(false);
                    afterFailure?.Invoke();
                }
            }
        }

        public static Task<T> Retry<T>(Func<T> function)
             => Retry(function, 3, TimeSpan.FromMilliseconds(500));

        public static Task<T> Retry<T>(Func<T> function, int retryCount)
            => Retry(function, retryCount, TimeSpan.FromMilliseconds(500));

        public static async Task<T> Retry<T>(Func<T> function, int retryCount, TimeSpan delay, Action afterFailure = null)
        {
            int currentCount = 0;

            while (true)
            {
                try
                {
                    T result = function();
                    return result;
                }
                catch (Exception ex)
                {

                    currentCount++;
                    if (currentCount == retryCount)
                    {
                        throw;
                    }
                    Console.WriteLine($"Failed to execute action! Retry count {currentCount}");
                    await Task.Delay(delay);
                    afterFailure?.Invoke();
                }
            }
        }
    }
}

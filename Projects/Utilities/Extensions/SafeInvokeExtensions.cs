using System;

namespace Utilities.Extensions
{
    public static class SafeInvokeExtensions
    {
        public static void SafeInvoke(this EventHandler eventHandler, object sender, EventArgs eventArgs)
        {
            eventHandler?.Invoke(sender, eventArgs);
        }

        public static void SafeInvoke(this EventHandler eventHandler, object sender)
        {
            eventHandler?.Invoke(sender, EventArgs.Empty);
        }

        public static void SafeInvoke(this Action action)
        {
            action?.Invoke();
        }

        public static void SafeInvoke<T>(this Action<T> action, T args)
        {
            action?.Invoke(args);
        }

        public static void SafeInvoke<T, T1>(this Action<T, T1> action, T args0, T1 args1)
        {
            action?.Invoke(args0, args1);
        }

        public static void SafeInvoke<T, T1, T2>(this Action<T, T1, T2> action, T args0, T1 args1, T2 args2)
        {
            action?.Invoke(args0, args1, args2);
        }

        public static void SafeInvoke<T>(this EventHandler<T> eventHandler, object sender, T eventArgs)
            where T : EventArgs
        {
            eventHandler?.Invoke(sender, eventArgs);
        }

        public static void SafeInvoke(this Delegate action, params object[] args)
        {
            action?.DynamicInvoke(args);
        }
    }
}
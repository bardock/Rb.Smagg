using System;
using System.Linq;

namespace Fetcher.Twitter.CLI
{
    public static class ActionExtensions
    {
        /// <summary>
        /// Generates an action that executes each specified action in order
        /// </summary>
        public static Action<T> Decorate<T>(this Action<T> @this, params Action<T>[] actions)
        {
            return actions.Aggregate(
                seed: @this,
                func: (prev, current) => x => { prev(x); current(x); });
        }
    }
}
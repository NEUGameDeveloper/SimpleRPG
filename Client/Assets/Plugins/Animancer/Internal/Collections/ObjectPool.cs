// Animancer // Copyright 2020 Kybernetik //

//#define ANIMANCER_LOG_OBJECT_POOLING

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Animancer
{
    /// <summary>Convenience methods for accessing <see cref="ObjectPool{T}"/>.</summary>
    public static class ObjectPool
    {
        /************************************************************************************************************************/

        /// <summary>
        /// Calls <see cref="ObjectPool{T}.Acquire"/> to get a spare item if there are any, or create a new one.
        /// </summary>
        public static T Acquire<T>()
            where T : class, new()
            => ObjectPool<T>.Acquire();

        /// <summary>
        /// Calls <see cref="ObjectPool{T}.Acquire"/> to get a spare item if there are any, or create a new one.
        /// </summary>
        public static void Acquire<T>(out T item)
            where T : class, new()
            => item = ObjectPool<T>.Acquire();

        /************************************************************************************************************************/

        /// <summary>
        /// Calls <see cref="ObjectPool{T}.Release"/> to add the `item` to the list of spares so it can be reused.
        /// </summary>
        public static void Release<T>(T item)
            where T : class, new()
            => ObjectPool<T>.Release(item);

        /// <summary>
        /// Calls <see cref="ObjectPool{T}.Release"/> to add the `item` to the list of spares so it can be reused.
        /// </summary>
        public static void Release<T>(ref T item) where T : class, new()
        {
            if (item != null)
            {
                ObjectPool<T>.Release(item);
                item = null;
            }
        }

        /************************************************************************************************************************/

        private const string
            NotClearError = " They must be cleared before being released to the pool and not modified after that.";

        /************************************************************************************************************************/

        /// <summary>
        /// Calls <see cref="ObjectPool{T}.Acquire"/> to get a spare <see cref="List{T}"/> if
        /// there are any or create a new one.
        /// </summary>
        public static List<T> AcquireList<T>()
        {
            var list = ObjectPool<List<T>>.Acquire();
            Debug.Assert(list.Count == 0, "A pooled list is not empty." + NotClearError);
            return list;
        }

        /// <summary>
        /// Calls <see cref="ObjectPool{T}.Release"/> to clear the `list` and mark it as a spare
        /// so it can be later returned by <see cref="AcquireList"/>.
        /// </summary>
        public static void Release<T>(List<T> list)
        {
            list.Clear();
            ObjectPool<List<T>>.Release(list);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Calls <see cref="ObjectPool{T}.Acquire"/> to get a spare <see cref="HashSet{T}"/> if
        /// there are any or create a new one.
        /// </summary>
        public static HashSet<T> AcquireSet<T>()
        {
            var set = ObjectPool<HashSet<T>>.Acquire();
            Debug.Assert(set.Count == 0, "A pooled set is not empty." + NotClearError);
            return set;
        }

        /// <summary>
        /// Calls <see cref="ObjectPool{T}.Release"/> to clear the `set` and mark it as a spare
        /// so it can be later returned by <see cref="AcquireSet"/>.
        /// </summary>
        public static void Release<T>(HashSet<T> set)
        {
            set.Clear();
            ObjectPool<HashSet<T>>.Release(set);
        }

        /************************************************************************************************************************/

        /// <summary>
        /// Calls <see cref="ObjectPool{T}.Acquire"/> to get a spare <see cref="StringBuilder"/> if
        /// there are any or create a new one.
        /// </summary>
        public static StringBuilder AcquireStringBuilder()
        {
            var builder = ObjectPool<StringBuilder>.Acquire();
            Debug.Assert(builder.Length == 0, $"A pooled {nameof(StringBuilder)} is not empty." + NotClearError);
            return builder;
        }

        /// <summary>
        /// Sets <see cref="StringBuilder.Length"/> = 0 and <see cref="ObjectPool{T}.Release"/> to mark it as a spare
        /// so it can be later returned by <see cref="AcquireStringBuilder"/>.
        /// </summary>
        public static void Release(StringBuilder builder)
        {
            builder.Length = 0;
            ObjectPool<StringBuilder>.Release(builder);
        }

        /// <summary>
        /// Calls <see cref="StringBuilder.ToString()"/> and <see cref="Release(StringBuilder)"/>.
        /// </summary>
        public static string ReleaseToString(this StringBuilder builder)
        {
            var result = builder.ToString();
            Release(builder);
            return result;
        }

        /************************************************************************************************************************/

        private static class Cache<T>
        {
            public static readonly Dictionary<MethodInfo, KeyValuePair<Func<T>, T>>
                Results = new Dictionary<MethodInfo, KeyValuePair<Func<T>, T>>();
        }

        /// <summary>
        /// Creates an object using the provided delegate and caches it so the same style can be returned when this
        /// method is called again for the same delegate.
        /// </summary>
        public static T GetCachedResult<T>(Func<T> function)
        {
            var method = function.Method;
            if (!Cache<T>.Results.TryGetValue(method, out var result))
            {

                result = new KeyValuePair<Func<T>, T>(function, function());
                Cache<T>.Results.Add(method, result);
            }
            else if (result.Key != function)
            {
                Debug.LogWarning(
                    $"{nameof(GetCachedResult)}<{typeof(T).Name}>" +
                    $" was previously called on {method.Name} with a different target." +
                    " This likely means that a new delegate is being passed into every call" +
                    " so it can't actually return the same cached object.");
            }

            return result.Value;
        }

        /************************************************************************************************************************/
    }

    /// <summary>A simple object pooling system.</summary>
    public static class ObjectPool<T> where T : class, new()
    {
        /************************************************************************************************************************/

        private static readonly List<T>
            Items = new List<T>();

        /************************************************************************************************************************/

        /// <summary>The number of spare items currently in the pool.</summary>
        public static int Count
        {
            get => Items.Count;
            set
            {
                var count = Items.Count;
                if (count < value)
                {
                    if (Items.Capacity < value)
                        Items.Capacity = value;

                    do
                    {
                        Items.Add(new T());
                        count++;
                    }
                    while (count < value);

                }
                else if (count > value)
                {
                    Items.RemoveRange(value, count - value);
                }
            }
        }

        /************************************************************************************************************************/

        /// <summary>
        /// If the <see cref="Count"/> is less than the specified value, this method increases it to that value by
        /// creating new objects.
        /// </summary>
        public static void SetMinCount(int count)
        {
            if (Count < count)
                Count = count;
        }

        /************************************************************************************************************************/

        /// <summary>The <see cref="List{T}.Capacity"/> of the internal list of spare items.</summary>
        public static int Capacity
        {
            get => Items.Capacity;
            set
            {
                if (Items.Count > value)
                    Items.RemoveRange(value, Items.Count - value);
                Items.Capacity = value;
            }
        }

        /************************************************************************************************************************/

        /// <summary>Returns a spare item if there are any, or creates a new one.</summary>
        public static T Acquire()
        {
            var count = Items.Count;
            if (count == 0)
            {
                return new T();
            }
            else
            {
                count--;
                var item = Items[count];
                Items.RemoveAt(count);

                return item;
            }
        }

        /************************************************************************************************************************/

        /// <summary>Adds the `item` to the list of spares so it can be reused.</summary>
        public static void Release(T item)
        {
            Items.Add(item);

        }

        /************************************************************************************************************************/

        /// <summary>Returns a description of the state of this pool.</summary>
        public static string GetDetails()
        {
            return
                $"{typeof(T).Name}" +
                $" ({nameof(Count)} = {Items.Count}" +
                $", {nameof(Capacity)} = {Items.Capacity}" +
                ")";
        }

        /************************************************************************************************************************/
    }
}


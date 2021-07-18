using System;
using System.Collections.Generic;
using UnityEngine;


namespace Platformer.Core
{
    /// <summary>
    /// Simulation类实现了离散事件模拟，事件池默认容量为10。这个类来自平台游戏Microgame模板。
    /// </summary>
    public static partial class Simulation
    {

        static HeapQueue<Event> eventQueue = new HeapQueue<Event>();
        static Dictionary<System.Type, Stack<Event>> eventPools = new Dictionary<System.Type, Stack<Event>>();

        /// <summary>
        /// 创建一个T类型的新事件，并将其返回。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        static public T New<T>() where T : Event, new()
        {
            Stack<Event> pool;
            if (!eventPools.TryGetValue(typeof(T), out pool))
            {
                pool = new Stack<Event>(10); //事件池容量为10
                pool.Push(new T());
                eventPools[typeof(T)] = pool;
            }
            if (pool.Count > 0)
                return (T)pool.Pop();
            else
                return new T();
        }

        /// <summary>
        /// 清除所有未决事件并将tick重置为0。
        /// </summary>
        public static void Clear()
        {
            eventQueue.Clear();
        }

        /// <summary>
        /// 安排一个事件，并将其返回。
        /// </summary>
        /// <returns>The event.</returns>
        /// <param name="tick">Tick.</param>
        /// <typeparam name="T">事件类型参数。</typeparam>
        static public T Schedule<T>(float tick = 0) where T : Event, new()
        {
            var ev = New<T>();
            ev.tick = Time.time + tick;
            eventQueue.Push(ev);
            return ev;
        }

        /// <summary>
        /// 重新安排现有事件，并将其返回。
        /// </summary>
        /// <returns>事件。</returns>
        /// <param name="tick">Tick.</param>
        /// <typeparam name="T">事件类型参数。</typeparam>
        static public T Reschedule<T>(T ev, float tick) where T : Event, new()
        {
            ev.tick = Time.time + tick;
            eventQueue.Push(ev);
            return ev;
        }

        /// <summary>
        /// 返回类的仿真模型实例。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        static public T GetModel<T>() where T : class, new()
        {
            return InstanceRegister<T>.instance;
        }

        /// <summary>
        /// 为一个类设置一个仿真模型实例。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        static public void SetModel<T>(T instance) where T : class, new()
        {
            InstanceRegister<T>.instance = instance;
        }

        /// <summary>
        /// 销毁类的仿真模型实例。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        static public void DestroyModel<T>() where T : class, new()
        {
            InstanceRegister<T>.instance = null;
        }

        /// <summary>
        /// Tick模拟。返回剩余事件的数量。如果剩余事件为零，则模拟结束，除非通过Schedule()调用，从外部系统注入事件。
        /// </summary>
        /// <returns></returns>
        static public int Tick()
        {
            var time = Time.time;
            var executedEventCount = 0;
            while (eventQueue.Count > 0 && eventQueue.Peek().tick <= time)
            {
                var ev = eventQueue.Pop();
                var tick = ev.tick;
                ev.ExecuteEvent();
                if (ev.tick > tick)
                {
                    //事件已重新安排，因此不将其返回到事件池中。
                }
                else
                {
                    // Debug.Log($"<color=green>{ev.tick} {ev.GetType().Name}</color>");
                    ev.Cleanup();
                    try
                    {
                        eventPools[ev.GetType()].Push(ev);
                    }
                    catch (KeyNotFoundException)
                    {
                        Debug.LogError($"No Pool for: {ev.GetType()}");
                    }
                }
                executedEventCount++;
            }
            return eventQueue.Count;
        }
    }
}



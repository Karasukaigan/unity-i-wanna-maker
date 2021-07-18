using System.Collections.Generic;

namespace Platformer.Core
{
    public static partial class Simulation
    {
        /// <summary>
        /// Event是在模拟中的某个时间点发生的事情。Precondition()用于检查是否应该执行该事件。这个类来自平台游戏Microgame模板。
        /// </summary>
        /// <typeparam name="Event"></typeparam>
        public abstract class Event : System.IComparable<Event>
        {
            internal float tick;

            public int CompareTo(Event other)
            {
                return tick.CompareTo(other.tick);
            }

            public abstract void Execute();

            public virtual bool Precondition() => true;

            internal virtual void ExecuteEvent()
            {
                if (Precondition())
                    Execute();
            }

            /// <summary>
            /// Cleanup()通常用于在需要时将引用设置为null。当事件完成时，Simulation会自动调用它。
            /// </summary>
            internal virtual void Cleanup()
            {

            }
        }

        /// <summary>
        /// Event<T>添加了在执行事件时挂钩OnExecute回调的功能。使用此类允许以最少或零配置将功能插入到应用程序中。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public abstract class Event<T> : Event where T : Event<T>
        {
            public static System.Action<T> OnExecute;

            internal override void ExecuteEvent()
            {
                if (Precondition())
                {
                    Execute();
                    OnExecute?.Invoke((T)this);
                }
            }
        }
    }
}
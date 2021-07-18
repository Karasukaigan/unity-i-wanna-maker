using System;
using System.Collections.Generic;

namespace Platformer.Core
{
    /// <summary>
    /// HeapQueue提供了一个始终有序的队列集合。这个类来自平台游戏Microgame模板。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HeapQueue<T> where T : IComparable<T>
    {
        List<T> items;

        public int Count { get { return items.Count; } }

        public bool IsEmpty { get { return items.Count == 0; } }

        public T First { get { return items[0]; } }

        public void Clear() => items.Clear();

        public bool Contains(T item) => items.Contains(item);

        public void Remove(T item) => items.Remove(item);

        public T Peek() => items[0];

        public HeapQueue()
        {
            items = new List<T>();
        }

        public void Push(T item)
        {
            items.Add(item); //将项目添加到树的末尾
            SiftDown(0, items.Count - 1); //找到新项目的正确位置
        }

        public T Pop()
        {
            //如果有超过一个项目，返回的项目将在树中排在第一位。然后将最后一项添加到树的前面，缩小列表并在树中为第一项找到正确的索引
            T item;
            var last = items[items.Count - 1];
            items.RemoveAt(items.Count - 1);
            if (items.Count > 0)
            {
                item = items[0];
                items[0] = last;
                SiftUp();
            }
            else
            {
                item = last;
            }
            return item;
        }


        int Compare(T A, T B) => A.CompareTo(B);

        void SiftDown(int startpos, int pos)
        {
            //保持新添加的项目
            var newitem = items[pos]; 
            while (pos > startpos)
            {
                //在二叉树中找到父索引
                var parentpos = (pos - 1) >> 1;
                var parent = items[parentpos];
                //如果新项目在父项之前或等于父项，则pos是新项目的位置
                if (Compare(parent, newitem) <= 0)
                    break;
                //否则将父项移动到pos，然后重复父项
                items[pos] = parent;
                pos = parentpos;
            }
            items[pos] = newitem;
        }

        void SiftUp()
        {
            var endpos = items.Count;
            var startpos = 0;
            //保留插入的项目
            var newitem = items[0];
            var childpos = 1;
            var pos = 0;
            //找到要插入二叉树的子位置
            while (childpos < endpos)
            {
                //得到正确的分支
                var rightpos = childpos + 1;
                //如果右分支应该在左分支之前，则将右分支向上移动
                if (rightpos < endpos && Compare(items[rightpos], items[childpos]) <= 0)
                    childpos = rightpos;
                //将子项目添加到树上
                items[pos] = items[childpos];
                pos = childpos;
                //沿着树向下移动并重复
                childpos = 2 * pos + 1;
            }
            //新项目的子位置
            items[pos] = newitem;
            SiftDown(startpos, pos);
        }
    }
}
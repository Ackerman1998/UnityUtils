using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveList<T> : List<T>
{
    public void Move(int startIndex,int endIndex) {
        if (startIndex >= Count || endIndex >= Count) return;
        T tt = this[startIndex];
        RemoveAt(startIndex);
        if (endIndex==Count) {
            Add(tt);
            return;
        }
        Insert(endIndex,tt);
    }
    /// <summary>
    /// 移动元素位置
    /// </summary>
    /// <param name="value"></param>
    /// <param name="index"></param>
    public void Move(T value, int index)
    {
        int pos = IndexOf(value);

        if (pos < 0)
        {
            Add(value);
            return;
        }
        Move(pos, index);
    }
}

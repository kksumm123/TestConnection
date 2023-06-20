using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoonyList<T> : IList<T>
{
    public T this[int index] { get => _list[index]; set => _list[index] = value; }
    public int Count => _list.Count;
    public bool IsReadOnly => false;

    private List<T> _list = new List<T>();
    private Dictionary<T, int> _cachedListIndexes = new Dictionary<T, int>();
    private T _tempItem;
    private int _index;

    public void Add(T item)
    {
        if (_cachedListIndexes.ContainsKey(item))
        {
            Debug.LogError("값이 이미 존재하면 넣을 수 없습니다.");
            return;
        }

        _list.Add(item);
        _cachedListIndexes[item] = _list.Count - 1;
    }

    public void Clear()
    {
        _list.Clear();
        _cachedListIndexes.Clear();
    }

    public bool Contains(T item)
    {
        return _cachedListIndexes.ContainsKey(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        _list.CopyTo(array, arrayIndex);
    }

    public int IndexOf(T item)
    {
        if (_cachedListIndexes.ContainsKey(item))
        {
            return _cachedListIndexes[item];
        }

        return -1;
    }

    public void Insert(int index, T item)
    {
        _list.Insert(index, item);
        _cachedListIndexes.Clear();
    }

    public bool Remove(T item)
    {
        if (_list.Count > 0
            && _cachedListIndexes.ContainsKey(item))
        {
            _tempItem = _list.GetLastItem();
            _index = _cachedListIndexes[item];

            _list.RemoveAtUnordered(_index);
            _cachedListIndexes.Remove(item);
            if (EqualityComparer<T>.Default.Equals(_tempItem, item))
            {
                return true;
            }

            _cachedListIndexes[_tempItem] = _index;
            return true;
        }

        return false;
    }

    public void RemoveAt(int index)
    {
        if (_list.Count <= index) return;

        Remove(_list[index]);
    }

    public T GetLastItem()
    {
        return _list.GetLastItem();
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _list.GetEnumerator();
    }
}

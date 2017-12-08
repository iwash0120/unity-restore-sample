using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;

namespace RestoreSample.Model
{
    /// <summary>
    /// JSON形式でファイルとしてモデルを保存するデータストア
    /// </summary>
    public abstract class JSONDataStore<T> : IDataStore<T> where T : class, IData, new()
    {
        [SerializeField]
        int lastID = 0;

        [SerializeField]
        List<T> items;

        readonly string path;

        public JSONDataStore(string path)
        {
            this.path = path;
        }

        public IEnumerator Initialize()
        {
            if (!File.Exists(path))
            {
                Save();
            }
                
            JsonUtility.FromJsonOverwrite(File.ReadAllText(path), this);

            yield break;
        }

        public T Create()
        {
            var item = new T();
            item.ID = ++lastID;

            items.Add(item);

            return item;
        }

        public void Delete(T item)
        {
            items.Remove(item);
        }

        public void Save()
        {
            var json = JsonUtility.ToJson(this);
            File.WriteAllText(path, json);
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in items)
                yield return item;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void Dispose()
        {
            //nop
        }
    }
}
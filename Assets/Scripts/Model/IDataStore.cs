using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;

namespace RestoreSample.Model
{
    public interface IDataStore<T> : IEnumerable<T>, IDisposable where T : class, IData, new()
    {
        /// <summary>
        /// データストアを初期化する
        /// </summary>
        IEnumerator Initialize();

        /// <summary>
        /// 新たなモデルを作成する
        /// </summary>
        T Create();

        /// <summary>
        /// 既存のモデルをデータストアから削除する
        /// </summary>
        /// <param name="item">Item.</param>
        void Delete(T item);

        /// <summary>
        /// データストアの状態を保存する
        /// </summary>
        void Save();
    }
}
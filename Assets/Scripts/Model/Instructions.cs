using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RestoreSample.Model
{
    [Serializable]
    public class Instructions : JSONDataStore<Instruction>
    {
        public Instructions(string path)
            : base(path)
        {
            
        }

        /// <summary>
        /// 全ての実行中の命令の状態を更新する
        /// </summary>
        /// <returns>The status.</returns>
        public List<Instruction.Status> CheckStatus()
        {
            var results = default(List<Instruction.Status>);
            foreach (var item in this)
            {
                var status = item.CheckStatus();
                if (status.needToDelete || 0 < status.newlyCompleteCount)
                {
                    if (results == null)
                        results = new List<Instruction.Status>();

                    results.Add(status);
                }
            }

            return results;
        }
    }
}
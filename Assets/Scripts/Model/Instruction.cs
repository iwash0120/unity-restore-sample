using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;

namespace RestoreSample.Model
{
    [Serializable]
    public class Instruction : IData
    {
        public int ID
        {
            get
            {
                return id;
            }
            set
            {
                this.id = value;
            }
        }

        /// <summary>
        /// 開始した時刻
        /// </summary>
        /// <value>The start date.</value>
        public DateTime StartDate
        {
            get
            {
                return DateTime.Parse(startDate);
            }
            set
            {
                this.startDate = value.ToString();
            }
        }

        /// <summary>
        /// 最後の状態の更新を行った時刻
        /// </summary>
        /// <value>The last checked date.</value>
        public DateTime LastCheckedDate
        {
            get
            {
                return DateTime.Parse(lastCheckedDate);
            }
            set
            {
                this.lastCheckedDate = value.ToString();
            }
        }

        /// <summary>
        /// 命令が1回完了するのに要する期間(秒)
        /// </summary>
        /// <value>The duration.</value>
        public int Duration
        {
            get
            {
                return duration;
            }
            set
            {
                duration = value;
            }
        }

        /// <summary>
        /// 命令が完了できる回数
        /// </summary>
        /// <value>The limit count.</value>
        public int LimitCount
        {
            get
            {
                return limitCount;
            }
            set
            {
                limitCount = value;
            }
        }

        [SerializeField]
        int id;

        [SerializeField]
        string startDate;

        [SerializeField]
        string lastCheckedDate;

        [SerializeField]
        int duration;

        [SerializeField]
        int limitCount;

        /// <summary>
        /// 状態を更新する
        /// </summary>
        /// <returns>The status.</returns>
        public Status CheckStatus()
        {
            var now = DateTime.Now;

            var alreadyElapsedTime = (int)(LastCheckedDate - StartDate).TotalSeconds;
            var alreadyCompleteCount = (alreadyElapsedTime - alreadyElapsedTime % duration) / duration;
            var elapsedTime = alreadyElapsedTime + (int)(now - LastCheckedDate).TotalSeconds;
            var completeCount = Mathf.Clamp((elapsedTime - elapsedTime % duration) / duration, 0, limitCount);

            var status = new Status();
            status.item = this;
            status.totalCompleteCount = completeCount;
            status.newlyCompleteCount = completeCount - alreadyCompleteCount;
            status.needToDelete = limitCount <= completeCount;

            LastCheckedDate = now;

            return status;
        }

        /// <summary>
        /// 命令の状態変化を表す
        /// </summary>
        public struct Status
        {
            public Instruction item { get; set; }

            /// <summary>
            /// 命令が完了した総回数
            /// </summary>
            /// <value>The total complete count.</value>
            public int totalCompleteCount { get; set; }

            /// <summary>
            /// 前回のチェック以降、新たに命令が完了した回数
            /// </summary>
            /// <value>The newly complete count.</value>
            public int newlyCompleteCount { get; set; }

            /// <summary>
            /// 命令が指定回数完了し、データストアから削除する必要が有るかどうか
            /// </summary>
            /// <value><c>true</c> if need to delete; otherwise, <c>false</c>.</value>
            public bool needToDelete { get; set; }
        }
    }
}

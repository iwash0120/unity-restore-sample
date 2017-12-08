using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Linq;
using System;

namespace RestoreSample
{
    [DisallowMultipleComponent]
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance
        {
            get
            {
                return instance;
            }
        }

        static GameManager instance;

        [SerializeField, Tooltip("1度の命令完了に掛かる時間")]
        int instructionSeconds = 5;

        [SerializeField, Tooltip("ループ型命令の回数")]
        int loopCount = 5;

        [SerializeField]
        Button btnNewInstruction;

        [SerializeField]
        Button btnNewLoopInstruction;

        [SerializeField]
        Text txtLog;

        public Model.Instructions Instructions { get; private set; }

        bool initialized = false;

        void Awake()
        {
            if (instance != null && instance != this)
            {
                GameObject.Destroy(this);
                return;
            }

            instance = this;
            DontDestroyOnLoad(this.gameObject);

            //データストアを作成
            Instructions = new Model.Instructions(Constants.InstructionsSavePath);

            //UI処理
            btnNewInstruction.onClick.AddListener(() => CreateInstruction(1));
            btnNewLoopInstruction.onClick.AddListener(() => CreateInstruction(loopCount));
            Application.logMessageReceived += (condition, stackTrace, type) =>
            {
                txtLog.text = string.Format("{0}{1}\n", txtLog.text, condition);
            };
        }

        IEnumerator Start()
        {
            //データストア初期化処理
            yield return Instructions.Initialize();

            initialized = true;

            //一定間隔で命令の完了をチェックするコルーチンを開始
            StartCoroutine(CheckInstructionCompleteCoroutine(1f));
        }

        IEnumerator CheckInstructionCompleteCoroutine(float waitTime)
        {
            var wait = new WaitForSeconds(waitTime);
            while (true)
            {
                CheckInstructionCompleteCore();
                yield return wait;
            }
        }

        void CheckInstructionCompleteCore()
        {
            //実行中の命令を走査し、新たに完了した命令が無いか調べる
            var results = Instructions.CheckStatus();
            if (results != null)
            {
                foreach (var result in results)
                {
                    if (0 < result.newlyCompleteCount)
                    {
                        Debug.LogFormat("ID: {0} が 新たに {1} 回 完了しました (合計: {2}/{3})", result.item.ID, result.newlyCompleteCount, result.totalCompleteCount, result.item.LimitCount);
                    }

                    if (result.needToDelete)
                    {
                        Instructions.Delete(result.item);
                        Debug.LogFormat("ID: {0} を削除しました", result.item.ID);
                    }
                }
                Instructions.Save();
            }
        }

        void CreateInstruction(int limitCount)
        {
            var item = Instructions.Create();
            item.StartDate = item.LastCheckedDate = DateTime.Now;
            item.Duration = instructionSeconds;
            item.LimitCount = limitCount;
            Instructions.Save();

            if (limitCount == 1)
            {
                Debug.LogFormat("ID: {0} の命令が開始されました. {1}秒後に完了します.", item.ID, instructionSeconds);
            }
            else
            {
                Debug.LogFormat("ID: {0} の命令(繰り返し)が開始されました. {1}秒毎に完了します.", item.ID, instructionSeconds);
            }
        }

        void OnApplicationPause(bool paused)
        {
            if (!initialized)
                return;

            if (paused)
            {
                Debug.Log("アプリケーションがバックグラウンドに回りました");
            }
            else
            {
                Debug.Log("アプリケーションがアクティブになりました");
            }

            CheckInstructionCompleteCore();
        }

        void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;

                Instructions.Dispose();
            }
        }
    }
}
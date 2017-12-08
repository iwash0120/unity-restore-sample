using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

namespace RestoreSample
{
    public static class MyMenu
    {
        [MenuItem("MyMenu/セーブデータをリセット")]
        public static void ResetInstructionSaveData()
        {
            if (File.Exists(Constants.InstructionsSavePath))
            {
                File.Delete(Constants.InstructionsSavePath);
            }
        }

        [MenuItem("MyMenu/セーブデータが保存されているパスを表示")]
        public static void CheckSaveDataPath()
        {
            Debug.Log(Constants.InstructionsSavePath);
        }
    }
}
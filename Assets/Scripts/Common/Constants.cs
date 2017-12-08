using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace RestoreSample
{
    public static class Constants
    {
        public static string InstructionsSavePath
        {
            get
            {
                return Path.Combine(Application.persistentDataPath, "t_instructions.json");
            }
        }
    }
}
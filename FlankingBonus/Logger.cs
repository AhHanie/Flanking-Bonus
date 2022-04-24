using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HarmonyLib;
using Verse;

namespace SK.FlankingBonus
{
    public static class Logger
    {
        // Harmony Log File
        public static void WriteToHarmonyFile(string message)
        {
            if (Mod.SHOULD_PRINT_LOG)
                FileLog.Log(message);
        }

        public static void WriteToGameConsole(string message)
        {
            if (Mod.SHOULD_PRINT_LOG)
                Log.Message(message);
        }
    }
}

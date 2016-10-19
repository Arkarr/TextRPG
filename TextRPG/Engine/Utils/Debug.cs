using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Engine.Utils
{
    public enum DebugMessageType
    {
        Message,
        Warning,
        Error
    }

    public static class Debug
    {
        public static bool DebugMessagesEnabled { get; set; }

        static Debug()
        {
            DebugMessagesEnabled = false;
        }

        public static void Log(object instance, string message, DebugMessageType type = DebugMessageType.Message, [CallerFilePathAttribute]string callerFile = "")
        {
            if (!DebugMessagesEnabled) return;

            EngineObject obj = instance as EngineObject;

            if (type == DebugMessageType.Message) LogMessage(message, callerFile, obj);
            else if (type == DebugMessageType.Warning) LogWarning(message, callerFile, obj);
            else LogError(message, callerFile, obj);
        }

        private static void Log (string prefix, string message, [CallerFilePathAttribute]string callerFile = "", EngineObject instance = null)
        {
            Console.WriteLine(string.Format("{0} <{1}>@{2}{3} : {4}", prefix, DateTime.Now.ToString("HH:mm:ss"), Path.GetFileNameWithoutExtension(callerFile), (instance != null ? "::" + instance.Name : ""), message));
        }

        private static void LogMessage (string text, [CallerFilePathAttribute]string callerFile = "", EngineObject instance = null)
        {
            Log("MSG", text, callerFile, instance);
        }

        private static void LogWarning(string text, [CallerFilePathAttribute]string callerFile = "", EngineObject instance = null)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Log("WRN", text, callerFile, instance);
            Console.ResetColor();
        }
        private static void LogError(string text, [CallerFilePathAttribute]string callerFile = "", EngineObject instance = null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Log("ERR", text, callerFile, instance);
            Console.ResetColor();
        }
    }
}

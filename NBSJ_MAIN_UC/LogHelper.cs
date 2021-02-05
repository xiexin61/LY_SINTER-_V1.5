using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;



namespace NBSJ_MAIN_UC
{
    public class LogHelper
    {

        private static string Sinter_Log = "MAIN_UC";
        private static readonly Thread WriteThread;
        private static readonly Queue<string> MsgQueue;
        private static readonly object FileLock;


        private static readonly string FilePath;


        static LogHelper()
        {
            FileLock = new object();
            FilePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            FilePath = FilePath.Replace("\\Exe\\", "\\Log\\");
            WriteThread = new Thread(WriteMsg);
            MsgQueue = new Queue<string>();
            WriteThread.Start();
        }

        public static void Exit()
        {
            MsgQueue.Clear();
            WriteThread.Abort();

        }


        public static void LogInfo(string msg)
        {
            Monitor.Enter(MsgQueue);
            MsgQueue.Enqueue(string.Format("{1}_{0}   {1}   {2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss"), "INFO", msg));
            Monitor.Exit(MsgQueue);
        }
        public static void LogError(string msg)
        {
            Monitor.Enter(MsgQueue);
            MsgQueue.Enqueue(string.Format("{1}_{0}   {1}   {2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss"), "ERROR", msg));
            Monitor.Exit(MsgQueue);
        }
        public static void LogWarn(string msg)
        {
            Monitor.Enter(MsgQueue);
            MsgQueue.Enqueue(string.Format("{1}_{0}   {1}   {2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss"), "WARN", msg));
            Monitor.Exit(MsgQueue);
        }
        public static void LogBlue(string msg)
        {
            Monitor.Enter(MsgQueue);
            MsgQueue.Enqueue(string.Format("{1}_{0}   {1}   {2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss"), "BLUE", msg));
            Monitor.Exit(MsgQueue);
        }
        public static void LogWhite(string msg)
        {
            Monitor.Enter(MsgQueue);
            MsgQueue.Enqueue(string.Format("{1}_{0}   {1}   {2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss"), "WHITE", msg));
            Monitor.Exit(MsgQueue);
        }
        private static ConsoleColor GetMsgColor(string eventType)
        {
            switch (eventType)
            {
                case "INFO ":
                    return ConsoleColor.Green;

                case "ERROR":
                    return ConsoleColor.Red;

                case "WARN ":
                    return ConsoleColor.Yellow;

                case "BLUE ":
                    return ConsoleColor.Cyan;

                case "WHITE":
                    return ConsoleColor.White;
            }
            return ConsoleColor.Green;
        }

        private static void WriteMsg()
        {
            while (true)
            {
                if (MsgQueue.Count > 0)
                {
                    Monitor.Enter(FileLock);
                    string newPath = FilePath + "Logs" + "\\" + Sinter_Log + "\\";
                    if (!Directory.Exists(newPath))
                    {
                        Directory.CreateDirectory(newPath);
                    }
                    string fileName = newPath + DateTime.Now.ToString("yyyy_MM_dd") + ".txt";
                    try
                    {
                        var logStreamWriter = new StreamWriter(fileName, true);
                        Monitor.Enter(MsgQueue);
                        string msg = MsgQueue.Dequeue();
                        Monitor.Exit(MsgQueue);

                        ConsoleColor foregroundColor = Console.ForegroundColor;
                        Console.ForegroundColor = GetMsgColor(msg.Substring(0, msg.IndexOf("_")));
                        Console.WriteLine(msg);
                        Console.ForegroundColor = ConsoleColor.White;

                        logStreamWriter.WriteLine(msg);
                        logStreamWriter.Close();
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine(ex.Message);
                    }
                    Monitor.Exit(FileLock);

                }

                Thread.Sleep(1);
            }
        }

    }
}

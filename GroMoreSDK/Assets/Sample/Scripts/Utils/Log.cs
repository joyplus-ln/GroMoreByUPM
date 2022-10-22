using UnityEngine;

namespace ByteDance.Union
{
    public static class Log
    {
        // 方便查阅的 tag
        private const string CommonTAG = "Unity-Demo:";

        // log.v
        private const int VERBOSE = 2;

        // log.d debug 级别
        private const int DEBUG = 3;

        // log.e 错误级别
        private const int ERROR = 6;

        // 当前日志登记
        private static readonly int Level = VERBOSE;


        // debug
        public static void D(string msg)
        {
            if (Level <= DEBUG)
            {
                Debug.Log(CommonTAG + msg);
            }
        }

        // error
        public static void E(string msg)
        {
            if (Level <= ERROR)
            {
                Debug.LogError(CommonTAG + msg);
            }
        }


        // debug模式，格式化信息
        public static void DebugFormat(string format, params object[] args)
        {
            if (Level <= DEBUG)
            {
                Debug.LogFormat(CommonTAG + format, args);
            }
        }

        // error模式，格式化信息
        public static void ErrorFormat(string format, params object[] args)
        {
            if (Level <= ERROR)
            {
                Debug.LogErrorFormat(CommonTAG + format, args);
            }
        }
    }
}
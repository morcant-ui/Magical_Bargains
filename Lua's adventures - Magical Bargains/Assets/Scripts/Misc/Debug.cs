using UnityEngine;

namespace DebugStuff
{
    public class Debug : MonoBehaviour
    {
        //#if !UNITY_EDITOR
        static string myLog = "";
        private string output;
        private string stack;
        bool doShow = false;

        void Update() { if (Input.GetKeyDown(KeyCode.Escape)) { doShow = !doShow; } }

        void OnEnable()
        {
            Application.logMessageReceived += Log;
        }

        void OnDisable()
        {
            Application.logMessageReceived -= Log;
        }

        public void Log(string logString, string stackTrace, LogType type)
        {
            output = logString;
            stack = stackTrace;
            myLog = output + "\n" + myLog;
            if (myLog.Length > 5000)
            {
                myLog = myLog.Substring(0, 4000);
            }
        }

        void OnGUI()
        {
            //if (!Application.isEditor) //Do not display in editor ( or you can use the UNITY_EDITOR macro to also disable the rest)
            if (!doShow) { return; }
            {
                myLog = GUI.TextArea(new Rect(10, 10, Screen.width /2, Screen.height /2), myLog);
            }
        }
        //#endif
    }
}
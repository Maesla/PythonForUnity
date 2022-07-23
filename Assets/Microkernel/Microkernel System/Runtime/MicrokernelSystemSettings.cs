using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;

namespace MicrokernelSystem
{
    /// <summary>
    /// This serializable class provides the functionality to save and get all type of information that the microkernel system would need
    /// </summary>
    [Serializable]
    public class MicrokernelSystemSettings
    {
        private readonly JObject jobject;

        public MicrokernelSystemSettings(string json):this(JObject.Parse(json)){}

        private MicrokernelSystemSettings(JObject jobject)
        {
            this.jobject = jobject;
        }

        public T GetValue<T>(string key)
        {
            JToken token = jobject.GetValue(key);
            return token.Value<T>();
        }

        public IEnumerable<T> GetValues<T>(string key)
        {
            JToken token = jobject.GetValue(key);
            return token.Values<T>();
        }

        public MicrokernelSystemSettings GetSubSettings(string key)
        {
            JToken token = jobject.GetValue(key);
            return new MicrokernelSystemSettings((JObject)token);
        }

        public T FromJson<T>()
        {
            return jobject.ToObject<T>();
        }

        public static MicrokernelSystemSettings DefaultSettings
        {
            get
            {
                string template = $"{_settingsTemplate}";
                template.Replace("{PYTHONDLLPATH}", GetPythonDllPath());
                return new MicrokernelSystemSettings(template);          
            }
        }

        public static MicrokernelSystemSettings Empty
        {
            get
            {
                return new MicrokernelSystemSettings(_settingsTemplate);
            }
        }

        private static string GetPythonDllPath()
        {
            if (File.Exists(_defaultPython39Path))
            {

                return _defaultPython39Path;
            }
            else
            {
                throw new Exception("User does not have python 3.9 installed on their machine");
            }
        }

        private const string _defaultPython39Path = "C:\\Program Files\\Python\\Python39\\python39.dll";

        private static string _settingsTemplate =
            @"
                {
                  ""pathRoots"": [
                        ""Assets/StreamingAssets/Plugins/"",
                        ""SmartWorld Professional_Data\\StreamingAssets\\Plugins""
                    ],
                  ""pythonEngineSettings"": {
                                    ""dllPath"": ""{PYTHONDLLPATH}""
                    }
                }
            ";
    }
}

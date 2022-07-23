using System;
using System.IO;
using UnityEngine;

namespace MicrokernelSystem
{
    [Serializable]
    public class Contract : IContract
    {
        [SerializeField]
        private string name;

        [SerializeField]
        private string id;

        [SerializeField]
        private string author;

        [SerializeField]
        private string version;

        public string Name => name;
        public string Id => id;
        public string Author => string.IsNullOrEmpty(author) ? string.Empty : author;
        public SemanticVersioning Version => string.IsNullOrEmpty(version) ? new SemanticVersioning() : new SemanticVersioning(version);

        public bool Validate()
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Name is null");
            }
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException($"Id null in plugin {Name}");
            }

            return true;
        }

        public static Contract Create(string path)
        {
            Contract contract = null;

            if (File.Exists(path))
            {
                string contractStr = File.ReadAllText(path);
                contract = JsonUtility.FromJson<Contract>(contractStr);
            }

            return contract;
        }
    } 
}

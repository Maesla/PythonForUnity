using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace MicrokernelSystem
{
    /// <summary>
    /// This class provides semantic versioning functionality, such as 0, 0.1, 0.1.2, 0.1.2.3, etc.
    /// </summary>
    public class SemanticVersioning
    {
        public int[] Version { get => version; }
        public string VersionStr { get => versionStr; }
        public int Deep => Version.Length;

        private readonly int[] version;
        private readonly string versionStr;

        public SemanticVersioning():this("0.0.0")
        { }

        public SemanticVersioning(string versionStr)
        {
            if (!SyntaxCheck(versionStr))
            {
                throw new ArgumentException("Version must have the Semantic Versioning Syntax. Example: 1.2.12.102, 1.4.3.5249, 100.0.1.0");
            }
            version = StrToArray(versionStr);
            this.versionStr = versionStr;
        }

        private static bool SyntaxCheck(string versionStr)
        {
            //start line, zero or more times of digit plus dot (123.). After a digit. After, end line
            string pattern = @"^(\d+\.)*(\d+)$";
            // Create a Regex  
            Regex regex = new Regex(pattern);
            Match match = regex.Match(versionStr);
            return match.Success;
        }

        public SemanticVersioning(int[] version)
        {
            if (version.Any(v => v < 0))
            {
                throw new ArgumentException("Versions must be greater or equal to zero");
            }

            this.version = version;
            this.versionStr = ArrayToStr(version);
        }

        private static int[] StrToArray(string str)
        {
            return   
                str
                .Split('.')
                .Select(v => int.Parse(v))
                .ToArray();
        }

        public override bool Equals(object o)
        {
            if (o == null)
            {
                return false;
            }

            var second = o as SemanticVersioning;

            return second != null && this == second;
        }

        public override int GetHashCode()
        {
            return versionStr.GetHashCode();
        }

        private static string ArrayToStr(int[] v)
        {
            return string.Join(".", v.Select(e => e.ToString()));
        }

        public override string ToString()
        {
            return versionStr;
        }

        public static bool operator ==(SemanticVersioning v1, SemanticVersioning v2)
        {
            if (ReferenceEquals(v1, v2))
            {
                return true;
            }
            
            if (ReferenceEquals(v1, null) || ReferenceEquals(v2, null))
            {
                return false;
            }

            int maxDeep = Mathf.Max(v1.Deep, v2.Deep);

            int[] versions1 = ZeroPadding(v1.version, maxDeep);
            int[] versions2 = ZeroPadding(v2.version, maxDeep);

            for (int i = 0; i < maxDeep; i++)
            {
                if (versions1[i] != versions2[i])
                {
                    return false;
                }
            }

            return true;
        }

        public static bool operator !=(SemanticVersioning v1, SemanticVersioning v2)
        {
            if (ReferenceEquals(v1, v2))
            {
                return false;
            }

            if (ReferenceEquals(v1, null) || ReferenceEquals(v2, null))
            {
                return true;
            }

            int maxDeep = Mathf.Max(v1.Deep, v2.Deep);

            int[] versions1 = ZeroPadding(v1.version, maxDeep);
            int[] versions2 = ZeroPadding(v2.version, maxDeep);

            for (int i = 0; i < maxDeep; i++)
            {
                if (versions1[i] != versions2[i])
                {
                    return true;
                }
            }

            return false;
        }
        public static bool operator <(SemanticVersioning v1, SemanticVersioning v2)
        {
            int maxDeep = Mathf.Max(v1.Deep, v2.Deep);

            int[] versions1 = ZeroPadding(v1.version, maxDeep);
            int[] versions2 = ZeroPadding(v2.version, maxDeep);

            for (int i = 0; i < maxDeep; i++)
            {
                if (versions1[i] == versions2[i])
                {
                    continue;
                }

                if (versions1[i] < versions2[i])
                {
                    return true;
                }
                else if (versions1[i] > versions2[i])
                {
                    return false;
                }
            }

            return false;
        }

        public static bool operator >(SemanticVersioning v1, SemanticVersioning v2)
        {
            int maxDeep = Mathf.Max(v1.Deep, v2.Deep);

            int[] versions1 = ZeroPadding(v1.version, maxDeep);
            int[] versions2 = ZeroPadding(v2.version, maxDeep);

            for (int i = 0; i < maxDeep; i++)
            {
                if (versions1[i] == versions2[i])
                {
                    continue;
                }

                if (versions1[i] > versions2[i])
                {
                    return true;
                }
                else if (versions1[i] < versions2[i])
                {
                    return false;
                }
            }

            return false;
        }

        public static bool operator <=(SemanticVersioning v1, SemanticVersioning v2)
        {
            int maxDeep = Mathf.Max(v1.Deep, v2.Deep);

            int[] versions1 = ZeroPadding(v1.version, maxDeep);
            int[] versions2 = ZeroPadding(v2.version, maxDeep);

            for (int i = 0; i < maxDeep; i++)
            {
                if (versions1[i] <= versions2[i])
                {
                    continue;
                }

                if (versions1[i] > versions2[i])
                {
                    return false;
                }
            }

            return true;

        }
        public static bool operator >=(SemanticVersioning v1, SemanticVersioning v2)
        {
            int maxDeep = Mathf.Max(v1.Deep, v2.Deep);

            int[] versions1 = ZeroPadding(v1.version, maxDeep);
            int[] versions2 = ZeroPadding(v2.version, maxDeep);

            for (int i = 0; i < maxDeep; i++)
            {
                if (versions1[i] > versions2[i])
                {
                    return true;
                }

                if (versions1[i] == versions2[i])
                {
                    continue;
                }

                if (versions1[i] < versions2[i])
                {
                    return false;
                }
            }

            return true;

        }

        /// <summary>
        /// Add zeros at the end
        /// </summary>
        /// <remarks>This is useful for array comparation, so the have the same size</remarks>
        /// <example> ZeroPadding({1,2,3}, 5) => {1,2,3,0,0}</example>
        public static int[] ZeroPadding(int[] array, int length)
        {
            if (array.Length >= length)
            {
                return array;
            }

            int [] newArray = new int[length];

            Array.Copy(array, newArray, array.Length);

            return newArray;
        }
    } 
}

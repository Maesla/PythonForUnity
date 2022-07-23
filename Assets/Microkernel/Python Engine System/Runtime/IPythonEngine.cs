using System;
using System.Collections.Generic;

namespace PythonEngineSystem
{
    /// <summary>
    /// Provides a mechanism to run python code
    /// </summary>
    public interface IPythonEngine : IDisposable
    {
        /// <summary>
        /// Execute python code an returns the result
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        string Execute(string code);

        /// <summary>
        /// Sets a variable inside the python scope
        /// </summary>
        /// <param name="name">the variable name</param>
        /// <param name="value">the variable value</param>
        void SetVariable(string name, object value);
        
        /// <summary>
        /// Gets a variable inside the python scope
        /// </summary>
        /// <typeparam name="T">the varible type</typeparam>
        /// <param name="name">the variable name</param>
        /// <returns>the variable</returns>
        T GetVariable<T>(string name);

        IList<string> SearchPaths();

        void SetSearchPath(IList<string> paths);

    } 
}

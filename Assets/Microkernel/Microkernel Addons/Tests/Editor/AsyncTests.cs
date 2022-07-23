using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine.TestTools;
using Unity.EditorCoroutines.Editor;

namespace MicrokernelSystem.Addons.Editor.Tests
{
    public class AsyncTests
    {
        private readonly string testVariableName = "test_variable";
        private int initialValue = 17;
        private readonly int increment = 10;
        
        private CallbackProvider callbackProvider;
        private PythonRunnableContextFactory pythonFactory;
        private PluginSettings settings;

        [SetUp]
        public void SetUp()
        {
            callbackProvider = new CallbackProvider(increment);
            pythonFactory = new PythonRunnableContextFactory(new List<IAPIProvider>() { callbackProvider });
            pythonFactory.Initialize();

            settings = PluginSettings.Default();

        }

        [TearDown]
        public void TearDown()
        {
            pythonFactory.Finish();
        }

        /// <summary>
        /// Test that we can use a shared delegate between python and c#
        /// </summary>
        /// <remarks>
        /// Python creates a delegate. That delegates go to c# and back to python
        /// </remarks>
        [Test]
        public void CallbackTest()
        {
            settings.code = GetCode(nameof(callbackProvider.CallbackTest));
            PythonRunnableContext context = pythonFactory.Create(settings) as PythonRunnableContext;
            context.Execute();
            
            int expected = initialValue + increment;
            int testValue = context.GetVariable<int>(testVariableName);
            Assert.That(testValue, Is.EqualTo(expected));
            
        }

        /// <summary>
        /// Test that we can use a delayed delegate between python and c#
        /// </summary>
        /// <remarks>
        /// Python creates a delegate. That delegates go to c# and it gets delayed (server query, for example) and back to python
        /// </remarks>
        [UnityTest]
        public IEnumerator AsyncTest()
        {
            settings.code = GetCode(nameof(callbackProvider.AsyncTest));
            PythonRunnableContext context = pythonFactory.Create(settings) as PythonRunnableContext;
            context.Execute();

            while (!callbackProvider.isReady)
            {
                yield return null;
            }

            int testValue = context.GetVariable<int>(testVariableName);
            int expected = initialValue + increment;
            Assert.That(testValue, Is.EqualTo(expected));
            
        }

        private string GetCode(string methodName)
        {
            return
$@"
from System import Action

{testVariableName} = {initialValue}

def Increment(inc):
    global {testVariableName}
    {testVariableName} = {testVariableName} + inc

callback = Action[int](Increment)
callbackProvider.{methodName}(callback)
";
        }

        private class CallbackProvider : BasePythonAPIProvider
        {

            public int increment;
            public bool isReady = false;

            public CallbackProvider(int increment)
            {
                this.increment = increment;
            }

            public void CallbackTest(Action<int> func)
            {
                func(increment);
                isReady = true;
            }

            public void AsyncTest(Action<int> func)
            {
                EditorCoroutineUtility.StartCoroutine(ITest(func), this);
            }

            private IEnumerator ITest(Action<int> func)
            {
                yield return null;
                yield return null;
                yield return null;
                func(increment);
                isReady = true;
            }
        }
    } 
}

using NUnit.Framework;
using System;

namespace PythonEngineSystem.Editor.Tests
{
    public class PythonEngineSytemTests
    {
        private PythonEngine engine;

        [OneTimeSetUp]
        public void Setup()
        {
            PythonEngine.Startup();
            engine = new PythonEngine(new PythonLogger());
            engine.Initialize();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            engine.Finish();
            PythonEngine.Shutdown();
        }

        [Test]
        public void ExecuteCommandTest()
        {
            using (engine)
            {
                int a = 1;
                int b = 2;
                engine.SetVariable("a", a);
                engine.SetVariable("b", b);
                string code = "result = a + b\nprint(result)";
                string msg = engine.Execute(code);
                int result = engine.GetVariable<int>("result");
                Assert.That(result, Is.EqualTo(a + b));
                Assert.That(msg, Is.EqualTo((a + b).ToString()));
            }
        }

        [Test]
        public void CompileError()
        {
            void ThrowException()
            {
                string code = "result = a + 1\n";
                string result = engine.Execute(code);
            }

            Assert.That(ThrowException, Throws.InstanceOf<Exception>().With.Message.Contains("name 'a' is not defined"));
        }

        [Test]
        public void InitializationTest()
        {
            PythonEngine.Shutdown();
            using (engine)
            {
                string code = "a = 1+1";
                Assert.That(() => engine.Execute(code), Throws.InstanceOf<InvalidOperationException>());
            }
        }
    } 
}

using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MicrokernelSystem.Addons.Editor.Tests
{
    public class PythonAPIProvidersTests
    {
        [Test]
        public void UnityProvider()
        {
            using (var runner = new Runner<APITest>())
            {
                Assert.That(() => runner.Python.Execute("GameObject.CreatePrimitive(PrimitiveType.Plane)"), Throws.InstanceOf<Exception>());
                GameObject plane = GameObject.Find("Plane");
                Assert.That(plane, Is.EqualTo(null));
            }

            using (var runner = new Runner<UnityApiProvider>())
            {
                runner.Python.Execute("GameObject.CreatePrimitive(PrimitiveType.Plane)");
                GameObject plane = GameObject.Find("Plane");
                Assert.That(plane, Is.Not.EqualTo(null));
            }
        }

        [Test]
        public void ObjectProvider()
        {
            using (var runner = new Runner<APITest>())
            {
                float expectedPosition = 12345;
                runner.Python.Execute($"test.Create({expectedPosition})");
                GameObject test = GameObject.Find("test");
                Assert.That(test.transform.position.z, Is.EqualTo(expectedPosition));
                UnityEngine.Object.DestroyImmediate(test);
            }
        }

        [Test]
        public void GetTest()
        {
            using (var runner = new Runner<APITest>())
            {
                string variable = "primitive";
                runner.Python.Execute($"{variable} = test.Create(0)");
                GameObject testFind = GameObject.Find("test");
                GameObject testGet = runner.Python.GetVariable<GameObject>(variable);
                Assert.That(testFind, Is.EqualTo(testGet));
                UnityEngine.Object.DestroyImmediate(testFind);
            }
        }

        [Test]
        public void SetTest()
        {
            using (var runner = new Runner<UnityApiProvider>())
            {
                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                string variable = "sphere";
                float expectedZPosition = 100f;
                runner.Python.SetVariable(variable, go);
                var code = FormattableString.Invariant($"{variable}.transform.position = Vector3(0.0,0.0,{expectedZPosition:F1})");
                runner.Python.Execute(code);
                Assert.That(go.transform.position.z, Is.EqualTo(expectedZPosition));
                UnityEngine.Object.DestroyImmediate(go);
            }
        }

        private class Runner<T> :IDisposable where T: IPyhonAPIProvider, new()
        {
            public PythonRunnableContext Python => python;
            private readonly PythonRunnableContext python;

            public Runner()
            {
                T api = new T();
                python = new PythonRunnableContext(new List<IPyhonAPIProvider>() { api });
                PythonRunnableContext.Startup();
                python.Initialize();
            }


            public void Dispose()
            {
                python.Finish();
                PythonRunnableContext.Shutdown();
            }
        }

        private class APITest : IPyhonAPIProvider
        {
            public string Code => string.Empty;

            public virtual PythonVariable[] Variables => new PythonVariable[] { new PythonVariable { name = "test", obj = this } };


            public string[] Types => new string[] { "Python" };
            public SemanticVersioning Version => new SemanticVersioning("0.0.0");

            public GameObject Create(float z)
            {
                GameObject primitive = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                primitive.name = "test";
                primitive.transform.position = Vector3.forward * z;

                return primitive;
            }
        }
    } 
}

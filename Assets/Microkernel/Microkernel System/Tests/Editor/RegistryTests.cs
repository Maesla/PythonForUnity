
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;

namespace MicrokernelSystem.Editor.Tests
{
    public class RegistryTests
    {
        private Registry register;
        private Microkernel microkernel;

        [SetUp]
        public void SetUp()
        {
            register = new Registry();
            var factory = Substitute.For<IRunnableContextFactory>();
            factory.Type.Returns("Test");
            microkernel = new Microkernel(new List<IRunnableContextFactory>() { factory }, null, register);
            microkernel.Initialize();
        }

        [TearDown]
        public void TearDown()
        {
            microkernel.Finish();
        }

        [Test]
        public void AddTest()
        {

            PluginSettings settings = new PluginSettings() { type = "Test" };
            microkernel.Execute(settings);
            microkernel.Execute(settings);
            microkernel.Execute(settings);
            microkernel.Execute(settings);

            Assert.That(register.Count, Is.EqualTo(4));
        }

        [Test]
        public void EventTest()
        {
            PluginSettings settings = new PluginSettings() { type = "Test" };
            bool wasCalled = false;
            register.OnRunAdded += r => wasCalled = true;
            Assert.That(wasCalled, Is.False);
            microkernel.Execute(settings);
            Assert.That(wasCalled, Is.True);
        }
    } 
}

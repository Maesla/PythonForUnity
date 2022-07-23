using NSubstitute;
using NUnit;
using NUnit.Framework;
using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace MicrokernelSystem.Editor.Tests
{
    public class PluginExplorerTests
    {
        [Test]
        public void ContractTests()
        {
            string json = @"{""name"": ""Test Name""}";
            Contract contract = JsonUtility.FromJson<Contract>(json);
            Assert.That(contract.Name, Is.EqualTo("Test Name"));
        }

        [Test]
        public void ExploreTest()
        {
            string rootPath = GetPluginPath();

            PluginExplorer pluginExplorer = new PluginExplorer
            {
                RootPaths = new string[] { rootPath }
            };
            pluginExplorer.Explore();

            Assert.That(pluginExplorer.Plugins, Has.Length.EqualTo(2));

            IPlugin pluginA = pluginExplorer.Plugins[0];
            Assert.That(pluginA.Contract.Name, Is.EqualTo("Plugin A"));
            Assert.That(pluginA.Contract.Id, Is.Not.Empty);
            Assert.That(pluginA.Contract.Author, Is.EqualTo("Author"));
            Assert.That(pluginA.Contract.Version, Is.EqualTo(new SemanticVersioning("1.2.3")));
            string codeA = "go = GameObject.CreatePrimitive(PrimitiveType.Cube)\ngo.transform.position = Vector3(6.0,0.0,-2.0)";
            Assert.That(pluginA.Code.GetCode(), Is.EqualTo(codeA).Using<string>(StringComparerNewLines));
            Assert.That(pluginA.UI.GetCode(), Is.Not.Empty);
            Assert.That(pluginA.Icon, Is.Not.Null);
            Assert.That(pluginA.Icon.Texture, Is.Not.Null);
            Assert.That(pluginA.Icon.Sprite, Is.Not.Null);

            IPlugin pluginB = pluginExplorer.Plugins[1];
            Assert.That(pluginB.Contract.Name, Is.EqualTo("Plugin B"));
            Assert.That(pluginB.Contract.Id, Is.Not.Empty);
            Assert.That(pluginB.Contract.Author, Is.Empty);
            Assert.That(pluginB.Contract.Version, Is.EqualTo(new SemanticVersioning()));
            string codeB = "go = GameObject.CreatePrimitive(PrimitiveType.Sphere)\ngo.transform.position = Vector3(8.0, 0.0, -2.0)";
            Assert.That(pluginB.Code.GetCode(), Is.EqualTo(codeB).Using<string>(StringComparerNewLines));
            Assert.That(pluginB.UI.GetCode(), Is.Not.Empty);
            Assert.That(pluginB.Icon, Is.Not.Null);
            Assert.That(pluginB.Icon.Texture, Is.Null);
            Assert.That(pluginB.Icon.Sprite, Is.Null);
        }

        [Test]
        public void DuplicatedTest()
        {
            var pluginA = Substitute.For<IPlugin>();
            var pluginB = Substitute.For<IPlugin>();
            pluginA.Contract.Name.Returns("A");
            pluginA.Contract.Id.Returns("000");
            pluginB.Contract.Name.Returns("B");
            pluginB.Contract.Id.Returns("000");
            PluginExplorer pluginExplorer = new PluginExplorer();
            TestDelegate checkUniques = () => pluginExplorer.CheckUniqueIds(new[] { pluginA, pluginB });
            var ex = Assert.Throws<InvalidOperationException>(checkUniques);
            Assert.That(ex.Message, Does.Contain("B").And.Contain("000"));
            pluginB.Contract.Id.Returns("001");
            Assert.DoesNotThrow(checkUniques);

        }

        private static string GetPluginPath()
        {
            string fileName = new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName();
            string rootPath = Path.GetDirectoryName(fileName);
            rootPath = Path.Combine(rootPath, "Plugins");
            return rootPath;
        }

        private static int StringComparerNewLines(string a, string b)
        {
            string newLines = @"\r\n|\n\r|\n|\r";
            a = Regex.Replace(a, newLines, Env.NewLine);
            b = Regex.Replace(b, newLines, Env.NewLine);
            return string.Compare(a, b);
        }
    } 
}

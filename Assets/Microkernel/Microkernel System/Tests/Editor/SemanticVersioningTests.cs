using NUnit.Framework;
using System;

namespace MicrokernelSystem.Editor.Tests
{
    public class SemanticVersioningTests
    {
        [TestCase("1", new int[] {1}, 1)]
        [TestCase("1.2.3", new int[] {1,2,3}, 3)]
        [TestCase("6.2", new int[] {6,2}, 2)]
        public void CreationStringTest(string versionStr, int[] expectedVersion, int expectedDeep)
        {
            var semanticVersioning = new SemanticVersioning(versionStr);
            CollectionAssert.AreEqual(expectedVersion, semanticVersioning.Version);
            Assert.That(semanticVersioning.Deep, Is.EqualTo(expectedDeep));
        }

        [TestCase(new int[] {1}, "1")]
        [TestCase(new int[] {1,2}, "1.2")]
        [TestCase(new int[] {1,2,333}, "1.2.333")]
        public void CreationArrayTest(int[] version, string expectedVersion)
        {
            var semantincVersioning = new SemanticVersioning(version);
            Assert.That(semantincVersioning.VersionStr, Is.EqualTo(expectedVersion));
        }

        [TestCase("2.1.")]
        [TestCase("2.a.0")]
        [TestCase("2mm.a.0")]
        public void CreationStringException(string versionStr)
        {
            Assert.Throws<ArgumentException>(() => new SemanticVersioning(versionStr));
        }

        [TestCase(new int[] { -1 })]
        [TestCase(new int[] { 1, 0, -20 })]
        public void CreationArrayException(int[] version)
        {
            Assert.Throws<ArgumentException>(() => new SemanticVersioning(version));
        }

        [TestCase("1.2.3", "1.2.3", ExpectedResult = true)]
        [TestCase("1.2.3", "1.2.2", ExpectedResult = false)]
        [TestCase("1", "1.2.3", ExpectedResult = false)]
        [TestCase("1.0.0.0", "1", ExpectedResult = true)]
        public bool EqualTest(string v1, string v2)
        {
            var sv1 = new SemanticVersioning(v1);
            var sv2 = new SemanticVersioning(v2);
            return sv1 == sv2;
        }

        [TestCase("1.2.3", "1.2.3", ExpectedResult = false)]
        [TestCase("1.2.3", "1.2.2", ExpectedResult = true)]
        [TestCase("1", "1.2.3", ExpectedResult = true)]
        [TestCase("1.0.0.0", "1", ExpectedResult = false)]
        public bool NotEqualTest(string v1, string v2)
        {
            var sv1 = new SemanticVersioning(v1);
            var sv2 = new SemanticVersioning(v2);
            return sv1 != sv2;
        }

        [TestCase("1", "1.2.3", ExpectedResult = false)]
        [TestCase("1.2.4", "1.2.3", ExpectedResult = true)]
        [TestCase("2", "1.2.3.4", ExpectedResult = true)]
        [TestCase("1.2.3.4", "1.2.3.4", ExpectedResult = false)]
        [TestCase("1.2.3.4", "1", ExpectedResult = true)]
        public bool GreaterThan(string v1, string v2)
        {
            var sv1 = new SemanticVersioning(v1);
            var sv2 = new SemanticVersioning(v2);
            return sv1 > sv2;
        }

        [TestCase("1", "1.2.3", ExpectedResult = true)]
        [TestCase("1.2.4", "1.2.3", ExpectedResult = false)]
        [TestCase("2", "1.2.3.4", ExpectedResult = false)]
        [TestCase("1.2.3.4", "1.2.3.4", ExpectedResult = false)]
        public bool LessThan(string v1, string v2)
        {
            var sv1 = new SemanticVersioning(v1);
            var sv2 = new SemanticVersioning(v2);
            return sv1 < sv2;
        }

        [TestCase("1", "1.2.3", ExpectedResult = false)]
        [TestCase("1.2.4", "1.2.3", ExpectedResult = true)]
        [TestCase("2", "1.2.3.4", ExpectedResult = true)]
        [TestCase("1.2.3.4", "1.2.3.4", ExpectedResult = true)]
        [TestCase("1.2.3.4", "1", ExpectedResult = true)]
        public bool GreaterOrEqualThan(string v1, string v2)
        {
            var sv1 = new SemanticVersioning(v1);
            var sv2 = new SemanticVersioning(v2);
            return sv1 >= sv2;
        }

        [TestCase("1", "1.2.3", ExpectedResult = true)]
        [TestCase("1.2.4", "1.2.3", ExpectedResult = false)]
        [TestCase("2", "1.2.3.4", ExpectedResult = false)]
        [TestCase("1.2.3.4", "1.2.3.4", ExpectedResult = true)]
        [TestCase("1.0.0.0", "1.0", ExpectedResult = true)]
        public bool LessOrEqualThan(string v1, string v2)
        {
            var sv1 = new SemanticVersioning(v1);
            var sv2 = new SemanticVersioning(v2);
            return sv1 <= sv2;
        }

        [TestCase(new[] { 1,2,3}, 5, new[] { 1,2,3,0,0})]
        [TestCase(new[] { 1,2,3}, 2, new[] { 1,2,3})]
        [TestCase(new[] { 1,2,3}, 3, new[] { 1,2,3})]
        [TestCase(new[] { 1,2,3}, 4, new[] { 1,2,3,0})]
        [TestCase(new[] { 1,2,3}, 6, new[] { 1,2,3,0,0,0})]
        public void ZeroPaddingTest(int [] source, int length, int [] expected)
        {
            int[] result = SemanticVersioning.ZeroPadding(source, length);
            CollectionAssert.AreEqual(expected, result);
        }

        [Test]
        public void NullTest()
        {
            SemanticVersioning v1 = new SemanticVersioning();
            SemanticVersioning v2 = v1;
            SemanticVersioning v3 = null;
            SemanticVersioning v4 = null;

            Assert.That(v1, Is.Not.EqualTo(v3));
            Assert.That(v3, Is.Not.EqualTo(v1));
            Assert.That(v1, Is.EqualTo(v2));
            Assert.That(v3, Is.EqualTo(v4));
        }

    } 
}

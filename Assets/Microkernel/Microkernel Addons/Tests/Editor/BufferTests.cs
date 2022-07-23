using NUnit.Framework;
using System;

namespace MicrokernelSystem.Addons.Editor.Tests
{
    public class BufferTests
    {
        [Test]
        public void BufferTest()
        {
            Buffer<int> buffer = new Buffer<int>();
            Assert.That(buffer.Open, Is.Not.True);
            Assert.Throws<InvalidOperationException>(() => buffer.Write(0));

            buffer.Open();
            Assert.That(buffer.IsOpen, Is.True);
            Assert.That(buffer.Count, Is.EqualTo(0));
            int[] testValues = new[] { 1, 11, 111, 1111, 1 };

            foreach (int value in testValues)
            {
                buffer.Write(value);
            }

            Assert.That(buffer.Count, Is.EqualTo(testValues.Length));

            int[] flushValues = buffer.Flush();
            CollectionAssert.AreEqual(flushValues, testValues);
            Assert.That(buffer.IsOpen, Is.True);
            Assert.That(buffer.Count, Is.EqualTo(0));

            foreach (int value in testValues)
            {
                buffer.Write(value);
            }

            flushValues = buffer.Close();
            CollectionAssert.AreEqual(flushValues, testValues);
            Assert.That(buffer.IsOpen, Is.Not.True);
            Assert.That(buffer.Count, Is.EqualTo(0));
        }
    } 
}

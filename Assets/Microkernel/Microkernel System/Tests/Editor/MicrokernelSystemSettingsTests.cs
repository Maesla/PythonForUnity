using NUnit.Framework;
using System;

namespace MicrokernelSystem.Editor.Tests
{
    public class MicrokernelSystemSettingsTests
    {
        MicrokernelSystemSettings settings;

        [SetUp]
        public void Setup()
        {
            string json =
                @"{
                'name': 'James',
                'hobbies': ['.NET', 'Blogging', 'Reading', 'Xbox', 'LOLCATS'],
                'account': 
                {
                'path': 'C:\\Program Files\\Key\\mainKey.key',
                'value': 1234
                }
                }";

            settings = new MicrokernelSystemSettings(json);
        }

        [Test]
        public void KeyValueTest()
        {
            Assert.That(settings.GetValue<string>("name"), Is.EqualTo("James"));
        }

        [Test]
        public void KeyValuesTest()
        {
            var hobbies = settings.GetValues<string>("hobbies");
            CollectionAssert.AreEquivalent(new string[] { ".NET", "Blogging", "Reading", "Xbox", "LOLCATS" }, hobbies);
        }

        [Test]
        public void SubJsonTest()
        {
            MicrokernelSystemSettings account = settings.GetSubSettings("account");
            Assert.That(account, Is.Not.Null);
            Assert.That(account.GetValue<string>("path"), Is.EqualTo("C:\\Program Files\\Key\\mainKey.key"));
        }

        [Test]
        public void DeserializationTest()
        {
            MicrokernelSystemSettings subSettings = settings.GetSubSettings("account");
            Account account = subSettings.FromJson<Account>();
            Assert.That(account.path, Is.EqualTo("C:\\Program Files\\Key\\mainKey.key"));
            Assert.That(account.value, Is.EqualTo(1234));
        }

        [Serializable]
        private class Account
        {
            public string path;
            public int value;
        }
    } 
}

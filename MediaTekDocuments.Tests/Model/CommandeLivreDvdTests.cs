using System;
using MediaTekDocuments.model;
using Newtonsoft.Json;
using NUnit.Framework;

namespace MediaTekDocuments.Tests.Model
{
    [TestFixture]
    [TestOf(typeof(CommandeLivreDvd))]
    public class CommandeLivreDvdTests
    {
        [Test]
        public void CommandeLivreDvdDeserialisationTest()
        {
            string json = "{\"id\":\"00015\",\"dateCommande\":\"2025-03-12\",\"montant\":10.4,\"nbExemplaire\":3,\"idLivreDvd\":\"00010\",\"idSuivi\":\"00001\",\"stade\":\"En cours\"}";
            CommandeLivreDvd commandeLivreDvd = JsonConvert.DeserializeObject<CommandeLivreDvd>(json);
            
            Assert.IsNotNull(commandeLivreDvd);
            Assert.AreEqual("00015", commandeLivreDvd.Id);
            Assert.AreEqual(new DateTime(2025, 03, 12, 0, 0, 0, DateTimeKind.Local), commandeLivreDvd.DateCommande);
            Assert.AreEqual(10.4, commandeLivreDvd.Montant);
            Assert.AreEqual(3, commandeLivreDvd.NbExemplaire);
            Assert.AreEqual("00010", commandeLivreDvd.IdLivreDvd);
            Assert.AreEqual("00001", commandeLivreDvd.IdSuivi);
            Assert.AreEqual("En cours", commandeLivreDvd.Stade);
        }
    }
}
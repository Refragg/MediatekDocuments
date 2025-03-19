using System;
using MediaTekDocuments.model;
using Newtonsoft.Json;
using NUnit.Framework;

namespace MediaTekDocuments.Tests.Model
{
    [TestFixture]
    [TestOf(typeof(Exemplaire))]
    public class ExemplaireTests
    {
        [Test]
        public void ExemplaireDeserialisationTest()
        {
            string json = "{\"id\":\"10001\",\"numero\":2,\"dateAchat\":\"2025-03-18\",\"photo\":\"\",\"idEtat\":\"00002\",\"etat\":\"usagé\"}";
            Exemplaire exemplaire = JsonConvert.DeserializeObject<Exemplaire>(json);
            
            Assert.IsNotNull(exemplaire);
            Assert.AreEqual("10001", exemplaire.Id);
            Assert.AreEqual(2, exemplaire.Numero);
            Assert.AreEqual(new DateTime(2025, 03, 18, 0, 0, 0, DateTimeKind.Local), exemplaire.DateAchat);
            Assert.IsEmpty(exemplaire.Photo);
            Assert.AreEqual("00002", exemplaire.IdEtat);
            Assert.AreEqual("usagé", exemplaire.Etat);
        }

        [Test]
        public void ExemplaireToRestApiObjectTest()
        {
            Exemplaire exemplaire = new Exemplaire(1, new DateTime(2025, 03, 14, 0, 0, 0, DateTimeKind.Local), "", "00002", "usagé", "10001");
            string exemplaireRestApiObjectJson = JsonConvert.SerializeObject(exemplaire.ToRestApiObject());
            string expectedJson = "{\"Numero\":1,\"DateAchat\":\"2025-03-14T00:00:00+01:00\",\"Photo\":\"\",\"IdEtat\":\"00002\",\"Id\":\"10001\"}";
            
            Assert.AreEqual(expectedJson, exemplaireRestApiObjectJson);
        }
    }
}
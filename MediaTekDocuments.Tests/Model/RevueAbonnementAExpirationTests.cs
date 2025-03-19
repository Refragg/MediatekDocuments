using System;
using MediaTekDocuments.model;
using Newtonsoft.Json;
using NUnit.Framework;

namespace MediaTekDocuments.Tests.Model
{
    [TestFixture]
    [TestOf(typeof(RevueAbonnementAExpiration))]
    public class RevueAbonnementAExpirationTests
    {
        [Test]
        public void RevueAbonnementAExpirationDeserialisationTest()
        {
            string json = "{\"idRevue\": \"10001\", \"titre\": \"Arts Magazine\", \"dateFinAbonnement\": \"2025-03-20\"}";
            RevueAbonnementAExpiration revueExpirant = JsonConvert.DeserializeObject<RevueAbonnementAExpiration>(json);
            
            Assert.IsNotNull(revueExpirant);
            Assert.AreEqual("10001", revueExpirant.IdRevue);
            Assert.AreEqual("Arts Magazine", revueExpirant.Titre);
            Assert.AreEqual(new DateTime(2025, 03, 20, 0, 0, 0, DateTimeKind.Local), revueExpirant.DateFinAbonnement);
        }
    }
}
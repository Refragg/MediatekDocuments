using System;
using MediaTekDocuments.model;
using Newtonsoft.Json;
using NUnit.Framework;

namespace MediaTekDocuments.Tests.Model
{
    [TestFixture]
    [TestOf(typeof(Abonnement))]
    public class AbonnementTests
    {
        [Test]
        public void ParutionDansAbonnementTest()
        {
            Abonnement abonnementTest = new Abonnement(
                "00001",
                new DateTime(2025, 3, 1, 0, 0, 0, DateTimeKind.Local),
                9.99, 
                new DateTime(2025, 4, 1, 0, 0, 0, DateTimeKind.Local),
                "10001"
            );
            
            Assert.False(abonnementTest.ParutionDansAbonnement(new DateTime(2025, 2, 25, 0, 0, 0, DateTimeKind.Local)), "Cette date ne peut pas être parue dans l'abonnement");
            Assert.True(abonnementTest.ParutionDansAbonnement(new DateTime(2025, 3, 1, 0, 0, 0, DateTimeKind.Local)), "Cette date est parue dans l'abonnement");
            Assert.True(abonnementTest.ParutionDansAbonnement(new DateTime(2025, 3, 15, 0, 0, 0, DateTimeKind.Local)), "Cette date est parue dans l'abonnement");
            Assert.False(abonnementTest.ParutionDansAbonnement(new DateTime(2025, 4, 1, 0, 0, 0, DateTimeKind.Local)), "Cette date ne peut pas être parue dans l'abonnement");
            Assert.False(abonnementTest.ParutionDansAbonnement(new DateTime(2025, 4, 15, 0, 0, 0, DateTimeKind.Local)), "Cette date ne peut pas être parue dans l'abonnement");
        }

        [Test]
        public void AbonnementDeserialisationTest()
        {
            string json = "{\"id\":\"00051\",\"dateCommande\":\"2025-03-12\",\"montant\":50.99,\"dateFinAbonnement\":\"2025-03-20\",\"idRevue\":\"10001\"}";
            Abonnement abonnement = JsonConvert.DeserializeObject<Abonnement>(json);
            
            Assert.IsNotNull(abonnement);
            Assert.AreEqual("00051", abonnement.Id);
            Assert.AreEqual(new DateTime(2025, 03, 12, 0, 0, 0, DateTimeKind.Local), abonnement.DateCommande);
            Assert.AreEqual(50.99, abonnement.Montant);
            Assert.AreEqual(new DateTime(2025, 03, 20, 0, 0, 0, DateTimeKind.Local), abonnement.DateFinAbonnement);
            Assert.AreEqual("10001", abonnement.IdRevue);
        }
    }
}
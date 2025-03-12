using System;
using MediaTekDocuments.model;
using NUnit.Framework;

namespace MediaTekDocuments.Tests.Model
{
    [TestFixture]
    public class AbonnementTests
    {
        [Test]
        public void ParutionDansAbonnementTest()
        {
            Abonnement abonnementTest = new Abonnement(
                "00001",
                new DateTime(2025, 3, 1),
                9.99, 
                new DateTime(2025, 4, 1),
                "10001"
            );
            
            Assert.False(abonnementTest.ParutionDansAbonnement(new DateTime(2025, 2, 25)), "Cette date ne peut pas être parue dans l'abonnement");
            Assert.True(abonnementTest.ParutionDansAbonnement(new DateTime(2025, 3, 1)), "Cette date est parue dans l'abonnement");
            Assert.True(abonnementTest.ParutionDansAbonnement(new DateTime(2025, 3, 15)), "Cette date est parue dans l'abonnement");
            Assert.False(abonnementTest.ParutionDansAbonnement(new DateTime(2025, 4, 1)), "Cette date ne peut pas être parue dans l'abonnement");
            Assert.False(abonnementTest.ParutionDansAbonnement(new DateTime(2025, 4, 15)), "Cette date ne peut pas être parue dans l'abonnement");
        }
    }
}
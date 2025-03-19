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
    }
}
using System;
using MediaTekDocuments.model;
using NUnit.Framework;

namespace MediaTekDocuments.Tests.Model
{
    [TestFixture]
    [TestOf(typeof(Commande))]
    public class CommandeTests
    {
        [Test]
        public void CommandeBaseClassTest()
        {
            Abonnement abonnement = new Abonnement("00001", DateTime.Now, 10.0, DateTime.Now, "20001");
            
            Assert.True(abonnement.GetType().IsSubclassOf(typeof(Commande)));

            CommandeLivreDvd commandeLivreDvd = new CommandeLivreDvd("00001", DateTime.Now, 10.0, 3, "20001", "00001", "En cours");
            
            Assert.True(commandeLivreDvd.GetType().IsSubclassOf(typeof(Commande)));
        }
    }
}
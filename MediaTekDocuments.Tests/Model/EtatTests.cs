using MediaTekDocuments.model;
using Newtonsoft.Json;
using NUnit.Framework;

namespace MediaTekDocuments.Tests.Model
{
    [TestFixture]
    [TestOf(typeof(Etat))]
    public class EtatTests
    {
        [Test]
        public void EtatDeserialisationTest()
        {
            string json = "{\"id\": \"00001\", \"libelle\": \"neuf\"}";
            Etat etat = JsonConvert.DeserializeObject<Etat>(json);
            
            Assert.IsNotNull(etat);
            Assert.AreEqual("00001", etat.Id);
            Assert.AreEqual("neuf", etat.Libelle);
        }

        [Test]
        public void EtatToStringTest()
        {
            Etat etat = new Etat("00002", "usagé");
            
            Assert.AreEqual("usagé", etat.ToString());
        }
    }
}
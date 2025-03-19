using MediaTekDocuments.model;
using Newtonsoft.Json;
using NUnit.Framework;

namespace MediaTekDocuments.Tests.Model
{
    [TestFixture]
    [TestOf(typeof(Revue))]
    public class RevueTests
    {
        [Test]
        public void RevueDeserialisationTest()
        {
            string json = "{\"id\":\"10002\",\"periodicite\":\"MS\",\"titre\":\"Alternatives Economiques\",\"image\":\"\",\"delaiMiseADispo\":52,\"idrayon\":\"PR002\",\"idpublic\":\"00002\",\"idgenre\":\"10015\",\"genre\":\"Presse Economique\",\"lePublic\":\"Adultes\",\"rayon\":\"Magazines\"}";
            Revue revue = JsonConvert.DeserializeObject<Revue>(json);
            
            Assert.IsNotNull(revue);
            Assert.AreEqual("10002", revue.Id);
            Assert.AreEqual("MS", revue.Periodicite);
            Assert.AreEqual("Alternatives Economiques", revue.Titre);
            Assert.IsEmpty(revue.Image);
            Assert.AreEqual(52, revue.DelaiMiseADispo);
            
            Assert.AreEqual("PR002", revue.IdRayon);
            Assert.AreEqual("00002", revue.IdPublic);
            Assert.AreEqual("10015", revue.IdGenre);
            Assert.AreEqual("Magazines", revue.Rayon);
            Assert.AreEqual("Adultes", revue.Public);
            Assert.AreEqual("Presse Economique", revue.Genre);
        }
    }
}
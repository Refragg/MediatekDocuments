using MediaTekDocuments.model;
using Newtonsoft.Json;
using NUnit.Framework;

namespace MediaTekDocuments.Tests.Model
{
    [TestFixture]
    [TestOf(typeof(Dvd))]
    public class DvdTests
    {
        [Test]
        public void DvdDeserialisationTest()
        {
            string json = "{\"id\":\"20003\",\"duree\":128,\"realisateur\":\"Steven Spielberg\",\"titre\":\"Jurassic Park\",\"image\":\"\",\"synopsis\":\"Un milliardaire et des généticiens créent des dinosaures à partir de clonage.\",\"idrayon\":\"DF001\",\"idpublic\":\"00003\",\"idgenre\":\"10002\",\"genre\":\"Science Fiction\",\"lePublic\":\"Tous publics\",\"rayon\":\"DVD films\"}";
            Dvd dvd = JsonConvert.DeserializeObject<Dvd>(json);
            
            Assert.IsNotNull(dvd);
            Assert.AreEqual("20003", dvd.Id);
            Assert.AreEqual(128, dvd.Duree);
            Assert.AreEqual("Steven Spielberg", dvd.Realisateur);
            Assert.AreEqual("Jurassic Park", dvd.Titre);
            Assert.IsEmpty(dvd.Image);
            Assert.AreEqual("Un milliardaire et des généticiens créent des dinosaures à partir de clonage.", dvd.Synopsis);
            
            Assert.AreEqual("DF001", dvd.IdRayon);
            Assert.AreEqual("00003", dvd.IdPublic);
            Assert.AreEqual("10002", dvd.IdGenre);
            Assert.AreEqual("DVD films", dvd.Rayon);
            Assert.AreEqual("Tous publics", dvd.Public);
            Assert.AreEqual("Science Fiction", dvd.Genre);
        }
    }
}
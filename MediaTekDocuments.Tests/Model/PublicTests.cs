using MediaTekDocuments.model;
using Newtonsoft.Json;
using NUnit.Framework;

namespace MediaTekDocuments.Tests.Model
{
    [TestFixture]
    [TestOf(typeof(Public))]
    public class PublicTests
    {
        [Test]
        public void PublicDeserialisationTest()
        {
            string json = "{\"id\": \"00002\", \"libelle\": \"Adultes\"}";
            Public lePublic = JsonConvert.DeserializeObject<Public>(json);
            
            Assert.IsNotNull(lePublic);
            Assert.AreEqual("00002", lePublic.Id);
            Assert.AreEqual("Adultes", lePublic.Libelle);
        }

        [Test]
        public void PublicToStringTest()
        {
            Public lePublic = new Public("00003", "Tous publics");
            
            Assert.AreEqual("Tous publics", lePublic.ToString());
        }
    }
}
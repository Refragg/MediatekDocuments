using MediaTekDocuments.model;
using Newtonsoft.Json;
using NUnit.Framework;

namespace MediaTekDocuments.Tests.Model
{
    [TestFixture]
    [TestOf(typeof(Suivi))]
    public class SuiviTests
    {
        [Test]
        public void SuiviDeserialisationTest()
        {
            string json = "{\"id\": \"00004\",\"stade\": \"Réglée\"}";
            Suivi suivi = JsonConvert.DeserializeObject<Suivi>(json);
            
            Assert.IsNotNull(suivi);
            Assert.AreEqual("00004", suivi.Id);
            Assert.AreEqual("Réglée", suivi.Stade);
        }

        [Test]
        public void SuiviToStringTest()
        {
            Suivi suivi = new Suivi("00001", "En cours");
            
            Assert.AreEqual("En cours", suivi.ToString());
        }
    }
}
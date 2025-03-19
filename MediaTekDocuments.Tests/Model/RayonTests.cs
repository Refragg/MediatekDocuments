using MediaTekDocuments.model;
using Newtonsoft.Json;
using NUnit.Framework;

namespace MediaTekDocuments.Tests.Model
{
    [TestFixture]
    [TestOf(typeof(Rayon))]
    public class RayonTests
    {
        [Test]
        public void RayonDeserialisationTest()
        {
            string json = "{\"id\": \"PR002\", \"libelle\": \"Magazines\"}";
            Rayon rayon = JsonConvert.DeserializeObject<Rayon>(json);
            
            Assert.IsNotNull(rayon);
            Assert.AreEqual("PR002", rayon.Id);
            Assert.AreEqual("Magazines", rayon.Libelle);
        }

        [Test]
        public void RayonToStringTest()
        {
            Rayon rayon = new Rayon("PR001", "Presse quotidienne");
            
            Assert.AreEqual("Presse quotidienne", rayon.ToString());
        }
    }
}
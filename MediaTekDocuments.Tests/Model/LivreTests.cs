using MediaTekDocuments.model;
using Newtonsoft.Json;
using NUnit.Framework;

namespace MediaTekDocuments.Tests.Model
{
    [TestFixture]
    [TestOf(typeof(Livre))]
    public class LivreTests
    {
        [Test]
        public void LivreDeserialisationTest()
        {
            string json = "{\"id\":\"00017\",\"ISBN\":\"123456\",\"auteur\":\"Philippe Masson\",\"titre\":\"Catastrophes au Brésil\",\"image\":\"\",\"collection\":\"\",\"idrayon\":\"JN002\",\"idpublic\":\"00004\",\"idgenre\":\"10014\",\"genre\":\"Policier\",\"lePublic\":\"Ados\",\"rayon\":\"Jeunesse romans\"}";
            Livre livre = JsonConvert.DeserializeObject<Livre>(json);
            
            Assert.IsNotNull(livre);
            Assert.AreEqual("00017", livre.Id);
            Assert.AreEqual("123456", livre.Isbn);
            Assert.AreEqual("Philippe Masson", livre.Auteur);
            Assert.AreEqual("Catastrophes au Brésil", livre.Titre);
            Assert.IsEmpty(livre.Image);
            Assert.IsEmpty(livre.Collection);
            
            Assert.AreEqual("JN002", livre.IdRayon);
            Assert.AreEqual("00004", livre.IdPublic);
            Assert.AreEqual("10014", livre.IdGenre);
            Assert.AreEqual("Jeunesse romans", livre.Rayon);
            Assert.AreEqual("Ados", livre.Public);
            Assert.AreEqual("Policier", livre.Genre);
        }
    }
}
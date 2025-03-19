using MediaTekDocuments.model;
using Newtonsoft.Json;
using NUnit.Framework;

namespace MediaTekDocuments.Tests.Model
{
    [TestFixture]
    [TestOf(typeof(Genre))]
    public class GenreTests
    {
        [Test]
        public void GenreDeserialisationTest()
        {
            string json = "{\"id\": \"10014\", \"libelle\": \"Policier\"}";
            Genre genre = JsonConvert.DeserializeObject<Genre>(json);
            
            Assert.IsNotNull(genre);
            Assert.AreEqual("10014", genre.Id);
            Assert.AreEqual("Policier", genre.Libelle);
        }

        [Test]
        public void GenreToStringTest()
        {
            Genre genre = new Genre("10006", "Roman");
            
            Assert.AreEqual("Roman", genre.ToString());
        }
    }
}
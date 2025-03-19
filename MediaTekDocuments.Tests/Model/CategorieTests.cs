using MediaTekDocuments.model;
using NUnit.Framework;

namespace MediaTekDocuments.Tests.Model
{
    [TestFixture]
    [TestOf(typeof(Categorie))]
    public class CategorieTests
    {
        [Test]
        public void CategorieBaseClassTest()
        {
            Public lePublic = new Public("00001", "Tous public");
            
            Assert.True(lePublic.GetType().IsSubclassOf(typeof(Categorie)));
            
            Genre genre = new Genre("00001", "Fantasy");
            
            Assert.True(genre.GetType().IsSubclassOf(typeof(Categorie)));

            Rayon rayon = new Rayon("00001", "Romans");
            
            Assert.That(rayon.GetType().IsSubclassOf(typeof(Categorie)));
        }
    }
}
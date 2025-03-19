using MediaTekDocuments.model;
using NUnit.Framework;

namespace MediaTekDocuments.Tests.Model
{
    [TestFixture]
    [TestOf(typeof(Document))]
    public class DocumentTests
    {
        [Test]
        public void DocumentBaseClassTest()
        {
            Livre livre = new Livre("00001", "titre", "", "123456789123", "Philippe Masson", "", "00001", "Comédie",
                "00001", "Tous public", "PR001", "Romans");
            
            Assert.True(livre.GetType().IsSubclassOf(typeof(Document)));
            
            Dvd dvd = new Dvd("00001", "titre", "", 120, "Philippe Masson", "", "00001", "Comédie",
                "00001", "Tous public", "PR001", "Romans");
            
            Assert.True(dvd.GetType().IsSubclassOf(typeof(Document)));
            
            Revue revue = new Revue("00001", "titre", "", "00001", "Comédie",
                "00001", "Tous public", "PR001", "Romans", "MS", 52);
            
            Assert.True(revue.GetType().IsSubclassOf(typeof(Document)));
        }
    }
}
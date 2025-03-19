using MediaTekDocuments.model;
using Newtonsoft.Json;
using NUnit.Framework;

namespace MediaTekDocuments.Tests.Model
{
    [TestFixture]
    [TestOf(typeof(LivreDvd))]
    public class LivreDvdTests
    {
        [Test]
        public void LivreDvdBaseClassTest()
        {
            Livre livre = new Livre("00001", "titre", "", "123456789123", "Philippe Masson", "", "00001", "Comédie",
                "00001", "Tous public", "PR001", "Romans");
            
            Assert.True(livre.GetType().BaseType == typeof(LivreDvd));
            
            Dvd dvd = new Dvd("00001", "titre", "", 120, "Philippe Masson", "", "00001", "Comédie",
                "00001", "Tous public", "PR001", "Romans");
            
            Assert.True(dvd.GetType().BaseType == typeof(LivreDvd));
        }
    }
}
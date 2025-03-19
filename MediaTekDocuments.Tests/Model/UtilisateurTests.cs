using MediaTekDocuments.model;
using Newtonsoft.Json;
using NUnit.Framework;

namespace MediaTekDocuments.Tests.Model
{
    [TestFixture]
    [TestOf(typeof(Utilisateur))]
    public class UtilisateurTests
    {
        [Test]
        public void UtilisateurDeserialisationTest()
        {
            string json = "{\"id\": 49,\"login\": \"jjohn\", \"administrateur\": true, \"service\": null}";
            Utilisateur utilisateur = JsonConvert.DeserializeObject<Utilisateur>(json);
            
            Assert.IsNotNull(utilisateur);
            Assert.AreEqual(49, utilisateur.Id);
            Assert.AreEqual("jjohn", utilisateur.Login);
            Assert.True(utilisateur.Administrateur);
            Assert.Null(utilisateur.Service);
            
            json = "{\"id\": 12,\"login\": \"adumont\", \"administrateur\": false, \"service\": \"Culturel\"}";
            utilisateur = JsonConvert.DeserializeObject<Utilisateur>(json);
            
            Assert.IsNotNull(utilisateur);
            Assert.AreEqual(12, utilisateur.Id);
            Assert.AreEqual("adumont", utilisateur.Login);
            Assert.False(utilisateur.Administrateur);
            Assert.AreEqual("Culturel", utilisateur.Service);
        }
    }
}
using System;
using System.Collections.Generic;
using MediaTekDocuments.model;
using MediaTekDocuments.manager;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Linq;

namespace MediaTekDocuments.dal
{
    /// <summary>
    /// Classe d'accès aux données
    /// </summary>
    public class Access
    {
        /// <summary>
        /// adresse de l'API
        /// </summary>
        private static readonly string uriApi = "http://localhost/rest_mediatekdocuments/";
        /// <summary>
        /// instance unique de la classe
        /// </summary>
        private static Access instance = null;
        /// <summary>
        /// instance de ApiRest pour envoyer des demandes vers l'api et recevoir la réponse
        /// </summary>
        private readonly ApiRest api = null;
        /// <summary>
        /// méthode HTTP pour select
        /// </summary>
        private const string GET = "GET";
        /// <summary>
        /// méthode HTTP pour insert
        /// </summary>
        private const string POST = "POST";
        /// <summary>
        /// méthode HTTP pour update
        /// </summary>
        private const string PUT = "PUT";
        /// <summary>
        /// méthode HTTP pour delete
        /// </summary>
        private const string DELETE = "DELETE";

        /// <summary>
        /// Méthode privée pour créer un singleton
        /// initialise l'accès à l'API
        /// </summary>
        private Access()
        {
            String authenticationString;
            try
            {
                authenticationString = "admin:adminpwd";
                api = ApiRest.GetInstance(uriApi, authenticationString);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Création et retour de l'instance unique de la classe
        /// </summary>
        /// <returns>instance unique de la classe</returns>
        public static Access GetInstance()
        {
            if(instance == null)
            {
                instance = new Access();
            }
            return instance;
        }

        /// <summary>
        /// Retourne tous les genres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Genre</returns>
        public List<Categorie> GetAllGenres()
        {
            IEnumerable<Genre> lesGenres = TraitementRecup<Genre>(GET, "genre", null);
            return new List<Categorie>(lesGenres);
        }

        /// <summary>
        /// Retourne tous les rayons à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Rayon</returns>
        public List<Categorie> GetAllRayons()
        {
            IEnumerable<Rayon> lesRayons = TraitementRecup<Rayon>(GET, "rayon", null);
            return new List<Categorie>(lesRayons);
        }

        /// <summary>
        /// Retourne toutes les catégories de public à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Public</returns>
        public List<Categorie> GetAllPublics()
        {
            IEnumerable<Public> lesPublics = TraitementRecup<Public>(GET, "public", null);
            return new List<Categorie>(lesPublics);
        }
        
        /// <summary>
        /// Retourne tous les suivis possibles à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Suivi</returns>
        public List<Suivi> GetAllSuivis()
        {
            IEnumerable<Suivi> lesSuivis = TraitementRecup<Suivi>(GET, "suivi", null);
            return new List<Suivi>(lesSuivis);
        }

        /// <summary>
        /// Retourne toutes les livres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Livre</returns>
        public List<Livre> GetAllLivres()
        {
            List<Livre> lesLivres = TraitementRecup<Livre>(GET, "livre", null);
            return lesLivres;
        }

        /// <summary>
        /// Retourne toutes les dvd à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Dvd</returns>
        public List<Dvd> GetAllDvd()
        {
            List<Dvd> lesDvd = TraitementRecup<Dvd>(GET, "dvd", null);
            return lesDvd;
        }

        /// <summary>
        /// Retourne toutes les revues à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Revue</returns>
        public List<Revue> GetAllRevues()
        {
            List<Revue> lesRevues = TraitementRecup<Revue>(GET, "revue", null);
            return lesRevues;
        }

        /// <summary>
        /// Retourne les revues dont l'abonnement arrive a expiration dans moins de 30 jours à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets RevueAbonnementAExpiration</returns>
        public List<RevueAbonnementAExpiration> GetRevuesAbonnementAExpirationProchaine()
        {
            List<RevueAbonnementAExpiration> lesRevues = TraitementRecup<RevueAbonnementAExpiration>(GET, "revue_abonnement_expiration", null);
            return lesRevues;
        }

        /// <summary>
        /// Retourne les exemplaires d'un document
        /// </summary>
        /// <param name="idDocument">id du document concerné</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplaires(string idDocument)
        {
            String jsonIdDocument = convertToJson("id", idDocument);
            List<Exemplaire> lesExemplaires = TraitementRecup<Exemplaire>(GET, "exemplaire/" + jsonIdDocument, null);
            return lesExemplaires;
        }

        /// <summary>
        /// Retourne le nombre de commandes d'un livre / DVD
        /// </summary>
        /// <param name="idDocument">id du livre / DVD concerné</param>
        /// <returns>Nombre de commandes du livre / DVD</returns>
        public int GetCommandesCountLivreDvd(string idDocument)
        {
            String jsonIdDocument = convertToJson("id", idDocument);
            List<object> lesExemplaires = TraitementRecup<object>(GET, "commandedocument/" + jsonIdDocument, null);
            return lesExemplaires.Count;
        }
        
        /// <summary>
        /// Retourne le nombre de commandes d'une revue
        /// </summary>
        /// <param name="idDocument">id de la revue concernée</param>
        /// <returns>Nombre de commandes de la revue</returns>
        public int GetCommandesCountRevue(string idDocument)
        {
            String jsonIdDocument = convertToJson("id", idDocument);
            List<object> lesExemplaires = TraitementRecup<object>(GET, "abonnement/" + jsonIdDocument, null);
            return lesExemplaires.Count;
        }
        
        /// <summary>
        /// Retourne le nombre de commandes d'un livre / DVD
        /// </summary>
        /// <param name="livreDvd">Le livre / DVD concerné</param>
        /// <returns>Nombre de commandes du livre / DVD</returns>
        public List<CommandeLivreDvd> GetCommandes(LivreDvd livreDvd)
        {
            String jsonIdDocument = convertToJson("id", livreDvd.Id);
            List<CommandeLivreDvd> lesCommandes = TraitementRecup<CommandeLivreDvd>(GET, "commandedocument/" + jsonIdDocument, null);
            return lesCommandes;
        }
        
        /// <summary>
        /// Retourne les commandes d'une revue
        /// </summary>
        /// <param name="revue">La revue concernée</param>
        /// <returns>La liste des commandes de la revue</returns>
        public List<Abonnement> GetCommandes(Revue revue)
        {
            String jsonIdDocument = convertToJson("id", revue.Id);
            List<Abonnement> lesCommandes = TraitementRecup<Abonnement>(GET, "abonnement/" + jsonIdDocument, null);
            return lesCommandes;
        }

        /// <summary>
        /// ecriture d'un exemplaire en base de données
        /// </summary>
        /// <param name="exemplaire">exemplaire à insérer</param>
        /// <returns>true si l'insertion a pu se faire (retour != null)</returns>
        public bool CreerExemplaire(Exemplaire exemplaire)
        {
            String jsonExemplaire = JsonConvert.SerializeObject(exemplaire, new CustomDateTimeConverter());
            try
            {
                List<Exemplaire> liste = TraitementRecup<Exemplaire>(POST, "exemplaire", "champs=" + jsonExemplaire, false);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Crée et ajoute un livre à la base de données
        /// </summary>
        /// <param name="livre">L'objet du livre à créer</param>
        /// <returns>True si la création à réussi, false sinon</returns>
        public bool CreerLivre(Livre livre)
        {
            String jsonLivre = JsonConvert.SerializeObject(livre);
            try
            {
                List<Livre> liste = TraitementRecup<Livre>(POST, "livre", "champs=" + jsonLivre);
                return liste.Count > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
        
        /// <summary>
        /// Modifie un livre dans la base de données
        /// </summary>
        /// <param name="livre">Le livre avec ses champs modifiés</param>
        /// <returns>True si la modification à réussi, false sinon</returns>
        public bool ModifierLivre(Livre livre)
        {
            String jsonLivre = JsonConvert.SerializeObject(livre);
            try
            {
                List<Livre> liste = TraitementRecup<Livre>(PUT, "livre", "champs=" + jsonLivre);
                return liste.Count > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
        
        /// <summary>
        /// Supprime un livre de la base de données
        /// </summary>
        /// <param name="idLivre">L'identifiant du livre à supprimer</param>
        /// <returns>True si la suppression à réussi, false sinon</returns>
        public bool SupprimerLivre(string idLivre)
        {
            String jsonLivre = convertToJson("Id", idLivre);
            try
            {
                List<Livre> liste = TraitementRecup<Livre>(DELETE, "livre", "champs=" + jsonLivre, false);
                return liste != null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
        
        /// <summary>
        /// Crée et ajoute un DVD à la base de données
        /// </summary>
        /// <param name="dvd">L'objet du DVD à créer</param>
        /// <returns>True si la création à réussi, false sinon</returns>
        public bool CreerDvd(Dvd dvd)
        {
            String jsonDvd = JsonConvert.SerializeObject(dvd);
            try
            {
                List<Dvd> liste = TraitementRecup<Dvd>(POST, "dvd", "champs=" + jsonDvd);
                return liste.Count > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
        
        /// <summary>
        /// Modifie un DVD dans la base de données
        /// </summary>
        /// <param name="dvd">Le DVD avec ses champs modifiés</param>
        /// <returns>True si la modification à réussi, false sinon</returns>
        public bool ModifierDvd(Dvd dvd)
        {
            String jsonDvd = JsonConvert.SerializeObject(dvd);
            try
            {
                List<Dvd> liste = TraitementRecup<Dvd>(PUT, "dvd", "champs=" + jsonDvd);
                return liste.Count > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
        
        /// <summary>
        /// Supprime un DVD de la base de données
        /// </summary>
        /// <param name="idDvd">L'identifiant du DVD à supprimer</param>
        /// <returns>True si la suppression à réussi, false sinon</returns>
        public bool SupprimerDvd(string idDvd)
        {
            String jsonDvd = convertToJson("Id", idDvd);
            try
            {
                List<Dvd> liste = TraitementRecup<Dvd>(DELETE, "dvd", "champs=" + jsonDvd, false);
                return liste != null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
        
        /// <summary>
        /// Crée et ajoute une revue à la base de données
        /// </summary>
        /// <param name="revue">L'objet de la revue à créer</param>
        /// <returns>True si la création à réussi, false sinon</returns>
        public bool CreerRevue(Revue revue)
        {
            String jsonRevue = JsonConvert.SerializeObject(revue);
            try
            {
                List<Revue> liste = TraitementRecup<Revue>(POST, "revue", "champs=" + jsonRevue);
                return liste.Count > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
        
        /// <summary>
        /// Modifie une revue dans la base de données
        /// </summary>
        /// <param name="revue">La revue avec ses champs modifiés</param>
        /// <returns>True si la modification à réussi, false sinon</returns>
        public bool ModifierRevue(Revue revue)
        {
            String jsonRevue = JsonConvert.SerializeObject(revue);
            try
            {
                List<Revue> liste = TraitementRecup<Revue>(PUT, "revue", "champs=" + jsonRevue);
                return liste.Count > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
        
        /// <summary>
        /// Supprime une revue de la base de données
        /// </summary>
        /// <param name="idRevue">L'identifiant de la revue à supprimer</param>
        /// <returns>True si la suppression à réussi, false sinon</returns>
        public bool SupprimerRevue(string idRevue)
        {
            String jsonRevue = convertToJson("Id", idRevue);
            try
            {
                List<Revue> liste = TraitementRecup<Revue>(DELETE, "revue", "champs=" + jsonRevue, false);
                return liste != null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
        
        /// <summary>
        /// Crée et ajoute une commande à la base de données
        /// </summary>
        /// <param name="commande">L'objet de la commande à créer</param>
        /// <returns>True si la création à réussi, false sinon</returns>
        public bool CreerCommande(CommandeLivreDvd commande)
        {
            String jsonCommande = JsonConvert.SerializeObject(commande, new CustomDateTimeConverter());
            try
            {
                List<CommandeLivreDvd> liste = TraitementRecup<CommandeLivreDvd>(POST, "commandedocument", "champs=" + jsonCommande);
                return liste.Count > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
        
        /// <summary>
        /// Modifie une commande dans la base de données
        /// </summary>
        /// <param name="commande">La commande avec ses champs modifiés</param>
        /// <returns>True si la modification à réussi, false sinon</returns>
        public bool ModifierCommande(CommandeLivreDvd commande)
        {
            String jsonCommande = JsonConvert.SerializeObject(commande, new CustomDateTimeConverter());
            try
            {
                List<CommandeLivreDvd> liste = TraitementRecup<CommandeLivreDvd>(PUT, "commandedocument", "champs=" + jsonCommande);
                return liste.Count > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
        
        /// <summary>
        /// Supprime une commande de la base de données
        /// </summary>
        /// <param name="idCommande">L'identifiant de la commande à supprimer</param>
        /// <returns>True si la suppression à réussi, false sinon</returns>
        public bool SupprimerCommandeLivreDvd(string idCommande)
        {
            String jsonRevue = convertToJson("Id", idCommande);
            try
            {
                List<CommandeLivreDvd> liste = TraitementRecup<CommandeLivreDvd>(DELETE, "commandedocument", "champs=" + jsonRevue, false);
                return liste != null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
        
        /// <summary>
        /// Crée et ajoute une commande d'une revue à la base de données
        /// </summary>
        /// <param name="commande">L'objet de la commande à créer</param>
        /// <returns>True si la création à réussi, false sinon</returns>
        public bool CreerCommande(Abonnement commande)
        {
            String jsonCommande = JsonConvert.SerializeObject(commande, new CustomDateTimeConverter());
            try
            {
                List<Abonnement> liste = TraitementRecup<Abonnement>(POST, "abonnement", "champs=" + jsonCommande);
                return liste.Count > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
        
        /// <summary>
        /// Modifie une commande d'une revue dans la base de données
        /// </summary>
        /// <param name="commande">La commande avec ses champs modifiés</param>
        /// <returns>True si la modification à réussi, false sinon</returns>
        public bool ModifierCommande(Abonnement commande)
        {
            String jsonCommande = JsonConvert.SerializeObject(commande, new CustomDateTimeConverter());
            try
            {
                List<Abonnement> liste = TraitementRecup<Abonnement>(PUT, "abonnement", "champs=" + jsonCommande);
                return liste.Count > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }
        
        /// <summary>
        /// Supprime une commande d'une revue de la base de données
        /// </summary>
        /// <param name="idCommande">L'identifiant de la commande à supprimer</param>
        /// <returns>True si la suppression à réussi, false sinon</returns>
        public bool SupprimerCommandeRevue(string idCommande)
        {
            String jsonCommande = convertToJson("Id", idCommande);
            try
            {
                List<Abonnement> liste = TraitementRecup<Abonnement>(DELETE, "abonnement", "champs=" + jsonCommande, false);
                return liste != null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Traitement de la récupération du retour de l'api, avec conversion du json en liste pour les select (GET)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methode">verbe HTTP (GET, POST, PUT, DELETE)</param>
        /// <param name="message">information envoyée dans l'url</param>
        /// <param name="parametres">paramètres à envoyer dans le body, au format "chp1=val1&chp2=val2&..."</param>
        /// <returns>liste d'objets récupérés (ou liste vide)</returns>
        private List<T> TraitementRecup<T> (String methode, String message, String parametres, bool recupListeRetour = true)
        {
            // trans
            List<T> liste = new List<T>();
            try
            {
                JObject retour = api.RecupDistant(methode, message, parametres);
                // extraction du code retourné
                String code = (String)retour["code"];
                if (code.Equals("200"))
                {
                    if (recupListeRetour)
                    {
                        String resultString = JsonConvert.SerializeObject(retour["result"]);
                        // construction de la liste d'objets à partir du retour de l'api
                        liste = JsonConvert.DeserializeObject<List<T>>(resultString, new CustomBooleanJsonConverter());
                    }
                }
                else
                {
                    Console.WriteLine("code erreur = " + code + " message = " + (String)retour["message"]);
                }
            }catch(Exception e)
            {
                Console.WriteLine("Erreur lors de l'accès à l'API : "+e.Message);
                Environment.Exit(0);
            }
            return liste;
        }

        /// <summary>
        /// Convertit en json un couple nom/valeur
        /// </summary>
        /// <param name="nom"></param>
        /// <param name="valeur"></param>
        /// <returns>couple au format json</returns>
        private String convertToJson(Object nom, Object valeur)
        {
            Dictionary<Object, Object> dictionary = new Dictionary<Object, Object>();
            dictionary.Add(nom, valeur);
            return JsonConvert.SerializeObject(dictionary);
        }

        /// <summary>
        /// Modification du convertisseur Json pour gérer le format de date
        /// </summary>
        private sealed class CustomDateTimeConverter : IsoDateTimeConverter
        {
            public CustomDateTimeConverter()
            {
                base.DateTimeFormat = "yyyy-MM-dd";
            }
        }

        /// <summary>
        /// Modification du convertisseur Json pour prendre en compte les booléens
        /// classe trouvée sur le site :
        /// https://www.thecodebuzz.com/newtonsoft-jsonreaderexception-could-not-convert-string-to-boolean/
        /// </summary>
        private sealed class CustomBooleanJsonConverter : JsonConverter<bool>
        {
            public override bool ReadJson(JsonReader reader, Type objectType, bool existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                return Convert.ToBoolean(reader.ValueType == typeof(string) ? Convert.ToByte(reader.Value) : reader.Value);
            }

            public override void WriteJson(JsonWriter writer, bool value, JsonSerializer serializer)
            {
                serializer.Serialize(writer, value);
            }
        }

    }
}

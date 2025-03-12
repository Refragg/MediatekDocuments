using System;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe représentant une revue dont l'abonnement arrive a expiration 
    /// </summary>
    public class RevueAbonnementAExpiration
    {
        public string IdRevue { get; }
        public string Titre { get; }
        public DateTime DateFinAbonnement { get; }

        public RevueAbonnementAExpiration(string idRevue, string titre, DateTime dateFinAbonnement)
        {
            IdRevue = idRevue;
            Titre = titre;
            DateFinAbonnement = dateFinAbonnement;
        }
    }
}
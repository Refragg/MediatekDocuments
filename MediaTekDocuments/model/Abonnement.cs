using System;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Abonnement représentant une commande de revue
    /// </summary>
    public class Abonnement : Commande
    {
        public DateTime DateFinAbonnement { get; }
        public string IdRevue { get; }
        
        public Abonnement(string id, DateTime dateCommande, double montant, DateTime dateFinAbonnement, string idRevue) : base(id, dateCommande, montant)
        {
            DateFinAbonnement = dateFinAbonnement;
            IdRevue = idRevue;
        }
        
        /// <summary>
        /// Vérifie si une date d'un exemplaire d'une revue est paru dans un abonnement
        /// </summary>
        /// <param name="dateParution">La date de parution</param>
        /// <returns>True si la date de parution est comprise dans l'abonnement, sinon false</returns>
        public bool ParutionDansAbonnement(DateTime dateParution)
        {
            return dateParution >= DateCommande && dateParution < DateFinAbonnement;
        }
    }
}
using System;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Commande représentant une commande d'un document
    /// </summary>
    public class Commande
    {
        public string Id { get; }
        public DateTime DateCommande { get; }
        public double Montant { get; }

        protected Commande(string id, DateTime dateCommande, double montant)
        {
            Id = id;
            DateCommande = dateCommande;
            Montant = montant;
        }
    }
}
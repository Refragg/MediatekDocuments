using System;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier CommandeLivreDvd (réunit les informations des tables Commande et CommandeDocument)
    /// </summary>
    public class CommandeLivreDvd
    {
        public string Id { get; }
        public DateTime DateCommande { get; }
        public double Montant { get; }
        public int NbExemplaire { get; }
        public string IdLivreDvd { get; }
        public string IdSuivi { get; }
        public string Stade { get; }

        public CommandeLivreDvd(string id, DateTime dateCommande, double montant, int nbExemplaire, string idLivreDvd, string idSuivi, string stade)
        {
            Id = id;
            DateCommande = dateCommande;
            Montant = montant;
            NbExemplaire = nbExemplaire;
            IdLivreDvd = idLivreDvd;
            IdSuivi = idSuivi;
            Stade = stade;
        }
    }
}
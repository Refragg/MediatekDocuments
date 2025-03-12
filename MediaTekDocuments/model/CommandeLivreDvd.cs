using System;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier CommandeLivreDvd représentant une commande d'un livre ou DVD
    /// </summary>
    public class CommandeLivreDvd : Commande
    {
        public int NbExemplaire { get; }
        public string IdLivreDvd { get; }
        public string IdSuivi { get; }
        public string Stade { get; }

        public CommandeLivreDvd(string id, DateTime dateCommande, double montant, int nbExemplaire, string idLivreDvd, string idSuivi, string stade) : base(id, dateCommande, montant)
        {
            NbExemplaire = nbExemplaire;
            IdLivreDvd = idLivreDvd;
            IdSuivi = idSuivi;
            Stade = stade;
        }
    }
}
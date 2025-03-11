namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Suivi exprimant les différentes étapes d'une commande
    /// </summary>
    public class Suivi
    {
        public string Id { get; }
        
        public string Stade { get; }

        public Suivi(string id, string stade)
        {
            Id = id;
            Stade = stade;
        }

        /// <summary>
        /// Retourne le libellé d'une étape d'une commande 
        /// </summary>
        /// <returns>Le stade (étape) de cet objet Suivi</returns>
        public override string ToString()
        {
            return Stade;
        }
    }
}
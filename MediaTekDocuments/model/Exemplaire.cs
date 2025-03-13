using System;

namespace MediaTekDocuments.model
{
    /// <summary>
    /// Classe métier Exemplaire (exemplaire d'une revue)
    /// </summary>
    public class Exemplaire
    {
        public int Numero { get; set; }
        public string Photo { get; set; }
        public DateTime DateAchat { get; set; }
        public string IdEtat { get; set; }
        public string Etat { get; set; }
        public string Id { get; set; }

        public Exemplaire(int numero, DateTime dateAchat, string photo, string idEtat, string etat, string idDocument)
        {
            this.Numero = numero;
            this.DateAchat = dateAchat;
            this.Photo = photo;
            this.IdEtat = idEtat;
            this.Etat = etat;
            this.Id = idDocument;
        }

        /// <summary>
        /// Méthode retournant un objet compatible avec les méthodes d'insertion et modification de l'API REST
        /// </summary>
        /// <returns>L'objet Exemplaire compatible avec les méthodes d'insertion et modification de l'API REST</returns>
        public object ToRestApiObject()
        {
            return new
            {
                Numero,
                DateAchat,
                Photo,
                IdEtat,
                Id
            };
        }
    }
}

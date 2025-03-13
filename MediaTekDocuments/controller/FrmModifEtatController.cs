using System.Collections.Generic;
using MediaTekDocuments.dal;
using MediaTekDocuments.model;

namespace MediaTekDocuments.controller
{
    /// <summary>
    /// Contrôleur lié à FrmModifEtatController
    /// </summary>
    class FrmModifEtatController
    {
        /// <summary>
        /// Objet d'accès aux données
        /// </summary>
        private readonly Access access;

        /// <summary>
        /// Récupération de l'instance unique d'accès aux données
        /// </summary>
        public FrmModifEtatController()
        {
            access = Access.GetInstance();
        }

        /// <summary>
        /// Retourne tous les états possible d'un exemplaire à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Etat</returns>
        public List<Etat> GetAllEtats()
        {
            return access.GetAllEtats();
        }
    }
}
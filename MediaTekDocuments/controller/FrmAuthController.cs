using MediaTekDocuments.dal;
using MediaTekDocuments.model;

namespace MediaTekDocuments.controller
{
    /// <summary>
    /// Contrôleur lié à FrmAuth
    /// </summary>
    public class FrmAuthController
    {
        /// <summary>
        /// Objet d'accès aux données
        /// </summary>
        private readonly Access access;

        /// <summary>
        /// Récupération de l'instance unique d'accès aux données
        /// </summary>
        public FrmAuthController()
        {
            access = Access.GetInstance();
        }

        public Utilisateur ControleAuthentification(string login, string password)
        {
            return access.ControleAuthentification(login, password);
        }
    }
}
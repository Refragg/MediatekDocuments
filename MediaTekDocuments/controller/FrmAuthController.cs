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

        /// <summary>
        /// Contrôle l'authentification d'un utilisateur en fonction de son login et de son mot de passe
        /// </summary>
        /// <param name="login">Le login de l'utilisateur</param>
        /// <param name="password">Le mot de passe de l'utilisateur</param>
        /// <returns>L'utilisateur authentifié ou null si erreur</returns>
        public Utilisateur ControleAuthentification(string login, string password)
        {
            return access.ControleAuthentification(login, password);
        }
    }
}
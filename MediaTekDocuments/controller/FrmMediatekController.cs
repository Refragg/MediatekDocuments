using System.Collections.Generic;
using MediaTekDocuments.model;
using MediaTekDocuments.dal;

namespace MediaTekDocuments.controller
{
    /// <summary>
    /// Contrôleur lié à FrmMediatek
    /// </summary>
    class FrmMediatekController
    {
        /// <summary>
        /// Objet d'accès aux données
        /// </summary>
        private readonly Access access;

        /// <summary>
        /// Récupération de l'instance unique d'accès aux données
        /// </summary>
        public FrmMediatekController()
        {
            access = Access.GetInstance();
        }

        /// <summary>
        /// getter sur la liste des genres
        /// </summary>
        /// <returns>Liste d'objets Genre</returns>
        public List<Categorie> GetAllGenres()
        {
            return access.GetAllGenres();
        }

        /// <summary>
        /// getter sur la liste des livres
        /// </summary>
        /// <returns>Liste d'objets Livre</returns>
        public List<Livre> GetAllLivres()
        {
            return access.GetAllLivres();
        }

        /// <summary>
        /// getter sur la liste des Dvd
        /// </summary>
        /// <returns>Liste d'objets dvd</returns>
        public List<Dvd> GetAllDvd()
        {
            return access.GetAllDvd();
        }

        /// <summary>
        /// getter sur la liste des revues
        /// </summary>
        /// <returns>Liste d'objets Revue</returns>
        public List<Revue> GetAllRevues()
        {
            return access.GetAllRevues();
        }

        /// <summary>
        /// Retourne les revues dont l'abonnement arrive a expiration dans moins de 30 jours à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets RevueAbonnementAExpiration</returns>
        public List<RevueAbonnementAExpiration> GetRevuesAbonnementAExpirationProchaine()
        {
            return access.GetRevuesAbonnementAExpirationProchaine();
        }

        /// <summary>
        /// getter sur les rayons
        /// </summary>
        /// <returns>Liste d'objets Rayon</returns>
        public List<Categorie> GetAllRayons()
        {
            return access.GetAllRayons();
        }

        /// <summary>
        /// getter sur les publics
        /// </summary>
        /// <returns>Liste d'objets Public</returns>
        public List<Categorie> GetAllPublics()
        {
            return access.GetAllPublics();
        }


        /// <summary>
        /// récupère les exemplaires d'une revue
        /// </summary>
        /// <param name="idDocuement">id de la revue concernée</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplaires(string idDocuement)
        {
            return access.GetExemplaires(idDocuement);
        }
        
        /// <summary>
        /// Retourne le nombre de commandes d'un livre / DVD
        /// </summary>
        /// <param name="idDocument">id du livre / DVD concerné</param>
        /// <returns>Nombre de commandes du livre / DVD</returns>
        public int GetCommandesCountLivreDvd(string idDocuement)
        {
            return access.GetCommandesCountLivreDvd(idDocuement);
        }
        
        /// <summary>
        /// Retourne le nombre de commandes d'une revue
        /// </summary>
        /// <param name="idDocument">id de la revue concernée</param>
        /// <returns>Nombre de commandes de la revue</returns>
        public int GetCommandesCountRevue(string idDocuement)
        {
            return access.GetCommandesCountRevue(idDocuement);
        }

        /// <summary>
        /// Crée un exemplaire d'une revue dans la bdd
        /// </summary>
        /// <param name="exemplaire">L'objet Exemplaire concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerExemplaire(Exemplaire exemplaire)
        {
            return access.CreerExemplaire(exemplaire);
        }

        /// <summary>
        /// Modification d'un exemplaire en base de données
        /// </summary>
        /// <param name="exemplaire">L'exemplaire à modifier</param>
        /// <returns>true si la modification a pu se faire (retour != null)</returns>
        public bool ModifierExemplaire(Exemplaire exemplaire)
        {
            return access.ModifierExemplaire(exemplaire);
        }
        
        /// <summary>
        /// Crée un livre dans la bdd
        /// </summary>
        /// <param name="livre">L'objet Livre concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerLivre(Livre livre)
        {
            return access.CreerLivre(livre);
        }

        /// <summary>
        /// Modifie un livre dans la base de données
        /// </summary>
        /// <param name="livre">Le livre avec ses champs modifiés</param>
        /// <returns>True si la modification à réussi, false sinon</returns>
        public bool ModifierLivre(Livre livre)
        {
            return access.ModifierLivre(livre);
        }

        /// <summary>
        /// Supprime un livre de la base de données
        /// </summary>
        /// <param name="livre">Le livre à supprimer</param>
        /// <returns>True si la suppression à réussi, false sinon</returns>
        public bool SupprimerLivre(Livre livre)
        {
            return SupprimerLivre(livre.Id);
        }
        
        /// <summary>
        /// Supprime un livre de la base de données
        /// </summary>
        /// <param name="idLivre">L'identifiant du livre à supprimer</param>
        /// <returns>True si la suppression à réussi, false sinon</returns>
        public bool SupprimerLivre(string idLivre)
        {
            return access.SupprimerLivre(idLivre);
        }
        
        /// <summary>
        /// Crée un DVD dans la bdd
        /// </summary>
        /// <param name="dvd">L'objet DVD concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerDvd(Dvd dvd)
        {
            return access.CreerDvd(dvd);
        }

        /// <summary>
        /// Modifie un DVD dans la base de données
        /// </summary>
        /// <param name="dvd">Le DVD avec ses champs modifiés</param>
        /// <returns>True si la modification à réussi, false sinon</returns>
        public bool ModifierDvd(Dvd dvd)
        {
            return access.ModifierDvd(dvd);
        }

        /// <summary>
        /// Supprime un DVD de la base de données
        /// </summary>
        /// <param name="dvd">Le DVD à supprimer</param>
        /// <returns>True si la suppression à réussi, false sinon</returns>
        public bool SupprimerDvd(Dvd dvd)
        {
            return SupprimerDvd(dvd.Id);
        }
        
        /// <summary>
        /// Supprime un DVD de la base de données
        /// </summary>
        /// <param name="idDvd">L'identifiant du DVD à supprimer</param>
        /// <returns>True si la suppression à réussi, false sinon</returns>
        public bool SupprimerDvd(string idDvd)
        {
            return access.SupprimerDvd(idDvd);
        }
        
        /// <summary>
        /// Crée une revue dans la bdd
        /// </summary>
        /// <param name="revue">L'objet revue concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerRevue(Revue revue)
        {
            return access.CreerRevue(revue);
        }

        /// <summary>
        /// Modifie une revue dans la base de données
        /// </summary>
        /// <param name="revue">La revue avec ses champs modifiés</param>
        /// <returns>True si la modification à réussi, false sinon</returns>
        public bool ModifierRevue(Revue revue)
        {
            return access.ModifierRevue(revue);
        }

        /// <summary>
        /// Supprime une revue de la base de données
        /// </summary>
        /// <param name="revue">La revue à supprimer</param>
        /// <returns>True si la suppression à réussi, false sinon</returns>
        public bool SupprimerRevue(Revue revue)
        {
            return SupprimerRevue(revue.Id);
        }
        
        /// <summary>
        /// Supprime une revue de la base de données
        /// </summary>
        /// <param name="idRevue">L'identifiant de la revue à supprimer</param>
        /// <returns>True si la suppression à réussi, false sinon</returns>
        public bool SupprimerRevue(string idRevue)
        {
            return access.SupprimerRevue(idRevue);
        }
    }
}

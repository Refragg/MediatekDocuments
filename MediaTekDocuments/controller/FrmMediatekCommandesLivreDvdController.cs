using System.Collections.Generic;
using MediaTekDocuments.dal;
using MediaTekDocuments.model;

namespace MediaTekDocuments.controller
{
    /// <summary>
    /// Contrôleur lié à FrmMediatekCommandesLivreDvd
    /// </summary>
    public class FrmMediatekCommandesLivreDvdController
    {
        /// <summary>
        /// Objet d'accès aux données
        /// </summary>
        private readonly Access access;

        /// <summary>
        /// Récupération de l'instance unique d'accès aux données
        /// </summary>
        public FrmMediatekCommandesLivreDvdController()
        {
            access = Access.GetInstance();
        }

        /// <summary>
        /// Récupération des commandes pour un livre / DVD
        /// </summary>
        /// <param name="livreDvd">Le livre / DVD duquel obtenir les commandes</param>
        /// <returns>La liste des commandes du livre / DVD</returns>
        public List<CommandeLivreDvd> GetCommandes(LivreDvd livreDvd)
        {
            return access.GetCommandes(livreDvd);
        }

        /// <summary>
        /// Suppression d'une commande de la base de données
        /// </summary>
        /// <param name="commande">La commande a supprimer</param>
        /// <returns>True si la commande a été supprimée avec succès, sinon false</returns>
        public bool SupprimerCommande(CommandeLivreDvd commande)
        {
            return access.SupprimerCommandeLivreDvd(commande.Id);
        }

        /// <summary>
        /// Récupération de toutes les étapes d'une commande
        /// </summary>
        /// <returns>La liste des étapes possibles d'une commande</returns>
        public List<Suivi> GetAllSuivis()
        {
            return access.GetAllSuivis();
        }
        
        /// <summary>
        /// Crée une commande dans la bdd
        /// </summary>
        /// <param name="commande">L'objet Commande concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerCommande(CommandeLivreDvd commande)
        {
            return access.CreerCommande(commande);
        }

        /// <summary>
        /// Modifie une commande dans la base de données
        /// </summary>
        /// <param name="commande">La commande avec ses champs modifiés</param>
        /// <returns>True si la modification à réussi, false sinon</returns>
        public bool ModifierCommande(CommandeLivreDvd commande)
        {
            return access.ModifierCommande(commande);
        }
    }
}
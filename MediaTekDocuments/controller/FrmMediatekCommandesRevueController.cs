using System;
using System.Collections.Generic;
using MediaTekDocuments.dal;
using MediaTekDocuments.model;

namespace MediaTekDocuments.controller
{
    public class FrmMediatekCommandesRevueController
    {
        /// <summary>
        /// Objet d'accès aux données
        /// </summary>
        private readonly Access access;

        /// <summary>
        /// Récupération de l'instance unique d'accès aux données
        /// </summary>
        public FrmMediatekCommandesRevueController()
        {
            access = Access.GetInstance();
        }
        
        /// <summary>
        /// Récupération des commandes pour une revue
        /// </summary>
        /// <param name="revue">La revue de laquelle obtenir les commandes</param>
        /// <returns>La liste des commandes de la revue</returns>
        public List<Abonnement> GetCommandes(Revue revue)
        {
            return access.GetCommandes(revue);
        }

        /// <summary>
        /// Suppression d'une commande d'une revue de la base de données
        /// </summary>
        /// <param name="commande">La commande a supprimer</param>
        /// <returns>True si la commande a été supprimée avec succès, sinon false</returns>
        public bool SupprimerCommande(Abonnement commande)
        {
            return access.SupprimerCommandeRevue(commande.Id);
        }
        
        /// <summary>
        /// Crée une commande d'une revue dans la bdd
        /// </summary>
        /// <param name="commande">L'objet Commande concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerCommande(Abonnement commande)
        {
            return access.CreerCommande(commande);
        }

        /// <summary>
        /// Modifie une commande d'une revue dans la base de données
        /// </summary>
        /// <param name="commande">La commande avec ses champs modifiés</param>
        /// <returns>True si la modification à réussi, false sinon</returns>
        public bool ModifierCommande(Abonnement commande)
        {
            return access.ModifierCommande(commande);
        }

        /// <summary>
        /// Retourne les exemplaires d'un document
        /// </summary>
        /// <param name="idDocument">id du document concerné</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplaires(string idDocument)
        {
            return access.GetExemplaires(idDocument);
        }
    }
}
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
        /// Suppression d'un abonnement d'une revue de la base de données
        /// </summary>
        /// <param name="abonnement">L'abonnement a supprimer</param>
        /// <returns>True si l'abonnement a été supprimé avec succès, sinon false</returns>
        public bool SupprimerAbonnement(Abonnement abonnement)
        {
            return access.SupprimerAbonnement(abonnement.Id);
        }
        
        /// <summary>
        /// Crée un abonnement d'une revue dans la bdd
        /// </summary>
        /// <param name="abonnement">L'objet Abonnement concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerAbonnement(Abonnement abonnement)
        {
            return access.CreerAbonnement(abonnement);
        }

        /// <summary>
        /// Modifie un abonnement d'une revue dans la base de données
        /// </summary>
        /// <param name="abonnement">L'abonnement avec ses champs modifiés</param>
        /// <returns>True si la modification à réussi, false sinon</returns>
        public bool ModifierAbonnement(Abonnement abonnement)
        {
            return access.ModifierAbonnement(abonnement);
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MediaTekDocuments.controller;
using MediaTekDocuments.model;

namespace MediaTekDocuments.view
{
    /// <summary>
    /// Classe du formulaire de gestion des commandes pour les revues
    /// </summary>
    public partial class FrmMediatekCommandesRevue : Form
    {
        private bool commandesEnAjout = false;
        private bool commandesEnModif = false;
        private Revue revue;
        private FrmMediatekCommandesRevueController controller;
        private readonly BindingSource bdgCommandesListe = new BindingSource();
        private List<Abonnement> lesCommandes;
        
        /// <summary>
        /// Constructeur : création du formulaire et remplissage des informations
        /// </summary>
        /// <param name="revue"></param>
        public FrmMediatekCommandesRevue(Revue revue)
        {
            InitializeComponent();

            controller = new FrmMediatekCommandesRevueController();
            this.revue = revue;

            lesCommandes = controller.GetCommandes(revue);
            RemplirCommandesListe(lesCommandes);
            dgvCommandes_SelectionChanged(null, null);
        }
        
        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="commandes">liste de commandes</param>
        private void RemplirCommandesListe(List<Abonnement> commandes)
        {
            bdgCommandesListe.DataSource = commandes;
            dgvCommandes.DataSource = bdgCommandesListe;
            dgvCommandes.Columns["dateCommande"].HeaderText = "Date commande";
            dgvCommandes.Columns["dateFinAbonnement"].HeaderText = "Date fin abonnement";
            dgvCommandes.Columns["dateCommande"].DisplayIndex = 0;
            dgvCommandes.Columns["montant"].DisplayIndex = 1;
            dgvCommandes.Columns["dateFinAbonnement"].DisplayIndex = 2;
            dgvCommandes.Columns["id"].Visible = false;
            dgvCommandes.Columns["idRevue"].Visible = false;
        }
        
        /// <summary>
        /// Changement de l'état d'activation des boutons d'action pour les commandes
        /// </summary>
        /// <param name="etatActions">Les actions doivent-elles être activées ou non (les boutons valider et annuler prennent la valeur inverse)</param>
        private void ChangerEtatActions(bool etatActions)
        {
            btnAjouter.Enabled = etatActions;
            btnModifier.Enabled = etatActions;
            btnSupprimer.Enabled = etatActions;
            
            btnValider.Enabled = !etatActions;
            btnAnnuler.Enabled = !etatActions;
        }
        
        /// <summary>
        /// Changement de l'état d'écriture des champs d'édition d'une commande
        /// </summary>
        /// <param name="etatInfos">True si les champs doivent être en lecture / écriture, false s'il doivent être en lecture seule</param>
        /// <param name="ajout">True si cela concerne un ajout, false sinon (ce paramètre change l'état du champ numéro)</param>
        private void ChangerEtatInfos(bool etatInfos, bool ajout)
        {
            txbNumCommande.ReadOnly = !(ajout && etatInfos);
            txbMontant.ReadOnly = !etatInfos;
            dtpDateCommande.Visible = etatInfos;
            dtpDateFinAbonnement.Visible = etatInfos;
        }

        /// <summary>
        /// Méthode événementielle au clic sur le bouton ajouter
        /// Cette méthode prépare le formulaire pour l'ajout d'une commande
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAjouter_Click(object sender, EventArgs e)
        {
            dgvCommandes.Enabled = false;
            commandesEnAjout = true;
            ChangerEtatActions(false);
            ChangerEtatInfos(true, true);
            VideCommandesInfos();
        }
        
        /// <summary>
        /// Méthode événementielle au clic sur le bouton modifier
        /// Cette méthode prépare le formulaire pour la modification d'une commande
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModifier_Click(object sender, EventArgs e)
        {
            dgvCommandes.Enabled = false;
            commandesEnModif = true;
            ChangerEtatActions(false);
            ChangerEtatInfos(true, false);
        }

        /// <summary>
        /// Méthode événementielle au clic sur le bouton supprimer
        /// Cette méthode gère la suppression d'une commande après vérification et confirmation de l'utilisateur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSupprimer_Click(object sender, EventArgs e)
        {
            Abonnement commande = (Abonnement)bdgCommandesListe.List[bdgCommandesListe.Position];
            List<Exemplaire> exemplaires = controller.GetExemplaires(revue.Id);
            if (exemplaires.Any(exemplaire => commande.ParutionDansAbonnement(exemplaire.DateAchat)))
            {
                MessageBox.Show("Un abonnement ne peut pas être supprimé si des exemplaires y sont rattachés");
                return;
            }

            if (MessageBox.Show($"Voulez vous vraiment supprimer la commande '{commande.Id}' ?", "Suppression de commande",
                    MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            if (!controller.SupprimerAbonnement(commande))
            {
                MessageBox.Show("La commande n'a pas pu être supprimée");
                return;
            }

            lesCommandes.Remove(commande);
            RemplirCommandesListe(lesCommandes);
            bdgCommandesListe.ResetBindings(false);
            dgvCommandes_SelectionChanged(null, null);
        }
        
        /// <summary>
        /// Méthode événementielle au clic sur le bouton annuler
        /// Cette méthode annule l'ajout / modification de la commande en cours
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnnuler_Click(object sender, EventArgs e)
        {
            dgvCommandes.Enabled = true;
            commandesEnAjout = false;
            commandesEnModif = false;
            ChangerEtatActions(true);
            ChangerEtatInfos(false, false);
            dgvCommandes_SelectionChanged(null, null);   
        }

        /// <summary>
        /// Méthode événementielle au clic sur le bouton valider
        /// Cette méthode gère l'ajout / modification d'une commande
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnValider_Click(object sender, EventArgs e)
        {
            if (!ValiderFormulaireCommandes())
                return;
            
            string numero = txbNumCommande.Text;
            double montant = double.Parse(txbMontant.Text);
            DateTime dateCommande = dtpDateCommande.Value.Date;
            DateTime dateFinAbonnement = dtpDateFinAbonnement.Value.Date;
            
            if (commandesEnModif)
            {
                commandesEnModif = false;
                Abonnement commandeModif = new Abonnement(numero, dateCommande, montant, dateFinAbonnement, revue.Id);

                bool resultat = controller.ModifierAbonnement(commandeModif);
                if (resultat)
                {
                    bdgCommandesListe.List[bdgCommandesListe.Position] = commandeModif;
                    bdgCommandesListe.ResetBindings(false);
                    dgvCommandes.Invalidate();
                }
                else
                    MessageBox.Show("La commande n'a pas pu être modifiée.");
            }
            else
            {
                commandesEnAjout = false;
                Abonnement nouvelleCommande = new Abonnement(numero, dateCommande, montant, dateFinAbonnement, revue.Id);
                
                bool resultat = controller.CreerAbonnement(nouvelleCommande);
                if (resultat)
                {
                    lesCommandes.Add(nouvelleCommande);
                    
                    int scrollPosition = dgvCommandes.FirstDisplayedScrollingRowIndex;
                    RemplirCommandesListe(lesCommandes);
                    bdgCommandesListe.ResetBindings(false);
                    int index = bdgCommandesListe.IndexOf(nouvelleCommande);
                    bdgCommandesListe.Position = index;
                    dgvCommandes.Invalidate();
                    if (scrollPosition >= 0)
                        dgvCommandes.FirstDisplayedScrollingRowIndex = scrollPosition;
                }
                else
                    MessageBox.Show("La commande n'a pas pu être ajoutée. Veuillez vérifier que son numéro de commande est unique.");
            }
            
            dgvCommandes.Enabled = true;
            ChangerEtatActions(true);
            ChangerEtatInfos(false, false);
            dgvCommandes_SelectionChanged(null, null);
        }

        /// <summary>
        /// Méthode événementielle au clic sur une colonne
        /// Cette méthode gère le tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCommandes_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvCommandes.Columns[e.ColumnIndex].HeaderText;
            List<Abonnement> sortedList = new List<Abonnement>();
            switch (titreColonne)
            {
                case "N° Commande":
                    sortedList = lesCommandes.OrderBy(o => o.Id).ToList();
                    break;
                case "Date commande":
                    sortedList = lesCommandes.OrderBy(o => o.DateCommande).ToList();
                    break;
                case "Montant":
                    sortedList = lesCommandes.OrderBy(o => o.Montant).ToList();
                    break;
                case "Date fin abonnement":
                    sortedList = lesCommandes.OrderBy(o => o.DateFinAbonnement).ToList();
                    break;
            }
            RemplirCommandesListe(sortedList);
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations de la commande
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCommandes_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvCommandes.CurrentCell != null)
            {
                try
                {
                    Abonnement commande = (Abonnement)bdgCommandesListe.List[bdgCommandesListe.Position];
                    AfficheCommandesInfos(commande);
                    btnModifier.Enabled = true;
                    btnSupprimer.Enabled = true;
                }
                catch
                {
                    btnModifier.Enabled = false;
                    btnSupprimer.Enabled = false;
                }
            }
            else
            {
                btnModifier.Enabled = false;
                btnSupprimer.Enabled = false;
                VideCommandesInfos();
            }
        }
        
        /// <summary>
        /// Affichage des informations de la commande sélectionnée
        /// </summary>
        /// <param name="commande">La commande</param>
        private void AfficheCommandesInfos(Abonnement commande)
        {
            txbNumCommande.Text = commande.Id;
            txbMontant.Text = commande.Montant.ToString();
            txbDateCommande.Text = commande.DateCommande.ToShortDateString();
            dtpDateCommande.Value = commande.DateCommande;
            txbDateFinAbonnement.Text = commande.DateFinAbonnement.ToShortDateString();
            dtpDateFinAbonnement.Value = commande.DateFinAbonnement;
        }
        
        /// <summary>
        /// Vide les zones d'affichage des informations d'une commande
        /// </summary>
        private void VideCommandesInfos()
        {
            txbNumCommande.Text = "";
            txbMontant.Text = "";
            txbDateCommande.Text = "";
            dtpDateCommande.Value = DateTime.Now;
            txbDateFinAbonnement.Text = "";
            dtpDateFinAbonnement.Value = DateTime.Now;
        }
        
        /// <summary>
        /// Validation du formulaire d'édition d'une commande
        /// Si le formulaire n'est pas valide, la méthode ouvre une boite de dialogue pour en informer l'utilisateur
        /// </summary>
        /// <returns>True si le formulaire est valide, sinon false</returns>
        private bool ValiderFormulaireCommandes()
        {
            if (!double.TryParse(txbMontant.Text, out _))
            {
                MessageBox.Show("Le montant renseigné est invalide. Les caractères autorisés sont les chiffres et la virgule.");
                return false;
            }

            if (string.IsNullOrEmpty(txbNumCommande.Text))
            {
                MessageBox.Show("Le numéro de commande ne peut être vide");
                return false;
            }
            
            if (!txbNumCommande.Text.All(char.IsDigit))
            {
                MessageBox.Show("Le numéro de commande doit être composé seulement de chiffres");
                return false;
            }
            
            return true;
        }

        private void FrmMediatekCommandesRevue_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (commandesEnModif || commandesEnAjout)
            {
                MessageBox.Show("Veuillez valider ou annuler l'opération en cours avant de fermer cette fenêtre.");
                e.Cancel = true;
            }
        }
    }
}
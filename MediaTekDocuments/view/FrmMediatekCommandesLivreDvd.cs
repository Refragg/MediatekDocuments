using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MediaTekDocuments.controller;
using MediaTekDocuments.model;

namespace MediaTekDocuments.view
{
    /// <summary>
    /// Classe du formulaire de gestion des commandes pour les livres et DVDs 
    /// </summary>
    public partial class FrmMediatekCommandesLivreDvd : Form
    {
        private bool commandesEnAjout = false;
        private bool commandesEnModif = false;
        private LivreDvd livreDvd;
        private FrmMediatekCommandesLivreDvdController controller;
        private readonly BindingSource bdgCommandesListe = new BindingSource();
        private readonly BindingSource bdgSuivis = new BindingSource();
        private List<CommandeLivreDvd> lesCommandes = new List<CommandeLivreDvd>();
        
        /// <summary>
        /// Constructeur : création du formulaire et remplissage des informations
        /// </summary>
        /// <param name="livreDvd"></param>
        public FrmMediatekCommandesLivreDvd(LivreDvd livreDvd)
        {
            InitializeComponent();
            nudNbExemplaires.Text = "";

            controller = new FrmMediatekCommandesLivreDvdController();
            this.livreDvd = livreDvd;

            lesCommandes = controller.GetCommandes(livreDvd);
            List<Suivi> suivis = controller.GetAllSuivis();
            RemplirComboSuivis(suivis, bdgSuivis, cbxStade);
            RemplirCommandesListe(lesCommandes);
            dgvCommandes_SelectionChanged(null, null);
        }
        
        /// <summary>
        /// Remplit un des combobox avec les suivis
        /// </summary>
        /// <param name="lesSuivis">liste des objets de type suivi</param>
        /// <param name="bdg">bindingsource contenant les informations</param>
        /// <param name="cbx">combobox à remplir</param>
        public void RemplirComboSuivis(List<Suivi> lesSuivis, BindingSource bdg, ComboBox cbx)
        {
            bdg.DataSource = lesSuivis;
            cbx.DataSource = bdg;
            if (cbx.Items.Count > 0)
            {
                cbx.SelectedIndex = -1;
            }
        }
        
        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="commandes">liste de commandes</param>
        private void RemplirCommandesListe(List<CommandeLivreDvd> commandes)
        {
            bdgCommandesListe.DataSource = commandes;
            dgvCommandes.DataSource = bdgCommandesListe;
            dgvCommandes.Columns["dateCommande"].HeaderText = "Date commande";
            dgvCommandes.Columns["nbExemplaire"].HeaderText = "Nombre d'exemplaires";
            dgvCommandes.Columns["dateCommande"].DisplayIndex = 0;
            dgvCommandes.Columns["montant"].DisplayIndex = 1;
            dgvCommandes.Columns["nbExemplaire"].DisplayIndex = 2;
            dgvCommandes.Columns["stade"].DisplayIndex = 3;
            dgvCommandes.Columns["id"].Visible = false;
            dgvCommandes.Columns["idSuivi"].Visible = false;
            dgvCommandes.Columns["idLivreDvd"].Visible = false;
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
                    CommandeLivreDvd commande = (CommandeLivreDvd)bdgCommandesListe.List[bdgCommandesListe.Position];
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
        private void AfficheCommandesInfos(CommandeLivreDvd commande)
        {
            txbNumCommande.Text = commande.Id;
            txbMontant.Text = commande.Montant.ToString();
            txbDateCommande.Text = commande.DateCommande.ToShortDateString();
            dtpDateCommande.Value = commande.DateCommande;
            nudNbExemplaires.Value = commande.NbExemplaire;
            txbStade.Text = commande.Stade;
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
            nudNbExemplaires.Text = "";
            cbxStade.SelectedIndex = 0;
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
            List<CommandeLivreDvd> sortedList = new List<CommandeLivreDvd>();
            Console.WriteLine(titreColonne);
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
                case "Nombre d'exemplaires":
                    sortedList = lesCommandes.OrderBy(o => o.NbExemplaire).ToList();
                    break;
                case "Stade":
                    sortedList = lesCommandes.OrderBy(o => o.Stade).ToList();
                    break;
            }
            RemplirCommandesListe(sortedList);
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
            txbStade.Text = "En cours";
            nudNbExemplaires.Value = 1;
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
            
            CommandeLivreDvd commande = (CommandeLivreDvd)bdgCommandesListe.List[bdgCommandesListe.Position];
            cbxStade.SelectedItem = cbxStade.Items.OfType<Suivi>().FirstOrDefault(x => x.Stade == commande.Stade);
        }

        /// <summary>
        /// Méthode événementielle au clic sur le bouton supprimer
        /// Cette méthode gère la suppression d'une commande après vérification et confirmation de l'utilisateur
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSupprimer_Click(object sender, EventArgs e)
        {
            CommandeLivreDvd commande = (CommandeLivreDvd)bdgCommandesListe.List[bdgCommandesListe.Position];
            
            if (commande.Stade == "Livrée" || commande.Stade == "Réglée")
            {
                MessageBox.Show("Une commande ne peut pas être supprimée si elle a déjà été livrée");
                return;
            }

            if (MessageBox.Show($"Voulez vous vraiment supprimer la commande '{commande.Id}' ?", "Suppression de commande",
                    MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            if (!controller.SupprimerCommande(commande))
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
            CommandeLivreDvd commandePrecedente = null;

            if (commandesEnModif)
                commandePrecedente = (CommandeLivreDvd)bdgCommandesListe.List[bdgCommandesListe.Position];
            
            if (!ValiderFormulaireCommandes(commandePrecedente))
                return;
            
            string numero = txbNumCommande.Text;
            double montant = double.Parse(txbMontant.Text);
            int nbExemplaires = (int)nudNbExemplaires.Value;
            DateTime dateCommande = dtpDateCommande.Value.Date;
            Suivi stade = (Suivi)cbxStade.SelectedItem;
            
            if (commandesEnModif)
            {
                commandesEnModif = false;
                CommandeLivreDvd commandeModif = new CommandeLivreDvd(numero, dateCommande, montant, nbExemplaires, livreDvd.Id,
                    stade.Id, stade.Stade);

                bool resultat = controller.ModifierCommande(commandeModif);
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
                CommandeLivreDvd nouvelleCommande = new CommandeLivreDvd(numero, dateCommande, montant, nbExemplaires, livreDvd.Id,
                    stade.Id, stade.Stade);
                
                bool resultat = controller.CreerCommande(nouvelleCommande);
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
            nudNbExemplaires.ReadOnly = !etatInfos;
            dtpDateCommande.Visible = etatInfos;
            cbxStade.Visible = !ajout && etatInfos;
        }
        
        /// <summary>
        /// Validation du formulaire d'édition d'une commande
        /// Si le formulaire n'est pas valide, la méthode ouvre une boite de dialogue pour en informer l'utilisateur
        /// </summary>
        /// <returns>True si le formulaire est valide, sinon false</returns>
        private bool ValiderFormulaireCommandes(CommandeLivreDvd commandeAvantModif)
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

            if (commandeAvantModif is null)
                return true;
            
            string stadePrecedent = commandeAvantModif.Stade;
            string nouveauStade = ((Suivi)cbxStade.SelectedItem).Stade;

            if ((stadePrecedent == "Livrée" || stadePrecedent == "Réglée") &&
                (nouveauStade == "En cours" || nouveauStade == "Relancée"))
            {
                MessageBox.Show("Une commande déjà livrée / réglée ne peut pas revenir à une étape précédente");
                return false;
            }

            if (nouveauStade == "Réglée" && stadePrecedent != "Livrée")
            {
                MessageBox.Show("Une commande ne peut pas être réglée si elle n'est pas livrée");
                return false;
            }
            
            return true;
        }
    }
}
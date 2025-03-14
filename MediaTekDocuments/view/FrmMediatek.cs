using System;
using System.Windows.Forms;
using MediaTekDocuments.model;
using MediaTekDocuments.controller;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.IO;

namespace MediaTekDocuments.view

{
    /// <summary>
    /// Classe d'affichage
    /// </summary>
    public partial class FrmMediatek : Form
    {
        #region Commun
        private readonly Utilisateur utilisateur;
        private readonly FrmMediatekController controller;
        private readonly BindingSource bdgGenres = new BindingSource();
        private readonly BindingSource bdgPublics = new BindingSource();
        private readonly BindingSource bdgRayons = new BindingSource();
        private readonly BindingSource bdgGenresEdit = new BindingSource();
        private readonly BindingSource bdgPublicsEdit = new BindingSource();
        private readonly BindingSource bdgRayonsEdit = new BindingSource();

        /// <summary>
        /// Constructeur : création du contrôleur lié à ce formulaire et activation / désactivation
        /// des fonctionnalités en fonction de l'utilisateur de l'application
        /// </summary>
        internal FrmMediatek(Utilisateur utilisateur)
        {
            InitializeComponent();
            this.controller = new FrmMediatekController();
            this.utilisateur = utilisateur;

            if (!utilisateur.Administrateur && utilisateur.Service != "Administratif")
            {
                if (utilisateur.Service == "Prets")
                {
                    grpLivresActions.Enabled = false;
                    btnLivresExemplairesSupprimer.Visible = false;
                    dgvLivresExemplaires.CellMouseDoubleClick -= dgvLivresExemplaires_CellMouseDoubleClick;
                    
                    grpDvdActions.Enabled = false;
                    btnDvdExemplairesSupprimer.Visible = false;
                    dgvDvdExemplaires.CellMouseDoubleClick -= dgvDvdExemplaires_CellMouseDoubleClick;
                    
                    grpRevuesActions.Enabled = false;
                    
                    grpReceptionExemplaire.Enabled = false;
                    btnReceptionParutionsSupprimer.Visible = false;
                    dgvReceptionExemplairesListe.CellMouseDoubleClick -= dgvReceptionExemplairesListe_CellMouseDoubleClick;
                }

                return;
            }
            
            List<RevueAbonnementAExpiration> revuesExpirationProchaine = controller.GetRevuesAbonnementAExpirationProchaine();

            if (revuesExpirationProchaine.Count != 0)
            {
                string titre = "Des revues arrivent à expiration";
                string message = "Les revues suivantes arrivent à expiration dans moins de 30 jours :\n";
                
                foreach (RevueAbonnementAExpiration revue in revuesExpirationProchaine)
                    message += "- '" + revue.Titre + "' - Expire le " + revue.DateFinAbonnement.ToShortDateString() + '\n';

                // Enlèvement du dernière retour à la ligne
                message.Remove(message.Length - 1);

                MessageBox.Show(message, titre);
            }
        }

        /// <summary>
        /// Rempli un des 3 combo (genre, public, rayon)
        /// </summary>
        /// <param name="lesCategories">liste des objets de type Genre ou Public ou Rayon</param>
        /// <param name="bdg">bindingsource contenant les informations</param>
        /// <param name="cbx">combobox à remplir</param>
        public void RemplirComboCategorie(List<Categorie> lesCategories, BindingSource bdg, ComboBox cbx)
        {
            bdg.DataSource = lesCategories;
            cbx.DataSource = bdg;
            if (cbx.Items.Count > 0)
            {
                cbx.SelectedIndex = -1;
            }
        }
        
        /// <summary>
        /// Méthode événementielle à la section d'un onglet
        /// Cette méthode empêche le changement d'onglet si une modification est en cours
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabOngletsApplication_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (livresEnAjout || livresEnModif || dvdsEnAjout || dvdsEnModif || revuesEnAjout || revuesEnModif)
                e.Cancel = true;
        }
        #endregion

        #region Onglet Livres
        private readonly BindingSource bdgLivresListe = new BindingSource();
        private readonly BindingSource bdgLivresExemplaires = new BindingSource();
        private List<Livre> lesLivres = new List<Livre>();
        private List<Exemplaire> lesExemplairesLivre = new List<Exemplaire>();
        private bool livresEnAjout = false;
        private bool livresEnModif = false;

        /// <summary>
        /// Ouverture de l'onglet Livres : 
        /// appel des méthodes pour remplir le datagrid des livres et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabLivres_Enter(object sender, EventArgs e)
        {
            lesLivres = controller.GetAllLivres();

            List<Categorie> genres = controller.GetAllGenres();
            List<Categorie> publics = controller.GetAllPublics();
            List<Categorie> rayons = controller.GetAllRayons();
            
            RemplirComboCategorie(genres, bdgGenres, cbxLivresGenres);
            RemplirComboCategorie(publics, bdgPublics, cbxLivresPublics);
            RemplirComboCategorie(rayons, bdgRayons, cbxLivresRayons);
            RemplirComboCategorie(genres, bdgGenresEdit, cbxLivresGenreEdit);
            RemplirComboCategorie(publics, bdgPublicsEdit, cbxLivresPublicEdit);
            RemplirComboCategorie(rayons, bdgRayonsEdit, cbxLivresRayonEdit);
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="livres">liste de livres</param>
        private void RemplirLivresListe(List<Livre> livres)
        {
            bdgLivresListe.DataSource = livres;
            dgvLivresListe.DataSource = bdgLivresListe;
            dgvLivresListe.Columns["isbn"].Visible = false;
            dgvLivresListe.Columns["idRayon"].Visible = false;
            dgvLivresListe.Columns["idGenre"].Visible = false;
            dgvLivresListe.Columns["idPublic"].Visible = false;
            dgvLivresListe.Columns["image"].Visible = false;
            dgvLivresListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvLivresListe.Columns["id"].DisplayIndex = 0;
            dgvLivresListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage du livre dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbLivresNumRecherche.Text.Equals(""))
            {
                txbLivresTitreRecherche.Text = "";
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                Livre livre = lesLivres.Find(x => x.Id.Equals(txbLivresNumRecherche.Text));
                if (livre != null)
                {
                    List<Livre> livres = new List<Livre>() { livre };
                    RemplirLivresListe(livres);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirLivresListeComplete();
                }
            }
            else
            {
                RemplirLivresListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des livres dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbLivresTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbLivresTitreRecherche.Text.Equals(""))
            {
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                txbLivresNumRecherche.Text = "";
                List<Livre> lesLivresParTitre;
                lesLivresParTitre = lesLivres.FindAll(x => x.Titre.ToLower().Contains(txbLivresTitreRecherche.Text.ToLower()));
                RemplirLivresListe(lesLivresParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxLivresGenres.SelectedIndex < 0 && cbxLivresPublics.SelectedIndex < 0 && cbxLivresRayons.SelectedIndex < 0
                    && txbLivresNumRecherche.Text.Equals(""))
                {
                    RemplirLivresListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations du livre sélectionné
        /// </summary>
        /// <param name="livre">le livre</param>
        private void AfficheLivresInfos(Livre livre)
        {
            txbLivresAuteur.Text = livre.Auteur;
            txbLivresCollection.Text = livre.Collection;
            txbLivresImage.Text = livre.Image;
            txbLivresIsbn.Text = livre.Isbn;
            txbLivresNumero.Text = livre.Id;
            txbLivresGenre.Text = livre.Genre;
            txbLivresPublic.Text = livre.Public;
            txbLivresRayon.Text = livre.Rayon;
            txbLivresTitre.Text = livre.Titre;
            string image = livre.Image;
            try
            {
                pcbLivresImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbLivresImage.Image = null;
            }

            AfficheLivresExemplaires();
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du livre
        /// </summary>
        private void VideLivresInfos()
        {
            txbLivresAuteur.Text = "";
            txbLivresCollection.Text = "";
            txbLivresImage.Text = "";
            txbLivresIsbn.Text = "";
            txbLivresNumero.Text = "";
            
            txbLivresGenre.Text = "";
            txbLivresPublic.Text = "";
            txbLivresRayon.Text = "";

            cbxLivresGenreEdit.SelectedIndex = 0;
            cbxLivresPublicEdit.SelectedIndex = 0;
            cbxLivresRayonEdit.SelectedIndex = 0;
            
            txbLivresTitre.Text = "";
            pcbLivresImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresGenres.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Genre genre = (Genre)cbxLivresGenres.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirLivresListe(livres);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresPublics.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Public lePublic = (Public)cbxLivresPublics.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirLivresListe(livres);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresRayons.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxLivresRayons.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirLivresListe(livres);
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLivresListe.CurrentCell != null)
            {
                try
                {
                    Livre livre = (Livre)bdgLivresListe.List[bdgLivresListe.Position];
                    AfficheLivresInfos(livre);
                    btnLivresModifier.Enabled = true;
                    btnLivresSupprimer.Enabled = true;
                    btnLivresCommandes.Enabled = true;
                }
                catch
                {
                    btnLivresModifier.Enabled = false;
                    btnLivresSupprimer.Enabled = false;
                    btnLivresCommandes.Enabled = false;
                    RemplirLivresExemplairesListe(null);
                    VideLivresZones();
                }
            }
            else
            {
                btnLivresModifier.Enabled = false;
                btnLivresSupprimer.Enabled = false;
                btnLivresCommandes.Enabled = false;
                RemplirLivresExemplairesListe(null);
                VideLivresInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des livres
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirLivresListeComplete()
        {
            RemplirLivresListe(lesLivres);
            VideLivresZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideLivresZones()
        {
            cbxLivresGenres.SelectedIndex = -1;
            cbxLivresRayons.SelectedIndex = -1;
            cbxLivresPublics.SelectedIndex = -1;
            txbLivresNumRecherche.Text = "";
            txbLivresTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideLivresZones();
            string titreColonne = dgvLivresListe.Columns[e.ColumnIndex].HeaderText;
            List<Livre> sortedList = new List<Livre>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesLivres.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesLivres.OrderBy(o => o.Titre).ToList();
                    break;
                case "Collection":
                    sortedList = lesLivres.OrderBy(o => o.Collection).ToList();
                    break;
                case "Auteur":
                    sortedList = lesLivres.OrderBy(o => o.Auteur).ToList();
                    break;
                case "Genre":
                    sortedList = lesLivres.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesLivres.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesLivres.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirLivresListe(sortedList);
        }
        
        /// <summary>
        /// Méthode événementielle au clic sur le bouton ajouter
        /// Cette méthode initie l'ajout d'un livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLivresAjouter_Click(object sender, EventArgs e)
        {
            grpLivresRecherche.Enabled = false;
            livresEnAjout = true;
            ChangerEtatActionsLivres(false);
            ChangerEtatInfosLivres(true, true);
            VideLivresInfos();
        }
        
        /// <summary>
        /// Méthode événementielle au clic sur le bouton modifier
        /// Cette méthode initie la modification d'un livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLivresModifier_Click(object sender, EventArgs e)
        {
            grpLivresRecherche.Enabled = false;
            livresEnModif = true;
            ChangerEtatActionsLivres(false);
            ChangerEtatInfosLivres(true, false);
            
            Livre livre = (Livre)bdgLivresListe.List[bdgLivresListe.Position];
            cbxLivresGenreEdit.SelectedItem = cbxLivresGenreEdit.Items.OfType<Genre>().FirstOrDefault(x => x.Libelle == livre.Genre);
            cbxLivresRayonEdit.SelectedItem = cbxLivresRayonEdit.Items.OfType<Rayon>().FirstOrDefault(x => x.Libelle == livre.Rayon);
            cbxLivresPublicEdit.SelectedItem = cbxLivresPublicEdit.Items.OfType<Public>().FirstOrDefault(x => x.Libelle == livre.Public);
        }
        
        /// <summary>
        /// Méthode événementielle au clic sur le bouton supprimer
        /// Cette méthode gère la suppression d'un livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLivresSupprimer_Click(object sender, EventArgs e)
        {
            Livre livre = (Livre)bdgLivresListe.List[bdgLivresListe.Position];
            
            if (controller.GetExemplaires(livre.Id).Count != 0)
            {
                MessageBox.Show("Un livre ne peut pas être supprimé si il a des exemplaires liés");
                return;
            }

            if (controller.GetCommandesCountLivreDvd(livre.Id) != 0)
            {
                MessageBox.Show("Un livre ne peut pas être supprimé si il a des commandes liées");
                return;
            }

            if (MessageBox.Show($"Voulez vous vraiment supprimer le livre '{livre.Titre}' ?", "Suppression de livre",
                    MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            if (!controller.SupprimerLivre(livre))
            {
                MessageBox.Show("Le livre n'a pas pu être supprimé");
                return;
            }

            lesLivres.Remove(livre);
            RemplirLivresListe(lesLivres);
            bdgLivresListe.ResetBindings(false);
            DgvLivresListe_SelectionChanged(null, null);
        }
        
        /// <summary>
        /// Méthode événementielle au clic sur le bouton annuler
        /// Cette méthode annule un ajout / modification d'un livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLivresAnnuler_Click(object sender, EventArgs e)
        {
            grpLivresRecherche.Enabled = true;
            livresEnAjout = false;
            livresEnModif = false;
            ChangerEtatActionsLivres(true);
            ChangerEtatInfosLivres(false, false);
            DgvLivresListe_SelectionChanged(null, null);
        }

        /// <summary>
        /// Méthode événementielle au clic sur le bouton valider
        /// Cette méthode valide et ajoute / modifie un livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLivresValider_Click(object sender, EventArgs e)
        {
            if (!ValiderFormulaireLivres())
                return;
            
            string numero = txbLivresNumero.Text;
            string titre = txbLivresTitre.Text;
            string image = txbLivresImage.Text;
            string isbn = txbLivresIsbn.Text;
            string auteur = txbLivresAuteur.Text;
            string collection = txbLivresCollection.Text;

            Categorie genre = (Categorie)cbxLivresGenreEdit.SelectedItem;
            Categorie lePublic = (Categorie)cbxLivresPublicEdit.SelectedItem;
            Categorie rayon = (Categorie)cbxLivresRayonEdit.SelectedItem;
            
            if (livresEnModif)
            {
                livresEnModif = false;
                Livre livreModif = new Livre(numero, titre, image, isbn, auteur, collection, genre.Id, genre.Libelle,
                    lePublic.Id, lePublic.Libelle, rayon.Id, rayon.Libelle);

                bool resultat = controller.ModifierLivre(livreModif);
                if (resultat)
                {
                    bdgLivresListe.List[bdgLivresListe.Position] = livreModif;
                    bdgLivresListe.ResetBindings(false);
                    dgvLivresListe.Invalidate();
                }
                else
                    MessageBox.Show("Le livre n'a pas pu être modifié.");
            }
            else
            {
                livresEnAjout = false;
                Livre nouveauLivre = new Livre(numero, titre, image, isbn, auteur, collection, genre.Id, genre.Libelle,
                    lePublic.Id, lePublic.Libelle, rayon.Id, rayon.Libelle);
                
                bool resultat = controller.CreerLivre(nouveauLivre);
                if (resultat)
                {
                    
                    lesLivres.Add(nouveauLivre);
                    
                    int scrollPosition = dgvLivresListe.FirstDisplayedScrollingRowIndex;
                    RemplirLivresListe(lesLivres);
                    bdgLivresListe.ResetBindings(false);
                    int index = bdgLivresListe.IndexOf(nouveauLivre);
                    bdgLivresListe.Position = index;
                    dgvLivresListe.Invalidate();
                    dgvLivresListe.FirstDisplayedScrollingRowIndex = scrollPosition;
                }
                else
                    MessageBox.Show("Le livre n'a pas pu être ajouté. Veuillez vérifier que son numéro de document est unique.");
            }
            
            grpLivresRecherche.Enabled = true;
            ChangerEtatActionsLivres(true);
            ChangerEtatInfosLivres(false, false);
            DgvLivresListe_SelectionChanged(null, null);
        }
        
        /// <summary>
        /// Méthode événementielle au clic sur le bouton parcourir
        /// Cette méthode demande à l'utilisateur de sélectionner une image et remplis le chemin de l'image du livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLivresCheminEdit_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Images|*.png;*.jpg;*.jpeg;*.gif;*.ico;*.bmp;*.tiff";
            if (fileDialog.ShowDialog() != DialogResult.OK)
                return;
            
            txbLivresImage.Text = fileDialog.FileName;
            try
            {
                pcbLivresImage.Image = Image.FromFile(fileDialog.FileName);
            }
            catch
            {
                pcbLivresImage.Image = null;
            }
        }

        /// <summary>
        /// Changement de l'état d'activation des boutons d'action pour les livres
        /// </summary>
        /// <param name="etatActions">Les actions doivent-elles être activées ou non (les boutons valider et annuler prennent la valeur inverse)</param>
        private void ChangerEtatActionsLivres(bool etatActions)
        {
            btnLivresAjouter.Enabled = etatActions;
            btnLivresModifier.Enabled = etatActions;
            btnLivresSupprimer.Enabled = etatActions;
            btnLivresCommandes.Enabled = etatActions;
            
            btnLivresValider.Enabled = !etatActions;
            btnLivresAnnuler.Enabled = !etatActions;
        }

        /// <summary>
        /// Changement de l'état d'écriture des champs d'édition d'un livre
        /// </summary>
        /// <param name="etatInfos">True si les champs doivent être en lecture / écriture, false s'il doivent être en lecture seule</param>
        /// <param name="ajout">True si cela concerne un ajout, false sinon (ce paramètre change l'état du champ numéro)</param>
        private void ChangerEtatInfosLivres(bool etatInfos, bool ajout)
        {
            txbLivresNumero.ReadOnly = !(ajout && etatInfos);
            txbLivresIsbn.ReadOnly = !etatInfos;
            txbLivresTitre.ReadOnly = !etatInfos;
            txbLivresAuteur.ReadOnly = !etatInfos;
            txbLivresCollection.ReadOnly = !etatInfos;
            
            txbLivresGenre.Visible = !etatInfos;
            txbLivresRayon.Visible = !etatInfos;
            txbLivresPublic.Visible = !etatInfos;

            cbxLivresGenreEdit.Visible = etatInfos;
            cbxLivresRayonEdit.Visible = etatInfos;
            cbxLivresPublicEdit.Visible = etatInfos;
            
            btnLivresCheminEdit.Visible = etatInfos;
        }

        /// <summary>
        /// Validation du formulaire d'édition d'un livre
        /// Si le formulaire n'est pas valide, la méthode ouvre une boite de dialogue pour en informer l'utilisateur
        /// </summary>
        /// <returns>True si le formulaire est valide, sinon false</returns>
        private bool ValiderFormulaireLivres()
        {
            if (!string.IsNullOrEmpty(txbLivresIsbn.Text) && !txbLivresIsbn.Text.All(char.IsDigit))
            {
                MessageBox.Show("L'ISBN doit être composé seulement de chiffres");
                return false;
            }

            if (string.IsNullOrEmpty(txbLivresNumero.Text))
            {
                MessageBox.Show("Le numéro de document ne peut être vide");
                return false;
            }
            
            if (!txbLivresNumero.Text.All(char.IsDigit))
            {
                MessageBox.Show("Le numéro de document doit être composé seulement de chiffres");
                return false;
            }
            
            return true;
        }
        
        /// <summary>
        /// Méthode événementielle au clic sur le bouton gérer les commandes
        /// Cette méthode ouvre la fenêtre de gestion des commandes pour le livre actuel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLivresCommandes_Click(object sender, EventArgs e)
        {
            FrmMediatekCommandesLivreDvd formCommandes =
                new FrmMediatekCommandesLivreDvd((Livre)bdgLivresListe.List[bdgLivresListe.Position]);
            formCommandes.ShowDialog();
        }
        
        /// <summary>
        /// Méthode événementielle au clic sur le titre d'une colonne des exemplaires
        /// Cette méthode trie en fonction de la colonne cliquée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvLivresExemplaires_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvLivresExemplaires.Columns[e.ColumnIndex].HeaderText;
            List<Exemplaire> sortedList = new List<Exemplaire>();
            switch (titreColonne)
            {
                case "Numero":
                    sortedList = lesExemplairesLivre.OrderBy(o => o.Numero).Reverse().ToList();
                    break;
                case "DateAchat":
                    sortedList = lesExemplairesLivre.OrderBy(o => o.DateAchat).Reverse().ToList();
                    break;
                case "Etat":
                    sortedList = lesExemplairesLivre.OrderBy(o => o.Etat).ToList();
                    break;
            }
            RemplirLivresExemplairesListe(sortedList);
        }
        
        /// <summary>
        /// Récupère et affiche les exemplaires d'un livre
        /// </summary>
        private void AfficheLivresExemplaires()
        {
            string idDocuement = ((Livre)bdgLivresListe.List[bdgLivresListe.Position]).Id;
            lesExemplairesLivre = controller.GetExemplaires(idDocuement);
            RemplirLivresExemplairesListe(lesExemplairesLivre);
        }
        
        /// <summary>
        /// Remplit le dategrid des exemplaires avec la liste reçue en paramètre
        /// </summary>
        /// <param name="exemplaires">liste d'exemplaires</param>
        private void RemplirLivresExemplairesListe(List<Exemplaire> exemplaires)
        {
            if (exemplaires != null)
            {
                bdgLivresExemplaires.DataSource = exemplaires;
                dgvLivresExemplaires.DataSource = bdgLivresExemplaires;
                dgvLivresExemplaires.Columns["idEtat"].Visible = false;
                dgvLivresExemplaires.Columns["photo"].Visible = false;
                dgvLivresExemplaires.Columns["id"].Visible = false;
                dgvLivresExemplaires.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvLivresExemplaires.Columns["numero"].DisplayIndex = 0;
                dgvLivresExemplaires.Columns["dateAchat"].DisplayIndex = 1;
            }
            else
            {
                bdgLivresExemplaires.DataSource = null;
            }
        }

        /// <summary>
        /// Méthode événementielle au double clic sur une case de la DataGridView
        /// Cette méthode gère le double clic sur la case 'état' d'un des exemplaires
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvLivresExemplaires_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex != dgvLivresExemplaires.Columns["etat"].Index)
                return;

            FrmModifEtat formModifEtat =
                new FrmModifEtat(dgvLivresExemplaires[e.ColumnIndex, e.RowIndex].Value.ToString());

            Exemplaire exemplaire = (Exemplaire)bdgLivresExemplaires.List[bdgLivresExemplaires.Position];

            if (formModifEtat.ShowDialog() == DialogResult.OK && formModifEtat.Etat.Libelle != exemplaire.Etat)
            {
                exemplaire.IdEtat = formModifEtat.Etat.Id;
                exemplaire.Etat = formModifEtat.Etat.Libelle;
                controller.ModifierExemplaire(exemplaire);
            }
        }
        
        /// <summary>
        /// Méthode événementielle au changement de selection du DataGridView des exemplaires
        /// Cette méthode gère l'état d'activation du bouton de suppression d'exemplaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvLivresExemplaires_SelectionChanged(object sender, EventArgs e)
        {
            btnLivresExemplairesSupprimer.Enabled = dgvLivresExemplaires.CurrentCell != null;
        }

        /// <summary>
        /// Méthode événementielle au clic sur le bouton supprimer des exemplaires
        /// Cette méthode gère la suppression d'un exemplaire après confirmation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLivresExemplairesSupprimer_Click(object sender, EventArgs e)
        {
            Exemplaire exemplaire = (Exemplaire)bdgLivresExemplaires.List[bdgLivresExemplaires.Position];

            if (MessageBox.Show("Voulez-vous vraiment supprimer l'exemplaire N°" + exemplaire.Numero + " ?",
                    "Suppression d'un exemplaire", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (controller.SupprimerExemplaire(exemplaire))
                    DgvLivresListe_SelectionChanged(null, null);
            }
        }
        #endregion

        #region Onglet Dvd
        private readonly BindingSource bdgDvdListe = new BindingSource();
        private readonly BindingSource bdgDvdExemplaires = new BindingSource();
        private List<Dvd> lesDvd = new List<Dvd>();
        private List<Exemplaire> lesExemplairesDvd = new List<Exemplaire>();
        private bool dvdsEnAjout = false;
        private bool dvdsEnModif = false;

        /// <summary>
        /// Ouverture de l'onglet Dvds : 
        /// appel des méthodes pour remplir le datagrid des dvd et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabDvd_Enter(object sender, EventArgs e)
        {
            lesDvd = controller.GetAllDvd();
            
            List<Categorie> genres = controller.GetAllGenres();
            List<Categorie> publics = controller.GetAllPublics();
            List<Categorie> rayons = controller.GetAllRayons();
            
            RemplirComboCategorie(genres, bdgGenres, cbxDvdGenres);
            RemplirComboCategorie(publics, bdgPublics, cbxDvdPublics);
            RemplirComboCategorie(rayons, bdgRayons, cbxDvdRayons);
            RemplirComboCategorie(genres, bdgGenresEdit, cbxDvdGenreEdit);
            RemplirComboCategorie(publics, bdgPublicsEdit, cbxDvdPublicEdit);
            RemplirComboCategorie(rayons, bdgRayonsEdit, cbxDvdRayonEdit);
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="Dvds">liste de dvd</param>
        private void RemplirDvdListe(List<Dvd> Dvds)
        {
            bdgDvdListe.DataSource = Dvds;
            dgvDvdListe.DataSource = bdgDvdListe;
            dgvDvdListe.Columns["idRayon"].Visible = false;
            dgvDvdListe.Columns["idGenre"].Visible = false;
            dgvDvdListe.Columns["idPublic"].Visible = false;
            dgvDvdListe.Columns["image"].Visible = false;
            dgvDvdListe.Columns["synopsis"].Visible = false;
            dgvDvdListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvDvdListe.Columns["id"].DisplayIndex = 0;
            dgvDvdListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage du Dvd dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbDvdNumRecherche.Text.Equals(""))
            {
                txbDvdTitreRecherche.Text = "";
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
                Dvd dvd = lesDvd.Find(x => x.Id.Equals(txbDvdNumRecherche.Text));
                if (dvd != null)
                {
                    List<Dvd> Dvd = new List<Dvd>() { dvd };
                    RemplirDvdListe(Dvd);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirDvdListeComplete();
                }
            }
            else
            {
                RemplirDvdListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des Dvd dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbDvdTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbDvdTitreRecherche.Text.Equals(""))
            {
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
                txbDvdNumRecherche.Text = "";
                List<Dvd> lesDvdParTitre;
                lesDvdParTitre = lesDvd.FindAll(x => x.Titre.ToLower().Contains(txbDvdTitreRecherche.Text.ToLower()));
                RemplirDvdListe(lesDvdParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxDvdGenres.SelectedIndex < 0 && cbxDvdPublics.SelectedIndex < 0 && cbxDvdRayons.SelectedIndex < 0
                    && txbDvdNumRecherche.Text.Equals(""))
                {
                    RemplirDvdListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations du dvd sélectionné
        /// </summary>
        /// <param name="dvd">le dvd</param>
        private void AfficheDvdInfos(Dvd dvd)
        {
            txbDvdRealisateur.Text = dvd.Realisateur;
            txbDvdSynopsis.Text = dvd.Synopsis;
            txbDvdImage.Text = dvd.Image;
            txbDvdDuree.Text = dvd.Duree.ToString();
            txbDvdNumero.Text = dvd.Id;
            txbDvdGenre.Text = dvd.Genre;
            txbDvdPublic.Text = dvd.Public;
            txbDvdRayon.Text = dvd.Rayon;
            txbDvdTitre.Text = dvd.Titre;
            string image = dvd.Image;
            try
            {
                pcbDvdImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbDvdImage.Image = null;
            }
            
            AfficheDvdExemplaires();
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du dvd
        /// </summary>
        private void VideDvdInfos()
        {
            txbDvdRealisateur.Text = "";
            txbDvdSynopsis.Text = "";
            txbDvdImage.Text = "";
            txbDvdDuree.Text = "";
            txbDvdNumero.Text = "";
            
            txbDvdGenre.Text = "";
            txbDvdPublic.Text = "";
            txbDvdRayon.Text = "";
            
            cbxDvdGenreEdit.SelectedIndex = 0;
            cbxDvdPublicEdit.SelectedIndex = 0;
            cbxDvdRayonEdit.SelectedIndex = 0;
            
            txbDvdTitre.Text = "";
            pcbDvdImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdGenres.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Genre genre = (Genre)cbxDvdGenres.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdPublics.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Public lePublic = (Public)cbxDvdPublics.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdRayons.SelectedIndex = -1;
                cbxDvdGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxDvdRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxDvdRayons.SelectedIndex >= 0)
            {
                txbDvdTitreRecherche.Text = "";
                txbDvdNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxDvdRayons.SelectedItem;
                List<Dvd> Dvd = lesDvd.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirDvdListe(Dvd);
                cbxDvdGenres.SelectedIndex = -1;
                cbxDvdPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvDvdListe.CurrentCell != null)
            {
                try
                {
                    Dvd dvd = (Dvd)bdgDvdListe.List[bdgDvdListe.Position];
                    AfficheDvdInfos(dvd);
                    btnDvdModifier.Enabled = true;
                    btnDvdSupprimer.Enabled = true;
                    btnDvdCommandes.Enabled = true;
                }
                catch
                {
                    VideDvdZones();
                    RemplirDvdExemplairesListe(null);
                    btnDvdModifier.Enabled = false;
                    btnDvdSupprimer.Enabled = false;
                    btnDvdCommandes.Enabled = false;
                }
            }
            else
            {
                VideDvdInfos();
                RemplirDvdExemplairesListe(null);
                btnDvdModifier.Enabled = false;
                btnDvdSupprimer.Enabled = false;
                btnDvdCommandes.Enabled = false;
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des Dvd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirDvdListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des Dvd
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirDvdListeComplete()
        {
            RemplirDvdListe(lesDvd);
            VideDvdZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideDvdZones()
        {
            cbxDvdGenres.SelectedIndex = -1;
            cbxDvdRayons.SelectedIndex = -1;
            cbxDvdPublics.SelectedIndex = -1;
            txbDvdNumRecherche.Text = "";
            txbDvdTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideDvdZones();
            string titreColonne = dgvDvdListe.Columns[e.ColumnIndex].HeaderText;
            List<Dvd> sortedList = new List<Dvd>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesDvd.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesDvd.OrderBy(o => o.Titre).ToList();
                    break;
                case "Duree":
                    sortedList = lesDvd.OrderBy(o => o.Duree).ToList();
                    break;
                case "Realisateur":
                    sortedList = lesDvd.OrderBy(o => o.Realisateur).ToList();
                    break;
                case "Genre":
                    sortedList = lesDvd.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesDvd.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesDvd.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirDvdListe(sortedList);
        }
        
        /// <summary>
        /// Méthode événementielle au clic sur le bouton ajouter
        /// Cette méthode prépare l'ajout d'un DVD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAjouter_Click(object sender, EventArgs e)
        {
            grpDvdRecherche.Enabled = false;
            dvdsEnAjout = true;
            ChangerEtatActionsDvds(false);
            ChangerEtatInfosDvds(true, true);
            VideDvdInfos();
        }

        /// <summary>
        /// Méthode événementielle au clic sur le bouton modifier
        /// Cette méthode prépare la modification d'un DVD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdModifier_Click(object sender, EventArgs e)
        {
            grpDvdRecherche.Enabled = false;
            dvdsEnModif = true;
            ChangerEtatActionsDvds(false);
            ChangerEtatInfosDvds(true, false);
            
            Dvd dvd = (Dvd)bdgDvdListe.List[bdgDvdListe.Position];
            cbxDvdGenreEdit.SelectedItem = cbxDvdGenreEdit.Items.OfType<Genre>().FirstOrDefault(x => x.Libelle == dvd.Genre);
            cbxDvdRayonEdit.SelectedItem = cbxDvdRayonEdit.Items.OfType<Rayon>().FirstOrDefault(x => x.Libelle == dvd.Rayon);
            cbxDvdPublicEdit.SelectedItem = cbxDvdPublicEdit.Items.OfType<Public>().FirstOrDefault(x => x.Libelle == dvd.Public);
        }

        /// <summary>
        /// Méthode événementielle au clic sur le bouton supprimer
        /// Cette méthode effectue la suppression d'un DVD après confirmation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdSupprimer_Click(object sender, EventArgs e)
        {
            Dvd dvd = (Dvd)bdgDvdListe.List[bdgDvdListe.Position];
            
            if (controller.GetExemplaires(dvd.Id).Count != 0)
            {
                MessageBox.Show("Un DVD ne peut pas être supprimé si il a des exemplaires liés");
                return;
            }

            if (controller.GetCommandesCountLivreDvd(dvd.Id) != 0)
            {
                MessageBox.Show("Un DVD ne peut pas être supprimé si il a des commandes liées");
                return;
            }

            if (MessageBox.Show($"Voulez vous vraiment supprimer le DVD '{dvd.Titre}' ?", "Suppression de DVD",
                    MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            if (!controller.SupprimerDvd(dvd))
            {
                MessageBox.Show("Le livre n'a pas pu être supprimé");
                return;
            }

            lesDvd.Remove(dvd);
            RemplirDvdListe(lesDvd);
            bdgDvdListe.ResetBindings(false);
            dgvDvdListe_SelectionChanged(null, null);
        }
        
        /// <summary>
        /// Méthode événementielle au clic sur le bouton annuler
        /// Cette méthode annule l'ajout / modification du DVD en cours 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdAnnuler_Click(object sender, EventArgs e)
        {
            grpDvdRecherche.Enabled = true;
            dvdsEnAjout = false;
            dvdsEnModif = false;
            ChangerEtatActionsDvds(true);
            ChangerEtatInfosDvds(false, false);
            dgvDvdListe_SelectionChanged(null, null);
        }

        /// <summary>
        /// Méthode évenementielle au clic sur le bouton valider
        /// Cette méthode effectue l'ajout / modification d'un DVD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdValider_Click(object sender, EventArgs e)
        {
            if (!ValiderFormulaireDvds())
                return;
            
            string numero = txbDvdNumero.Text;
            string titre = txbDvdTitre.Text;
            string image = txbDvdImage.Text;
            int duree = int.Parse(txbDvdDuree.Text);
            string realisateur = txbDvdRealisateur.Text;
            string synopsis = txbDvdSynopsis.Text;

            Categorie genre = (Categorie)cbxDvdGenreEdit.SelectedItem;
            Categorie lePublic = (Categorie)cbxDvdPublicEdit.SelectedItem;
            Categorie rayon = (Categorie)cbxDvdRayonEdit.SelectedItem;
            
            if (dvdsEnModif)
            {
                dvdsEnModif = false;
                Dvd dvdModif = new Dvd(numero, titre, image, duree, realisateur, synopsis, genre.Id, genre.Libelle,
                    lePublic.Id, lePublic.Libelle, rayon.Id, rayon.Libelle);

                bool resultat = controller.ModifierDvd(dvdModif);
                if (resultat)
                {
                    bdgDvdListe.List[bdgDvdListe.Position] = dvdModif;
                    bdgDvdListe.ResetBindings(false);
                    dgvDvdListe.Invalidate();
                }
                else
                    MessageBox.Show("Le DVD n'a pas pu être modifié.");
            }
            else
            {
                dvdsEnAjout = false;
                Dvd nouveauDvd = new Dvd(numero, titre, image, duree, realisateur, synopsis, genre.Id, genre.Libelle,
                    lePublic.Id, lePublic.Libelle, rayon.Id, rayon.Libelle);
                
                bool resultat = controller.CreerDvd(nouveauDvd);
                if (resultat)
                {
                    lesDvd.Add(nouveauDvd);
                    
                    int scrollPosition = dgvDvdListe.FirstDisplayedScrollingRowIndex;
                    RemplirDvdListe(lesDvd);
                    bdgDvdListe.ResetBindings(false);
                    int index = bdgDvdListe.IndexOf(nouveauDvd);
                    bdgDvdListe.Position = index;
                    dgvDvdListe.Invalidate();
                    dgvDvdListe.FirstDisplayedScrollingRowIndex = scrollPosition;
                }
                else
                    MessageBox.Show("Le DVD n'a pas pu être ajouté. Veuillez vérifier que son numéro de document est unique.");
            }
            
            grpDvdRecherche.Enabled = true;
            ChangerEtatActionsDvds(true);
            ChangerEtatInfosDvds(false, false);
            dgvDvdListe_SelectionChanged(null, null);
        }
        
        /// <summary>
        /// Méthode évenementielle au clic sur le bouton parcourir
        /// Cette méthode demande à l'utilisateur de sélectionner une image et remplit le champ chemin de l'image d'un DVD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdCheminEdit_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Images|*.png;*.jpg;*.jpeg;*.gif;*.ico;*.bmp;*.tiff";
            if (fileDialog.ShowDialog() != DialogResult.OK)
                return;
            
            txbDvdImage.Text = fileDialog.FileName;
            try
            {
                pcbDvdImage.Image = Image.FromFile(fileDialog.FileName);
            }
            catch
            {
                pcbDvdImage.Image = null;
            }
        }
        
        /// <summary>
        /// Changement de l'état d'activation des boutons d'action pour les DVDs
        /// </summary>
        /// <param name="etatActions">Les actions doivent-elles être activées ou non (les boutons valider et annuler prennent la valeur inverse)</param>
        private void ChangerEtatActionsDvds(bool etatActions)
        {
            btnDvdAjouter.Enabled = etatActions;
            btnDvdModifier.Enabled = etatActions;
            btnDvdSupprimer.Enabled = etatActions;
            btnDvdCommandes.Enabled = etatActions;
            
            btnDvdValider.Enabled = !etatActions;
            btnDvdAnnuler.Enabled = !etatActions;
        }

        /// <summary>
        /// Changement de l'état d'écriture des champs d'édition d'un DVD
        /// </summary>
        /// <param name="etatInfos">True si les champs doivent être en lecture / écriture, false s'il doivent être en lecture seule</param>
        /// <param name="ajout">True si cela concerne un ajout, false sinon (ce paramètre change l'état du champ numéro)</param>
        private void ChangerEtatInfosDvds(bool etatInfos, bool ajout)
        {
            txbDvdNumero.ReadOnly = !(ajout && etatInfos);
            txbDvdDuree.ReadOnly = !etatInfos;
            txbDvdTitre.ReadOnly = !etatInfos;
            txbDvdRealisateur.ReadOnly = !etatInfos;
            txbDvdSynopsis.ReadOnly = !etatInfos;
            
            txbDvdGenre.Visible = !etatInfos;
            txbDvdRayon.Visible = !etatInfos;
            txbDvdPublic.Visible = !etatInfos;

            cbxDvdGenreEdit.Visible = etatInfos;
            cbxDvdRayonEdit.Visible = etatInfos;
            cbxDvdPublicEdit.Visible = etatInfos;
            
            btnDvdCheminEdit.Visible = etatInfos;
        }

        /// <summary>
        /// Validation du formulaire d'édition d'un DVD
        /// Si le formulaire n'est pas valide, la méthode ouvre une boite de dialogue pour en informer l'utilisateur
        /// </summary>
        /// <returns>True si le formulaire est valide, sinon false</returns>
        private bool ValiderFormulaireDvds()
        {
            if (!string.IsNullOrEmpty(txbDvdDuree.Text) && !int.TryParse(txbDvdDuree.Text, out _))
            {
                MessageBox.Show("La durée doit être composée seulement de chiffres");
                return false;
            }

            if (string.IsNullOrEmpty(txbDvdNumero.Text))
            {
                MessageBox.Show("Le numéro de document ne peut être vide");
                return false;
            }
            
            if (!txbDvdNumero.Text.All(char.IsDigit))
            {
                MessageBox.Show("Le numéro de document doit être composé seulement de chiffres");
                return false;
            }
            
            return true;
        }
        
        /// <summary>
        /// Méthode événementielle au clic sur le bouton gérer les commandes
        /// Cette méthode affiche la fenêtre de gestion des commandes pour le DVD actuel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdCommandes_Click(object sender, EventArgs e)
        {
            FrmMediatekCommandesLivreDvd formCommandes =
                new FrmMediatekCommandesLivreDvd((Dvd)bdgDvdListe.List[bdgDvdListe.Position]);
            formCommandes.ShowDialog();
        }
        
        /// <summary>
        /// Récupère et affiche les exemplaires d'un DVD
        /// </summary>
        private void AfficheDvdExemplaires()
        {
            string idDocuement = ((Dvd)bdgDvdListe.List[bdgDvdListe.Position]).Id;
            lesExemplairesDvd = controller.GetExemplaires(idDocuement);
            RemplirDvdExemplairesListe(lesExemplairesDvd);
        }
        
        /// <summary>
        /// Remplit le dategrid des exemplaires avec la liste reçue en paramètre
        /// </summary>
        /// <param name="exemplaires">liste d'exemplaires</param>
        private void RemplirDvdExemplairesListe(List<Exemplaire> exemplaires)
        {
            if (exemplaires != null)
            {
                bdgDvdExemplaires.DataSource = exemplaires;
                dgvDvdExemplaires.DataSource = bdgDvdExemplaires;
                dgvDvdExemplaires.Columns["idEtat"].Visible = false;
                dgvDvdExemplaires.Columns["photo"].Visible = false;
                dgvDvdExemplaires.Columns["id"].Visible = false;
                dgvDvdExemplaires.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvDvdExemplaires.Columns["numero"].DisplayIndex = 0;
                dgvDvdExemplaires.Columns["dateAchat"].DisplayIndex = 1;
            }
            else
            {
                bdgDvdExemplaires.DataSource = null;
            }
        }
        
        /// <summary>
        /// Méthode événementielle au clic sur le titre d'une colonne des exemplaires
        /// Cette méthode trie en fonction de la colonne cliquée
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdExemplaires_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvDvdExemplaires.Columns[e.ColumnIndex].HeaderText;
            List<Exemplaire> sortedList = new List<Exemplaire>();
            switch (titreColonne)
            {
                case "Numero":
                    sortedList = lesExemplairesDvd.OrderBy(o => o.Numero).Reverse().ToList();
                    break;
                case "DateAchat":
                    sortedList = lesExemplairesDvd.OrderBy(o => o.DateAchat).Reverse().ToList();
                    break;
                case "Etat":
                    sortedList = lesExemplairesDvd.OrderBy(o => o.Etat).ToList();
                    break;
            }
            RemplirDvdExemplairesListe(sortedList);
        }
        
        /// <summary>
        /// Méthode événementielle au double clic sur une case de la DataGridView
        /// Cette méthode gère le double clic sur la case 'état' d'un des exemplaires
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdExemplaires_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex != dgvDvdExemplaires.Columns["etat"].Index)
                return;

            FrmModifEtat formModifEtat =
                new FrmModifEtat(dgvDvdExemplaires[e.ColumnIndex, e.RowIndex].Value.ToString());

            Exemplaire exemplaire = (Exemplaire)bdgDvdExemplaires.List[bdgDvdExemplaires.Position];

            if (formModifEtat.ShowDialog() == DialogResult.OK && formModifEtat.Etat.Libelle != exemplaire.Etat)
            {
                exemplaire.IdEtat = formModifEtat.Etat.Id;
                exemplaire.Etat = formModifEtat.Etat.Libelle;
                controller.ModifierExemplaire(exemplaire);
            }
        }
        
        /// <summary>
        /// Méthode événementielle au changement de selection du DataGridView des exemplaires
        /// Cette méthode gère l'état d'activation du bouton de suppression d'exemplaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDvdExemplaires_SelectionChanged(object sender, EventArgs e)
        {
            btnDvdExemplairesSupprimer.Enabled = dgvDvdExemplaires.CurrentCell != null;
        }

        /// <summary>
        /// Méthode événementielle au clic sur le bouton supprimer des exemplaires
        /// Cette méthode gère la suppression d'un exemplaire après confirmation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDvdExemplairesSupprimer_Click(object sender, EventArgs e)
        {
            Exemplaire exemplaire = (Exemplaire)bdgDvdExemplaires.List[bdgDvdExemplaires.Position];

            if (MessageBox.Show("Voulez-vous vraiment supprimer l'exemplaire N°" + exemplaire.Numero + " ?",
                    "Suppression d'un exemplaire", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (controller.SupprimerExemplaire(exemplaire))
                    dgvDvdListe_SelectionChanged(null, null);
            }
        }
        #endregion

        #region Onglet Revues
        private readonly BindingSource bdgRevuesListe = new BindingSource();
        private List<Revue> lesRevues = new List<Revue>();
        private bool revuesEnAjout = false;
        private bool revuesEnModif = false;

        /// <summary>
        /// Ouverture de l'onglet Revues : 
        /// appel des méthodes pour remplir le datagrid des revues et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabRevues_Enter(object sender, EventArgs e)
        {
            lesRevues = controller.GetAllRevues();
            
            List<Categorie> genres = controller.GetAllGenres();
            List<Categorie> publics = controller.GetAllPublics();
            List<Categorie> rayons = controller.GetAllRayons();
            
            RemplirComboCategorie(genres, bdgGenres, cbxRevuesGenres);
            RemplirComboCategorie(publics, bdgPublics, cbxRevuesPublics);
            RemplirComboCategorie(rayons, bdgRayons, cbxRevuesRayons);
            RemplirComboCategorie(genres, bdgGenresEdit, cbxRevuesGenreEdit);
            RemplirComboCategorie(publics, bdgPublicsEdit, cbxRevuesPublicEdit);
            RemplirComboCategorie(rayons, bdgRayonsEdit, cbxRevuesRayonEdit);
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="revues"></param>
        private void RemplirRevuesListe(List<Revue> revues)
        {
            bdgRevuesListe.DataSource = revues;
            dgvRevuesListe.DataSource = bdgRevuesListe;
            dgvRevuesListe.Columns["idRayon"].Visible = false;
            dgvRevuesListe.Columns["idGenre"].Visible = false;
            dgvRevuesListe.Columns["idPublic"].Visible = false;
            dgvRevuesListe.Columns["image"].Visible = false;
            dgvRevuesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvRevuesListe.Columns["id"].DisplayIndex = 0;
            dgvRevuesListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage de la revue dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbRevuesNumRecherche.Text.Equals(""))
            {
                txbRevuesTitreRecherche.Text = "";
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbRevuesNumRecherche.Text));
                if (revue != null)
                {
                    List<Revue> revues = new List<Revue>() { revue };
                    RemplirRevuesListe(revues);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirRevuesListeComplete();
                }
            }
            else
            {
                RemplirRevuesListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des revues dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbRevuesTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbRevuesTitreRecherche.Text.Equals(""))
            {
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                txbRevuesNumRecherche.Text = "";
                List<Revue> lesRevuesParTitre;
                lesRevuesParTitre = lesRevues.FindAll(x => x.Titre.ToLower().Contains(txbRevuesTitreRecherche.Text.ToLower()));
                RemplirRevuesListe(lesRevuesParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxRevuesGenres.SelectedIndex < 0 && cbxRevuesPublics.SelectedIndex < 0 && cbxRevuesRayons.SelectedIndex < 0
                    && txbRevuesNumRecherche.Text.Equals(""))
                {
                    RemplirRevuesListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionné
        /// </summary>
        /// <param name="revue">la revue</param>
        private void AfficheRevuesInfos(Revue revue)
        {
            txbRevuesPeriodicite.Text = revue.Periodicite;
            txbRevuesImage.Text = revue.Image;
            txbRevuesDateMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbRevuesNumero.Text = revue.Id;
            txbRevuesGenre.Text = revue.Genre;
            txbRevuesPublic.Text = revue.Public;
            txbRevuesRayon.Text = revue.Rayon;
            txbRevuesTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbRevuesImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbRevuesImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations de la reuve
        /// </summary>
        private void VideRevuesInfos()
        {
            txbRevuesPeriodicite.Text = "";
            txbRevuesImage.Text = "";
            txbRevuesDateMiseADispo.Text = "";
            txbRevuesNumero.Text = "";
            
            txbRevuesGenre.Text = "";
            txbRevuesPublic.Text = "";
            txbRevuesRayon.Text = "";
            
            cbxRevuesGenreEdit.SelectedIndex = 0;
            cbxRevuesPublicEdit.SelectedIndex = 0;
            cbxRevuesRayonEdit.SelectedIndex = 0;
            
            txbRevuesTitre.Text = "";
            pcbRevuesImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesGenres.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Genre genre = (Genre)cbxRevuesGenres.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesPublics.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Public lePublic = (Public)cbxRevuesPublics.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesRayons.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxRevuesRayons.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations de la revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRevuesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvRevuesListe.CurrentCell != null)
            {
                try
                {
                    Revue revue = (Revue)bdgRevuesListe.List[bdgRevuesListe.Position];
                    AfficheRevuesInfos(revue);
                    btnRevuesModifier.Enabled = true;
                    btnRevuesSupprimer.Enabled = true;
                    btnRevuesAbonnements.Enabled = true;
                }
                catch
                {
                    VideRevuesZones();
                    btnRevuesModifier.Enabled = false;
                    btnRevuesSupprimer.Enabled = false;
                    btnRevuesAbonnements.Enabled = false;
                }
            }
            else
            {
                VideRevuesInfos();
                btnRevuesModifier.Enabled = false;
                btnRevuesSupprimer.Enabled = false;
                btnRevuesAbonnements.Enabled = false;
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des revues
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirRevuesListeComplete()
        {
            RemplirRevuesListe(lesRevues);
            VideRevuesZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideRevuesZones()
        {
            cbxRevuesGenres.SelectedIndex = -1;
            cbxRevuesRayons.SelectedIndex = -1;
            cbxRevuesPublics.SelectedIndex = -1;
            txbRevuesNumRecherche.Text = "";
            txbRevuesTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRevuesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideRevuesZones();
            string titreColonne = dgvRevuesListe.Columns[e.ColumnIndex].HeaderText;
            List<Revue> sortedList = new List<Revue>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesRevues.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesRevues.OrderBy(o => o.Titre).ToList();
                    break;
                case "Periodicite":
                    sortedList = lesRevues.OrderBy(o => o.Periodicite).ToList();
                    break;
                case "DelaiMiseADispo":
                    sortedList = lesRevues.OrderBy(o => o.DelaiMiseADispo).ToList();
                    break;
                case "Genre":
                    sortedList = lesRevues.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesRevues.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesRevues.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirRevuesListe(sortedList);
        }
        
        /// <summary>
        /// Méthode événementielle au clic sur le bouton ajouter
        /// Cette méthode prépare l'ajout d'une revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAjouter_Click(object sender, EventArgs e)
        {
            grpRevuesRecherche.Enabled = false;
            revuesEnAjout = true;
            ChangerEtatActionsRevues(false);
            ChangerEtatInfosRevues(true, true);
            VideRevuesInfos();
        }
        
        /// <summary>
        /// Méthode événementielle au clic sur le bouton modifier
        /// Cette méthode prépare la modification d'une revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesModifier_Click(object sender, EventArgs e)
        {
            grpRevuesRecherche.Enabled = false;
            revuesEnModif = true;
            ChangerEtatActionsRevues(false);
            ChangerEtatInfosRevues(true, false);
            
            Revue revue = (Revue)bdgRevuesListe.List[bdgRevuesListe.Position];
            cbxRevuesGenreEdit.SelectedItem = cbxRevuesGenreEdit.Items.OfType<Genre>().FirstOrDefault(x => x.Libelle == revue.Genre);
            cbxRevuesRayonEdit.SelectedItem = cbxRevuesRayonEdit.Items.OfType<Rayon>().FirstOrDefault(x => x.Libelle == revue.Rayon);
            cbxRevuesPublicEdit.SelectedItem = cbxRevuesPublicEdit.Items.OfType<Public>().FirstOrDefault(x => x.Libelle == revue.Public);
        }
        
        /// <summary>
        /// Méthode événementielle au clic sur le bouton supprimer
        /// Cette méthode effectue la suppression d'une revue après confirmation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesSupprimer_Click(object sender, EventArgs e)
        {
            Revue revue = (Revue)bdgRevuesListe.List[bdgRevuesListe.Position];
            
            if (controller.GetExemplaires(revue.Id).Count != 0)
            {
                MessageBox.Show("Une revue ne peut pas être supprimé si elle a des exemplaires liés");
                return;
            }

            if (controller.GetCommandesCountRevue(revue.Id) != 0)
            {
                MessageBox.Show("Une revue ne peut pas être supprimée si elle a des abonnements liés");
                return;
            }

            if (MessageBox.Show($"Voulez vous vraiment supprimer la revue '{revue.Titre}' ?", "Suppression de revue",
                    MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            if (!controller.SupprimerRevue(revue))
            {
                MessageBox.Show("La revue n'a pas pu être supprimé");
                return;
            }

            lesRevues.Remove(revue);
            RemplirRevuesListe(lesRevues);
            bdgRevuesListe.ResetBindings(false);
            dgvRevuesListe_SelectionChanged(null, null);
        }

        /// <summary>
        /// Méthode événementielle au clic sur le bouton annuler
        /// Cette méthode annule l'ajout / modification de la revue en cours 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnuler_Click(object sender, EventArgs e)
        {
            grpRevuesRecherche.Enabled = true;
            revuesEnAjout = false;
            revuesEnModif = false;
            ChangerEtatActionsRevues(true);
            ChangerEtatInfosRevues(false, false);
            dgvRevuesListe_SelectionChanged(null, null);
        }
        
        /// <summary>
        /// Méthode évenementielle au clic sur le bouton valider
        /// Cette méthode effectue l'ajout / modification d'une revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesValider_Click(object sender, EventArgs e)
        {
            if (!ValiderFormulaireRevues())
                return;
            
            string numero = txbRevuesNumero.Text;
            string titre = txbRevuesTitre.Text;
            string image = txbRevuesImage.Text;
            int delaiMiseADispo = int.Parse(txbRevuesDateMiseADispo.Text);
            string periodicite = txbRevuesPeriodicite.Text;

            Categorie genre = (Categorie)cbxRevuesGenreEdit.SelectedItem;
            Categorie lePublic = (Categorie)cbxRevuesPublicEdit.SelectedItem;
            Categorie rayon = (Categorie)cbxRevuesRayonEdit.SelectedItem;
            
            if (revuesEnModif)
            {
                revuesEnModif = false;
                Revue revueModif = new Revue(numero, titre, image, genre.Id, genre.Libelle, lePublic.Id,
                    lePublic.Libelle, rayon.Id, rayon.Libelle, periodicite, delaiMiseADispo);

                bool resultat = controller.ModifierRevue(revueModif);
                if (resultat)
                {
                    bdgRevuesListe.List[bdgRevuesListe.Position] = revueModif;
                    bdgRevuesListe.ResetBindings(false);
                    dgvRevuesListe.Invalidate();
                }
                else
                    MessageBox.Show("La revue n'a pas pu être modifiée.");
            }
            else
            {
                revuesEnAjout = false;
                Revue nouvelleRevue = new Revue(numero, titre, image, genre.Id, genre.Libelle, lePublic.Id,
                    lePublic.Libelle, rayon.Id, rayon.Libelle, periodicite, delaiMiseADispo);
                
                bool resultat = controller.CreerRevue(nouvelleRevue);
                if (resultat)
                {
                    lesRevues.Add(nouvelleRevue);
                    
                    int scrollPosition = dgvRevuesListe.FirstDisplayedScrollingRowIndex;
                    RemplirRevuesListe(lesRevues);
                    bdgRevuesListe.ResetBindings(false);
                    int index = bdgRevuesListe.IndexOf(nouvelleRevue);
                    bdgRevuesListe.Position = index;
                    dgvRevuesListe.Invalidate();
                    dgvRevuesListe.FirstDisplayedScrollingRowIndex = scrollPosition;
                }
                else
                    MessageBox.Show("La revue n'a pas pu être ajoutée. Veuillez vérifier que son numéro de document est unique.");
            }
            
            grpRevuesRecherche.Enabled = true;
            ChangerEtatActionsRevues(true);
            ChangerEtatInfosRevues(false, false);
            dgvRevuesListe_SelectionChanged(null, null);
        }

        /// <summary>
        /// Méthode événementielle au clic sur le bouton parcourir
        /// Cette méthode demande à l'utilisateur de sélectionner une image puis rempli le chemin de l'image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesCheminEdit_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Images|*.png;*.jpg;*.jpeg;*.gif;*.ico;*.bmp;*.tiff";
            if (fileDialog.ShowDialog() != DialogResult.OK)
                return;
            
            txbRevuesImage.Text = fileDialog.FileName;
            try
            {
                pcbRevuesImage.Image = Image.FromFile(fileDialog.FileName);
            }
            catch
            {
                pcbRevuesImage.Image = null;
            }
        }
        
        /// <summary>
        /// Méthode événementielle au clic sur le bouton gérer les abonnements
        /// Cette méthode affiche la fenêtre de gestion des abonnements pour cette revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAbonnements_Click(object sender, EventArgs e)
        {
            FrmMediatekCommandesRevue formCommandes =
                new FrmMediatekCommandesRevue((Revue)bdgRevuesListe.List[bdgRevuesListe.Position]);
            formCommandes.ShowDialog();
        }
        
        /// <summary>
        /// Changement de l'état d'activation des boutons d'action pour les revues
        /// </summary>
        /// <param name="etatActions">Les actions doivent-elles être activées ou non (les boutons valider et annuler prennent la valeur inverse)</param>
        private void ChangerEtatActionsRevues(bool etatActions)
        {
            btnRevuesAjouter.Enabled = etatActions;
            btnRevuesModifier.Enabled = etatActions;
            btnRevuesSupprimer.Enabled = etatActions;
            btnRevuesAbonnements.Enabled = etatActions;
            
            btnRevuesValider.Enabled = !etatActions;
            btnRevuesAnnuler.Enabled = !etatActions;
        }

        /// <summary>
        /// Changement de l'état d'écriture des champs d'édition d'une revue
        /// </summary>
        /// <param name="etatInfos">True si les champs doivent être en lecture / écriture, false s'il doivent être en lecture seule</param>
        /// <param name="ajout">True si cela concerne un ajout, false sinon (ce paramètre change l'état du champ numéro)</param>
        private void ChangerEtatInfosRevues(bool etatInfos, bool ajout)
        {
            txbRevuesNumero.ReadOnly = !(ajout && etatInfos);
            txbRevuesTitre.ReadOnly = !etatInfos;
            txbRevuesPeriodicite.ReadOnly = !etatInfos;
            txbRevuesDateMiseADispo.ReadOnly = !etatInfos;
            
            txbRevuesGenre.Visible = !etatInfos;
            txbRevuesRayon.Visible = !etatInfos;
            txbRevuesPublic.Visible = !etatInfos;

            cbxRevuesGenreEdit.Visible = etatInfos;
            cbxRevuesRayonEdit.Visible = etatInfos;
            cbxRevuesPublicEdit.Visible = etatInfos;
            
            btnRevuesCheminEdit.Visible = etatInfos;
        }

        /// <summary>
        /// Validation du formulaire d'édition d'une revue
        /// Si le formulaire n'est pas valide, la méthode ouvre une boite de dialogue pour en informer l'utilisateur
        /// </summary>
        /// <returns>True si le formulaire est valide, sinon false</returns>
        private bool ValiderFormulaireRevues()
        {
            if (!string.IsNullOrEmpty(txbRevuesDateMiseADispo.Text) && !int.TryParse(txbRevuesDateMiseADispo.Text, out _))
            {
                MessageBox.Show("Le délai de mise à disposition doit être composé seulement de chiffres");
                return false;
            }

            if (string.IsNullOrEmpty(txbRevuesNumero.Text))
            {
                MessageBox.Show("Le numéro de document ne peut être vide");
                return false;
            }
            
            if (!txbRevuesNumero.Text.All(char.IsDigit))
            {
                MessageBox.Show("Le numéro de document doit être composé seulement de chiffres");
                return false;
            }
            
            return true;
        }
        #endregion

        #region Onglet Paarutions
        private readonly BindingSource bdgParutionsExemplairesListe = new BindingSource();
        private List<Exemplaire> lesParutionsExemplaires = new List<Exemplaire>();
        const string ETATNEUF = "00001";
        const string ETATNEUFTEXTE = "neuf";

        /// <summary>
        /// Ouverture de l'onglet : récupère le revues et vide tous les champs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabReceptionRevue_Enter(object sender, EventArgs e)
        {
            lesRevues = controller.GetAllRevues();
            txbReceptionRevueNumero.Text = "";
        }

        /// <summary>
        /// Remplit le dategrid des exemplaires avec la liste reçue en paramètre
        /// </summary>
        /// <param name="exemplaires">liste d'exemplaires</param>
        private void RemplirReceptionExemplairesListe(List<Exemplaire> exemplaires)
        {
            if (exemplaires != null)
            {
                bdgParutionsExemplairesListe.DataSource = exemplaires;
                dgvReceptionExemplairesListe.DataSource = bdgParutionsExemplairesListe;
                dgvReceptionExemplairesListe.Columns["idEtat"].Visible = false;
                dgvReceptionExemplairesListe.Columns["photo"].Visible = false;
                dgvReceptionExemplairesListe.Columns["id"].Visible = false;
                dgvReceptionExemplairesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvReceptionExemplairesListe.Columns["numero"].DisplayIndex = 0;
                dgvReceptionExemplairesListe.Columns["dateAchat"].DisplayIndex = 1;
            }
            else
            {
                bdgParutionsExemplairesListe.DataSource = null;
            }
        }

        /// <summary>
        /// Recherche d'un numéro de revue et affiche ses informations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionRechercher_Click(object sender, EventArgs e)
        {
            if (!txbReceptionRevueNumero.Text.Equals(""))
            {
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbReceptionRevueNumero.Text));
                if (revue != null)
                {
                    AfficheReceptionRevueInfos(revue);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                }
            }
        }

        /// <summary>
        /// Si le numéro de revue est modifié, la zone de l'exemplaire est vidée et inactive
        /// les informations de la revue son aussi effacées
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbReceptionRevueNumero_TextChanged(object sender, EventArgs e)
        {
            txbReceptionRevuePeriodicite.Text = "";
            txbReceptionRevueImage.Text = "";
            txbReceptionRevueDelaiMiseADispo.Text = "";
            txbReceptionRevueGenre.Text = "";
            txbReceptionRevuePublic.Text = "";
            txbReceptionRevueRayon.Text = "";
            txbReceptionRevueTitre.Text = "";
            pcbReceptionRevueImage.Image = null;
            RemplirReceptionExemplairesListe(null);
            AccesReceptionExemplaireGroupBox(false);
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionnée et les exemplaires
        /// </summary>
        /// <param name="revue">la revue</param>
        private void AfficheReceptionRevueInfos(Revue revue)
        {
            // informations sur la revue
            txbReceptionRevuePeriodicite.Text = revue.Periodicite;
            txbReceptionRevueImage.Text = revue.Image;
            txbReceptionRevueDelaiMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbReceptionRevueNumero.Text = revue.Id;
            txbReceptionRevueGenre.Text = revue.Genre;
            txbReceptionRevuePublic.Text = revue.Public;
            txbReceptionRevueRayon.Text = revue.Rayon;
            txbReceptionRevueTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbReceptionRevueImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbReceptionRevueImage.Image = null;
            }
            // affiche la liste des exemplaires de la revue
            AfficheReceptionExemplairesRevue();
        }

        /// <summary>
        /// Récupère et affiche les exemplaires d'une revue
        /// </summary>
        private void AfficheReceptionExemplairesRevue()
        {
            string idDocuement = txbReceptionRevueNumero.Text;
            lesParutionsExemplaires = controller.GetExemplaires(idDocuement);
            RemplirReceptionExemplairesListe(lesParutionsExemplaires);
            AccesReceptionExemplaireGroupBox(true);
        }

        /// <summary>
        /// Permet ou interdit l'accès à la gestion de la réception d'un exemplaire
        /// et vide les objets graphiques
        /// </summary>
        /// <param name="acces">true ou false</param>
        private void AccesReceptionExemplaireGroupBox(bool acces)
        {
            if(!utilisateur.Administrateur && utilisateur.Service != "Prets")
                grpReceptionExemplaire.Enabled = acces;
            txbReceptionExemplaireImage.Text = "";
            txbReceptionExemplaireNumero.Text = "";
            pcbReceptionExemplaireImage.Image = null;
            dtpReceptionExemplaireDate.Value = DateTime.Now;
        }

        /// <summary>
        /// Recherche image sur disque (pour l'exemplaire à insérer)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionExemplaireImage_Click(object sender, EventArgs e)
        {
            string filePath = "";
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                // positionnement à la racine du disque où se trouve le dossier actuel
                InitialDirectory = Path.GetPathRoot(Environment.CurrentDirectory),
                Filter = "Files|*.jpg;*.bmp;*.jpeg;*.png;*.gif"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
            }
            txbReceptionExemplaireImage.Text = filePath;
            try
            {
                pcbReceptionExemplaireImage.Image = Image.FromFile(filePath);
            }
            catch
            {
                pcbReceptionExemplaireImage.Image = null;
            }
        }

        /// <summary>
        /// Enregistrement du nouvel exemplaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionExemplaireValider_Click(object sender, EventArgs e)
        {
            if (!txbReceptionExemplaireNumero.Text.Equals(""))
            {
                try
                {
                    int numero = int.Parse(txbReceptionExemplaireNumero.Text);
                    DateTime dateAchat = dtpReceptionExemplaireDate.Value;
                    string photo = txbReceptionExemplaireImage.Text;
                    string idEtat = ETATNEUF;
                    string etat = ETATNEUFTEXTE;
                    string idDocument = txbReceptionRevueNumero.Text;
                    Exemplaire exemplaire = new Exemplaire(numero, dateAchat, photo, idEtat, etat, idDocument);
                    if (controller.CreerExemplaire(exemplaire))
                    {
                        AfficheReceptionExemplairesRevue();
                    }
                    else
                    {
                        MessageBox.Show("numéro de publication déjà existant", "Erreur");
                    }
                }
                catch
                {
                    MessageBox.Show("le numéro de parution doit être numérique", "Information");
                    txbReceptionExemplaireNumero.Text = "";
                    txbReceptionExemplaireNumero.Focus();
                }
            }
            else
            {
                MessageBox.Show("numéro de parution obligatoire", "Information");
            }
        }

        /// <summary>
        /// Tri sur une colonne
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvExemplairesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvReceptionExemplairesListe.Columns[e.ColumnIndex].HeaderText;
            List<Exemplaire> sortedList = new List<Exemplaire>();
            switch (titreColonne)
            {
                case "Numero":
                    sortedList = lesParutionsExemplaires.OrderBy(o => o.Numero).Reverse().ToList();
                    break;
                case "DateAchat":
                    sortedList = lesParutionsExemplaires.OrderBy(o => o.DateAchat).Reverse().ToList();
                    break;
                case "Etat":
                    sortedList = lesParutionsExemplaires.OrderBy(o => o.Photo).ToList();
                    break;
            }
            RemplirReceptionExemplairesListe(sortedList);
        }

        /// <summary>
        /// affichage de l'image de l'exemplaire suite à la sélection d'un exemplaire dans la liste
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvReceptionExemplairesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvReceptionExemplairesListe.CurrentCell != null)
            {
                btnReceptionParutionsSupprimer.Enabled = true;
                Exemplaire exemplaire = (Exemplaire)bdgParutionsExemplairesListe.List[bdgParutionsExemplairesListe.Position];
                string image = exemplaire.Photo;
                try
                {
                    pcbReceptionExemplaireRevueImage.Image = Image.FromFile(image);
                }
                catch
                {
                    pcbReceptionExemplaireRevueImage.Image = null;
                }
            }
            else
            {
                btnReceptionParutionsSupprimer.Enabled = false;
                pcbReceptionExemplaireRevueImage.Image = null;
            }
        }
        
        /// <summary>
        /// Méthode événementielle au double clic sur une case de la DataGridView
        /// Cette méthode gère le double clic sur la case 'état' d'un des exemplaires
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvReceptionExemplairesListe_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == -1 || e.ColumnIndex != dgvReceptionExemplairesListe.Columns["etat"].Index)
                return;

            FrmModifEtat formModifEtat =
                new FrmModifEtat(dgvReceptionExemplairesListe[e.ColumnIndex, e.RowIndex].Value.ToString());

            Exemplaire exemplaire = (Exemplaire)bdgParutionsExemplairesListe.List[bdgParutionsExemplairesListe.Position];

            if (formModifEtat.ShowDialog() == DialogResult.OK && formModifEtat.Etat.Libelle != exemplaire.Etat)
            {
                exemplaire.IdEtat = formModifEtat.Etat.Id;
                exemplaire.Etat = formModifEtat.Etat.Libelle;
                controller.ModifierExemplaire(exemplaire);
            }
        }
        
        /// <summary>
        /// Méthode événementielle au clic sur le bouton supprimer des parutions
        /// Cette méthode gère la suppression d'une parution après confirmation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionParutionsSupprimer_Click(object sender, EventArgs e)
        {
            Exemplaire exemplaire = (Exemplaire)bdgParutionsExemplairesListe.List[bdgParutionsExemplairesListe.Position];

            if (MessageBox.Show("Voulez-vous vraiment supprimer la parution N°" + exemplaire.Numero + " ?",
                    "Suppression d'une parution", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (controller.SupprimerExemplaire(exemplaire))
                {
                    AfficheReceptionExemplairesRevue();
                    dgvReceptionExemplairesListe_SelectionChanged(null, null);
                }
            }
        }
        #endregion
    }
}

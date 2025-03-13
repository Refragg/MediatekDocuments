using System;
using System.Linq;
using System.Windows.Forms;
using MediaTekDocuments.controller;
using MediaTekDocuments.model;

namespace MediaTekDocuments.view
{
    /// <summary>
    /// Formulaire demandant à l'utilisateur de modifier un état (utilisé pour modifier les états des exemplaires)
    /// </summary>
    public partial class FrmModifEtat : Form
    {
        private FrmModifEtatController controller = new FrmModifEtatController();
        
        public Etat Etat { get; private set; }
        
        /// <summary>
        /// Constructeur du formulaire
        /// </summary>
        /// <param name="etatInitial">L'état initial de l'exemplaire</param>
        public FrmModifEtat(string etatInitial = null)
        {
            InitializeComponent();
            AcceptButton = btnValider;
            CancelButton = btnAnnuler;
            
            cbxEtat.Items.AddRange(controller.GetAllEtats().OrderBy(x => x.Id).ToArray<object>());

            if (etatInitial == null)
                cbxEtat.SelectedIndex = 0;
            else
                cbxEtat.SelectedIndex = cbxEtat.Items.IndexOf(cbxEtat.Items.OfType<Etat>().First(x => x.Libelle == etatInitial));
        }

        /// <summary>
        /// Méthode événementielle au clic sur le bouton valider
        /// Cette méthode change l'état et le résultat du dialogue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnValider_Click(object sender, EventArgs e)
        {
            Etat = (Etat)cbxEtat.SelectedItem;
            DialogResult = DialogResult.OK;
        }
    }
}
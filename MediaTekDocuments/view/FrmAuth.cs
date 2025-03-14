using System;
using System.Windows.Forms;
using MediaTekDocuments.controller;
using MediaTekDocuments.model;

namespace MediaTekDocuments.view
{
    /// <summary>
    /// Classe du formulaire de connexion à l'application
    /// </summary>
    public partial class FrmAuth : Form
    {
        private FrmAuthController controller;
        
        /// <summary>
        /// Constructeur du formulaire, valorisation du controlleur
        /// </summary>
        public FrmAuth()
        {
            InitializeComponent();
            AcceptButton = btnConnexion;
            
            controller = new FrmAuthController();
        }

        /// <summary>
        /// Méthode événementielle au clic sur le bouton 'Se connecter'
        /// Cette méthode vérifie si la connexion est valide et soit indique des identifiants incorrects,
        /// soit ferme l'application pour un utilisateur non habilité, soit ouvre l'application en passant
        /// l'utilisateur connecté.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnexion_Click(object sender, EventArgs e)
        {
            Utilisateur utilisateur = controller.ControleAuthentification(txbLogin.Text, txbPassword.Text);
            if (utilisateur is null)
            {
                MessageBox.Show("Les identifiants sont incorrects.", "Authentification incorrecte", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (utilisateur.Service == "Culture")
            {
                MessageBox.Show("Vous n'êtes pas habilité à utiliser cette application. L'application va maintenant se fermer.", "Autorisations manquantes", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return;
            }
            
            FrmMediatek formApplication = new FrmMediatek(utilisateur);
            Hide();
            formApplication.Show();
            formApplication.FormClosed += (s, args) =>
            {
                formApplication.Dispose();
                Close();
            };
        }

        /// <summary>
        /// Méthode événementielle au changement de texte pour l'entrée du login
        /// Cette méthode active ou désactive le bouton 'Se connecter' en fonction de
        /// l'état de remplissage des 2 champs du formulaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbLogin_TextChanged(object sender, EventArgs e)
        {
            btnConnexion.Enabled = !string.IsNullOrEmpty(txbPassword.Text) && !string.IsNullOrEmpty(txbLogin.Text);
        }

        /// <summary>
        /// Méthode événementielle au changement de texte pour l'entrée du mot de passe
        /// Cette méthode active ou désactive le bouton 'Se connecter' en fonction de
        /// l'état de remplissage des 2 champs du formulaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbPassword_TextChanged(object sender, EventArgs e)
        {
            btnConnexion.Enabled = !string.IsNullOrEmpty(txbPassword.Text) && !string.IsNullOrEmpty(txbLogin.Text);
        }
    }
}
using MediaTekDocuments.dal;
using MediaTekDocuments.model;
using MediaTekDocuments.view;
using NUnit.Framework;

namespace SpecFlowMediaTekDocuments.StepDefinitions
{
    [Binding]
    public class RechercheStepDefinitions
    {
        private FrmMediatek frmMediatek;

        public RechercheStepDefinitions()
        {
            Access access = Access.GetInstance();
            Utilisateur admin = access.ControleAuthentification("admin", "admin");
            Assert.NotNull(admin);
            frmMediatek = new FrmMediatek(admin, false);
        }

        private TabPage GetTabPage(string tabPageName)
        {
            return (TabPage)frmMediatek.Controls["tabOngletsApplication"].Controls[tabPageName];
        }

        [Given(@"je suis dans longlet '([^']*)'")]
        public void GivenJeSuisDansLonglet(string livres)
        {
            TabControl tabOnglets = (TabControl)frmMediatek.Controls["tabOngletsApplication"];
            frmMediatek.Visible = true;
            tabOnglets.SelectedTab = GetTabPage("tabLivres");
            Assert.AreEqual("tabLivres", tabOnglets.SelectedTab.Name);
        }

        [When(@"je selectionne le genre '([^']*)'")]
        public void WhenJeSelectionneLeGenre(string p0)
        {
            ComboBox cbxLivresGenres = (ComboBox)GetTabPage("tabLivres").Controls["grpLivresRecherche"].Controls["cbxLivresGenres"];
            int indiceLigne = cbxLivresGenres.FindStringExact(p0);
            cbxLivresGenres.SelectedIndex = indiceLigne;
            Assert.AreNotEqual(-1, cbxLivresGenres.SelectedIndex);
        }

        [Then(@"le nombre de livres obtenu est de (.*)")]
        public void ThenLeNombreDeLivresObtenuEstDe(int p0)
        {
            DataGridView listeLivres = (DataGridView)GetTabPage("tabLivres").Controls["grpLivresRecherche"].Controls["dgvLivresListe"];
            Assert.AreEqual(5, listeLivres.Rows.Count);
        }
    }
}

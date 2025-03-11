using System.ComponentModel;

namespace MediaTekDocuments.view
{
    partial class FrmMediatekCommandesLivreDvd
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvCommandes = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbxStade = new System.Windows.Forms.ComboBox();
            this.dtpDateCommande = new System.Windows.Forms.DateTimePicker();
            this.txbDateCommande = new System.Windows.Forms.TextBox();
            this.txbStade = new System.Windows.Forms.TextBox();
            this.nudNbExemplaires = new System.Windows.Forms.NumericUpDown();
            this.txbMontant = new System.Windows.Forms.TextBox();
            this.txbNumCommande = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnAnnuler = new System.Windows.Forms.Button();
            this.btnValider = new System.Windows.Forms.Button();
            this.btnSupprimer = new System.Windows.Forms.Button();
            this.btnModifier = new System.Windows.Forms.Button();
            this.btnAjouter = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCommandes)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudNbExemplaires)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvCommandes
            // 
            this.dgvCommandes.AllowUserToAddRows = false;
            this.dgvCommandes.AllowUserToDeleteRows = false;
            this.dgvCommandes.AllowUserToResizeColumns = false;
            this.dgvCommandes.AllowUserToResizeRows = false;
            this.dgvCommandes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvCommandes.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvCommandes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCommandes.Location = new System.Drawing.Point(12, 12);
            this.dgvCommandes.MultiSelect = false;
            this.dgvCommandes.Name = "dgvCommandes";
            this.dgvCommandes.ReadOnly = true;
            this.dgvCommandes.RowHeadersVisible = false;
            this.dgvCommandes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCommandes.Size = new System.Drawing.Size(437, 209);
            this.dgvCommandes.TabIndex = 0;
            this.dgvCommandes.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvCommandes_ColumnHeaderMouseClick);
            this.dgvCommandes.SelectionChanged += new System.EventHandler(this.dgvCommandes_SelectionChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbxStade);
            this.groupBox1.Controls.Add(this.dtpDateCommande);
            this.groupBox1.Controls.Add(this.txbDateCommande);
            this.groupBox1.Controls.Add(this.txbStade);
            this.groupBox1.Controls.Add(this.nudNbExemplaires);
            this.groupBox1.Controls.Add(this.txbMontant);
            this.groupBox1.Controls.Add(this.txbNumCommande);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 227);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(437, 153);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Informations commandes";
            // 
            // cbxStade
            // 
            this.cbxStade.FormattingEnabled = true;
            this.cbxStade.Location = new System.Drawing.Point(147, 123);
            this.cbxStade.Name = "cbxStade";
            this.cbxStade.Size = new System.Drawing.Size(200, 21);
            this.cbxStade.TabIndex = 10;
            this.cbxStade.Visible = false;
            // 
            // dtpDateCommande
            // 
            this.dtpDateCommande.Location = new System.Drawing.Point(147, 45);
            this.dtpDateCommande.Name = "dtpDateCommande";
            this.dtpDateCommande.Size = new System.Drawing.Size(200, 20);
            this.dtpDateCommande.TabIndex = 6;
            this.dtpDateCommande.Visible = false;
            // 
            // txbDateCommande
            // 
            this.txbDateCommande.Location = new System.Drawing.Point(147, 45);
            this.txbDateCommande.Name = "txbDateCommande";
            this.txbDateCommande.ReadOnly = true;
            this.txbDateCommande.Size = new System.Drawing.Size(200, 20);
            this.txbDateCommande.TabIndex = 6;
            // 
            // txbStade
            // 
            this.txbStade.Location = new System.Drawing.Point(147, 123);
            this.txbStade.Name = "txbStade";
            this.txbStade.ReadOnly = true;
            this.txbStade.Size = new System.Drawing.Size(200, 20);
            this.txbStade.TabIndex = 9;
            // 
            // nudNbExemplaires
            // 
            this.nudNbExemplaires.Location = new System.Drawing.Point(147, 97);
            this.nudNbExemplaires.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            this.nudNbExemplaires.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.nudNbExemplaires.Name = "nudNbExemplaires";
            this.nudNbExemplaires.ReadOnly = true;
            this.nudNbExemplaires.Size = new System.Drawing.Size(200, 20);
            this.nudNbExemplaires.TabIndex = 8;
            this.nudNbExemplaires.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // txbMontant
            // 
            this.txbMontant.Location = new System.Drawing.Point(147, 71);
            this.txbMontant.Name = "txbMontant";
            this.txbMontant.ReadOnly = true;
            this.txbMontant.Size = new System.Drawing.Size(200, 20);
            this.txbMontant.TabIndex = 7;
            // 
            // txbNumCommande
            // 
            this.txbNumCommande.Location = new System.Drawing.Point(147, 19);
            this.txbNumCommande.Name = "txbNumCommande";
            this.txbNumCommande.ReadOnly = true;
            this.txbNumCommande.Size = new System.Drawing.Size(200, 20);
            this.txbNumCommande.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(6, 126);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Stade :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(6, 99);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(138, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Nombre d\'exemplaires :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(6, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Montant :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Date commande :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "N° Commande :";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnAnnuler);
            this.groupBox2.Controls.Add(this.btnValider);
            this.groupBox2.Controls.Add(this.btnSupprimer);
            this.groupBox2.Controls.Add(this.btnModifier);
            this.groupBox2.Controls.Add(this.btnAjouter);
            this.groupBox2.Location = new System.Drawing.Point(12, 386);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(437, 50);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Actions";
            // 
            // btnAnnuler
            // 
            this.btnAnnuler.Enabled = false;
            this.btnAnnuler.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAnnuler.Location = new System.Drawing.Point(353, 19);
            this.btnAnnuler.Name = "btnAnnuler";
            this.btnAnnuler.Size = new System.Drawing.Size(75, 23);
            this.btnAnnuler.TabIndex = 4;
            this.btnAnnuler.Text = "Annuler";
            this.btnAnnuler.UseVisualStyleBackColor = true;
            this.btnAnnuler.Click += new System.EventHandler(this.btnAnnuler_Click);
            // 
            // btnValider
            // 
            this.btnValider.Enabled = false;
            this.btnValider.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnValider.Location = new System.Drawing.Point(272, 19);
            this.btnValider.Name = "btnValider";
            this.btnValider.Size = new System.Drawing.Size(75, 23);
            this.btnValider.TabIndex = 3;
            this.btnValider.Text = "Valider";
            this.btnValider.UseVisualStyleBackColor = true;
            this.btnValider.Click += new System.EventHandler(this.btnValider_Click);
            // 
            // btnSupprimer
            // 
            this.btnSupprimer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSupprimer.Location = new System.Drawing.Point(168, 19);
            this.btnSupprimer.Name = "btnSupprimer";
            this.btnSupprimer.Size = new System.Drawing.Size(75, 23);
            this.btnSupprimer.TabIndex = 2;
            this.btnSupprimer.Text = "Supprimer";
            this.btnSupprimer.UseVisualStyleBackColor = true;
            this.btnSupprimer.Click += new System.EventHandler(this.btnSupprimer_Click);
            // 
            // btnModifier
            // 
            this.btnModifier.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnModifier.Location = new System.Drawing.Point(87, 19);
            this.btnModifier.Name = "btnModifier";
            this.btnModifier.Size = new System.Drawing.Size(75, 23);
            this.btnModifier.TabIndex = 1;
            this.btnModifier.Text = "Modifier";
            this.btnModifier.UseVisualStyleBackColor = true;
            this.btnModifier.Click += new System.EventHandler(this.btnModifier_Click);
            // 
            // btnAjouter
            // 
            this.btnAjouter.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAjouter.Location = new System.Drawing.Point(6, 19);
            this.btnAjouter.Name = "btnAjouter";
            this.btnAjouter.Size = new System.Drawing.Size(75, 23);
            this.btnAjouter.TabIndex = 0;
            this.btnAjouter.Text = "Ajouter";
            this.btnAjouter.UseVisualStyleBackColor = true;
            this.btnAjouter.Click += new System.EventHandler(this.btnAjouter_Click);
            // 
            // FrmMediatekCommandesLivreDvd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 445);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dgvCommandes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmMediatekCommandesLivreDvd";
            this.Text = "Gestion des commandes";
            ((System.ComponentModel.ISupportInitialize)(this.dgvCommandes)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudNbExemplaires)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.ComboBox cbxStade;

        private System.Windows.Forms.DateTimePicker dtpDateCommande;
        private System.Windows.Forms.TextBox txbDateCommande;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnAjouter;
        private System.Windows.Forms.Button btnModifier;
        private System.Windows.Forms.Button btnSupprimer;
        private System.Windows.Forms.Button btnValider;
        private System.Windows.Forms.Button btnAnnuler;

        private System.Windows.Forms.TextBox txbNumCommande;
        private System.Windows.Forms.TextBox txbMontant;
        private System.Windows.Forms.NumericUpDown nudNbExemplaires;
        private System.Windows.Forms.TextBox txbStade;

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;

        private System.Windows.Forms.Label label2;

        private System.Windows.Forms.Label label1;

        private System.Windows.Forms.GroupBox groupBox1;

        private System.Windows.Forms.DataGridView dgvCommandes;

        #endregion
    }
}
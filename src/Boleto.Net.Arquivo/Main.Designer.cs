namespace BoletoNet.Arquivo
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.lstReturnFields = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.arquivoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cNABToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.retornoToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.lerToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.gerarToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.impressãoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBoxBancos = new System.Windows.Forms.GroupBox();
            this.radioButtonBNB = new System.Windows.Forms.RadioButton();
            this.radioButtonBanrisul = new System.Windows.Forms.RadioButton();
            this.radioButtonSicredi = new System.Windows.Forms.RadioButton();
            this.radioButtonCaixa = new System.Windows.Forms.RadioButton();
            this.radioButtonSantander = new System.Windows.Forms.RadioButton();
            this.radioButtonUnibanco = new System.Windows.Forms.RadioButton();
            this.radioButtonSudameris = new System.Windows.Forms.RadioButton();
            this.radioButtonBancoBrasil = new System.Windows.Forms.RadioButton();
            this.radioButtonReal = new System.Windows.Forms.RadioButton();
            this.radioButtonBradesco = new System.Windows.Forms.RadioButton();
            this.radioButtonHsbc = new System.Windows.Forms.RadioButton();
            this.radioButtonSafra = new System.Windows.Forms.RadioButton();
            this.radioButtonItau = new System.Windows.Forms.RadioButton();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonCNAB240 = new System.Windows.Forms.RadioButton();
            this.radioButtonCNAB400 = new System.Windows.Forms.RadioButton();
            this.menuStrip.SuspendLayout();
            this.groupBoxBancos.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "Arquivos de Retorno (*.ret)|*.ret|Todos Arquivos (*.*)|*.*";
            // 
            // lstReturnFields
            // 
            this.lstReturnFields.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstReturnFields.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader9});
            this.lstReturnFields.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstReturnFields.FullRowSelect = true;
            this.lstReturnFields.GridLines = true;
            this.lstReturnFields.HideSelection = false;
            this.lstReturnFields.Location = new System.Drawing.Point(165, 32);
            this.lstReturnFields.MultiSelect = false;
            this.lstReturnFields.Name = "lstReturnFields";
            this.lstReturnFields.Size = new System.Drawing.Size(834, 402);
            this.lstReturnFields.TabIndex = 2;
            this.lstReturnFields.UseCompatibleStateImageBehavior = false;
            this.lstReturnFields.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Sacado";
            this.columnHeader1.Width = 150;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Data de Vencimento";
            this.columnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader2.Width = 110;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Data do Crédito";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader3.Width = 110;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Valor do Título";
            this.columnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader4.Width = 85;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Valor Pago";
            this.columnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader5.Width = 85;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Cód. de Retorno";
            this.columnHeader6.Width = 100;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Situação";
            this.columnHeader7.Width = 100;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "Nosso Número";
            this.columnHeader8.Width = 80;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "Numero Documento";
            this.columnHeader9.Width = 100;
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            this.openFileDialog.Filter = "Arquivos de Retorno (*.ret)|*.ret|Todos Arquivos (*.*)|*.*";
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.arquivoToolStripMenuItem,
            this.impressãoToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Margin = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1011, 24);
            this.menuStrip.TabIndex = 4;
            this.menuStrip.Text = "menuStrip1";
            // 
            // arquivoToolStripMenuItem
            // 
            this.arquivoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cNABToolStripMenuItem,
            this.retornoToolStripMenuItem1});
            this.arquivoToolStripMenuItem.Name = "arquivoToolStripMenuItem";
            this.arquivoToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.arquivoToolStripMenuItem.Text = "&Arquivo";
            // 
            // cNABToolStripMenuItem
            // 
            this.cNABToolStripMenuItem.Name = "cNABToolStripMenuItem";
            this.cNABToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.cNABToolStripMenuItem.Text = "&Remessa";
            this.cNABToolStripMenuItem.Click += new System.EventHandler(this.cNABToolStripMenuItem_Click);
            // 
            // retornoToolStripMenuItem1
            // 
            this.retornoToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lerToolStripMenuItem1,
            this.gerarToolStripMenuItem1});
            this.retornoToolStripMenuItem1.Name = "retornoToolStripMenuItem1";
            this.retornoToolStripMenuItem1.Size = new System.Drawing.Size(120, 22);
            this.retornoToolStripMenuItem1.Text = "R&etorno";
            // 
            // lerToolStripMenuItem1
            // 
            this.lerToolStripMenuItem1.Name = "lerToolStripMenuItem1";
            this.lerToolStripMenuItem1.Size = new System.Drawing.Size(102, 22);
            this.lerToolStripMenuItem1.Text = "&Ler";
            this.lerToolStripMenuItem1.Click += new System.EventHandler(this.lerToolStripMenuItem1_Click);
            // 
            // gerarToolStripMenuItem1
            // 
            this.gerarToolStripMenuItem1.Name = "gerarToolStripMenuItem1";
            this.gerarToolStripMenuItem1.Size = new System.Drawing.Size(102, 22);
            this.gerarToolStripMenuItem1.Text = "&Gerar";
            this.gerarToolStripMenuItem1.Click += new System.EventHandler(this.gerarToolStripMenuItem1_Click);
            // 
            // impressãoToolStripMenuItem
            // 
            this.impressãoToolStripMenuItem.Name = "impressãoToolStripMenuItem";
            this.impressãoToolStripMenuItem.Size = new System.Drawing.Size(73, 20);
            this.impressãoToolStripMenuItem.Text = "&Impressão";
            this.impressãoToolStripMenuItem.Click += new System.EventHandler(this.impressãoToolStripMenuItem_Click);
            // 
            // groupBoxBancos
            // 
            this.groupBoxBancos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxBancos.Controls.Add(this.radioButtonBNB);
            this.groupBoxBancos.Controls.Add(this.radioButtonBanrisul);
            this.groupBoxBancos.Controls.Add(this.radioButtonSicredi);
            this.groupBoxBancos.Controls.Add(this.radioButtonCaixa);
            this.groupBoxBancos.Controls.Add(this.radioButtonSantander);
            this.groupBoxBancos.Controls.Add(this.radioButtonUnibanco);
            this.groupBoxBancos.Controls.Add(this.radioButtonSudameris);
            this.groupBoxBancos.Controls.Add(this.radioButtonBancoBrasil);
            this.groupBoxBancos.Controls.Add(this.radioButtonReal);
            this.groupBoxBancos.Controls.Add(this.radioButtonBradesco);
            this.groupBoxBancos.Controls.Add(this.radioButtonHsbc);
            this.groupBoxBancos.Controls.Add(this.radioButtonSafra);
            this.groupBoxBancos.Controls.Add(this.radioButtonItau);
            this.groupBoxBancos.Location = new System.Drawing.Point(12, 110);
            this.groupBoxBancos.Name = "groupBoxBancos";
            this.groupBoxBancos.Padding = new System.Windows.Forms.Padding(10, 5, 10, 10);
            this.groupBoxBancos.Size = new System.Drawing.Size(147, 324);
            this.groupBoxBancos.TabIndex = 5;
            this.groupBoxBancos.TabStop = false;
            this.groupBoxBancos.Text = "Bancos";
            // 
            // radioButtonBNB
            // 
            this.radioButtonBNB.AutoSize = true;
            this.radioButtonBNB.Location = new System.Drawing.Point(13, 294);
            this.radioButtonBNB.Name = "radioButtonBNB";
            this.radioButtonBNB.Size = new System.Drawing.Size(68, 17);
            this.radioButtonBNB.TabIndex = 31;
            this.radioButtonBNB.Tag = "4";
            this.radioButtonBNB.Text = "Nordeste";
            this.radioButtonBNB.UseVisualStyleBackColor = true;
            // 
            // radioButtonBanrisul
            // 
            this.radioButtonBanrisul.AutoSize = true;
            this.radioButtonBanrisul.Location = new System.Drawing.Point(13, 274);
            this.radioButtonBanrisul.Name = "radioButtonBanrisul";
            this.radioButtonBanrisul.Size = new System.Drawing.Size(62, 17);
            this.radioButtonBanrisul.TabIndex = 30;
            this.radioButtonBanrisul.Tag = "104";
            this.radioButtonBanrisul.Text = "Banrisul";
            this.radioButtonBanrisul.UseVisualStyleBackColor = true;
            // 
            // radioButtonSicredi
            // 
            this.radioButtonSicredi.AutoSize = true;
            this.radioButtonSicredi.Location = new System.Drawing.Point(13, 251);
            this.radioButtonSicredi.Name = "radioButtonSicredi";
            this.radioButtonSicredi.Size = new System.Drawing.Size(57, 17);
            this.radioButtonSicredi.TabIndex = 29;
            this.radioButtonSicredi.Tag = "748";
            this.radioButtonSicredi.Text = "Sicredi";
            this.radioButtonSicredi.UseVisualStyleBackColor = true;
            // 
            // radioButtonCaixa
            // 
            this.radioButtonCaixa.AutoSize = true;
            this.radioButtonCaixa.Location = new System.Drawing.Point(13, 228);
            this.radioButtonCaixa.Name = "radioButtonCaixa";
            this.radioButtonCaixa.Size = new System.Drawing.Size(51, 17);
            this.radioButtonCaixa.TabIndex = 28;
            this.radioButtonCaixa.Tag = "104";
            this.radioButtonCaixa.Text = "Caixa";
            this.radioButtonCaixa.UseVisualStyleBackColor = true;
            // 
            // radioButtonSantander
            // 
            this.radioButtonSantander.AutoSize = true;
            this.radioButtonSantander.Location = new System.Drawing.Point(13, 205);
            this.radioButtonSantander.Name = "radioButtonSantander";
            this.radioButtonSantander.Size = new System.Drawing.Size(74, 17);
            this.radioButtonSantander.TabIndex = 27;
            this.radioButtonSantander.Tag = "409";
            this.radioButtonSantander.Text = "Santander";
            this.radioButtonSantander.UseVisualStyleBackColor = true;
            // 
            // radioButtonUnibanco
            // 
            this.radioButtonUnibanco.AutoSize = true;
            this.radioButtonUnibanco.Location = new System.Drawing.Point(13, 182);
            this.radioButtonUnibanco.Name = "radioButtonUnibanco";
            this.radioButtonUnibanco.Size = new System.Drawing.Size(71, 17);
            this.radioButtonUnibanco.TabIndex = 21;
            this.radioButtonUnibanco.Tag = "409";
            this.radioButtonUnibanco.Text = "Unibanco";
            this.radioButtonUnibanco.UseVisualStyleBackColor = true;
            // 
            // radioButtonSudameris
            // 
            this.radioButtonSudameris.AutoSize = true;
            this.radioButtonSudameris.Location = new System.Drawing.Point(13, 159);
            this.radioButtonSudameris.Name = "radioButtonSudameris";
            this.radioButtonSudameris.Size = new System.Drawing.Size(74, 17);
            this.radioButtonSudameris.TabIndex = 22;
            this.radioButtonSudameris.Tag = "347";
            this.radioButtonSudameris.Text = "Sudameris";
            this.radioButtonSudameris.UseVisualStyleBackColor = true;
            // 
            // radioButtonBancoBrasil
            // 
            this.radioButtonBancoBrasil.AutoSize = true;
            this.radioButtonBancoBrasil.Location = new System.Drawing.Point(13, 44);
            this.radioButtonBancoBrasil.Name = "radioButtonBancoBrasil";
            this.radioButtonBancoBrasil.Size = new System.Drawing.Size(99, 17);
            this.radioButtonBancoBrasil.TabIndex = 25;
            this.radioButtonBancoBrasil.Tag = "1";
            this.radioButtonBancoBrasil.Text = "Banco do Brasil";
            this.radioButtonBancoBrasil.UseVisualStyleBackColor = true;
            // 
            // radioButtonReal
            // 
            this.radioButtonReal.AutoSize = true;
            this.radioButtonReal.Location = new System.Drawing.Point(13, 113);
            this.radioButtonReal.Name = "radioButtonReal";
            this.radioButtonReal.Size = new System.Drawing.Size(47, 17);
            this.radioButtonReal.TabIndex = 23;
            this.radioButtonReal.Tag = "356";
            this.radioButtonReal.Text = "Real";
            this.radioButtonReal.UseVisualStyleBackColor = true;
            // 
            // radioButtonBradesco
            // 
            this.radioButtonBradesco.AutoSize = true;
            this.radioButtonBradesco.Location = new System.Drawing.Point(13, 67);
            this.radioButtonBradesco.Name = "radioButtonBradesco";
            this.radioButtonBradesco.Size = new System.Drawing.Size(70, 17);
            this.radioButtonBradesco.TabIndex = 26;
            this.radioButtonBradesco.Tag = "237";
            this.radioButtonBradesco.Text = "Bradesco";
            this.radioButtonBradesco.UseVisualStyleBackColor = true;
            // 
            // radioButtonHsbc
            // 
            this.radioButtonHsbc.AutoSize = true;
            this.radioButtonHsbc.Location = new System.Drawing.Point(13, 90);
            this.radioButtonHsbc.Name = "radioButtonHsbc";
            this.radioButtonHsbc.Size = new System.Drawing.Size(54, 17);
            this.radioButtonHsbc.TabIndex = 24;
            this.radioButtonHsbc.Tag = "399";
            this.radioButtonHsbc.Text = "HSBC";
            this.radioButtonHsbc.UseVisualStyleBackColor = true;
            // 
            // radioButtonSafra
            // 
            this.radioButtonSafra.AutoSize = true;
            this.radioButtonSafra.Location = new System.Drawing.Point(13, 136);
            this.radioButtonSafra.Name = "radioButtonSafra";
            this.radioButtonSafra.Size = new System.Drawing.Size(50, 17);
            this.radioButtonSafra.TabIndex = 20;
            this.radioButtonSafra.Tag = "422";
            this.radioButtonSafra.Text = "Safra";
            this.radioButtonSafra.UseVisualStyleBackColor = true;
            // 
            // radioButtonItau
            // 
            this.radioButtonItau.AutoSize = true;
            this.radioButtonItau.Checked = true;
            this.radioButtonItau.Location = new System.Drawing.Point(13, 21);
            this.radioButtonItau.Name = "radioButtonItau";
            this.radioButtonItau.Size = new System.Drawing.Size(43, 17);
            this.radioButtonItau.TabIndex = 0;
            this.radioButtonItau.TabStop = true;
            this.radioButtonItau.Tag = "341";
            this.radioButtonItau.Text = "Itaú";
            this.radioButtonItau.UseVisualStyleBackColor = true;
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.WorkerReportsProgress = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonCNAB240);
            this.groupBox1.Controls.Add(this.radioButtonCNAB400);
            this.groupBox1.Location = new System.Drawing.Point(12, 32);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(147, 72);
            this.groupBox1.TabIndex = 28;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Padrão";
            // 
            // radioButtonCNAB240
            // 
            this.radioButtonCNAB240.AutoSize = true;
            this.radioButtonCNAB240.Location = new System.Drawing.Point(12, 42);
            this.radioButtonCNAB240.Name = "radioButtonCNAB240";
            this.radioButtonCNAB240.Size = new System.Drawing.Size(75, 17);
            this.radioButtonCNAB240.TabIndex = 29;
            this.radioButtonCNAB240.TabStop = true;
            this.radioButtonCNAB240.Tag = "409";
            this.radioButtonCNAB240.Text = "CNAB 240";
            this.radioButtonCNAB240.UseVisualStyleBackColor = true;
            // 
            // radioButtonCNAB400
            // 
            this.radioButtonCNAB400.AutoSize = true;
            this.radioButtonCNAB400.Checked = true;
            this.radioButtonCNAB400.Location = new System.Drawing.Point(13, 19);
            this.radioButtonCNAB400.Name = "radioButtonCNAB400";
            this.radioButtonCNAB400.Size = new System.Drawing.Size(75, 17);
            this.radioButtonCNAB400.TabIndex = 28;
            this.radioButtonCNAB400.TabStop = true;
            this.radioButtonCNAB400.Tag = "409";
            this.radioButtonCNAB400.Text = "CNAB 400";
            this.radioButtonCNAB400.UseVisualStyleBackColor = true;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1011, 446);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBoxBancos);
            this.Controls.Add(this.lstReturnFields);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Geração do Arquivo Remessa";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.groupBoxBancos.ResumeLayout(false);
            this.groupBoxBancos.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        protected System.Windows.Forms.ListView lstReturnFields;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem arquivoToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBoxBancos;
        private System.Windows.Forms.RadioButton radioButtonItau;
        private System.Windows.Forms.ToolStripMenuItem impressãoToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.RadioButton radioButtonUnibanco;
        private System.Windows.Forms.RadioButton radioButtonSudameris;
        private System.Windows.Forms.RadioButton radioButtonBancoBrasil;
        private System.Windows.Forms.RadioButton radioButtonReal;
        private System.Windows.Forms.RadioButton radioButtonBradesco;
        private System.Windows.Forms.RadioButton radioButtonHsbc;
        private System.Windows.Forms.RadioButton radioButtonSafra;
        private System.Windows.Forms.RadioButton radioButtonSantander;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonCNAB240;
        private System.Windows.Forms.RadioButton radioButtonCNAB400;
        private System.Windows.Forms.ToolStripMenuItem cNABToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem retornoToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem lerToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem gerarToolStripMenuItem1;
        private System.Windows.Forms.RadioButton radioButtonCaixa;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.RadioButton radioButtonSicredi;
        private System.Windows.Forms.RadioButton radioButtonBanrisul;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.RadioButton radioButtonBNB;
    }
}


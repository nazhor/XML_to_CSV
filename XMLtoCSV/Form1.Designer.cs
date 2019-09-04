namespace XMLtoCSV
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.BT_Convertir = new System.Windows.Forms.Button();
            this.openFileDialog_FindFile = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // BT_Convertir
            // 
            this.BT_Convertir.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BT_Convertir.Location = new System.Drawing.Point(12, 71);
            this.BT_Convertir.Name = "BT_Convertir";
            this.BT_Convertir.Size = new System.Drawing.Size(269, 63);
            this.BT_Convertir.TabIndex = 0;
            this.BT_Convertir.Text = "Buscar fichero";
            this.BT_Convertir.UseVisualStyleBackColor = true;
            this.BT_Convertir.Click += new System.EventHandler(this.BT_Convertir_Click);
            // 
            // openFileDialog_FindFile
            // 
            this.openFileDialog_FindFile.FileName = "openFileDialog_FindFile";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(273, 40);
            this.label1.TabIndex = 1;
            this.label1.Text = "Selecciona el fichero de Chatmapper \r\npara convertirlo a Unreal";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 146);
            this.Controls.Add(this.BT_Convertir);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Chatmapper to No Wand";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BT_Convertir;
        private System.Windows.Forms.OpenFileDialog openFileDialog_FindFile;
        private System.Windows.Forms.Label label1;
    }
}


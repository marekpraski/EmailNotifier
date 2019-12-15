namespace EmailNotifier
{
    partial class EmailDisplayControl
    {
        /// <summary> 
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod wygenerowany przez Projektanta składników

        /// <summary> 
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować 
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            this.emailReadCheckBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.senderTextBox = new System.Windows.Forms.TextBox();
            this.subjectTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // emailReadCheckBox
            // 
            this.emailReadCheckBox.AutoSize = true;
            this.emailReadCheckBox.Location = new System.Drawing.Point(6, 3);
            this.emailReadCheckBox.Name = "emailReadCheckBox";
            this.emailReadCheckBox.Size = new System.Drawing.Size(87, 17);
            this.emailReadCheckBox.TabIndex = 0;
            this.emailReadCheckBox.Text = "mark as read";
            this.emailReadCheckBox.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "From";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Title";
            // 
            // senderTextBox
            // 
            this.senderTextBox.Location = new System.Drawing.Point(39, 21);
            this.senderTextBox.Name = "senderTextBox";
            this.senderTextBox.ReadOnly = true;
            this.senderTextBox.Size = new System.Drawing.Size(709, 20);
            this.senderTextBox.TabIndex = 3;
            // 
            // subjectTextBox
            // 
            this.subjectTextBox.Location = new System.Drawing.Point(39, 46);
            this.subjectTextBox.Name = "subjectTextBox";
            this.subjectTextBox.ReadOnly = true;
            this.subjectTextBox.Size = new System.Drawing.Size(709, 20);
            this.subjectTextBox.TabIndex = 4;
            this.subjectTextBox.MouseEnter += new System.EventHandler(this.SubjectTextBox_MouseEnter);
            // 
            // EmailDisplayControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.subjectTextBox);
            this.Controls.Add(this.senderTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.emailReadCheckBox);
            this.Name = "EmailDisplayControl";
            this.Size = new System.Drawing.Size(751, 70);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox emailReadCheckBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox senderTextBox;
        private System.Windows.Forms.TextBox subjectTextBox;
    }
}

﻿namespace DeportnetOffline
{
    partial class ModalVentas
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModalVentas));
            panel1 = new Panel();
            labelNombreApelldioCliente = new Label();
            label5 = new Label();
            comboBox1 = new ComboBox();
            panel2 = new Panel();
            buttonCobrar = new Button();
            panel3 = new Panel();
            label4 = new Label();
            labelCaracteristicasCobro = new Label();
            labelDescripcion = new Label();
            labelCantidad = new Label();
            labelPrecio = new Label();
            label3 = new Label();
            label2 = new Label();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(labelNombreApelldioCliente);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(comboBox1);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(label2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(800, 450);
            panel1.TabIndex = 1;
            // 
            // labelNombreApelldioCliente
            // 
            labelNombreApelldioCliente.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold);
            labelNombreApelldioCliente.Location = new Point(147, 54);
            labelNombreApelldioCliente.Name = "labelNombreApelldioCliente";
            labelNombreApelldioCliente.Size = new Size(488, 34);
            labelNombreApelldioCliente.TabIndex = 5;
            labelNombreApelldioCliente.Text = "Nombre y apellido del cliente";
            labelNombreApelldioCliente.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI Semibold", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(236, 9);
            label5.Name = "label5";
            label5.Size = new Size(309, 45);
            label5.TabIndex = 4;
            label5.Text = "Vender producto a: ";
            // 
            // comboBox1
            // 
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(220, 110);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(340, 29);
            comboBox1.TabIndex = 2;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // panel2
            // 
            panel2.BackColor = Color.Gainsboro;
            panel2.Controls.Add(buttonCobrar);
            panel2.Controls.Add(panel3);
            panel2.Controls.Add(labelPrecio);
            panel2.Controls.Add(label3);
            panel2.Location = new Point(63, 210);
            panel2.Name = "panel2";
            panel2.Size = new Size(677, 175);
            panel2.TabIndex = 0;
            // 
            // buttonCobrar
            // 
            buttonCobrar.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            buttonCobrar.Location = new Point(530, 128);
            buttonCobrar.Name = "buttonCobrar";
            buttonCobrar.Size = new Size(120, 35);
            buttonCobrar.TabIndex = 9;
            buttonCobrar.Text = "Cobrar";
            buttonCobrar.UseVisualStyleBackColor = true;
            buttonCobrar.Click += buttonCobrar_Click;
            // 
            // panel3
            // 
            panel3.BackColor = Color.Silver;
            panel3.Controls.Add(label4);
            panel3.Controls.Add(labelCaracteristicasCobro);
            panel3.Controls.Add(labelDescripcion);
            panel3.Controls.Add(labelCantidad);
            panel3.Location = new Point(24, 49);
            panel3.Name = "panel3";
            panel3.Size = new Size(626, 67);
            panel3.TabIndex = 8;
            // 
            // label4
            // 
            label4.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(442, 32);
            label4.Name = "label4";
            label4.Size = new Size(121, 23);
            label4.TabIndex = 7;
            label4.Text = "Importe";
            label4.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // labelCaracteristicasCobro
            // 
            labelCaracteristicasCobro.AutoSize = true;
            labelCaracteristicasCobro.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelCaracteristicasCobro.Location = new Point(42, 7);
            labelCaracteristicasCobro.Name = "labelCaracteristicasCobro";
            labelCaracteristicasCobro.Size = new Size(493, 21);
            labelCaracteristicasCobro.TabIndex = 3;
            labelCaracteristicasCobro.Text = "Cantidad           |                    Descripción                     |              Importe";
            // 
            // labelDescripcion
            // 
            labelDescripcion.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelDescripcion.Location = new Point(161, 32);
            labelDescripcion.Name = "labelDescripcion";
            labelDescripcion.Size = new Size(249, 23);
            labelDescripcion.TabIndex = 5;
            labelDescripcion.Text = "Descripción";
            labelDescripcion.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // labelCantidad
            // 
            labelCantidad.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelCantidad.Location = new Point(28, 30);
            labelCantidad.Name = "labelCantidad";
            labelCantidad.Size = new Size(97, 23);
            labelCantidad.TabIndex = 4;
            labelCantidad.Text = "cantidad";
            labelCantidad.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // labelPrecio
            // 
            labelPrecio.AutoSize = true;
            labelPrecio.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelPrecio.Location = new Point(72, 12);
            labelPrecio.Name = "labelPrecio";
            labelPrecio.Size = new Size(65, 25);
            labelPrecio.TabIndex = 2;
            labelPrecio.Text = "precio";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(26, 12);
            label3.Name = "label3";
            label3.Size = new Size(56, 20);
            label3.TabIndex = 1;
            label3.Text = "Total:  ";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(85, 172);
            label2.Name = "label2";
            label2.Size = new Size(186, 30);
            label2.TabIndex = 0;
            label2.Text = "Detalle de la venta";
            // 
            // ModalVentas
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(panel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ModalVentas";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "modalVentas";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private ComboBox comboBox1;
        private Panel panel2;
        private Button buttonCobrar;
        private Panel panel3;
        private Label label4;
        private Label labelCaracteristicasCobro;
        private Label labelDescripcion;
        private Label labelCantidad;
        private Label labelPrecio;
        private Label label3;
        private Label label2;
        private Label labelNombreApelldioCliente;
        private Label label5;
    }
}
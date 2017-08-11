namespace WinFormsAppFor157Recommend
{
    partial class FormTest
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
            this.buttonAPMAsyn = new System.Windows.Forms.Button();
            this.textBoxPage = new System.Windows.Forms.TextBox();
            this.buttonAutoReset = new System.Windows.Forms.Button();
            this.labelAutoReset = new System.Windows.Forms.Label();
            this.buttonSet = new System.Windows.Forms.Button();
            this.labelManualReset = new System.Windows.Forms.Label();
            this.buttonReset = new System.Windows.Forms.Button();
            this.btnBackgroudWorker = new System.Windows.Forms.Button();
            this.labelBackGroudWorker = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.btnThreadAbort = new System.Windows.Forms.Button();
            this.labelThreadAbort = new System.Windows.Forms.Label();
            this.btnTask = new System.Windows.Forms.Button();
            this.btnTaskThrow = new System.Windows.Forms.Button();
            this.btnTaskFactory = new System.Windows.Forms.Button();
            this.btnTaskAsync = new System.Windows.Forms.Button();
            this.labelTaskAsync = new System.Windows.Forms.Label();
            this.listBoxServer = new System.Windows.Forms.ListBox();
            this.listBoxClient = new System.Windows.Forms.ListBox();
            this.buttonStartServer = new System.Windows.Forms.Button();
            this.buttonStartSendToClient = new System.Windows.Forms.Button();
            this.buttonConnectAndReceiveMsg = new System.Windows.Forms.Button();
            this.buttonStartSendToServer = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonAPMAsyn
            // 
            this.buttonAPMAsyn.Location = new System.Drawing.Point(544, 12);
            this.buttonAPMAsyn.Name = "buttonAPMAsyn";
            this.buttonAPMAsyn.Size = new System.Drawing.Size(75, 23);
            this.buttonAPMAsyn.TabIndex = 0;
            this.buttonAPMAsyn.Text = "APMRequest";
            this.buttonAPMAsyn.UseVisualStyleBackColor = true;
            this.buttonAPMAsyn.Click += new System.EventHandler(this.buttonAPMAsyn_Click);
            // 
            // textBoxPage
            // 
            this.textBoxPage.Location = new System.Drawing.Point(12, 12);
            this.textBoxPage.Name = "textBoxPage";
            this.textBoxPage.Size = new System.Drawing.Size(524, 21);
            this.textBoxPage.TabIndex = 1;
            // 
            // buttonAutoReset
            // 
            this.buttonAutoReset.Location = new System.Drawing.Point(453, 39);
            this.buttonAutoReset.Name = "buttonAutoReset";
            this.buttonAutoReset.Size = new System.Drawing.Size(67, 23);
            this.buttonAutoReset.TabIndex = 2;
            this.buttonAutoReset.Text = "AutoReset";
            this.buttonAutoReset.UseVisualStyleBackColor = true;
            this.buttonAutoReset.Click += new System.EventHandler(this.buttonAutoReset_Click);
            // 
            // labelAutoReset
            // 
            this.labelAutoReset.AutoSize = true;
            this.labelAutoReset.Location = new System.Drawing.Point(12, 44);
            this.labelAutoReset.Name = "labelAutoReset";
            this.labelAutoReset.Size = new System.Drawing.Size(35, 12);
            this.labelAutoReset.TabIndex = 3;
            this.labelAutoReset.Text = "label";
            // 
            // buttonSet
            // 
            this.buttonSet.Location = new System.Drawing.Point(526, 39);
            this.buttonSet.Name = "buttonSet";
            this.buttonSet.Size = new System.Drawing.Size(45, 23);
            this.buttonSet.TabIndex = 4;
            this.buttonSet.Text = "Set";
            this.buttonSet.UseVisualStyleBackColor = true;
            this.buttonSet.Click += new System.EventHandler(this.buttonSet_Click);
            // 
            // labelManualReset
            // 
            this.labelManualReset.AutoSize = true;
            this.labelManualReset.Location = new System.Drawing.Point(226, 44);
            this.labelManualReset.Name = "labelManualReset";
            this.labelManualReset.Size = new System.Drawing.Size(41, 12);
            this.labelManualReset.TabIndex = 5;
            this.labelManualReset.Text = "label1";
            // 
            // buttonReset
            // 
            this.buttonReset.Location = new System.Drawing.Point(577, 39);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(45, 23);
            this.buttonReset.TabIndex = 6;
            this.buttonReset.Text = "Reset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // btnBackgroudWorker
            // 
            this.btnBackgroudWorker.Location = new System.Drawing.Point(228, 122);
            this.btnBackgroudWorker.Name = "btnBackgroudWorker";
            this.btnBackgroudWorker.Size = new System.Drawing.Size(105, 23);
            this.btnBackgroudWorker.TabIndex = 7;
            this.btnBackgroudWorker.Text = "Backgroudworker";
            this.btnBackgroudWorker.UseVisualStyleBackColor = true;
            this.btnBackgroudWorker.Click += new System.EventHandler(this.btnBackgroudWorker_Click);
            // 
            // labelBackGroudWorker
            // 
            this.labelBackGroudWorker.AutoSize = true;
            this.labelBackGroudWorker.Location = new System.Drawing.Point(21, 127);
            this.labelBackGroudWorker.Name = "labelBackGroudWorker";
            this.labelBackGroudWorker.Size = new System.Drawing.Size(95, 12);
            this.labelBackGroudWorker.TabIndex = 8;
            this.labelBackGroudWorker.Text = "BackGroudWorker";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(483, 78);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "协变逆变";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.buttonCovariant_Click);
            // 
            // btnThreadAbort
            // 
            this.btnThreadAbort.Location = new System.Drawing.Point(228, 84);
            this.btnThreadAbort.Name = "btnThreadAbort";
            this.btnThreadAbort.Size = new System.Drawing.Size(86, 23);
            this.btnThreadAbort.TabIndex = 10;
            this.btnThreadAbort.Text = "ThreadAbort";
            this.btnThreadAbort.UseVisualStyleBackColor = true;
            this.btnThreadAbort.Click += new System.EventHandler(this.btnThreadAbort_Click);
            // 
            // labelThreadAbort
            // 
            this.labelThreadAbort.AutoSize = true;
            this.labelThreadAbort.Location = new System.Drawing.Point(12, 89);
            this.labelThreadAbort.Name = "labelThreadAbort";
            this.labelThreadAbort.Size = new System.Drawing.Size(149, 12);
            this.labelThreadAbort.TabIndex = 11;
            this.labelThreadAbort.Text = "CoorperativeCancellation";
            // 
            // btnTask
            // 
            this.btnTask.Location = new System.Drawing.Point(73, 164);
            this.btnTask.Name = "btnTask";
            this.btnTask.Size = new System.Drawing.Size(75, 23);
            this.btnTask.TabIndex = 12;
            this.btnTask.Text = "Task";
            this.btnTask.UseVisualStyleBackColor = true;
            this.btnTask.Click += new System.EventHandler(this.btnTask_Click);
            // 
            // btnTaskThrow
            // 
            this.btnTaskThrow.Location = new System.Drawing.Point(249, 164);
            this.btnTaskThrow.Name = "btnTaskThrow";
            this.btnTaskThrow.Size = new System.Drawing.Size(75, 23);
            this.btnTaskThrow.TabIndex = 13;
            this.btnTaskThrow.Text = "TaskThrow";
            this.btnTaskThrow.UseVisualStyleBackColor = true;
            this.btnTaskThrow.Click += new System.EventHandler(this.btnTaskThrow_Click);
            // 
            // btnTaskFactory
            // 
            this.btnTaskFactory.Location = new System.Drawing.Point(396, 164);
            this.btnTaskFactory.Name = "btnTaskFactory";
            this.btnTaskFactory.Size = new System.Drawing.Size(81, 23);
            this.btnTaskFactory.TabIndex = 14;
            this.btnTaskFactory.Text = "TaskFactory";
            this.btnTaskFactory.UseVisualStyleBackColor = true;
            this.btnTaskFactory.Click += new System.EventHandler(this.btnTaskFactory_Click);
            // 
            // btnTaskAsync
            // 
            this.btnTaskAsync.Location = new System.Drawing.Point(125, 202);
            this.btnTaskAsync.Name = "btnTaskAsync";
            this.btnTaskAsync.Size = new System.Drawing.Size(75, 23);
            this.btnTaskAsync.TabIndex = 15;
            this.btnTaskAsync.Text = "TaskAsync";
            this.btnTaskAsync.UseVisualStyleBackColor = true;
            this.btnTaskAsync.Click += new System.EventHandler(this.btnTaskAsync_Click);
            // 
            // labelTaskAsync
            // 
            this.labelTaskAsync.AutoSize = true;
            this.labelTaskAsync.Location = new System.Drawing.Point(21, 207);
            this.labelTaskAsync.Name = "labelTaskAsync";
            this.labelTaskAsync.Size = new System.Drawing.Size(41, 12);
            this.labelTaskAsync.TabIndex = 16;
            this.labelTaskAsync.Text = "label2";
            // 
            // listBoxServer
            // 
            this.listBoxServer.FormattingEnabled = true;
            this.listBoxServer.ItemHeight = 12;
            this.listBoxServer.Location = new System.Drawing.Point(28, 242);
            this.listBoxServer.Name = "listBoxServer";
            this.listBoxServer.Size = new System.Drawing.Size(120, 88);
            this.listBoxServer.TabIndex = 17;
            // 
            // listBoxClient
            // 
            this.listBoxClient.FormattingEnabled = true;
            this.listBoxClient.ItemHeight = 12;
            this.listBoxClient.Location = new System.Drawing.Point(173, 242);
            this.listBoxClient.Name = "listBoxClient";
            this.listBoxClient.Size = new System.Drawing.Size(120, 88);
            this.listBoxClient.TabIndex = 18;
            // 
            // buttonStartServer
            // 
            this.buttonStartServer.Location = new System.Drawing.Point(308, 253);
            this.buttonStartServer.Name = "buttonStartServer";
            this.buttonStartServer.Size = new System.Drawing.Size(85, 23);
            this.buttonStartServer.TabIndex = 19;
            this.buttonStartServer.Text = "StartServer";
            this.buttonStartServer.UseVisualStyleBackColor = true;
            this.buttonStartServer.Click += new System.EventHandler(this.buttonStartServer_Click);
            // 
            // buttonStartSendToClient
            // 
            this.buttonStartSendToClient.Location = new System.Drawing.Point(483, 253);
            this.buttonStartSendToClient.Name = "buttonStartSendToClient";
            this.buttonStartSendToClient.Size = new System.Drawing.Size(123, 23);
            this.buttonStartSendToClient.TabIndex = 20;
            this.buttonStartSendToClient.Text = "StartSendToClient";
            this.buttonStartSendToClient.UseVisualStyleBackColor = true;
            this.buttonStartSendToClient.Click += new System.EventHandler(this.buttonStartSendToClient_Click);
            // 
            // buttonConnectAndReceiveMsg
            // 
            this.buttonConnectAndReceiveMsg.Location = new System.Drawing.Point(308, 295);
            this.buttonConnectAndReceiveMsg.Name = "buttonConnectAndReceiveMsg";
            this.buttonConnectAndReceiveMsg.Size = new System.Drawing.Size(169, 23);
            this.buttonConnectAndReceiveMsg.TabIndex = 21;
            this.buttonConnectAndReceiveMsg.Text = "ConnectAndReceiveMessage";
            this.buttonConnectAndReceiveMsg.UseVisualStyleBackColor = true;
            this.buttonConnectAndReceiveMsg.Click += new System.EventHandler(this.buttonConnectAndReceiveMsg_Click);
            // 
            // buttonStartSendToServer
            // 
            this.buttonStartSendToServer.Location = new System.Drawing.Point(485, 295);
            this.buttonStartSendToServer.Name = "buttonStartSendToServer";
            this.buttonStartSendToServer.Size = new System.Drawing.Size(121, 23);
            this.buttonStartSendToServer.TabIndex = 22;
            this.buttonStartSendToServer.Text = "StartSendToServer";
            this.buttonStartSendToServer.UseVisualStyleBackColor = true;
            this.buttonStartSendToServer.Click += new System.EventHandler(this.buttonStartSendToServer_Click);
            // 
            // FormTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(631, 443);
            this.Controls.Add(this.buttonStartSendToServer);
            this.Controls.Add(this.buttonConnectAndReceiveMsg);
            this.Controls.Add(this.buttonStartSendToClient);
            this.Controls.Add(this.buttonStartServer);
            this.Controls.Add(this.listBoxClient);
            this.Controls.Add(this.listBoxServer);
            this.Controls.Add(this.labelTaskAsync);
            this.Controls.Add(this.btnTaskAsync);
            this.Controls.Add(this.btnTaskFactory);
            this.Controls.Add(this.btnTaskThrow);
            this.Controls.Add(this.btnTask);
            this.Controls.Add(this.labelThreadAbort);
            this.Controls.Add(this.btnThreadAbort);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.labelBackGroudWorker);
            this.Controls.Add(this.btnBackgroudWorker);
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.labelManualReset);
            this.Controls.Add(this.buttonSet);
            this.Controls.Add(this.labelAutoReset);
            this.Controls.Add(this.buttonAutoReset);
            this.Controls.Add(this.textBoxPage);
            this.Controls.Add(this.buttonAPMAsyn);
            this.Name = "FormTest";
            this.Text = "测试";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonAPMAsyn;
        private System.Windows.Forms.TextBox textBoxPage;
        private System.Windows.Forms.Button buttonAutoReset;
        private System.Windows.Forms.Label labelAutoReset;
        private System.Windows.Forms.Button buttonSet;
        private System.Windows.Forms.Label labelManualReset;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Button btnBackgroudWorker;
        private System.Windows.Forms.Label labelBackGroudWorker;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnThreadAbort;
        private System.Windows.Forms.Label labelThreadAbort;
        private System.Windows.Forms.Button btnTask;
        private System.Windows.Forms.Button btnTaskThrow;
        private System.Windows.Forms.Button btnTaskFactory;
        private System.Windows.Forms.Button btnTaskAsync;
        private System.Windows.Forms.Label labelTaskAsync;
        private System.Windows.Forms.ListBox listBoxServer;
        private System.Windows.Forms.ListBox listBoxClient;
        private System.Windows.Forms.Button buttonStartServer;
        private System.Windows.Forms.Button buttonStartSendToClient;
        private System.Windows.Forms.Button buttonConnectAndReceiveMsg;
        private System.Windows.Forms.Button buttonStartSendToServer;
    }
}


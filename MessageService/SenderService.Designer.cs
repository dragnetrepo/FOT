namespace MessageService
{
    partial class SenderService
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.TimerProcess = new System.Timers.Timer();
            ((System.ComponentModel.ISupportInitialize)(this.TimerProcess)).BeginInit();
            // 
            // TimerProcess
            // 
            this.TimerProcess.AutoReset = false;
            this.TimerProcess.Interval = 20000D;
            this.TimerProcess.Elapsed += new System.Timers.ElapsedEventHandler(this.TimerProcess_Elapsed);
            // 
            // SenderService
            // 
            this.ServiceName = "MessageService";
            ((System.ComponentModel.ISupportInitialize)(this.TimerProcess)).EndInit();

        }

        #endregion

        private System.Timers.Timer TimerProcess;
    }
}

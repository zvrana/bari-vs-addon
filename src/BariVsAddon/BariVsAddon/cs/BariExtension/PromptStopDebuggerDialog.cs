using System.Drawing;
using System.Windows.Forms;

namespace Microsoft.BariVsPackage.BariExtension
{
    public partial class PromptStopDebuggerDialog : Form
    {
        public PromptStopDebuggerDialog()
        {
            InitializeComponent();

            iconBox.Image = Bitmap.FromHicon(SystemIcons.Question.Handle);
        }

        public PromptStopDebuggerResult Result { get; private set; }

        private void CancelButtonClick(object sender, System.EventArgs e)
        {
            Result = PromptStopDebuggerResult.Cancel;
            Close();
        }

        private void StopDebuggingButtonClick(object sender, System.EventArgs e)
        {
            Result = PromptStopDebuggerResult.StopDebuggerAndExecuteAction;
            Close();
        }

        private void KeepDebuggingButtonClick(object sender, System.EventArgs e)
        {
            Result = PromptStopDebuggerResult.KeepDebuggingAndExecuteAction;
            Close();
        }
    }
}

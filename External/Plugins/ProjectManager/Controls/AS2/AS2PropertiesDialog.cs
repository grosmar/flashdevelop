using System.IO;
using System.Windows.Forms;
using PluginCore.Localization;
using PluginCore.Managers;
using ProjectManager.Projects.AS2;

namespace ProjectManager.Controls.AS2
{
    public partial class AS2PropertiesDialog : PropertiesDialog
    {
        // For designer
        public AS2PropertiesDialog() 
        { 
            InitializeComponent();
            InitializeLocalization();
        }

        private AS2Project Project
        {
            get
            {

                return (AS2Project) BaseProject;
            }
        }

        protected override void BuildDisplay()
        {
            base.BuildDisplay();
            injectionCheckBox.Checked = Project.UsesInjection;
            inputSwfBox.Text = Project.InputPath;
            AssetsChanged = false;
        }

        private void InitializeLocalization()
        {
            this.infoLabel.Text = TextHelper.GetString("Info.CodeInjection");
            this.injectionTab.Text = TextHelper.GetString("Info.Injection");
            this.injectionCheckBox.Text = TextHelper.GetString("Info.UseCodeInjection");
            this.inputBrowseButton.Text = TextHelper.GetString("Label.Browse");
            this.inputFileLabel.Text = TextHelper.GetString("Info.InputSWF");
        }

        private void inputSwfBox_TextChanged(object sender, System.EventArgs e)
        {
            ClasspathsChanged = true;
            Modified();
        }

        private void injectionCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            if (injectionCheckBox.Checked && Project.LibraryAssets.Count > 0)
            {
                string msg = TextHelper.GetString("Info.InjectionConfirmation");
                string title = " " + TextHelper.GetString("FlashDevelop.Title.ConfirmDialog");

                DialogResult result = MessageBox.Show(this, msg, title,
                    MessageBoxButtons.OKCancel);

                if (result == DialogResult.Cancel)
                {
                    injectionCheckBox.Checked = false;
                    return;
                }
            }

            Modified();
            bool inject = injectionCheckBox.Checked;
            inputSwfBox.Enabled = inject;
            inputBrowseButton.Enabled = inject;
            widthTextBox.Enabled = !inject;
            heightTextBox.Enabled = !inject;
            colorTextBox.Enabled = !inject;
            fpsTextBox.Enabled = !inject;
        }

        private void inputBrowseButton_Click(object sender, System.EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = TextHelper.GetString("Info.FlashMovieFilter");
            dialog.InitialDirectory = Project.Directory;

            // try pre-setting the current input path
            try
            {
                string path = Project.GetAbsolutePath(inputSwfBox.Text);
                if (File.Exists(path)) dialog.FileName = path;
            }
            catch { }

            if (dialog.ShowDialog(this) == DialogResult.OK)
                inputSwfBox.Text = Project.GetRelativePath(dialog.FileName);
        }

        protected override bool Apply()
        {
            if (injectionCheckBox.Checked && inputSwfBox.Text.Length == 0)
            {
                string msg = TextHelper.GetString("Info.SpecifyInputSwfForInjection");
                ErrorManager.ShowInfo(msg);
            }
            else if (injectionCheckBox.Checked)
            {
                Project.InputPath = inputSwfBox.Text;

                // unassign any existing assets - you've been warned already
                if (Project.LibraryAssets.Count > 0)
                {
                    Project.LibraryAssets.Clear();
                    AssetsChanged = true;
                }
            }
            else
                Project.InputPath = "";

            return base.Apply();
        }
    }
}


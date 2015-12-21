using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using PluginCore;
using PluginCore.Controls;
using PluginCore.Localization;
using PluginCore.Managers;

namespace ProjectManager.Controls
{
    public partial class AddBuildConfigurationDialog : SmartForm
    {
        private IMultiConfigProject baseProject;

        public IMultiConfigProject BaseProject
        {
            get { return baseProject; }
            set
            {
                baseProject = value;
                configsCombo.Items.Clear();

                if (baseProject == null || baseProject.Configurations == null || baseProject.Configurations.Count == 0)
                {
                    okButton.Enabled = false;
                }
                else
                {
                    okButton.Enabled = true;
                    foreach (string config in baseProject.Configurations.Keys)
                        configsCombo.Items.Add(config);

                    if (!string.IsNullOrEmpty(sourceConfiguration) &&
                        baseProject.Configurations.ContainsKey(sourceConfiguration))
                    {
                        configsCombo.SelectedText = sourceConfiguration;
                    }
                    else
                    {
                        configsCombo.SelectedIndex = 0;
                    }
                }
            }
        }

        public string ConfigurationName
        {
            get { return nameTextBox.Text.Trim(); }
            set { nameTextBox.Text = value != null ? value.Trim() : value; }
        }

        private string sourceConfiguration;

        public string SourceConfiguration
        {
            get { return configsCombo.SelectedText; }
            set
            {
                sourceConfiguration = value;
                if (configsCombo.Items.Contains(value))
                {
                    configsCombo.SelectedText = value;
                }
            }
        }

        public AddBuildConfigurationDialog()
        {
            InitializeComponent();
            InitializeLocalization();
        }

        private void InitializeLocalization()
        {
            this.Text = TextHelper.GetString("Title.AddBuildConfigurations");
            this.nameLabel.Text = TextHelper.GetString("Label.Name");
            this.sourceLabel.Text = TextHelper.GetString("Label.SourceConfiguration");
            this.okButton.Text = TextHelper.GetString("Label.OK");
            this.cancelButton.Text = TextHelper.GetString("Label.Cancel");
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.AutoValidate = AutoValidate.Disable;
            }
            return base.ProcessDialogKey(keyData);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!this.nameTextBox.Focused) this.ValidateChildren(ValidationConstraints.ImmediateChildren);
            base.OnClosing(e);
        }

        private void NameTextBox_Validating(object sender, CancelEventArgs e)
        {
            string configName = ConfigurationName;
            if (configName.Length == 0 || configName.IndexOfAny(Path.GetInvalidFileNameChars()) > -1 ||
                baseProject.Configurations.ContainsKey(configName))
            {
                e.Cancel = true;
                ErrorManager.ShowWarning(TextHelper.GetString("Alert.Message.InvalidConfigurationName"), null);
            }
        }
    }
}

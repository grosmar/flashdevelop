using System;
using System.IO;
using System.Windows.Forms;
using PluginCore;
using PluginCore.Controls;
using PluginCore.Localization;
using PluginCore.Managers;

namespace ProjectManager.Controls
{
    public partial class BuildConfigurationsDialog : SmartForm
    {
        private IMultiConfigProject project;

        public IMultiConfigProject Project
        {
            get { return this.project; }
            set
            {
                this.project = value;

                this.ReloadProjectConfigurations(false);

                this.addButton.Enabled = value != null;
            }
        }

        public BuildConfigurationsDialog()
        {
            InitializeComponent();
            InitializeLocalization();
        }

        private void InitializeLocalization()
        {
            this.Text = TextHelper.GetString("Title.ManageBuildConfigurations");
            this.closeButton.Text = TextHelper.GetString("FlashDevelop.Label.Close");
            this.addButton.Text = TextHelper.GetString("Label.Add");
            this.renameButton.Text = TextHelper.GetString("Label.Rename");
            this.removeButton.Text = TextHelper.GetString("Label.Remove");
            this.nameLabel.Text = TextHelper.GetString("Label.Name");
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            using (AddBuildConfigurationDialog addDialog = new AddBuildConfigurationDialog())
            {
                addDialog.BaseProject = this.project;
                if (addDialog.ShowDialog(this) == DialogResult.OK)
                {
                    this.project.AddConfiguration(addDialog.ConfigurationName, addDialog.SourceConfiguration);

                    this.ReloadProjectConfigurations(true);
                }
            }
        }

        private void ConfigsList_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (e.Label == null) return;
            string newConfigName = e.Label.Trim();
            string oldConfigName = this.configsList.Items[e.Item].Text;

            e.CancelEdit = true;

            if (newConfigName == oldConfigName) return;

            if (newConfigName.Length == 0 || newConfigName.IndexOfAny(Path.GetInvalidFileNameChars()) > -1 ||
                project.Configurations.ContainsKey(newConfigName))
            {
                ErrorManager.ShowWarning(TextHelper.GetString("Alert.Message.InvalidConfigurationName"), null);

                return;
            }

            this.project.RenameConfiguration(oldConfigName, newConfigName);

            this.configsList.Items[e.Item].Text = newConfigName;

            this.ReloadProjectConfigurations(true);
        }

        private void ConfigsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.renameButton.Enabled = this.configsList.SelectedItems.Count > 0;
            this.removeButton.Enabled = this.renameButton.Enabled && this.configsList.Items.Count > 1;
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RenameButton_Click(object sender, EventArgs e)
        {
            this.configsList.SelectedItems[0].BeginEdit();
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            ListViewItem item = this.configsList.SelectedItems[0];
            this.project.RemoveConfiguration(item.Text);

            this.configsList.Items.Remove(item);

            this.ReloadProjectConfigurations(true);

            if (this.configsList.Items.Count == 1)
            {
                this.removeButton.Enabled = false;
            }
        }

        private void ReloadProjectConfigurations(bool keepFocus)
        {
            configsList.Items.Clear();

            if (project == null || project.Configurations == null) return;

            string selectedItem = keepFocus && configsList.SelectedItems.Count > 0 ? configsList.SelectedItems[0].Text : null;

            foreach (string configName in project.Configurations.Keys)
            {
                ListViewItem item = configsList.Items.Add(configName);

                if (configName == selectedItem)
                {
                    item.Selected = true;
                    item.EnsureVisible();
                }
            }
        }
    }
}

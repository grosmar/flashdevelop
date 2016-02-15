namespace LoomContext.Projects
{
    public partial class LoomPropertiesDialog : ProjectManager.Controls.PropertiesDialog
    {
        // For Designer
        public LoomPropertiesDialog() { InitializeComponent(); }

        private LoomProject Project { get { return (LoomProject)BaseProject; } }
    }
}


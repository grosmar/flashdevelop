using ProjectManager.Projects.AS3;

namespace ProjectManager.Controls.AS3
{
    public partial class AS3PropertiesDialog : PropertiesDialog
    {
        // For Designer
        public AS3PropertiesDialog() { InitializeComponent(); }

        private AS3Project Project { get { return (AS3Project)BaseProject; } }
    }
}


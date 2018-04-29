using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace MineS
{
    public sealed partial class AchDital : UserControl
    {
        public Achievements Local { get; set; }
        public AchDital()
        {

            this.InitializeComponent();
        }
    }
}

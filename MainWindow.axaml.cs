using Avalonia.Controls;
using PortraitMaker2.Vms;

namespace PortraitMaker2;
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        DataContext = new MainVm();
    }
}
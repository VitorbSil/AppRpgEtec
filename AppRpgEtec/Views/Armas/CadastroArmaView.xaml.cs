using AppRpgEtec.ViewModels.Armas;

namespace AppRpgEtec.Views.Armas;

public partial class CadastroArmaView : ContentPage
{
    private CadastroArmaViewModel cadViewModel;

    public CadastroArmaView()
	{
		InitializeComponent();

        cadViewModel = new CadastroArmaViewModel();
        BindingContext = cadViewModel;
        Title = "Nova Arma";
    }
}
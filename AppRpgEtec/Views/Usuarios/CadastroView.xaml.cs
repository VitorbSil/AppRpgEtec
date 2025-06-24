using AppRpgEtec.ViewModels.Usuarios;

namespace AppRpgEtec.Views.Usuarios;

public partial class CadastroView : ContentPage
{
    UsuarioViewModel _usuarioViewModel;
    public CadastroView()
	{
		InitializeComponent();

        _usuarioViewModel = new UsuarioViewModel();
        BindingContext = _usuarioViewModel;
    }
}
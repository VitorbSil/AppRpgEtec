using AppRpgEtec.Models;
using AppRpgEtec.Services.Armas;
using AppRpgEtec.Services.Personagens;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AppRpgEtec.ViewModels.Armas
{
    [QueryProperty("ArmaSelecionadaId", "aId")]
    public class CadastroArmaViewModel : BaseViewModel
    {
        private ArmaService aService;
        private PersonagemService pService;

        public CadastroArmaViewModel()
        {
            string token = Preferences.Get("UsuarioToken", string.Empty);
            aService = new ArmaService(token);
            pService = new PersonagemService(token);

            ObterPersonagens();

            SalvarCommand = new Command(async () => await SalvarArma());
            //SalvarCommand = new Command(SalvarArma);//Void
        }

        public ICommand SalvarCommand { get; set; }



        #region Atributos_Propriedades

        private int id;
        private string nome;
        private int dano;
        private int personagemId;

        public int Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public string Nome
        {
            get => nome;
            set
            {
                nome = value;
                OnPropertyChanged(nameof(Nome));
            }
        }
        public int Dano
        {
            get => dano;
            set
            {
                dano = value;
                OnPropertyChanged(nameof(Dano));

            }
        }
        public int PersonagemId
        {
            get => personagemId;
            set
            {
                personagemId = value;
                OnPropertyChanged(nameof(PersonagemId));
            }
        }

        private Personagem personagemSelecionado;
        public Personagem PersonagemSelecionado
        {
            get { return personagemSelecionado; }
            set
            {
                if (value != null)
                {
                    personagemSelecionado = value;
                    OnPropertyChanged(nameof(PersonagemSelecionado));
                }
            }
        }

        private string armaSelecionadaId;//CTRL + R,E
        public string ArmaSelecionadaId
        {
            set
            {
                if (value != null)
                {
                    armaSelecionadaId = Uri.UnescapeDataString(value);
                    CarregarArma();
                }
            }
        }

        public ObservableCollection<Personagem> Personagens { get; set; }

        #endregion

        #region Metodos



        public async void ObterPersonagens()
        {
            try
            {
                Personagens = await pService.GetPersonagensAsync();
                OnPropertyChanged(nameof(Personagens));
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops", ex.Message, "Ok");
            }
        }


        //Programe o método de salvar arma aqui
        public async Task SalvarArma()
        {
            try
            {
                Arma model = new Arma()
                {
                    Id = this.id,
                    Nome = this.nome,
                    Dano = this.dano,
                    PersonagemId = this.personagemSelecionado.Id
                };

                if (model.Id == 0)
                    await aService.PostArmaAsync(model);
                else
                    await aService.PutArmaAsync(model);

                await Application.Current.MainPage.DisplayAlert("Mensagem", "Dados salvo com sucesso", "Ok");

                await Shell.Current.GoToAsync("..");
            }
            catch (System.Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ops!", ex.Message, "Ok");
            }
        }


        public async void CarregarArma()
        {
            try
            {
                Arma model = await
                    aService.GetArmaAsync(int.Parse(armaSelecionadaId));

                this.Nome = model.Nome;
                this.Dano = model.Dano;
                this.Id = model.Id;
                this.PersonagemId = model.PersonagemId;

                this.PersonagemSelecionado =
                    Personagens.FirstOrDefault(x => x.Id == model.PersonagemId);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("ops", ex.Message, "Ok");
            }
        }

        #endregion



    }
}

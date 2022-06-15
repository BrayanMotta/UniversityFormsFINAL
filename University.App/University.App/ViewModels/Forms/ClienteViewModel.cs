using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using University.App.DTOs;
using University.App.Views.Forms;
using Xamarin.Forms;

namespace University.App.ViewModels.Forms
{
    public class ClienteViewModel : BaseViewModel
    {
        #region Attributes
        private string _firstname;
        private string _lastname;
        private string _telephone;
        private ObservableCollection<ClienteItemViewModel> _cliente;
        private bool _isRefreshing;
        #endregion

        #region Properties
        public string FirstName
        {
            get { return _firstname; }
            set { this.SetValue(ref _firstname, value); }
        }

        public string LastName
        {
            get { return _lastname; }
            set { this.SetValue(ref _lastname, value); }
        }

        public string Telephone
        {
            get { return _telephone; }
            set { this.SetValue(ref _telephone, value); }
        }

        public ObservableCollection<ClienteItemViewModel> Cliente
        {
            get { return _cliente; }
            set { this.SetValue(ref _cliente, value); }
        }

        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { this.SetValue(ref _isRefreshing, value); }
        }
        #endregion

        #region Methods
        async void GetClientes()
        {
            this.IsRefreshing = true;
            
            var url = "https://62a286bbcd2e8da9b00913a9.mockapi.io/api/Clientes";
            var result = string.Empty;

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                result = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var cliente = JsonConvert.DeserializeObject<ObservableCollection<ClienteItemViewModel>>(result);

                    this.Cliente = cliente;
                }
            }
            this.IsRefreshing = false;
        }

        async void NuevoCliente()
        {
            //TODO: Cambiar a RegisterCommmand
            await Application.Current.MainPage.Navigation.PushAsync(new NuevoClientePage());
        }

        async void OnItemClick()
        {
            await Application.Current.MainPage.DisplayAlert("Notify", $"Registered Client", "Cancel");

        }
        #endregion

        #region Commands
        public Command OnItemClickCommand { get; set; }
        public Command RefreshCommand { get; set; }
        public Command NuevoClienteCommand { get; set; }
        #endregion

        public ClienteViewModel()
        {
            this.OnItemClickCommand = new Command(OnItemClick);
            this.NuevoClienteCommand = new Command(NuevoCliente);
            this.RefreshCommand = new Command(GetClientes);
            this.RefreshCommand.Execute(null);
        }
    }
}

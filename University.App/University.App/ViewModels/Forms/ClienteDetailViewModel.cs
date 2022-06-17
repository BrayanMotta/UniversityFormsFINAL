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
    public class ClienteDetailViewModel : BaseViewModel
    {
        #region Attributes
        private string _placa;
        private string _modelo;
        private string _color;
        private string _marca;
        private ClienteDTO _cliente;
        private ObservableCollection<VehiculoDTO> _vehiculo;
        private bool _isRefreshing;
        #endregion

        #region Properties
        public string Placa
        {
            get { return _placa; }
            set { this.SetValue(ref _placa, value); }
        }

        public string Marca
        {
            get { return _marca; }
            set { this.SetValue(ref _marca, value); }
        }

        public string Color
        {
            get { return _color; }
            set { this.SetValue(ref _color, value); }
        }
        public string Modelo
        {
            get { return _modelo; }
            set { this.SetValue(ref _modelo, value); }
        }
        public ClienteDTO Cliente
        {
            get { return _cliente; }
            set { this.SetValue(ref _cliente, value); }
        }
        public ObservableCollection<VehiculoDTO> Vehiculo
        {
            get { return _vehiculo; }
            set { this.SetValue(ref _vehiculo, value); }
        }

        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { this.SetValue(ref _isRefreshing, value); }
        }
        #endregion


        public ClienteDetailViewModel(ClienteDTO cliente)
        {
            this.Cliente = cliente;
            this.RefreshCommand = new Command(GetVehiculo);
            this.RefreshCommand.Execute(null);
            this.OnItemClickCommand = new Command(OnItemClick);
            this.NuevoVehiculoCommand = new Command(NuevoVehiculo);

        }

        public ClienteDetailViewModel()
        {
            this.OnItemClickCommand = new Command(OnItemClick);
        }



        async void GetVehiculo()
        {
            this.IsRefreshing = true;
            
            var url = "https://62a286bbcd2e8da9b00913a9.mockapi.io/api/Vehiculos";
            var result = string.Empty;

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                result = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var vehiculo = JsonConvert.DeserializeObject<ObservableCollection<VehiculoItemViewModel>>(result);
                    var vehiculoFilter = vehiculo.Where(x => x.IdCliente == _cliente.ID).ToList();
                    this.Vehiculo = new ObservableCollection<VehiculoDTO>(vehiculoFilter);
                }
            }
            this.IsRefreshing = false;
        }

        async void NuevoVehiculo()
        {
            
            await Application.Current.MainPage.Navigation.PushAsync(new NuevoVehiculoPage());
            
        }

        async void OnItemClick()
        {
            await Application.Current.MainPage.DisplayAlert("Notify", $"Registered Vehicle", "Cancel");
            
        }

        public Command RefreshCommand { get; set; }
        public Command OnItemClickCommand { get; set; }
        public Command NuevoVehiculoCommand { get; set; }


    }
}

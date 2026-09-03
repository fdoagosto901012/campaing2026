using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using Cajas.MVVM.Models;
using Cajas.MVVM.ViewModels;
using Cajas.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cajas.MVVM.ViewModels.cards
{
    public partial class CardViewModel : BaseViewModel
    {

        private readonly iAwsS3 _AwsS3 = new AwsS3();

        [ObservableProperty]
        CardResponse _data = null;
        [ObservableProperty]
        bool _existcar = false;
        [ObservableProperty]
        string _gefet = "";
        [ObservableProperty]
        string _name = "";
        [ObservableProperty]
        bool _active = true;
        [ObservableProperty]
        string _type = "";
        [ObservableProperty]
        string _status = "";
        [ObservableProperty]
        string _ingreso = null;
        [ObservableProperty]
        EmplacamientoDTO _car = null;
        [ObservableProperty]
        amazonPicture _image;

        [ObservableProperty]
        ImageSource _imgs;


        [ObservableProperty]
        string _src;


        public CardViewModel()
        {

        }

        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData != null)
            {
                if (navigationData is CardResponse)
                {
                    this.Data = (CardResponse)navigationData;


                    if (this.Data.Operator != null)
                    {
                        this.Type = "Operador";
                        this.Active = true;
                        this.Name = this.Data.Operator.firstName + " " + this.Data.Operator.lastNameF + " " + this.Data.Operator.lastNameM;
                        this.Gefet = this.Data.Operator.partnerReference.Replace("OP-", "");
                        this.Ingreso = this.Data.Operator.FECHAING?.ToString("dd/MMMM/yyyy");

                        this.Active = this.Data.Operator.active;
                        this.Status = this.Active == true ? "ACTIVO" : "BAJA";

                        amazonPicture IMG = this.Data.Operator.amazonPictures.Where(x => x.pictureTypeId == 2).FirstOrDefault();
                        string _uri = _AwsS3.getImage(IMG.bucket, IMG.key);

                        // Obtenemos la imagen.
                        this.Imgs = new UriImageSource
                        {
                            Uri = new Uri(_uri),
                            CacheValidity = new TimeSpan(10, 0, 0, 0)
                        };

                    }
                    else if (this.Data.Partner != null)
                    {
                        this.Type = "Socio";
                        this.Active = true;
                        this.Name = this.Data.Partner.firstName + " " + this.Data.Partner.lastNameF + " " + this.Data.Partner.lastNameM;
                        this.Gefet = this.Data.Partner.partnerReference;
                        this.Active = (bool)this.Data.Partner?.active;
                        this.Status = this.Active == true ? "ACTIVO" : "BAJA";

                        // Obtenemos la imagen.
                        amazonPicture IMG = this.Data.Partner.amazonPictures.Where(x => x.pictureTypeId == 2).FirstOrDefault();
                        string _uri = _AwsS3.getImage(IMG.bucket, IMG.key);

                        // Obtenemos la imagen.
                        this.Imgs = new UriImageSource
                        {
                            Uri = new Uri(_uri),
                            CacheValidity = new TimeSpan(10, 0, 0, 0)
                        };


                        try
                        {
                            this.Ingreso = this.Data.Partner.fechaingreso?.ToString("dd/MMMM/yyyy");
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                    }

                    if (Data.Car != null)
                    {
                        this.Existcar = true;
                        this.Car = Data.Car;
                    }
                }
            }
            return base.InitializeAsync(navigationData);
        }
    }
}

using EHW_M;
using EHWM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace EHWM.ViewModel {

    public class ArikujtNavigationParameters {
        public List<Artikulli> Artikujt { get; set; }
    }
    public class ArtikujtViewModel : BaseViewModel {

        public ObservableCollection<Artikulli> Artikujt { get; set; }

        private ObservableCollection<Artikulli> _SearchedArtikujt;
        public ObservableCollection<Artikulli> SearchedArtikujt {
            get { return _SearchedArtikujt; }
            set { SetProperty(ref _SearchedArtikujt, value); }
        }
        private float _totali;
        public float Totali {
            get { return _totali; } set {  SetProperty(ref _totali, value);}
        }
        public ArtikujtViewModel(ArikujtNavigationParameters arikujtNavigationParameters) {
            Artikujt = new ObservableCollection<Artikulli>(arikujtNavigationParameters.Artikujt);
            SearchedArtikujt = new ObservableCollection<Artikulli>();
            Totali = (float)Artikujt.Sum(x => x.Sasia);
        }


    }
}

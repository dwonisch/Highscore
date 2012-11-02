using GalaSoft.MvvmLight;

namespace HighScore.ViewModel {
    public abstract class SaveableViewModel : ViewModelBase {
        public abstract void Save();
    }
}

namespace GBReaderAuquierC.Presenter.ViewModel
{
    public record PageViewModel(string Text, int Num, IList<ChoiceViewModel> Choices);
}
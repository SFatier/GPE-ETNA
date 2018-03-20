namespace IHM.ViewModel
{
    public interface IPageViewModel
    {
        string Name { get; }

        void LoadAction();
    }
}
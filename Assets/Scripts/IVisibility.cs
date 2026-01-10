public interface IVisibility
{
    bool IsVisibleOnStart { get; }
    void MakeVisible();
    void MakeInvisible();
}
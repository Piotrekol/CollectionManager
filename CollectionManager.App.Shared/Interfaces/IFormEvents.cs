namespace CollectionManager.App.Shared.Interfaces;

public interface IFormEvents
{
    event EventHandler FormClosed;
    event EventHandler FormClosing;
    void EmitFormClosing();
    void EmitFormClosed();
}
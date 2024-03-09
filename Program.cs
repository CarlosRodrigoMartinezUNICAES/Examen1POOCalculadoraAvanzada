namespace ParcialCalculadoraFunciones
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Gtk.Application.Init(); // Inicializa la aplicaci�n GTK#

            // Crea una instancia de la clase CalculadoraGUI
            CalculadoraGUI ventanaCalculadora = new CalculadoraGUI();
            ventanaCalculadora.Show();
            Gtk.Application.Run();
        }
    }
}

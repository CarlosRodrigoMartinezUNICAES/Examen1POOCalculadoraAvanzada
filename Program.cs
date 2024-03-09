namespace ParcialCalculadoraFunciones
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Gtk.Application.Init(); // Inicializa la aplicación GTK#

            // Crea una instancia de la clase CalculadoraGUI
            CalculadoraGUI ventanaCalculadora = new CalculadoraGUI();
            ventanaCalculadora.Show();
            Gtk.Application.Run();
        }
    }
}

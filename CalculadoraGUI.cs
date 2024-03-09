using Gtk;
using Button = Gtk.Button;
using Label = Gtk.Label;

public class CalculadoraGUI : Window
{
    // Declaración de cajas de entrada para las ecuaciones y otros elementos de la interfaz
    public Entry entry_eq1;
    public Entry entry_eq2;
    public Entry entry_eq3;

    public Entry entry_x_value;
    public Entry entry_result;

    public Entry entry_xmin;
    public Entry entry_xmax;
    public Entry entry_ymin;
    public Entry entry_ymax;
    public Entry entry_graduation;

    // Variables para almacenar los límites y graduación
    public double x_min;
    public double x_max;
    public double y_min;
    public double y_max;
    public double graduation;

    // Etiqueta para mostrar información al usuario
    public Label label_info;

    // Constructor de la clase
    public CalculadoraGUI() : base("GUI Calculadora")
    {
        // Configuración de la ventana principal
        this.Move(100, 100);
        this.SetDefaultSize(400, 300);
        this.DeleteEvent += new DeleteEventHandler(OnMyWindowDelete);
        this.Resizable = false;
        
        // Creación de la disposición de la interfaz
        VBox vbox = new VBox(false, 2);

        // Creación de una tabla de 6 por 10
        Table tableLayout = new Table(6, 10, false);

        // Creación de una caja de etiquetas y entrada para la función 1
        Label label_y1 = new Label("Funcion 1 :   y   =");
        tableLayout.Attach(label_y1, 0, 1, 0, 1);

        entry_eq1 = new Entry("");
        tableLayout.Attach(entry_eq1, 1, 2, 0, 1);
        entry_eq1.Activated += new EventHandler(button_trace_click);

        // Creación de una caja de etiquetas y entrada para la función 2
        Label label_y2 = new Label("Funcion 2 :   y   =");
        tableLayout.Attach(label_y2, 0, 1, 1, 2);

        entry_eq2 = new Entry("");
        tableLayout.Attach(entry_eq2, 1, 2, 1, 2);
        entry_eq2.Activated += new EventHandler(button_trace_click);

        // Creación de una caja de etiquetas y entrada para la función 3  
        Label label_y3 = new Label("Funcion 3 :   y   =");
        tableLayout.Attach(label_y3, 0, 1, 2, 3);

        entry_eq3 = new Entry("");
        tableLayout.Attach(entry_eq3, 1, 2, 2, 3);
        entry_eq3.Activated += new EventHandler(button_trace_click);

        // Creación de un botón para trazar las funciones
        Button button_trace = new Button("Graficar funcion");
        button_trace.Clicked += new EventHandler(button_trace_click);
        tableLayout.Attach(button_trace, 3, 4, 0, 3);


        // Creación de una caja de etiquetas y entrada para el valor de x
        Label label_x_value = new Label("Valor de x para funcion 1:  ");
        tableLayout.Attach(label_x_value, 0, 1, 3, 4);

        entry_x_value = new Entry("");
        tableLayout.Attach(entry_x_value, 1, 2, 3, 4);
        entry_x_value.Activated += new EventHandler(button_result_click);

        // Creación de un botón para calcular f(x)
        Button button_result = new Button("Respuesta f(x) = ");
        button_result.Clicked += new EventHandler(button_result_click);
        tableLayout.Attach(button_result, 2, 3, 3, 4);

        entry_result = new Entry("");
        entry_result.IsEditable = false;
        tableLayout.Attach(entry_result, 3, 4, 3, 4);

        // RANGO
        Label label_range = new Label("");
        tableLayout.Attach(label_range, 0, 1, 4, 5);

        //xmin
        Label label_xmin = new Label("Xmin   =");
        tableLayout.Attach(label_xmin, 0, 1, 5, 6);

        entry_xmin = new Entry("-10");
        tableLayout.Attach(entry_xmin, 1, 2, 5, 6);
        entry_xmin.Activated += new EventHandler(button_trace_click);

        //xmax
        Label label_xmax = new Label("Xmax   =");
        tableLayout.Attach(label_xmax, 0, 1, 6, 7);

        entry_xmax = new Entry("10");
        tableLayout.Attach(entry_xmax, 1, 2, 6, 7);
        entry_xmax.Activated += new EventHandler(button_trace_click);

        //ymin
        Label label_ymin = new Label("Ymin   =");
        tableLayout.Attach(label_ymin, 0, 1, 7, 8);

        entry_ymin = new Entry("-10");
        tableLayout.Attach(entry_ymin, 1, 2, 7, 8);
        entry_ymin.Activated += new EventHandler(button_trace_click);

        //ymax
        Label label_ymax = new Label("Ymax   =");
        tableLayout.Attach(label_ymax, 0, 1, 8, 9);

        entry_ymax = new Entry("10");
        tableLayout.Attach(entry_ymax, 1, 2, 8, 9);
        entry_ymax.Activated += new EventHandler(button_trace_click);

        //escala
        Label label_graduation = new Label("Escala =");
        tableLayout.Attach(label_graduation, 0, 1, 9, 10);

        entry_graduation = new Entry("1");
        tableLayout.Attach(entry_graduation, 1, 2, 9, 10);
        entry_graduation.Activated += new EventHandler(button_trace_click);

        //resetear rangos
        Button button_initialize_range = new Button("Resetear rangos");
        button_initialize_range.Clicked += new EventHandler(button_initialize_range_click);
        tableLayout.Attach(button_initialize_range, 3, 4, 8, 9);

        //resetear todo
        Button button_initialize_all = new Button("Resetear todo (funciones y rangos)");
        button_initialize_all.Clicked += new EventHandler(button_initialize_all_click);
        tableLayout.Attach(button_initialize_all, 3, 4, 7, 8);

        // info
        label_info = new Label("Introduzca una funcion");
        tableLayout.Attach(label_info, 3, 4, 5, 6);

        vbox.PackStart(tableLayout, true, true, 0);

        tableLayout.ShowAll();
        this.Add(vbox);
        this.ShowAll();

        // Inicialización de valores predeterminados para los límites
        x_min = -10f;
        x_max = 10f;
        y_min = -10f;
        y_max = 10f;
        graduation = 1f;
    }

    // Manejador de eventos para el botón que calcula f(x)
    private void button_result_click(object o, EventArgs args)
    {
        try
        {
            // Obtener la ecuación y el valor de x desde la interfaz gráfica
            string equation = this.entry_eq1.Text;
            string string_x = entry_x_value.Text;

            // Convertir la cadena de x a un valor decimal
            double x;
            if (equation != "")
            {
                if (string_x != "")
                {
                    decimal decimal_x = Convert.ToDecimal(string_x);
                    x = (double)decimal_x;
                }
                else
                {
                    x = 0f;
                }

                // Crear una instancia de la clase operation para realizar operaciones
                operacion op = new operacion(equation);

                // Corregir la ecuación si es necesario
                op.correct();
                // Calcular el resultado de f(x) y mostrarlo en la interfaz
                double res = op.compute(x);
                this.entry_result.Text = Convert.ToString(res);

                double res_verif = res;
                if (Double.IsNaN(res_verif))
                {
                    this.entry_result.Text = "NaN";
                    label_info.Text = "Error!";
                }
                else
                {
                    label_info.Text = "Introduzca una funcion";
                }
            }
        }
        catch (Exception ex)
        {
            // Manejar excepciones y mostrar mensajes de error en la interfaz
            label_info.Text = "Ocurrio una excepcion";
            this.entry_result.Text = "";
        }
    }

    // Manejador de eventos para el botón que traza las funciones
    private void button_trace_click(object o, EventArgs args)
    {
        try
        {
            // Verificar que las entradas necesarias no estén vacías
            if ((this.entry_eq1.Text != "" || this.entry_eq2.Text != "" || this.entry_eq3.Text != "") && entry_xmin.Text != "" && entry_xmax.Text != "" && entry_ymin.Text != "" && entry_ymax.Text != "" && entry_graduation.Text != "")
            {
                // Convertir las cadenas de las entradas a valores decimales
                decimal decimal_xmin = Convert.ToDecimal(entry_xmin.Text);
                x_min = (double)decimal_xmin;
                decimal decimal_xmax = Convert.ToDecimal(entry_xmax.Text);
                x_max = (double)decimal_xmax;
                decimal decimal_ymin = Convert.ToDecimal(entry_ymin.Text);
                y_min = (double)decimal_ymin;
                decimal decimal_ymax = Convert.ToDecimal(entry_ymax.Text);
                y_max = (double)decimal_ymax;
                decimal decimal_graduation = Convert.ToDecimal(entry_graduation.Text);
                graduation = (double)decimal_graduation;

                // Verificar que los rangos sean válidos
                if (x_min < x_max && y_min < y_max && graduation > 0)
                {
                    // Crear una nueva ventana de trazado con los parámetros dados
                    Pantalla_Form screen = new Pantalla_Form(entry_eq1.Text, entry_eq2.Text, entry_eq3.Text, x_min, x_max, y_min, y_max, graduation);
                    label_info.Text = "Introduzca una funcion";
                }
                else
                {
                    label_info.Text = "El rango de la funcion es invalido";
                }
            }
            else
            {
                // Mostrar mensajes de error si las entradas no son válidas
                if (entry_eq1.Text == "" && entry_eq2.Text == "" && entry_eq3.Text == "")
                {
                    label_info.Text = "No hay ecuaciones";
                }
                else
                {
                    label_info.Text = "El rango de la funcion es invalido";
                }
            }
        }
        catch (Exception ex)
        {
            // Manejar excepciones y mostrar mensajes de error en la interfaz
            label_info.Text = "El rango de la funcion es invalido";
            label_info.Text = ex.Message;
        }
    }

    private void button_initialize_range_click(object o, EventArgs args)
    {
        // Establecer valores predeterminados para el rango
        entry_xmin.Text = "-10";
        entry_xmax.Text = "10";
        entry_ymin.Text = "-10";
        entry_ymax.Text = "10";
        entry_graduation.Text = "1";
    }
    // Método para inicializar todos los campos de la interfaz
    private void button_initialize_all_click(object o, EventArgs args)
    {
        // Limpiar todos los campos de la interfaz
        entry_eq1.Text = "";
        entry_eq2.Text = "";
        entry_eq3.Text = "";

        entry_x_value.Text = "";
        entry_result.Text = "";

        entry_xmin.Text = "-10";
        entry_xmax.Text = "10";
        entry_ymin.Text = "-10";
        entry_ymax.Text = "10";
        entry_graduation.Text = "1";
    }
    // Método para manejar el evento de cierre de la ventana principal
    private void OnMyWindowDelete(object o, DeleteEventArgs args)
    {
        Gtk.Application.Quit();
    }
}
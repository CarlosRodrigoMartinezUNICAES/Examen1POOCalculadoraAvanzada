using Gtk;
using Cairo;
using Color = Cairo.Color;

class Pantalla_Form : DrawingArea
{
    private Window win;
    private HBox hbox;
    private VBox vbox;

    // Variables para almacenar ecuaciones y límites
    public string equation;
    public string equation2;
    public string equation3;
        
    public double x_min;
    public double x_max;
    public double y_min;
    public double y_max;
    public double graduation;
        
    // colores
    private Color red = new Color(0xff, 0, 0);
    private Color green = new Color(0 , 0xff, 0);
    private Color blue = new Color(0 , 0, 0xff);
    private Color black = new Color(0 , 0, 0);
    private Color white = new Color(0xff, 0xff, 0xff);
    private Color gray = new Color(0.85, 0.85, 0.85);
    private Color cyan = new Color(0, 0xff, 0xff);
    private Color orange = new Color(0xff, 140, 0);
    private Color color_a = new Color(0xff, 0, 0xff);

    // Variables para almacenar resultados y opciones
    public bool first_draw = true;
    private double[] tab_result1_x = new double[501];
    private double[] tab_result1_y = new double[501];
    private double[] tab_result2_x = new double[501];
    private double[] tab_result2_y = new double[501];
    private double[] tab_result3_x = new double[501];
    private double[] tab_result3_y = new double[501];
    
    public Gtk.Label label_coord;

    // contexto del gráfico
    private Context context;

    public Pantalla_Form (string equation1, string equation21, string equation31, double x_min1, double x_max1, double y_min1, double y_max1, double graduation1)
    {
        // Configuración del tamaño de la ventana
        this.WidthRequest = 501;
        this.HeightRequest = 501;
        vbox = new VBox(false,3);
        vbox.PackStart(this,true,true,0);
        hbox = new HBox(false,5);
        
        this.Events = Gdk.EventMask.ExposureMask | Gdk.EventMask.LeaveNotifyMask | Gdk.EventMask.ButtonPressMask | Gdk.EventMask.PointerMotionMask | Gdk.EventMask.PointerMotionHintMask;

        // Configuración del formulario principal por ej titulo tamaño y que no sea estirable
        vbox.PackStart(hbox,true,true,0);
        win = new Gtk.Window ("Grafico de la funcion");                                                                                                                                                                                                                   
        win.SetDefaultSize (501, 501);
        win.Resizable = false;

        // Configuración de eventos y visualización
        this.Drawn += OnDrawn;
                    
        win.Add (vbox);
        win.ShowAll ();

        // Inicialización de variables
        this.equation = equation1;
        this.equation2 = equation21; 
        this.equation3 = equation31;  
        this.x_min = x_min1;
        this.x_max = x_max1;
        this.y_min = y_min1;
        this.y_max = y_max1;
        this.graduation = graduation1;
    }

    // Manejador de eventos de dibujo
    void OnDrawn (object o, DrawnArgs args)
    {
        this.context = args.Cr;
        trace_all();
    }

    // Método para trazar todos los elementos en el área de dibujo
    void trace_all()
    {        
        clear_graph();
        trace_axe();

        if(first_draw){
            trace_equation();
            first_draw = false;
        }
        else
            trace_equation_buffer();
    }

    // Método para limpiar el gráfico
    void clear_graph()
     {
        context.SetSourceColor(black);
        context.Rectangle(x: -10, y: -10, width: 520, height: 520);
        context.Fill();
     }

    // Método para trazar los ejes X e Y
    void trace_axe()
    {
        int x0 = (int) Math.Round(trans_x(0f));    
        int y0 = (int) Math.Round(trans_y(0f));
        int xmax = (int) Math.Round(trans_x(x_max));
        int xmin = (int) Math.Round(trans_x(x_min));
        int ymax = (int) Math.Round(trans_y(y_max));
        int ymin = (int) Math.Round(trans_y(y_min));
            
        // intervalo para x
        if (x_max > 0){    
            //0 to max        
            for(double i2 = trans_x(0f); i2 <= trans_x(x_max); i2 = i2 + graduation * 500/(x_max - x_min)){
                context.SetSourceColor(blue);
                context.MoveTo((int) Math.Round(i2), y0);
                context.LineTo((int) Math.Round(i2), y0-5);
                context.Stroke();
            }
        }
        if (x_min<0){
            //0 to min
            for(double i2 = trans_x(0f); i2 >= trans_x(x_min); i2 = i2 - graduation * 500/(x_max - x_min) ){
                context.SetSourceColor(blue);
                context.MoveTo((int) Math.Round(i2), y0);
                context.LineTo((int) Math.Round(i2), y0-5);
                context.Stroke();
            }
        }                    
                
        // intervalo para y
        if (y_min<0){
            //0 to min
            for(double i2 = trans_y(0f); i2 <= trans_y(y_min); i2 = i2 + graduation * 500/(y_max - y_min) ){
                context.SetSourceColor(blue);
                context.MoveTo(x0, (int) Math.Round(i2));
                context.LineTo(x0+5, (int) Math.Round(i2));
                context.Stroke();
            }
        }
        if (y_max>0){
            //0 to max    
            for(double i2 = trans_y(0f); i2 >= trans_y(y_max); i2 = i2 - graduation * 500/(y_max - y_min) ){
                context.SetSourceColor(blue);
                context.MoveTo(x0, (int) Math.Round(i2));
                context.LineTo(x0+5, (int) Math.Round(i2));
                context.Stroke();
            }
        }
        
        //ejes
        context.SetSourceColor(blue);
        context.MoveTo(xmin, y0);
        context.LineTo(xmax, y0);
        context.Stroke();
        context.MoveTo(x0, ymin);
        context.LineTo(x0, ymax);
        context.Stroke();
     }

    // Dibujar las tres funciones      
    void trace_equation()
    {
        Color color_eq;    
        for(int i_eq = 0; i_eq <3; i_eq++){
             
            operacion op = new operacion(equation);
            color_eq = red;
                      
            if(i_eq==1){
                op = new operacion(equation2);                
                color_eq = color_a;
            }
            else if(i_eq==2){
                op = new operacion(equation3);
                color_eq = green;
            }
                 
            op.correct();

            try{
                double xc_1 = x_min;
                double yc_1 = op.compute(x_min);            
                double xc_2;
                double yc_2;
                                                
                double pitch = (x_max-x_min)/500;
                double x_1;
                double y_1;
                double x_2;
                double y_2;
                
                // guardar resultados
                int i_save = 0;
                if(i_eq == 0){
                    tab_result1_x[i_save] = trans_x(xc_1);
                    tab_result1_y[i_save] = trans_y(yc_1);
                }
                else if(i_eq ==1){
                    tab_result2_x[i_save] = trans_x(xc_1);
                    tab_result2_y[i_save] = trans_y(yc_1);
                }
                else if(i_eq ==2){
                    tab_result3_x[i_save] = trans_x(xc_1);
                    tab_result3_y[i_save] = trans_y(yc_1);
                }                            
                                                
                for(double i_graph = x_min; i_graph <= x_max; i_graph = i_graph + pitch){
                    xc_2 = xc_1 + pitch;
                    yc_2 = op.compute(xc_2);
                                                            
                    x_1 = trans_x(xc_1);
                    y_1 = trans_y(yc_1);
                    x_2 = trans_x(xc_2);
                    y_2 = trans_y(yc_2);

                    // Guardar resultados
                    i_save++;
                    if(i_eq == 0){
                        tab_result1_x[i_save] = x_2;
                        tab_result1_y[i_save] = y_2;
                    }
                    else if(i_eq ==1){
                        tab_result2_x[i_save] = x_2;
                        tab_result2_y[i_save] = y_2;
                    }
                    else if(i_eq ==2){
                        tab_result3_x[i_save] = x_2;
                        tab_result3_y[i_save] = y_2;
                    }

                    if(!Double.IsNaN(yc_1) && !Double.IsNaN(yc_2)){
                        if((yc_1 > y_min && yc_1 < y_max) || (yc_2 > y_min && yc_2 < y_max)){
                            context.SetSourceColor(color_eq);
                            context.MoveTo((int) Math.Round(x_1), (int) Math.Round(y_1));
                            context.LineTo((int) Math.Round(x_2), (int) Math.Round(y_2));
                            context.Stroke();
                        }
                    }

                    // Siguiente punto
                    xc_1 = xc_2;
                    yc_1 = yc_2;
                    
                }
            }    
            catch(Exception ex)
            {    
            }         
        }        
    }

    // Dibujamos las tres funciones usando búferes
    void trace_equation_buffer()
    {
        for(int i =0; i < 500; i++){
            if(!Double.IsNaN(tab_result1_y[i]) && !Double.IsNaN(tab_result1_y[i+1])){
                if((tab_result1_y[i] > 0 && tab_result1_y[i] < 500) || (tab_result1_y[i+1] > 0 && tab_result1_y[i+1] < 500)){    
                    context.SetSourceColor(red);
                    context.MoveTo((int) Math.Round(tab_result1_x[i]), (int) Math.Round(tab_result1_y[i]));
                    context.LineTo((int) Math.Round(tab_result1_x[i+1]), (int) Math.Round(tab_result1_y[i+1]));
                    context.Stroke();
                }
            }
        }
        
        for(int i =0; i < 500; i++){
            if(!Double.IsNaN(tab_result2_y[i]) && !Double.IsNaN(tab_result2_y[i+1])){
                if((tab_result2_y[i] > 0 && tab_result2_y[i] < 500) || (tab_result2_y[i+1] > 0 && tab_result2_y[i+1] < 500)){   
                    context.SetSourceColor(color_a);
                    context.MoveTo((int) Math.Round(tab_result2_x[i]), (int) Math.Round(tab_result2_y[i]));
                    context.LineTo((int) Math.Round(tab_result2_x[i+1]), (int) Math.Round(tab_result2_y[i+1]));
                    context.Stroke(); 
                }
            }
        }
        
        for(int i =0; i < 500; i++){
            if(!Double.IsNaN(tab_result3_y[i]) && !Double.IsNaN(tab_result3_y[i+1])){
                if((tab_result3_y[i] > 0 && tab_result3_y[i] < 500) || (tab_result3_y[i+1] > 0 && tab_result3_y[i+1] < 500)){   
                    context.SetSourceColor(green);
                    context.MoveTo((int) Math.Round(tab_result3_x[i]), (int) Math.Round(tab_result3_y[i]));
                    context.LineTo((int) Math.Round(tab_result3_x[i+1]), (int) Math.Round(tab_result3_y[i+1]));
                    context.Stroke();  
                }
            }
        }
    }
            
    // transformacion en x
    double trans_x(double x){
        return (500 / (x_max - x_min) * (x - x_min));    
    }
        
    // transformacion en y
    double trans_y(double y){
        return (-500 / (y_max - y_min) * (y - y_max));
    }
}

public class operacion : object
{
    public string text;                 // texto completo de la operación
    public string op;                   // operador
    public operacion right;             // lado derecho de la operación
    public operacion left;              // lado izquierdo de la operación

    public operacion(string text)
    {
        this.text = text;
    }

    // Método para dividir la expresión en operadores y operandos
    public void split()
    {
        // Buscar un '+' (el más a la derecha) que no esté dentro de paréntesis
        int plusPos = this.text.LastIndexOf("+");
        bool plusFind = false;
        while (plusPos != -1 && plusFind == false)
        {
            if (this.countUpTo("(", plusPos) != this.countUpTo(")", plusPos))
            {
                plusPos = this.text.LastIndexOf("+", plusPos - 1, plusPos);
            }
            else
            {
                plusFind = true;
            }
        }
        if (plusFind == false)
        {
            plusPos = -1;
        }

        // Buscar un '-' (el más a la derecha) que no esté dentro de paréntesis
        int lessPos = this.text.LastIndexOf("-");
        bool lessFind = false;
        while (lessPos != -1 && lessFind == false)
        {
            if (this.countUpTo("(", lessPos) != this.countUpTo(")", lessPos))
            {
                lessPos = this.text.LastIndexOf("-", lessPos - 1, lessPos);
            }
            else
            {
                lessFind = true;
            }
        }
        if (lessFind == false)
        {
            lessPos = -1;
        }

        // Prioridad para + y -
        // Inicialmente con un +
        if ((plusPos != -1 && lessPos == -1) || (plusPos != -1 && lessPos != -1 && plusPos > lessPos))
        {
            this.op = "+";

            string leftText = this.text;
            leftText = leftText.Remove(plusPos, leftText.Length - plusPos);
            this.left = new operacion(leftText);

            string rightText = this.text;
            rightText = rightText.Remove(0, plusPos + 1);
            this.right = new operacion(rightText);
        }
        // Inicialmente con un -
        else if ((plusPos == -1 && lessPos != -1) || (plusPos != -1 && lessPos != -1 && lessPos > plusPos))
        {
            this.op = "-";

            string leftText = this.text;
            leftText = leftText.Remove(lessPos, leftText.Length - lessPos);
            this.left = new operacion(leftText);

            string rightText = this.text;
            rightText = rightText.Remove(0, lessPos + 1);
            this.right = new operacion(rightText);
        }

        // Ahora * y /
        else
        {
            // Buscar un '*' (el más a la derecha) que no esté dentro de paréntesis
            int multiplyPos = this.text.LastIndexOf("*");
            bool multiplyFind = false;
            while (multiplyPos != -1 && multiplyFind == false)
            {
                if (this.countUpTo("(", multiplyPos) != this.countUpTo(")", multiplyPos))
                {
                    multiplyPos = this.text.LastIndexOf("*", multiplyPos - 1, multiplyPos);
                }
                else
                {
                    multiplyFind = true;
                }
            }
            if (multiplyFind == false)
            {
                multiplyPos = -1;
            }

            // Buscar un '/' (el más a la derecha) que no esté dentro de paréntesis
            int dividePos = this.text.LastIndexOf("/");
            bool divideFind = false;
            while (dividePos != -1 && divideFind == false)
            {
                if (this.countUpTo("(", dividePos) != this.countUpTo(")", dividePos))
                {
                    dividePos = this.text.LastIndexOf("/", dividePos - 1, dividePos);
                }
                else
                {
                    divideFind = true;
                }
            }
            if (divideFind == false)
            {
                dividePos = -1;
            }

            // Inicialmente con un *
            if ((multiplyPos != -1 && dividePos == -1) || (multiplyPos != -1 && dividePos != -1 && multiplyPos > dividePos))
            {
                this.op = "*";

                string leftText = this.text;
                leftText = leftText.Remove(multiplyPos, leftText.Length - multiplyPos);
                this.left = new operacion(leftText);

                string rightText = this.text;
                rightText = rightText.Remove(0, multiplyPos + 1);
                this.right = new operacion(rightText);
            }
            // Inicialmente con un /
            else if ((multiplyPos == -1 && dividePos != -1) || (multiplyPos != -1 && dividePos != -1 && dividePos > multiplyPos))
            {
                this.op = "/";

                string leftText = this.text;
                leftText = leftText.Remove(dividePos, leftText.Length - dividePos);
                this.left = new operacion(leftText);

                string rightText = this.text;
                rightText = rightText.Remove(0, dividePos + 1);
                this.right = new operacion(rightText);
            }

            // Ahora ^
            else
            {
                // Buscar un '^' (el más a la derecha) que no esté dentro de paréntesis
                int powerPos = this.text.IndexOf("^");
                bool powerFind = false;
                while (powerPos != -1 && powerFind == false)
                {
                    if (this.countUpTo("(", powerPos) != this.countUpTo(")", powerPos))
                    {
                        powerPos = this.text.IndexOf("^", powerPos + 1);
                    }
                    else
                    {
                        powerFind = true;
                    }
                }
                if (powerFind == false)
                {
                    powerPos = -1;
                }

                if ((powerPos != -1))
                {
                    this.op = "^";

                    string leftText = this.text;
                    leftText = leftText.Remove(powerPos, leftText.Length - powerPos);
                    this.left = new operacion(leftText);

                    string rightText = this.text;
                    rightText = rightText.Remove(0, powerPos + 1);
                    this.right = new operacion(rightText);
                }
                else
                {
                    // No hay + - / * o ^, por lo que el texto es como "(2+x-3)" o "2" o "x" o r(2*x)...
                    // Si el texto está entre paréntesis (2+x-3), eliminar los paréntesis
                    if (this.text.StartsWith("("))
                    {
                        this.op = "()";
                        string leftText = this.text;
                        leftText = leftText.Remove(0, 1);                        // Eliminar "("
                        leftText = leftText.Remove(leftText.Length - 1, 1);     // Eliminar ")"
                        this.left = new operacion(leftText);
                    }
                    // Raíz cuadrada
                    else if (this.text.StartsWith("sqrt("))
                    {
                        this.op = "sqrt()";
                        string leftText = this.text;
                        leftText = leftText.Remove(0, 5);                        // Eliminar "sqrt("
                        leftText = leftText.Remove(leftText.Length - 1, 1);     // Eliminar ")"
                        this.left = new operacion(leftText);
                    }
                    // Coseno
                    else if (this.text.StartsWith("cos("))
                    {
                        this.op = "cos()";
                        string leftText = this.text;
                        leftText = leftText.Remove(0, 4);                        // Eliminar "cos("
                        leftText = leftText.Remove(leftText.Length - 1, 1);     // Eliminar ")"
                        this.left = new operacion(leftText);
                    }
                    // Seno
                    else if (this.text.StartsWith("sin("))
                    {
                        this.op = "sin()";
                        string leftText = this.text;
                        leftText = leftText.Remove(0, 4);                        // Eliminar "sin("
                        leftText = leftText.Remove(leftText.Length - 1, 1);     // Eliminar ")"
                        this.left = new operacion(leftText);
                    }
                    // Tangente
                    else if (this.text.StartsWith("tan("))
                    {
                        this.op = "tan()";
                        string leftText = this.text;
                        leftText = leftText.Remove(0, 4);                        // Eliminar "tan("
                        leftText = leftText.Remove(leftText.Length - 1, 1);     // Eliminar ")"
                        this.left = new operacion(leftText);
                    }
                    // Logaritmo natural
                    else if (this.text.StartsWith("ln("))
                    {
                        this.op = "ln()";
                        string leftText = this.text;
                        leftText = leftText.Remove(0, 3);                        // Eliminar "ln("
                        leftText = leftText.Remove(leftText.Length - 1, 1);     // Eliminar ")"
                        this.left = new operacion(leftText);
                    }
                    // Logaritmo base 10
                    else if (this.text.StartsWith("log("))
                    {
                        this.op = "log()";
                        string leftText = this.text;
                        leftText = leftText.Remove(0, 4);                        // Eliminar "log("
                        leftText = leftText.Remove(leftText.Length - 1, 1);     // Eliminar ")"
                        this.left = new operacion(leftText);
                    }
                    // Arco coseno
                    else if (this.text.StartsWith("acos("))
                    {
                        this.op = "acos()";
                        string leftText = this.text;
                        leftText = leftText.Remove(0, 5);                        // Eliminar "acos("
                        leftText = leftText.Remove(leftText.Length - 1, 1);     // Eliminar ")"
                        this.left = new operacion(leftText);
                    }
                    // Arco seno
                    else if (this.text.StartsWith("asin("))
                    {
                        this.op = "asin()";
                        string leftText = this.text;
                        leftText = leftText.Remove(0, 5);                        // Eliminar "asin("
                        leftText = leftText.Remove(leftText.Length - 1, 1);     // Eliminar ")"
                        this.left = new operacion(leftText);
                    }
                    // Arco tangente
                    else if (this.text.StartsWith("atan("))
                    {
                        this.op = "atan()";
                        string leftText = this.text;
                        leftText = leftText.Remove(0, 5);                        // Eliminar "atan("
                        leftText = leftText.Remove(leftText.Length - 1, 1);     // Eliminar ")"
                        this.left = new operacion(leftText);
                    }
                    // Coseno hiperbólico
                    else if (this.text.StartsWith("cosh("))
                    {
                        this.op = "cosh()";
                        string leftText = this.text;
                        leftText = leftText.Remove(0, 5);                        // Eliminar "cosh("
                        leftText = leftText.Remove(leftText.Length - 1, 1);     // Eliminar ")"
                        this.left = new operacion(leftText);
                    }
                    // Seno hiperbólico
                    else if (this.text.StartsWith("sinh("))
                    {
                        this.op = "sinh()";
                        string leftText = this.text;
                        leftText = leftText.Remove(0, 5);                        // Eliminar "sinh("
                        leftText = leftText.Remove(leftText.Length - 1, 1);     // Eliminar ")"
                        this.left = new operacion(leftText);
                    }
                    // Tangente hiperbólica
                    else if (this.text.StartsWith("tanh("))
                    {
                        this.op = "tanh()";
                        string leftText = this.text;
                        leftText = leftText.Remove(0, 5);                        // Eliminar "tanh("
                        leftText = leftText.Remove(leftText.Length - 1, 1);     // Eliminar ")"
                        this.left = new operacion(leftText);
                    }
                    // Exponencial
                    else if (this.text.StartsWith("exp("))
                    {
                        this.op = "exp()";
                        string leftText = this.text;
                        leftText = leftText.Remove(0, 4);                        // Eliminar "exp("
                        leftText = leftText.Remove(leftText.Length - 1, 1);     // Eliminar ")"
                        this.left = new operacion(leftText);
                    }
                    // Valor absoluto
                    else if (this.text.StartsWith("abs("))
                    {
                        this.op = "abs()";
                        string leftText = this.text;
                        leftText = leftText.Remove(0, 4);                        // Eliminar "abs("
                        leftText = leftText.Remove(leftText.Length - 1, 1);     // Eliminar ")"
                        this.left = new operacion(leftText);
                    }
                    // Parte entera
                    else if (this.text.StartsWith("int("))
                    {
                        this.op = "int()";
                        string leftText = this.text;
                        leftText = leftText.Remove(0, 4);                        // Eliminar "int("
                        leftText = leftText.Remove(leftText.Length - 1, 1);     // Eliminar ")"
                        this.left = new operacion(leftText);
                    }
                }
            }
        }
    }

    // Método para corregir ciertos aspectos de la expresión
    public void correct()
    {
        // Agregar un "0" antes de un signo '-' al inicio de la expresión
        if (this.text.StartsWith("-"))
        {
            this.text = this.text.Insert(0, "0");
        }

        // Reemplazar "X" por "x"
        this.text = this.text.Replace("X", "x");
        // Reemplazar "(-" por "(0-"
        this.text = this.text.Replace("(-", "(0-");

        // Corregir formato para expresiones como "x("
        this.text = this.text.Replace("x(", "x*(");
        this.text = this.text.Replace("0(", "0*(");
        this.text = this.text.Replace("1(", "1*(");
        this.text = this.text.Replace("2(", "2*(");
        this.text = this.text.Replace("3(", "3*(");
        this.text = this.text.Replace("4(", "4*(");
        this.text = this.text.Replace("5(", "5*(");
        this.text = this.text.Replace("6(", "6*(");
        this.text = this.text.Replace("7(", "7*(");
        this.text = this.text.Replace("8(", "8*(");
        this.text = this.text.Replace("9(", "9*(");

        // Corregir formato para expresiones como ")x"
        this.text = this.text.Replace(")x", ")*x");

        // Corregir formato para expresiones como "0x", "1x", etc.
        for (int i = 0; i <= 9; i++)
        {
            this.text = this.text.Replace($"{i}x", $"{i}*x");
        }

        // Corregir formato para expresiones como ")("
        this.text = this.text.Replace(")(", ")*(");
    }

    // Método para evaluar la expresión para un valor dado de x
    public double compute(double x)
    {
        double result = 0;
        this.split();

        // Caso: + - * / ^
        if (this.left != null && this.right != null)
        {
            if (this.op == "+")
            {
                result = this.left.compute(x) + this.right.compute(x);
                return result;
            }
            else if (this.op == "*")
            {
                result = this.left.compute(x) * this.right.compute(x);
                return result;
            }
            else if (this.op == "-")
            {
                result = this.left.compute(x) - this.right.compute(x);
                return result;
            }
            else if (this.op == "/")
            {
                result = this.left.compute(x) / this.right.compute(x);
                return result;
            }
            else if (this.op == "^")
            {
                double left_double = this.left.compute(x);
                double right_double = this.right.compute(x);
                result = Math.Pow(left_double, right_double);
                return result;
            }
        }
        // Caso: () r() cos() sin() tan() y n()
        else if (this.left != null)
        {
            if (this.op == "()")
            {
                result = this.left.compute(x);
                return result;
            }
            else if (this.op == "sqrt()")
            {
                double left_double = this.left.compute(x);
                result = Math.Sqrt(left_double);
                return result;
            }
            else if (this.op == "cos()")
            {
                double left_double = this.left.compute(x);
                result = Math.Cos(left_double);
                return result;
            }
            else if (this.op == "sin()")
            {
                double left_double = this.left.compute(x);
                result = Math.Sin(left_double);
                return result;
            }
            else if (this.op == "tan()")
            {
                double left_double = this.left.compute(x);
                result = Math.Tan(left_double);
                return result;
            }
            else if (this.op == "ln()")
            {
                double left_double = this.left.compute(x);
                result = Math.Log(left_double);
                return result;
            }
            else if (this.op == "log()")
            {
                double left_double = this.left.compute(x);
                result = Math.Log10(left_double);
                return result;
            }
            else if (this.op == "acos()")
            {
                double left_double = this.left.compute(x);
                result = Math.Acos(left_double);
                return result;
            }
            else if (this.op == "asin()")
            {
                double left_double = this.left.compute(x);
                result = Math.Asin(left_double);
                return result;
            }
            else if (this.op == "atan()")
            {
                double left_double = this.left.compute(x);
                result = Math.Atan(left_double);
                return result;
            }
            else if (this.op == "cosh()")
            {
                double left_double = this.left.compute(x);
                result = Math.Cosh(left_double);
                return result;
            }
            else if (this.op == "sinh()")
            {
                double left_double = this.left.compute(x);
                result = Math.Sinh(left_double);
                return result;
            }
            else if (this.op == "tanh()")
            {
                double left_double = this.left.compute(x);
                result = Math.Tanh(left_double);
                return result;
            }
            else if (this.op == "exp()")
            {
                double left_double = this.left.compute(x);
                result = Math.Exp(left_double);
                return result;
            }
            else if (this.op == "abs()")
            {
                double left_double = this.left.compute(x);
                result = Math.Abs(left_double);
                return result;
            }
            else if (this.op == "int()")
            {
                double left_double = this.left.compute(x);
                result = Math.Floor(left_double);
                return result;
            }
        }
        // Caso: x
        else
        {
            if (this.text == "x")
            {
                return x;
            }
            else
            {
                return double.Parse(this.text);
            }
        }

        return result;
    }

    // Método para contar el número de ocurrencias de un carácter en una cadena hasta una posición dada
    public int countUpTo(string str, int position)
    {
        int count = 0;
        for (int i = 0; i <= position; i++)
        {
            if (this.text[i].ToString() == str)
            {
                count++;
            }
        }
        return count;
    }

    // Método ToString para imprimir la expresión
    public override string ToString()
    {
        return this.text;
    }
}

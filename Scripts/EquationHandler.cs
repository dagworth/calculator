using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace final_calculator.Scripts {
    public static class EquationHandler {

        public static string fix(string equation) {
            return equation;
            if (equation.IndexOf("E") > 0) {
                int index = equation.IndexOf("E");
                if (equation[index+1] == '-' || equation[index+1] == '+') {
                    return equation.Substring(0,index+1) + equation.Substring(index+2);
                }
            }
            return equation;
        }

        //almost clear
        public static string do_operation(string equation, char[] operations, bool include_negative){
            equation = fix(equation);
            for (int i = 1; i < equation.Length; i++){
                if (operations.Contains(equation[i])){
                    int last_index = Checker.find_last_number(equation, i, include_negative);
                    int next_index = Checker.find_next_number(equation, i);

                    string last_str = equation.Substring(last_index, i - last_index);
                    Console.WriteLine(last_str);
                    string next_str = equation.Substring(i + 1, next_index - i - 1);
               
                    double last_num = double.Parse(last_str);
                    double next_num = double.Parse(next_str);
                    double value = 0;


                    if (equation[i] == '^'){
                        value = Math.Pow(last_num, next_num);
                    } else if (equation[i] == '*'){
                        value = last_num * next_num;
                    } else if (equation[i] == '/'){
                        value = last_num / next_num;
                    } else if (equation[i] == '+'){
                        value = last_num + next_num;
                    } else if (equation[i] == '-'){
                        value = last_num - next_num;
                    }


                    equation = equation.Substring(0, last_index) + value.ToString() + equation.Substring(next_index);
                    i = last_index + value.ToString().Length - 1;
                }

                //i still feel like this isnt efficient idk tho
                if(equation[i] == '-' && equation[i - 1] == '-'){
                    if (i != 1){
                        equation = equation.Substring(0,i-1) + "+" + equation.Substring(i+1);
                    } else {
                        equation = equation.Substring(0,i-1) + equation.Substring(i+1);
                    }
                    i--;
                }
            }
            return equation;
        }

        //needs the most optimization
        private static string solve_line(string equation){
            int last_open_abs = -1;
            List<int> open_parentheses = new List<int>();
            for (int i = 0; i < equation.Length; i++){
                char current = equation[i];
                if (current == '(' && !open_parentheses.Contains(i)){
                    if (i != 0 && Checker.check_if_valid_end(equation.Substring(0,i))){
                        equation = equation.Substring(0, i) + "*" + equation.Substring(i);
                        i++;
                    }
                    open_parentheses.Add(i);
                }
                else if (current == ')'){
                    if (i < equation.Length-1 && Checker.check_if_valid_start(equation.Substring(i + 1))){
                        equation = equation.Substring(0,i + 1) + "*" + equation.Substring(i + 1);
                    }
                    int index = -1;
                    string front = "";
               
                    if (open_parentheses.Count > 0){
                        index = open_parentheses.Last();
                        open_parentheses.RemoveAt(open_parentheses.Count - 1);
                        front = equation.Substring(0, index);
                    }


                    if (i - index - 1 == 0){ return "error: nothing in ()"; }

                    string new_string = Checker.do_out(equation.Substring(index + 1, i - index - 1));

                    if(new_string.IndexOf("error") > -1){
                        return new_string;
                    }

                    if (front.Length > 3){
                        string check = front.Substring(front.Length-4,4);
                        string thing = "checked:" + Checker.do_out(new_string);
                        Console.WriteLine(thing);
                        if (check.Equals("sqrt")){
                            new_string = Math.Sqrt(double.Parse(new_string)).ToString();
                            front = front.Substring(0,front.Length-4);
                            index-=4;
                        } else if (check.Equals("asin")){
                            new_string = Math.Asin(double.Parse(Checker.do_out(new_string))).ToString();
                            front = front.Substring(0,front.Length-4);
                            index-=4;
                        } else if (check.Equals("acos")){
                            new_string = Math.Acos(double.Parse(new_string)).ToString();
                            front = front.Substring(0,front.Length-4);
                            index-=4;
                        } else if (check.Equals("asin")){
                            new_string = Math.Atan(double.Parse(new_string)).ToString();
                            front = front.Substring(0,front.Length-4);
                            index-=4;
                        } 
                    }
               
                    if (front.Length > 2){
                        string check = front.Substring(front.Length-3,3);
                        if (check.Equals("tan")){
                            new_string = Math.Tan(double.Parse(new_string)).ToString();
                            front = front.Substring(0,front.Length-3);
                            index-=3;
                        } else if (check.Equals("sin")){
                            new_string = Math.Sin(double.Parse(new_string)).ToString();
                            front = front.Substring(0,front.Length-3);
                            index-=3;
                        } else if (check.Equals("cos")){
                            new_string = Math.Cos(double.Parse(new_string)).ToString();
                            front = front.Substring(0,front.Length-3);
                            index-=3;
                        } else if (check.Equals("deg")){
                            new_string = (double.Parse(new_string) / Math.PI * 180).ToString();
                            front = front.Substring(0,front.Length-3);
                            index-=3;
                        } else if (check.Equals("rad")){
                            new_string = (double.Parse(new_string) / 180 * Math.PI).ToString();
                            front = front.Substring(0,front.Length-3);
                            index-=3;
                        }
                    }


                    equation = front + new_string + equation.Substring(i + 1);
                    i = index + new_string.Length - 1;
                }
                else if(current == '|'){
                    //definitely isnt optimal, repeated code from above
                    if(last_open_abs == -1){
                        if (i != 0 && Checker.check_if_valid_end(equation.Substring(0,i))){
                            equation = equation.Substring(0, i) + "*" + equation.Substring(i);
                            i++;
                        }
                        last_open_abs = i;
                    } else {
                        if (i - last_open_abs - 1 == 0){ return "error: nothing in ||"; }

                        if (i != equation.Length-1 && Checker.check_if_valid_start(equation.Substring(i + 1))){
                            equation = equation.Substring(0,i + 1) + "*" + equation.Substring(i + 1);
                        }

                        string front = equation.Substring(0, last_open_abs);
                        string new_string = Checker.do_out(equation.Substring(last_open_abs + 1, i - last_open_abs - 1));
                        if(new_string.IndexOf("error") > -1){
                            return new_string;
                        }
                        new_string = Math.Abs(double.Parse(new_string)).ToString();
                        equation = front + new_string + equation.Substring(i + 1);
                        i = last_open_abs + new_string.Length - 1;




                        last_open_abs = -1;
                    }
                }

                if (i + 1 == equation.Length){
                    if (last_open_abs > 0){
                        return "error: open abs val";
                    }
                    if (open_parentheses.Count > 0){
                        foreach (int thingy in open_parentheses){
                            equation += ")";
                        }
                        i = 0;
                    }
                }
            }
       
            return Checker.do_out(equation);
        }

        static string format(double number){
            string return_string = "";
            if (number >= 1e36) {
                return_string = (number / 1e36).ToString("0.#") + "Ud";
            } else if (number >= 1e33) {
                return_string = (number / 1e33).ToString("0.#") + "Dc";
            } else if (number >= 1e30) {
                return_string = (number / 1e30).ToString("0.#") + "No";
            } else if (number >= 1e27) {
                return_string = (number / 1e27).ToString("0.#") + "Oc";
            } else if (number >= 1e24) {
                return_string = (number / 1e24).ToString("0.#") + "Sp";
            } else if (number >= 1e21) {
                return_string = (number / 1e21).ToString("0.#") + "Sx";
            } else if (number >= 1e18) {
                return_string = (number / 1e18).ToString("0.#") + "Qi";
            } else if (number >= 1e15) {
                return_string = (number / 1e15).ToString("0.#") + "Qa";
            } else if (number >= 1e12) {
                return_string = (number / 1e12).ToString("0.#") + "T";
            } else if (number >= 1e9) {
                return_string = (number / 1e9).ToString("0.#") + "B";
            } else if (number >= 1e6) {
                return_string = (number / 1e6).ToString("0.#") + "M";
            } else if (number >= 1e3) {
                return_string = (number / 1e3).ToString("0.#") + "K";
            } else {
                return "";
            }
            if(return_string.Length > 6) {
                return "";
            }
            return " (" + return_string + ")";
        }
        //clear
        public static string solve(string text,UserData data){
            Checker.vars = new Dictionary<char, string>();
            string[] lines = text.Replace(" ", "").ToLower().Split(new[] {'\n','\r'}); //replace with {'\n','\r'} in the app, only '\n' outside of it
            string response = "";
            for (int line_index = 0; line_index < lines.Length; line_index++){
                if (line_index % 2 != 0) { continue; } //only in app

                string line = lines[line_index];
                string value = "";

                if (line.Length > 2 && line[1] == '='){
                    value = solve_line(line.Substring(2));
                    if (value.IndexOf("error") == -1){
                        Checker.vars[line[0]] = value;
                    }
                } else if (line.Length > 0){
                    value = solve_line(line);
                }

                if(value.Equals("")){
                    if(data.debug == true) {
                        response+="(debug) nothing";
                    }
                } else if (value.IndexOf("error") == -1){
                    value = Math.Round(double.Parse(value), 5).ToString();
                    response+=value;
                    if(data.prefix == true) {
                        response+= format(double.Parse(value)); 
                    }
                } else if (data.debug == true){
                    response+=value;
                }
           
                if (line_index+1 != lines.Length){
                    response += "\n";      
                }
            }
            return response;
        }

    }
}

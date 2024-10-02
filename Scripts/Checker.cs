namespace final_calculator.Scripts {
    public static class Checker {
        private static char[] numbers = {
            '1','2','3','4','5','6','7','8','9','0','.','E'
        };

        private static char[] valid_end = {
            '1','2','3','4','5','6','7','8','9','0',')','|'
        };
   
        private static char[] valid_middle = {
            '1','2','3','4','5','6','7','8','9','0','+','-','*','/','^','(',')','.','E'
        };

        private static char[] valid_start = {
            '1','2','3','4','5','6','7','8','9','0','(','-','.'
        };

        private static string[] valid_string_start = {
            "cos(","sin(","tan(","sqrt(","deg(", "rad(","asin(","acos(","atan("
        };

        public static Dictionary<char, string> vars = new Dictionary<char, string>();

        public static bool check_if_valid_start(string equation){
            if(vars.ContainsKey(equation[0]) || valid_start.Contains(equation[0])){
                return true;
            }
            foreach(string text in valid_string_start){
                if(equation.Length > text.Length){
                    if(equation.IndexOf(text) == 0){
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool check_if_valid_end(string equation){
            return valid_end.Contains(equation.Last()) || vars.ContainsKey(equation.Last());
        }

        public static int find_next_number(string equation, int index){
            for (int i = index + 1; i < equation.Length; i++){
                //in case of 5*-7, negative right after operation
                if (i == index + 1 && equation[i] == '-'){ continue;}
                if (!numbers.Contains(equation[i])){
                    return i;
                }
            }
            return equation.Length;
        }
        

        //find the index of the start of the latest number
        public static int find_last_number(string equation, int index, bool include_negative){
            for (int i = index - 1; i >= 0; i--){
                if (!numbers.Contains(equation[i])){
                    if (include_negative && equation[i] == '-'){
                        continue;
                    }
                    return i + 1;
                }
            }
            return 0;
        }

        public static string do_out(string equation){
            //this means the equation was invalid
            if(!check_if_valid_end(equation) || !(check_if_valid_start(equation) || equation[0] == '|')){
                return "error at do_out: '" + equation + "'";
            }
       
            for (int i = 0; i < equation.Length; i++){
                if (vars.ContainsKey(equation[i])){
                    string add = vars[equation[i]];
                    if (i != 0){
                        if (numbers.Contains(equation[i-1])){
                            add = "*" + add;
                        }
                    }
                    if(i != equation.Length-1){
                        if(numbers.Contains(equation[i+1])){
                            add = add + "*";
                        }
                    }

                    equation = equation.Substring(0, i) + add + equation.Substring(i + 1);
                    i += add.Length - 1;
                } else if (!valid_middle.Contains(equation[i])) {
                    return "error: '"+ equation[i] + "' is not valid";
                }
            }

            equation = EquationHandler.do_operation(equation, new char[]{'^'},false); //was false
            equation = EquationHandler.do_operation(equation, new char[]{'*','/'},false); //was false
            equation = EquationHandler.do_operation(equation, new char[]{'+','-'},true);

            return equation;
        }
    }
}

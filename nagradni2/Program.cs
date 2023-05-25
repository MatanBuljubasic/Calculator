using System;
using System.Collections.Generic;

namespace nagradni2
{
    class Program
    {
        static void Main(string[] args)
        {
            string equation = "((8-1+3)*6-((3+7)*2))/2-2+48*(21/3-7*(4+80))+25*((42-33)/3-7+11*2)-(542-21)*(11+3)+31380+1350/2*7";
            //equation=Console.ReadLine();
            Console.WriteLine(Calculate(equation));
        }

        public static double Calculate(string equation)
        {
            char[] operators = { '+', '-', '*', '/' };
            string[] numbers;
            List<char> ops=new List<char>();
            
            int minindex = 0;
            int negindex = 0;
            while (equation.Contains("(") && equation.Contains(")"))
            {
                List<int> openindexes=new List<int>();
                List<int> closedindexes=new List<int>();

                for(int i=0;i<equation.Length;i++)
                {
                    if (equation[i] == '(')
                        openindexes.Add(i);
                    if (equation[i] == ')')
                        closedindexes.Add(i);
                }
                int firstopen = openindexes[0];
                int lastclosed = closedindexes[closedindexes.Count - 1];
                for(int i = 0; i < openindexes.Count; i++)
                {
                    for(int j = 0; j < closedindexes.Count; j++)
                    if(openindexes[i]>=firstopen && closedindexes[closedindexes.Count - (j + 1)] <= lastclosed && openindexes[i]< closedindexes[closedindexes.Count - (j + 1)])
                    {
                        firstopen = openindexes[i];
                        lastclosed = closedindexes[closedindexes.Count - (j + 1)];
                    }
                }
                if (openindexes.Count > 1)
                {
                    string oldValue = equation.Substring(firstopen, lastclosed - firstopen+1);
                    equation = equation.Replace(oldValue, Convert.ToString(Calculate(equation.Substring(firstopen + 1, lastclosed - firstopen - 1))));
                    //if (openindexes[openindexes.Count - 1] < closedindexes[openindexes.Count - 2])
                    //{
                    //    string oldValue = equation.Substring(equation.IndexOf("("), equation.LastIndexOf(')') - equation.IndexOf("(") + 1);
                    //    equation = equation.Replace(oldValue, Convert.ToString(Calculate(equation.Substring(equation.IndexOf("(") + 1, equation.IndexOf(')') - equation.IndexOf("(") - 1))));
                    //}
                    //else
                    //{
                    //    string oldValue = equation.Substring(equation.IndexOf("("), equation.LastIndexOf(')') - equation.IndexOf("(") + 1);
                    //    equation = equation.Replace(oldValue, Convert.ToString(Calculate(equation.Substring(equation.IndexOf("(") + 1, equation.LastIndexOf(')') - equation.IndexOf("(") - 1))));
                    //}
                }
                else
                {
                    string oldValue = equation.Substring(equation.IndexOf("("), equation.LastIndexOf(')') - equation.IndexOf("(") + 1);
                    equation = equation.Replace(oldValue, Convert.ToString(Calculate(equation.Substring(equation.IndexOf("(") + 1, equation.LastIndexOf(')') - equation.IndexOf("(") - 1))));
                }
            }
            foreach (char character in equation)
            {
                if (character == '+' || character == '-' || character == '*' || character == '/')
                    ops.Add(character);
            }
            while (ops.Count != 0)
            {
                int mincount = 0;
                numbers = equation.Split(operators);
                ops.Clear();
                foreach (char character in equation)
                {
                    if (character == '+' || character == '-' || character == '*' || character == '/')
                    {
                        ops.Add(character);
                    }
                }
                var temp = new List<string>();
                for (int i=0;i<numbers.Length;i++)
                {
                    if (!string.IsNullOrEmpty(numbers[i]))
                        temp.Add(numbers[i]);
                    else
                        negindex = i;
                    numbers[i].Trim();
                }
                for (int i=0;i<negindex;i++)
                {
                    if (ops[i] == '-')
                        mincount++;
                }
                if (negindex != 0)
                {
                    for (int i = 0; mincount != -1; i++)
                    {
                        if (equation[i] == '-')
                        {
                            mincount--;
                        }
                        minindex = i;
                    }
                }
                numbers = temp.ToArray();
                if (ops.Count == numbers.Length)
                {
                    numbers[negindex] = Convert.ToString(double.Parse(numbers[negindex]) * -1);
                    ops.RemoveAt(negindex);
                }
                if (ops.Contains('*') && ops.Contains('/'))
                {
                    if (ops.IndexOf('*') < ops.IndexOf('/'))
                    {
                        if (Convert.ToDouble(numbers[ops.IndexOf('*')]) < 0 || Convert.ToDouble(numbers[ops.IndexOf('*')+1]) < 0)
                        {
                            if (negindex == 0)
                            {
                                string oldValue = equation.Substring(equation.IndexOf('-'), numbers[0].Length + numbers[1].Length + 1);
                                equation = equation.Replace(oldValue, Convert.ToString(double.Parse(numbers[0]) * double.Parse(numbers[1])));
                            }
                            else
                            {
                                string oldValue = equation.Substring(minindex - 1 - numbers[negindex - 1].Length, numbers[negindex - 1].Length + numbers[negindex].Length + 1);
                                equation = equation.Replace(oldValue, Convert.ToString(double.Parse(numbers[negindex - 1]) * double.Parse(numbers[negindex])));
                            }
                        }
                        else if (ops.Count > 1)
                        {
                            string oldValue = equation.Substring(equation.IndexOf("*") - numbers[ops.IndexOf('*')].Length, numbers[ops.IndexOf('*')].Length + numbers[ops.IndexOf('*') + 1].Length+1);
                            equation = equation.Replace(oldValue, Convert.ToString(Calculate(oldValue)));
                        }
                        
                        else
                            return Convert.ToDouble(numbers[0]) * Convert.ToDouble(numbers[1]);
                    }
                    else
                    {
                        if(Convert.ToDouble(numbers[ops.IndexOf('/')])<0 || Convert.ToDouble(numbers[ops.IndexOf('/')+1]) < 0)
                        {
                            if (negindex == 0)
                            {
                                string oldValue = equation.Substring(equation.IndexOf('-'), numbers[0].Length + numbers[1].Length + 1);
                                equation = equation.Replace(oldValue, Convert.ToString(double.Parse(numbers[0]) / double.Parse(numbers[1])));
                            }
                            else
                            {
                                string oldValue = equation.Substring(minindex - 1 - numbers[negindex - 1].Length, numbers[negindex - 1].Length + numbers[negindex].Length + 1);
                                equation = equation.Replace(oldValue, Convert.ToString(double.Parse(numbers[negindex - 1]) / double.Parse(numbers[negindex])));
                            }
                        }
                        else if (ops.Count > 1)
                        {
                            string oldValue = equation.Substring(equation.IndexOf("/") - numbers[ops.IndexOf('/')].Length, numbers[ops.IndexOf('/')].Length + numbers[ops.IndexOf('/') + 1].Length+1);
                            equation = equation.Replace(oldValue, Convert.ToString(Calculate(oldValue)));
                        }
                        else
                            return Convert.ToDouble(numbers[0]) / Convert.ToDouble(numbers[1]);
                    }
                }
                else if (ops.Contains('*'))
                {
                    if (Convert.ToDouble(numbers[ops.IndexOf('*')]) < 0 || Convert.ToDouble(numbers[ops.IndexOf('*') + 1]) < 0)
                    {
                        if (negindex == 0)
                        {
                            string oldValue = equation.Substring(equation.IndexOf('-'), numbers[0].Length + numbers[1].Length + 1);
                            equation = equation.Replace(oldValue, Convert.ToString(double.Parse(numbers[0]) * double.Parse(numbers[1])));
                        }
                        else
                        {
                            string oldValue = equation.Substring(minindex-1-numbers[negindex-1].Length, numbers[negindex-1].Length + numbers[negindex].Length + 1);
                            equation = equation.Replace(oldValue, Convert.ToString(double.Parse(numbers[negindex-1]) * double.Parse(numbers[negindex])));
                        }
                    }
                    else if (ops.Count > 1)
                    {
                        string oldValue = equation.Substring(equation.IndexOf("*") - numbers[ops.IndexOf('*')].Length, numbers[ops.IndexOf('*')].Length + numbers[ops.IndexOf('*') + 1].Length+1);
                        equation = equation.Replace(oldValue, Convert.ToString(Calculate(oldValue)));
                    }
                    else
                        return Convert.ToDouble(numbers[0]) * Convert.ToDouble(numbers[1]);
                }
                else if (ops.Contains('/'))
                {
                    if (Convert.ToDouble(numbers[ops.IndexOf('/')]) < 0 || Convert.ToDouble(numbers[ops.IndexOf('/') + 1]) < 0)
                    {
                        if (negindex == 0)
                        {
                            string oldValue = equation.Substring(equation.IndexOf('-'), numbers[0].Length + numbers[1].Length + 1);
                            equation = equation.Replace(oldValue, Convert.ToString(double.Parse(numbers[0]) / double.Parse(numbers[1])));
                        }
                        else
                        {
                            string oldValue = equation.Substring(minindex - 1 - numbers[negindex - 1].Length, numbers[negindex - 1].Length + numbers[negindex].Length + 1);
                            equation = equation.Replace(oldValue, Convert.ToString(double.Parse(numbers[negindex - 1]) / double.Parse(numbers[negindex])));
                        }
                    }
                    else if (ops.Count > 1)
                    {
                        string oldValue = equation.Substring(equation.IndexOf("/") - numbers[ops.IndexOf('/')].Length, numbers[ops.IndexOf('/')].Length + numbers[ops.IndexOf('/') + 1].Length+1);
                        equation = equation.Replace(oldValue, Convert.ToString(Calculate(oldValue)));
                    }
                    else
                        return Convert.ToDouble(numbers[0]) / Convert.ToDouble(numbers[1]);
                }
                if (ops.Contains('*') || ops.Contains('/'))
                    continue;

                if (ops.Contains('+') && ops.Contains('-'))
                {
                    if (ops.IndexOf('+') < ops.IndexOf('-'))
                    {
                        if (Convert.ToDouble(numbers[ops.IndexOf('+')]) < 0 || Convert.ToDouble(numbers[ops.IndexOf('+') + 1]) < 0)
                        {
                            if (negindex == 0)
                            {
                                string oldValue = equation.Substring(equation.IndexOf('-'), numbers[0].Length + numbers[1].Length + 1);
                                equation = equation.Replace(oldValue, Convert.ToString(double.Parse(numbers[0]) + double.Parse(numbers[1])));
                            }
                            else
                            {
                                string oldValue = equation.Substring(minindex - 1 - numbers[negindex - 1].Length, numbers[negindex - 1].Length + numbers[negindex].Length + 1);
                                equation = equation.Replace(oldValue, Convert.ToString(double.Parse(numbers[negindex - 1]) + double.Parse(numbers[negindex])));
                            }
                        }
                        else if (ops.Count > 1)
                        {
                            string oldValue = equation.Substring(equation.IndexOf("+") - numbers[ops.IndexOf('+')].Length, numbers[ops.IndexOf('+')].Length + numbers[ops.IndexOf('+') + 1].Length+1);
                            equation = equation.Replace(oldValue, Convert.ToString(Calculate(oldValue)));
                        }
                        else
                            return Convert.ToDouble(numbers[0]) + Convert.ToDouble(numbers[1]);
                    }
                    else
                    {
                        if (Convert.ToDouble(numbers[ops.IndexOf('-')]) < 0 || Convert.ToDouble(numbers[ops.IndexOf('-') + 1]) < 0)
                        {
                            if (negindex == 0)
                            {
                                string oldValue = equation.Substring(equation.IndexOf('-'), numbers[0].Length + numbers[1].Length + 1);
                                equation = equation.Replace(oldValue, Convert.ToString(double.Parse(numbers[0]) - double.Parse(numbers[1])));
                            }
                            else
                            {
                                string oldValue = equation.Substring(minindex - 1 - numbers[negindex - 1].Length, numbers[negindex - 1].Length + numbers[negindex].Length + 1);
                                equation = equation.Replace(oldValue, Convert.ToString(double.Parse(numbers[negindex - 1]) - double.Parse(numbers[negindex])));
                            }
                        }
                        else if (ops.Count > 1)
                        {
                            string oldValue = equation.Substring(equation.IndexOf("-") - numbers[ops.IndexOf('-')].Length, numbers[ops.IndexOf('-')].Length + numbers[ops.IndexOf('-') + 1].Length+1);
                            equation = equation.Replace(oldValue, Convert.ToString(Calculate(oldValue)));
                        }
                        else
                            return Convert.ToDouble(numbers[0]) - Convert.ToDouble(numbers[1]);
                    }
                }
                else if (ops.Contains('+'))
                {
                    if (Convert.ToDouble(numbers[ops.IndexOf('+')]) < 0 || Convert.ToDouble(numbers[ops.IndexOf('+') + 1]) < 0)
                    {
                        if (negindex == 0)
                        {
                            string oldValue = equation.Substring(equation.IndexOf('-'), numbers[0].Length + numbers[1].Length + 1);
                            equation = equation.Replace(oldValue, Convert.ToString(double.Parse(numbers[0]) + double.Parse(numbers[1])));
                        }
                        else
                        {
                            string oldValue = equation.Substring(minindex - 1 - numbers[negindex - 1].Length, numbers[negindex - 1].Length + numbers[negindex].Length + 1);
                            equation = equation.Replace(oldValue, Convert.ToString(double.Parse(numbers[negindex - 1]) + double.Parse(numbers[negindex])));
                        }
                    }
                    else if (ops.Count > 1)
                    {
                        string oldValue = equation.Substring(equation.IndexOf("+") - numbers[ops.IndexOf('+')].Length, numbers[ops.IndexOf('+')].Length + numbers[ops.IndexOf('+') + 1].Length+1);
                        equation = equation.Replace(oldValue, Convert.ToString(Calculate(oldValue)));
                    }
                    else
                        return Convert.ToDouble(numbers[0]) + Convert.ToDouble(numbers[1]);
                }
                else if (ops.Contains('-'))
                {
                    if (Convert.ToDouble(numbers[ops.IndexOf('-')]) < 0 || Convert.ToDouble(numbers[ops.IndexOf('-') + 1]) < 0)
                    {
                        if (negindex == 0)
                        {
                            string oldValue = equation.Substring(equation.IndexOf('-'), numbers[0].Length + numbers[1].Length + 1);
                            equation = equation.Replace(oldValue, Convert.ToString(double.Parse(numbers[0]) - double.Parse(numbers[1])));
                        }
                        else
                        {
                            string oldValue = equation.Substring(minindex - 1 - numbers[negindex - 1].Length, numbers[negindex - 1].Length + numbers[negindex].Length + 1);
                            equation = equation.Replace(oldValue, Convert.ToString(double.Parse(numbers[negindex - 1]) - double.Parse(numbers[negindex])));
                        }
                    }
                    else if (ops.Count > 1)
                    {
                        string oldValue = equation.Substring(equation.IndexOf("-") - numbers[ops.IndexOf('-')].Length, numbers[ops.IndexOf('-')].Length + numbers[ops.IndexOf('-') + 1].Length+1);
                        equation = equation.Replace(oldValue, Convert.ToString(Calculate(oldValue)));
                    }
                    else
                        return Convert.ToDouble(numbers[0]) - Convert.ToDouble(numbers[1]);
                }
                if (ops.Contains('+') || ops.Contains('-'))
                    continue;


            }
            return Convert.ToDouble(equation);
        }
    }
}

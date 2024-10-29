using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*WordAndFrequency we using for functions
"person_words"("p_name" varchar, "p_limit" int4=10)
  RETURNS TABLE("word" text, "frequency" int4)
"word_to_words_query"(IN "result_limit" int4, VARIADIC "keywords" _text)
  RETURNS TABLE("word" text, "frequency" int4)*/
namespace DataAcessLayer.Functions;

public class WordAndFrequency
{
    public string Word { get; set; } 
    public int Frequency {  get; set; }

}

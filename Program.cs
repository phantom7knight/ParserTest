using System;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using System.Collections.Generic;






namespace ParserTest
{
   public class DescriptionSchema
    {
        public string m_userName;

        public string m_operType;

        public string m_Info;

        public string m_symbol;

        public DescriptionSchema()
        {
            m_userName = "";
            m_operType = "";
            m_Info = "";
            m_symbol = "";
        }

    };
    
    [Serializable()]
    [System.Xml.Serialization.XmlRoot(ElementName = "Operations")]
    public class Operations
    {
        

        [XmlElement("Add", Type = typeof(Add))]
        [XmlElement("Multiply", Type = typeof(Multiply))]
        [XmlElement("Subtract", Type = typeof(Subtract))]
        [XmlElement("Divide", Type = typeof(Divide))]
        public System.Collections.Generic.List<BaseOperation> data_ { get; set; }


    }


    [Serializable()]
    public abstract class BaseOperation
    {
        [System.Xml.Serialization.XmlElement("Description")]
        public string m_Description;

        [System.Xml.Serialization.XmlElement("Value1")]
        public int m_val1 = 0;

        [System.Xml.Serialization.XmlElement("Value2")]
        public int m_val2 = 0;

        public abstract int CalculateResult() ;
        public abstract string OperationOperator() ;
    }

    [Serializable()]
    public class Subtract : BaseOperation
    {
       
        public override int CalculateResult()
        {
            return m_val1 - m_val2;
        }

        public override string OperationOperator()
        {
            return "-";
        }

    }


    [Serializable()]
    public class Add : BaseOperation
    {
        public override int CalculateResult()
        {
            return m_val1 + m_val2;
        }

        public override string OperationOperator()
        {
            return "+";
        }
    }

    [Serializable()]
    public class Multiply : BaseOperation
    {
        public override int CalculateResult()
        {
            return m_val1 * m_val2;
        }

        public override string OperationOperator()
        {
            return "*";
        }
    }

    [Serializable()]
    public class Divide : BaseOperation
    {
        public override int CalculateResult()
        {
            return m_val1 / m_val2;
        }

        public override string OperationOperator()
        {
            return "/";
        }

    }


    class XMLParser
    {
        //Subdivide the string based on the delimitter
        public DescriptionSchema StringSplitter(string a_input, char a_delimitter,string operatString)
        {
            DescriptionSchema result = new DescriptionSchema();


            string temp_copy = a_input;

            string[] arraY_here = temp_copy.Split(a_delimitter);


            result.m_userName = arraY_here[0];
            result.m_operType = arraY_here[1];
            result.m_Info = arraY_here[2];
            result.m_symbol = operatString;

            return result;
        }

        //Print based on the XML Document
        public void PrintInformation(DescriptionSchema a_descSchema, int a_val1, int a_val2, int a_res)
        {
            Console.WriteLine(a_descSchema.m_userName + " - " + a_descSchema.m_operType + " - " + a_val1 + " " + a_descSchema.m_symbol + " " + a_val2 + " = " + a_res );

            return;
        }


        //Additional Helper function for Reading XML Doc
        public string ReadXMLDoc(string a_fileName)
        {
            string result = "";

            result = System.IO.File.ReadAllText(a_fileName);

            return result;
        }

        // Deserializing XML file
        public void  DeserializeXML(string a_xmlFile)
        {
            Operations ops_here = null;

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Operations));
                StreamReader reader = new StreamReader(a_xmlFile);
                ops_here = (Operations)serializer.Deserialize(reader);
                reader.Close();
            }

            catch(Exception e)
            {
                Console.WriteLine(e.InnerException.ToString());
                return;
            }


            if (ops_here.data_.Count != 0)
            {
                for (int i = 0; i < ops_here.data_.Count; i++)
                {
                    //split the given description string
                    DescriptionSchema split_here = StringSplitter(ops_here.data_[i].m_Description, ';', ops_here.data_[i].OperationOperator());
           
                    int Calc_result = ops_here.data_[i].CalculateResult();
           
                    //print the output
                    PrintInformation(split_here, ops_here.data_[i].m_val1, ops_here.data_[i].m_val2, Calc_result);
           
                }
           
            }
           
        }


    }

    class Program
    {

        
        static void Main(string[] args)
        {
           
            //Read a file and get data
            XMLParser xmlObj = new XMLParser();

            int case_num = 2;

            switch(case_num)
            {
                case 0:
                    {
                        xmlObj.DeserializeXML("../../../Test1.xml");
                        break;
                    }
                case 1:
                    {
                        xmlObj.DeserializeXML("../../../Test2.xml");
                        break;
                    }
                case 2:
                    {
                        xmlObj.DeserializeXML("../../../Test3.xml");
                        break;
                    }
                case 3:
                    {
                        xmlObj.DeserializeXML("../../../Test4.xml");
                        break;
                    }
                case 4:
                    {
                        xmlObj.DeserializeXML("../../../math.xml");
                        break;
                    }
            }


            return;

            

        }
    }
}

using System;
using System.IO;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Software
{
    [Serializable]
    class Software
    {
        static void Main(string[] args)
        {
            PO[] soft = null;
            try
            {
                //string res;
                string[] Text = File.ReadAllLines("input.txt");
                int n = Int32.Parse(Text[0]);
                if (n > Text.Count<string>() - 1) { throw new IOException(); }
                soft = new PO[n];
                string[] temp;
                Regex r_free = new Regex("^free, ");
                Regex r_shareware = new Regex("^shareware, ");
                Regex r_commercial = new Regex("^commercial, ");
                for (int i = 1; i <= n; i++)
                {
                    if (r_free.IsMatch(Text[i]))
                    {
                        temp = Regex.Split(Text[i], ", ");
                        soft[i - 1] = new Free(temp[1], temp[2]);
                    }
                    else if (r_shareware.IsMatch(Text[i]))
                    {
                        temp = Regex.Split(Text[i], ", ");
                        soft[i - 1] = new Shareware(temp[1], temp[2], temp[3], Byte.Parse(temp[4]), Int32.Parse(temp[5]));
                    }
                    else if (r_commercial.IsMatch(Text[i]))
                    {
                        temp = Regex.Split(Text[i], ", ");
                        soft[i - 1] = new Commercial(temp[1], temp[2], temp[3], Byte.Parse(temp[4]), Int32.Parse(temp[5]), Int32.Parse(temp[6]));
                    }
                }

            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Файл input.txt не найден");
                Console.ReadKey();
                return;
            }
            catch (IOException)
            {
                Console.WriteLine("Неверный формат входного файла");
                Console.ReadKey();
                return;
            }

            
            XmlSerializer formatter = new XmlSerializer(typeof(PO[]));
            using (FileStream fs = new FileStream("data.xml", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, soft);
            }
           

            Console.WriteLine("Доступное ПО:");
            Console.WriteLine("-------------------------------------------------");
            foreach (PO p in soft)
                p.Info();

            Console.WriteLine("ПО готовое к использованию: ");
            Console.WriteLine("-------------------------------------------------");
            foreach (PO p in soft)
                if (p.ItIsAWorks()) Console.WriteLine(p.ProgramName);

            Console.ReadLine();
        }
    }
    [XmlInclude(typeof(Free)), XmlInclude(typeof(Shareware)), XmlInclude(typeof(Commercial))]
    /// <summary>
    /// Абстрактный класс "программное обеспечение"
    /// </summary>    
    [Serializable]
    public abstract class PO
    {

        /// <summary>
        /// конструктор абстрактного класса по умолчанию
        /// </summary>
        public PO() { }
        public string name;
        public string company;
        public DateTime dateOfInstall;
        public byte demo_preiod;
        public int cost;

        /// <summary>
        /// конструктор абстрактного класса
        /// </summary>
        /// <param name="name">название программы</param>
        /// <param name="company">компания-производитель</param>
        /// <param name="date">дата активации/установки на компьютер</param>
        /// <param name="demo_period">продолжительность демо-версии в днях</param>
        /// <param name="cost">стоимость</param>
        public PO(string name, string company, string date, byte demo_period, int cost)
        {
            this.name = name;
            this.company = company;
            this.dateOfInstall = DateTime.Parse(date);
            this.demo_preiod = demo_period;
            this.cost = cost;
        }
        /// <summary>
        /// метод, выводящий справку о ПО
        /// </summary>
        public virtual void Info()
        {
            Console.WriteLine("\nИмя продукта: " + name + " Производитель: "
                                 + company + " дата установки ПО: " + dateOfInstall
                                 + "Период бесплатного использования: " + demo_preiod
                                 + " стоимость продукта: " + cost + "\n");
        }
        /// <summary>
        /// метод, возвращаююций булевое значение, соответствующее текущей готовности приложения к работе
        /// </summary>
        public virtual bool ItIsAWorks()
        {
            if (dateOfInstall.AddDays(demo_preiod) >= DateTime.Now) return true;
            return false;
        }
        public string ProgramName
        {
            get { return name; }
        }
      
    }
    [Serializable]
    public class Free : PO
    {
        /// <summary>
        /// конструктор  по умолчанию
        /// </summary>
        public Free() { }
        /// <summary>
        /// конструктор класса Free
        /// </summary>
        /// <param name="name">название программы</param>
        /// <param name="company">компания-производитель</param>      
        public Free(string name, string company) : base(name, company, "01.01.2000", 0, 0)
        {
            Trace.WriteLine("Конструктор класса Free отработал");
        }
        /// <summary>
        /// метод, выводящий справку о ПО
        /// </summary>
        public override void Info()
        {
            Console.WriteLine("Имя продукта: " + name + "\nПроизводитель: " + company + "\n");
            Trace.WriteLine("Метод Info() в классе Free отработал");
        }
        /// <summary>
        /// метод, возвращаююций булевое значение, соответствующее текущей готовности приложения к работе
        /// </summary>
        public override bool ItIsAWorks()
        {
            Trace.WriteLine("Метод ItIsAWorks() в классе Free отработал");
            return true;
            
        }
    }
    [Serializable]
    public class Shareware : PO
    {
        /// <summary>
        /// конструктор  по умолчанию
        /// </summary>
        public Shareware() { }
        /// <summary>
        /// конструктор класса Shareware
        /// </summary>
        /// <param name="name">название программы</param>
        /// <param name="company">компания-производитель</param>
        /// <param name="date">дата активации/установки на компьютер</param>
        /// <param name="demo_period">продолжительность демо-версии в днях</param>
        /// <param name="cost">стоимость</param>
        public Shareware(string name, string company, string date, byte demo_period, int cost)
            : base(name, company, date, demo_period, cost)
        {
            Trace.WriteLine("Конструктор класса Shareware отработал");
        }
        /// <summary>
        /// метод, выводящий справку о ПО
        /// </summary>
        public override void Info()
        {
            Console.WriteLine("Имя продукта: " + name + "\nПроизводитель: "
                                 + company + "\nДата установки ПО: " + dateOfInstall.ToShortDateString()
                                 + "\nПериод бесплатного использования: " + demo_preiod
                                 + "\nCтоимость продукта: " + cost + "$\n");
            Trace.WriteLine("Метод Info() в классе Shareware отработал");
        }
        /// <summary>
        /// метод, возвращаююций булевое значение, соответствующее текущей готовности приложения к работе
        /// </summary>
        public override bool ItIsAWorks()
        {
            if (dateOfInstall.AddDays(demo_preiod) >= DateTime.Now) { Trace.WriteLine("Метод ItIsAWorks() в классе Shareware отработал"); return true; }
            Trace.WriteLine("Метод ItIsAWorks() в классе Shareware отработал");
            return false;
        }
    }
    [Serializable]
    public class Commercial : PO
    {
        /// <summary>
        /// конструктор  по умолчанию
        /// </summary>
        public Commercial() { }
        int period;
        /// <summary>
        /// конструктор класса Commercial
        /// </summary>
        /// <param name="name">название программы</param>
        /// <param name="company">компания-производитель</param>
        /// <param name="date">дата активации/установки на компьютер</param>
        /// <param name="demo_period">продолжительность демо-версии в днях</param>
        /// <param name="cost">стоимость</param>
        /// <param name="period">длительность подписки в днях</param>
        public Commercial(string name, string company, string date, byte demo_period, int cost, int period)
            : base(name, company, date, 0, cost)
        { 
            this.period = period;
            Trace.WriteLine("Конструктор класса Commercial отработал");
        }
        /// <summary>
        /// метод, выводящий справку о ПО
        /// </summary>
        public override void Info()
        {
            Console.WriteLine("Имя продукта: " + name + "\nПроизводитель: "
                                 + company + "\nДата установки ПО: " + dateOfInstall.ToShortDateString()
                                 + "\nСтоимость продукта: " + cost + "$ \nПериод использования: " + period + "\n");
            Trace.WriteLine("Метод Info() в классе Commercial отработал");
        }
        /// <summary>
        /// метод, возвращаююций булевое значение, соответствующее текущей готовности приложения к работе
        /// </summary>
        public override bool ItIsAWorks()
        {
            if (dateOfInstall.AddDays(period) >= DateTime.Now) { Trace.WriteLine("Метод ItIsAWorks() в классе Commercial отработал");  return true; }
            Trace.WriteLine("Метод ItIsAWorks() в классе Commercial отработал");
            return false;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace AutofacDemoApp
{
    public interface IOutput {
        void Write(string content);
    }

    public class ConsoleOutput : IOutput {
        public void Write(string content) {
            Console.WriteLine(content);
        }
    }

    public interface IDateWriter {
        void WriteDate();
    }
    public class TodayWriter : IDateWriter
    {
        private IOutput _output;

        public TodayWriter(IOutput output) {
            this._output = output;
        }
        public void WriteDate()
        {
            this._output.Write(DateTime.Today.ToShortDateString());
        }
    }

    public class TomorrowWriter : IDateWriter {
        private IOutput _output;

        public TomorrowWriter(IOutput output) {
            this._output = output;
        }

        public void WriteDate() {
            this._output.Write(DateTime.Today.AddDays(1).ToShortDateString());
        }

    }
}

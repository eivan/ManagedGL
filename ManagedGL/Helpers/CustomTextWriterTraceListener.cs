using System;
using System.Diagnostics;

namespace ManagedGL.Helpers
{
    public class CustomTextWriterTraceListener : TextWriterTraceListener
    {
        private static object dork = new Object();
        public CustomTextWriterTraceListener(string file) : base(file) { }
        public CustomTextWriterTraceListener(string file, string name) : base(file, name) { }

        public override void WriteLine(string message, string category)
        {
            lock (dork)
            {
                var t = DateTime.Now;
                message = t.ToString() + ":" + t.Millisecond.ToString() + "," + message + "," + category;
                base.WriteLine(message);
            }
        }

        public override void WriteLine(string message)
        {
            lock (dork)
            {
                var t = DateTime.Now;
                message = t.ToString() + ":" + t.Millisecond.ToString() + "," + message;
                base.WriteLine(message);
            }
        }
    }
}

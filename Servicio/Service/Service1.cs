using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;

namespace Service
{
    public partial class Service : ServiceBase
    {
        private String frec = ConfigurationManager.AppSettings.Get("frec");

        private String servName = ConfigurationManager.AppSettings.Get("name");
        private String servDesc = ConfigurationManager.AppSettings.Get("desc");

        private String monday = ConfigurationManager.AppSettings.Get("Monday");
        private String tuesday = ConfigurationManager.AppSettings.Get("Tuesday");
        private String wednesday = ConfigurationManager.AppSettings.Get("Wednesday");
        private String thursday = ConfigurationManager.AppSettings.Get("Thursday");
        private String friday = ConfigurationManager.AppSettings.Get("Friday");
        private String saturday = ConfigurationManager.AppSettings.Get("Saturday");
        private String sunday = ConfigurationManager.AppSettings.Get("Sunday");

        private int diaDesde = Int32.Parse(ConfigurationManager.AppSettings.Get("dia_desde"));
        private int diaHasta = Int32.Parse(ConfigurationManager.AppSettings.Get("dia_hasta"));

        private static TimeSpan desde = TimeSpan.Parse(ConfigurationManager.AppSettings.Get("desde"));
        private static TimeSpan hasta = TimeSpan.Parse(ConfigurationManager.AppSettings.Get("hasta"));

        private String procFile = ConfigurationManager.AppSettings.Get("procFile");
        private String procPath = ConfigurationManager.AppSettings.Get("procPath");

        bool process = false;

        public Service()
        {
            InitializeComponent();

            this.ServiceName = servName;
            
            // *** Inicializa EventLog
            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists(servName))
            {
                System.Diagnostics.EventLog.CreateEventSource(servName, "Application");
            }
            eventLog1.Source = servName;
            eventLog1.Log = "Application";
        }

        protected override void OnStart(string[] args)
        {
            timer1.Enabled = true;
            timer1.Start();
            timer1.Interval = Convert.ToDouble(frec);

            Mailer.SendMail(this.ServiceName, "El servicio se inició correctamente.");
        }

        protected override void OnStop()
        {
            timer1.Stop();
            timer1.Dispose();

            Mailer.SendMail(this.ServiceName, "El servicio se detuvo.");
        }

        private void timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                // *** Programador de Tareas 
                DateTime DTT = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.Local);
                TimeSpan DTN = DateTime.Now.TimeOfDay;
                TimeSpan DTH = new TimeSpan(DTN.Hours, DTN.Minutes, DTN.Seconds);

                // *** Días 
                switch (DTT.DayOfWeek.ToString())
                {
                    case "Monday": if (monday == "0") return; break;
                    case "Tuesday": if (tuesday == "0") return; break;
                    case "Wednesday": if (wednesday == "0") return; break;
                    case "Thursday": if (thursday == "0") return; break;
                    case "Friday": if (friday == "0") return; break;
                    case "Saturday": if (saturday == "0") return; break;
                    case "Sunday": if (sunday == "0") return; break;
                }

                // *** Horarios
                if (DTH >= desde && DTH <= hasta)
                {
                }
                else
                {
                    return;
                }

                // *** Retorna si está ocupado
                if (process)
                {
                    eventLog1.WriteEntry("Servicio Ocupado.");
                    return;
                }
                else
                {
                }

                if (stopService(DateTime.Now))
                {
                    eventLog1.WriteEntry("Servicio dentro de fecha de Cierre.");
                    return;
                }

                // Quitar la extensión .exe
                string procName = Path.GetFileNameWithoutExtension(procFile);

                // Verifica si el proceso ya está en ejecución
                var existingProcesses = Process.GetProcessesByName(procName);
                if (existingProcesses.Length == 0) // Si no hay procesos con ese nombre
                {
                    // *** Comienza
                    process = true;

                    Process proc = new Process();

                    proc.StartInfo.FileName = procFile;
                    proc.StartInfo.WorkingDirectory = procPath;
                    proc.StartInfo.UseShellExecute = true;
                    proc.Start();

                    // ** Espera a que el proceso termine
                    proc.WaitForExit();
                }
            }
            catch (Exception error)
            {
                eventLog1.WriteEntry("Error durante la ejecución del servicio: " + error.Message);
                Mailer.SendMail(this.ServiceName, "Error durante la ejecución del servicio: " + error.Message);
            }
            finally
            {
                // *** Finaliza
                process = false;
            }
        }

        private bool stopService(DateTime hoy)
        {
            // 1) Modo Stand-By del día 1 al 3 inclusive
            if (hoy.Day >= diaDesde && hoy.Day <= diaHasta)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

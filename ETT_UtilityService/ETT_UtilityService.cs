using DevExpress.Xpo;
using ETT_DAL.Abstract;
using ETT_DAL.Concrete;
using ETT_DAL.Helpers;
using ETT_UtilityService.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ETT_UtilityService
{
    public partial class ETT_UtilityService : ServiceBase
    {
        private Timer timerSchedular;
        private IUtilityServiceRepository utilityRepo;
        private Session session;

        public ETT_UtilityService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                this.ScheduleService();
            }
            catch (Exception ex)
            {
                string error = "", errorToThrow = "";
                CommonMethods.getError(ex, ref error);
                errorToThrow = CommonMethods.ConcatenateErrorIN_DB("", error, CommonMethods.GetCurrentMethodName());
                CommonMethods.LogThis(errorToThrow);
            }
        }

        protected override void OnStop()
        {
        }

        private void ScheduleService()
        {
            try
            {
                timerSchedular = new Timer(new TimerCallback(TimerScheduleCallback));
                //Set the Default Time.
                DateTime scheduledTime = DateTime.MinValue;
                CommonMethods.LogThis("Schedule mode: " + ConfigurationManager.AppSettings["ScheduleMode"].ToString());
                string scheduleMode = ConfigurationManager.AppSettings["ScheduleMode"].ToString();

                if (scheduleMode == "Dnevno")
                {
                    scheduledTime = DateTime.Parse(ConfigurationManager.AppSettings["ScheduledTime"]);
                    if (DateTime.Now > scheduledTime)
                    {
                        //If Scheduled Time is passed set Schedule for the next day.
                        scheduledTime = scheduledTime.AddDays(1);
                    }
                }
                else if (scheduleMode == "Interval")
                {
                    int intervalMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["IntervalMin"]);
                    scheduledTime = DateTime.Now.AddMinutes(intervalMinutes);

                    if (DateTime.Now > scheduledTime)
                    {
                        //If Scheduled Time is passed set Schedule for the next Interval.
                        scheduledTime = scheduledTime.AddMinutes(intervalMinutes);
                    }
                }

                TimeSpan timeSpan = scheduledTime.Subtract(DateTime.Now);
                //Get the difference in Minutes between the Scheduled and Current Time.
                int dueTime = Convert.ToInt32(timeSpan.TotalMilliseconds);

                //Change the Timer's Due Time.
                timerSchedular.Change(dueTime, Timeout.Infinite);
            }
            catch (Exception ex)
            {
                string error = "", errorToThrow = "";
                CommonMethods.getError(ex, ref error);
                errorToThrow = CommonMethods.ConcatenateErrorIN_DB("", error, CommonMethods.GetCurrentMethodName());
                CommonMethods.LogThis(errorToThrow);
            }
        }

        private void TimerScheduleCallback(object e)
        {
            try
            {
                CommonMethods.LogThis("Start");

                if (session == null)
                    session = XpoHelper.GetNewSession();

                utilityRepo = new UtilityServiceRepository(session);

                utilityRepo.MatchMobileTransWithInventoryDeliveries();

                CommonMethods.LogThis("END");
            }
            catch (Exception ex)
            {
                string error = "", errorToThrow = "";
                CommonMethods.getError(ex, ref error);
                errorToThrow = CommonMethods.ConcatenateErrorIN_DB("", error, CommonMethods.GetCurrentMethodName());
                CommonMethods.LogThis(errorToThrow);
            }

            this.ScheduleService();
        }
    }
}

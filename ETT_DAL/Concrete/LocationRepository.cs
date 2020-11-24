using DevExpress.Xpo;
using ETT_DAL.Abstract;
using ETT_DAL.ETTPotocnik;
using ETT_DAL.Helpers;
using ETT_DAL.Models.Location;
using ETT_Utilities.Common;
using ETT_Utilities.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ETT_DAL.Concrete
{
    public class LocationRepository : ILocationRepository
    {
        Session session;

        public LocationRepository(Session session = null)
        {
            if (session == null)
                this.session = XpoHelper.GetNewSession();
            else
                this.session = session;
        }

        public Location GetLocationByID(int lId, Session currentSession = null)
        {
            try
            {
                XPQuery<Location> location = null;

                if (currentSession != null)
                    location = currentSession.Query<Location>();
                else
                    location = session.Query<Location>();

                return location.Where(l => l.LocationID == lId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_10, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public int SaveLocation(Location model, int userID = 0)
        {
            try
            {
                model.tsUpdate = DateTime.Now;
                model.tsUpdateUserID = userID;

                if (model.LocationID == 0)
                {
                    model.tsInsert = DateTime.Now;
                    model.tsInsertUserID = userID;
                }

                model.Save();

                return model.LocationID;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_11, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public bool DeleteLocation(int lId)
        {
            try
            {
                GetLocationByID(lId).Delete();
                return true;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_12, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public bool DeleteLocation(Location model)
        {
            try
            {
                model.Delete();
                return true;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_12, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public Location GetLocationDefault(Session currentSession = null)
        {
            try
            {
                XPQuery<Location> location = null;

                if (currentSession != null)
                    location = currentSession.Query<Location>();
                else
                    location = session.Query<Location>();

                var obj = location.Where(l => l.Name.Contains("Neznana")).FirstOrDefault();

                if (obj == null)
                {
                    obj = new Location(session);
                    obj.IsBuyer = false;
                    obj.Name = "Neznana lokacija";
                    obj.Notes = "Avtomatsko generirana privzeta lokacija iz strani ETT aplikacije!";
                    obj.tsInsert = DateTime.Now;
                    obj.tsInsertUserID = 0;
                    obj.tsUpdate = DateTime.Now;
                    obj.tsUpdateUserID = 0;
                    obj.Save();
                }

                return obj;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_10, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public List<LocationAPIModel> GetLocationsMobile()
        {
            try
            {
                XPQuery<Location> location = location = session.Query<Location>();

                var query = from loc in location
                            select new LocationAPIModel
                            {
                                belongs_to = null,
                                created_at = loc.tsInsert.ToShortDateString(),
                                depth = null,
                                description = loc.Notes,
                                id = loc.LocationID.ToString(),
                                is_client = loc.IsBuyer ? "1" : "0",
                                lft = null,
                                name = loc.Name,
                                parent_id = null,
                                rgt = null,
                                updated_at = loc.tsUpdate.ToShortDateString()
                            };

                return query.ToList();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_10, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public List<Location> GetWarehouses(Session currentSession = null)
        {
            try
            {
                XPQuery<Location> location = null;

                if (currentSession != null)
                    location = currentSession.Query<Location>();
                else
                    location = session.Query<Location>();

                return location.Where(l => l.IsWarehouse).ToList();
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_10, error, CommonMethods.GetCurrentMethodName()));
            }
        }

        public bool IsLocationWarehouse(int locationID, Session currentSession = null)
        {
            try
            {
                XPQuery<Location> location = null;

                if (currentSession != null)
                    location = currentSession.Query<Location>();
                else
                    location = session.Query<Location>();

                return location.Where(l => l.LocationID == locationID).FirstOrDefault() != null ? location.Where(l => l.LocationID == locationID).FirstOrDefault().IsWarehouse : false;
            }
            catch (Exception ex)
            {
                string error = "";
                CommonMethods.getError(ex, ref error);
                throw new Exception(CommonMethods.ConcatenateErrorIN_DB(DB_Exception.res_10, error, CommonMethods.GetCurrentMethodName()));
            }
        }
    }
}
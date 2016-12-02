using Afonsoft.Petz.DataBase;
using Afonsoft.Petz.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Afonsoft.Petz.Controller
{
    public class StatusController
    {
        public StatusEntity GetStatus(int id)
        {
            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                return db.petz_Status
                    .Where(x => x.status_id == id)
                    .Select(x => new StatusEntity()
                    {
                        Id = x.status_id,
                        Name = x.status_name,
                        Description = x.status_description,
                        BackgroundColor = x.default_background_color,
                        BorderColor = x.default_border_color,
                        TextColor = x.default_text_color
                    }).FirstOrDefault();
            }
        }

        public StatusEntity GetStatus(StatusEnum status)
        {
            return GetStatus((int)status);
        }

        public StatusEntity[] GetStatus()
        {
            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                return db.petz_Status
                    .Select(x => new StatusEntity()
                    {
                        Id = x.status_id,
                        Name = x.status_name,
                        Description = x.status_description,
                        BackgroundColor = x.default_background_color,
                        BorderColor = x.default_border_color,
                        TextColor = x.default_text_color
                    }).ToArray();
            }
        }

        public StatusEntity GetStatusCompany(int companyId, StatusEnum status)
        {
            return GetStatusCompany(companyId, (int)status);
        }

        public StatusEntity GetStatusCompany(int companyId, int statusId)
        {
            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                var companySt = db.petz_Company_Status_Color
                                .Where(x => x.company_id == companyId && x.status_id == statusId && x.date_delete == null)
                                .Select(x => new StatusEntity()
                                {
                                    Id = x.status_id,
                                    //Name = x.status_name,
                                    //Description = x.status_description,
                                    BackgroundColor = x.status_background_color,
                                    BorderColor = x.status_border_color,
                                    TextColor = x.status_text_color
                                }).FirstOrDefault();

                if (companySt == null)
                    return GetStatus(statusId);
                else
                {
                    var info = GetStatus(statusId);
                    companySt.Name = info.Name;
                    companySt.Description = info.Description;
                    companySt.BackgroundColor = (string.IsNullOrEmpty(companySt.BackgroundColor) ? info.BackgroundColor : companySt.BackgroundColor);
                    companySt.BorderColor = (string.IsNullOrEmpty(companySt.BorderColor) ? info.BorderColor : companySt.BorderColor);
                    companySt.TextColor = (string.IsNullOrEmpty(companySt.TextColor) ? info.TextColor : companySt.TextColor);
                    return companySt;
                }
            }
        }

        public StatusEntity[] GetStatusCompany(int companyId)
        {
            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                var companySt = db.petz_Company_Status_Color
                                .Where(x => x.company_id == companyId && x.date_delete == null)
                                .Select(x => new StatusEntity()
                                {
                                    Id = x.status_id,
                                    //Name = x.status_name,
                                    //Description = x.status_description,
                                    BackgroundColor = x.status_background_color,
                                    BorderColor = x.status_border_color,
                                    TextColor = x.status_text_color
                                }).ToArray();

                if (companySt.Length <= 0)
                    return GetStatus();
                else
                {
                    List<StatusEntity> listStatusEntity = new List<StatusEntity>();
                    foreach (var st in companySt)
                    {
                        var info = GetStatus(st.Id);
                        st.Name = info.Name;
                        st.Description = info.Description;
                        st.BackgroundColor = (string.IsNullOrEmpty(st.BackgroundColor) ? info.BackgroundColor : st.BackgroundColor);
                        st.BorderColor = (string.IsNullOrEmpty(st.BorderColor) ? info.BorderColor : st.BorderColor);
                        st.TextColor = (string.IsNullOrEmpty(st.TextColor) ? info.TextColor : st.TextColor);
                        listStatusEntity.Add(st);
                    }
                    return listStatusEntity.ToArray();
                }
            }
        }

        public void SetStatusCompany(int companyId, StatusEntity statusEntity)
        {
            if (companyId <= 0)
                throw new ArgumentNullException(nameof(companyId), "CompanyID is null or invalid");

            if (statusEntity == null)
                throw new ArgumentNullException(nameof(statusEntity), "statusEntity is null or invalid");
            if (statusEntity.Id <= 0)
                throw new ArgumentNullException(nameof(statusEntity.Id), "statusEntity.ID is null or invalid");

            StatusEntity info = GetStatus(statusEntity.Id);
            statusEntity.BackgroundColor = (string.IsNullOrEmpty(statusEntity.BackgroundColor) ? info.BackgroundColor : statusEntity.BackgroundColor);
            statusEntity.BorderColor = (string.IsNullOrEmpty(statusEntity.BorderColor) ? info.BorderColor : statusEntity.BorderColor);
            statusEntity.TextColor = (string.IsNullOrEmpty(statusEntity.TextColor) ? info.TextColor : statusEntity.TextColor);

            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                var upd = db.petz_Company_Status_Color.FirstOrDefault(x => x.company_id == companyId && x.status_id == statusEntity.Id && x.date_delete == null);
                if (upd == null)
                {
                    upd = new petz_Company_Status_Color();
                    upd.status_background_color = statusEntity.BackgroundColor;
                    upd.status_border_color = statusEntity.BorderColor;
                    upd.status_text_color = statusEntity.TextColor;
                    db.petz_Company_Status_Color.Add(upd);
                }
                else
                {
                    upd.status_background_color = statusEntity.BackgroundColor;
                    upd.status_border_color = statusEntity.BorderColor;
                    upd.status_text_color = statusEntity.TextColor;
                }
                db.SaveChanges();
            }
        }

        public void SetStatusScheduling(int schedulingId, StatusEnum status)
        {
            SetStatusScheduling(schedulingId, (int)status);
        }

        public void SetStatusScheduling(int schedulingId, int statusId)
        {
            if (schedulingId <= 0)
                throw new ArgumentNullException(nameof(schedulingId), "SchedulingID is null or invalid");

            if (statusId <= 0 && statusId > 8)
                throw new ArgumentOutOfRangeException(nameof(statusId), statusId, "Status out of range, min 1 and max 8");

            using (Petz_dbEntities db = new Petz_dbEntities())
            {
                var upd = db.petz_Pet_Scheduling.FirstOrDefault(x => x.scheduling_id == schedulingId);

                if (upd != null)
                {
                    upd.status_id = statusId;
                    db.SaveChanges();
                }
            }
        }   
    }
}
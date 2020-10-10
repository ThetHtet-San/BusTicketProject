using Com.MrIT.Common;
using Com.MrIT.DBEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.MrIT.DataRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : GenericEntity
    {
        protected readonly DataContext DbContext;
        protected readonly ILogger repoLogger;

        protected DbSet<T> entities;

        string errorMessage = string.Empty;

        public GenericRepository(DataContext context, ILoggerFactory loggerFactory, string childClassName)
        {
            this.DbContext = context;
            entities = context.Set<T>();
            repoLogger = loggerFactory.CreateLogger(childClassName);
        }

        public IEnumerable<T> GetActiveAll()
        {
            repoLogger.LogDebug("GetActiveAll", null);
            return  entities.Where(s => s.SystemActive == true && s.Active == true).AsNoTracking().ToList();
        }

        public IEnumerable<T> GetAll()
        {
            repoLogger.LogDebug("GetAll", null);
            return  entities.AsNoTracking().ToList();
        }

        public async Task<T> Get(int id)
        {
            repoLogger.LogDebug("Get by id " + id, null);

            var result = await entities.AsNoTracking().SingleOrDefaultAsync(s => s.ID == id);

            return result;
        }

        public T GetWithoutAsync(int id)
        {
            repoLogger.LogDebug("Get by id " + id, null);

            var result = entities.AsNoTracking().SingleOrDefault(s => s.ID == id);

            return result;
        }

        public  T GetLastRow()
        {
            repoLogger.LogDebug("Get Last Row ", null);

            var result =  entities.OrderByDescending(e => e.ID).AsNoTracking().FirstOrDefault();

            return result;
        }

        public void Commit()
        {
            this.DbContext.SaveChanges();

        }



        public T Add(T entity)
        {
            if (entity == null)
            {
                repoLogger.LogError("Cannot insert entiry as entity is null", null);
                throw new ArgumentNullException("entity");
            }

            entity.CreatedOn = entity.ModifiedOn = DateTime.Now;
            entity.SystemActive = true;
            entity.Active = true;

            entities.Add(entity);
            var result = entity;
            this.Commit();
            //RecordChangeLog(entity, "Create");

            string tableID = entity.ID.ToString();
            try
            {
                var sourceProperties = entity.GetType().GetProperties();
                foreach (var sourceProperty in sourceProperties)
                {
                    var dbChangeLog = new ChangeLog();
                    dbChangeLog.TableName = typeof(T).FullName;
                    dbChangeLog.FieldName = sourceProperty.Name;
                    dbChangeLog.Action = "Create";
                    dbChangeLog.TableID = tableID;
                    if (sourceProperty.GetValue(entity) != null)
                    {
                        if (sourceProperty.PropertyType.FullName == "System.Byte[]")
                        {
                            dbChangeLog.FieldValue = Convert.ToBase64String((byte[])sourceProperty.GetValue(entity));
                        }
                        else
                        {
                            dbChangeLog.FieldValue = sourceProperty.GetValue(entity).ToString();
                        }

                    }
                    dbChangeLog.CreatedBy = dbChangeLog.ModifiedBy = entity.ModifiedBy;
                    dbChangeLog.CreatedOn = dbChangeLog.ModifiedOn = DateTime.Now;
                    dbChangeLog.SystemActive = true;
                    dbChangeLog.Active = true;

                    this.DbContext.ChangeLogs.Add(dbChangeLog);
                    this.Commit();
                }

            }
            catch (Exception ex)
            {
                string err = ex.Message;
            }

            return result;
        }

        public T Update(T entity)
        {
            if (entity == null)
            {
                repoLogger.LogError("Cannot insert entiry as entity is null", null);
                throw new ArgumentNullException("entity");
            }
            var oldEntity = this.entities.Where(e => e.ID == entity.ID).AsNoTracking().SingleOrDefault();

            entity.ModifiedOn = DateTime.Now;

            entities.Update(entity);
            //entities.Update(entity).State = EntityState.Modified;
            var result = entity;
            this.Commit();
            //RecordChangeLog(entity, "Update");

            string tableID = entity.ID.ToString();
            try
            {
                var sourceProperties = entity.GetType().GetProperties();
                var oldSourceProperties = oldEntity.GetType().GetProperties();
                foreach (var sourceProperty in sourceProperties)
                {
                    foreach (var oldSourceProperty in oldSourceProperties)
                    {
                        if (sourceProperty.Name == oldSourceProperty.Name)
                        {
                            var newValue = "";
                            if (sourceProperty.GetValue(entity) != null)
                            {
                                if (sourceProperty.PropertyType.FullName == "System.Byte[]")
                                {
                                    newValue = Convert.ToBase64String((byte[])sourceProperty.GetValue(entity));
                                }
                                else
                                {
                                    newValue = sourceProperty.GetValue(entity).ToString();
                                }

                            }

                            var oldValue = "";
                            if (oldSourceProperty.GetValue(oldEntity) != null)
                            {
                                oldValue = oldSourceProperty.GetValue(oldEntity).ToString();
                            }
                            if (oldSourceProperty.GetValue(oldEntity) != null)
                            {
                                if (oldSourceProperty.PropertyType.FullName == "System.Byte[]")
                                {
                                    oldValue = Convert.ToBase64String((byte[])oldSourceProperty.GetValue(oldEntity));
                                }
                                else
                                {
                                    oldValue = oldSourceProperty.GetValue(oldEntity).ToString();
                                }

                            }

                            if (newValue != oldValue)
                            {
                                var dbChangeLog = new ChangeLog();
                                dbChangeLog.TableName = typeof(T).FullName;
                                dbChangeLog.TableID = tableID;
                                dbChangeLog.FieldName = sourceProperty.Name;
                                dbChangeLog.Action = "Update";
                                dbChangeLog.FieldValue = newValue;
                                dbChangeLog.CreatedBy = dbChangeLog.ModifiedBy = entity.ModifiedBy;
                                dbChangeLog.CreatedOn = dbChangeLog.ModifiedOn = DateTime.Now;
                                dbChangeLog.Active = true;
                                dbChangeLog.SystemActive = true;

                                this.DbContext.ChangeLogs.Add(dbChangeLog);
                                this.Commit();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string err = ex.Message;
            }

            return result;
        }

        public void Delete(T entity)
        {
            if (entity == null)
            {
                repoLogger.LogError("Cannot insert entiry as entity is null", null);
                throw new ArgumentNullException("entity");
            }

            entities.Remove(entity);
        }

        public virtual async Task<PageResult<T>> GetPage(string keyword, int page, int totalRecords = 10)
        {
            var records = await entities.Where(e => e.SystemActive == true && e.Active == true)
                           .OrderBy(e => e.CreatedOn)
                           .Skip((totalRecords * page) - totalRecords)
                           .Take(totalRecords).AsNoTracking()
                           .ToListAsync<T>();

            int count = await entities.CountAsync(e => e.SystemActive == true && e.Active == true);

            var result = new PageResult<T>()
            {
                Records = records,
                TotalPage = (count + totalRecords - 1) / totalRecords,
                CurrentPage = page,
                TotalRecords = count
            };

            return result;
        }

        
    }
}

﻿namespace HS.ERP.DataAccess.Repogitory
{
   using HS.ERP.DataAccess.DbContexts;
   using System;
   using System.Collections.Generic;
   using System.Data.Entity;
   using System.Data.Entity.Infrastructure;
   using System.Diagnostics;
   using System.Linq;

   public class ERPRepogitary<TEntity> : IERPRepogitary<TEntity> where TEntity : class
   {
      private string _connectString;

      public ERPRepogitary(string connectString)
      {
         this._connectString = connectString;
      }

      public IEnumerable<TEntity> GetAll()
      {
         using (var context = new HSERPEntities(_connectString))
         {
            try
            {
               var list = context.Set<TEntity>().ToList();
               return list;
            }
            catch (Exception e)
            {
               Console.WriteLine(e.InnerException.Message);
               return null;
            }
         }
      }

      public TEntity GetById(object id)
      {
         using (var context = new HSERPEntities(_connectString))
         {
            try
            {
               return context.Set<TEntity>().Find(id);
            }
            catch
            {
               return null;
            }
         }
      }

      public TEntity Insert(TEntity parameter)
      {
         using (var context = new HSERPEntities(_connectString))
         {

            try
            {
               var para = context.Set<TEntity>().Add(parameter);
               context.SaveChanges();
               return para;

            }
            catch (DbUpdateException e)
            {
               Console.WriteLine(e.InnerException.Message);
               return null;
            }
         }
      }

      public void Update(TEntity parameter)
      {
         using (var context = new HSERPEntities(_connectString))
         {
            try
            {
               context.Set<TEntity>().Attach(parameter);
               context.Entry(parameter).State = EntityState.Modified;
               context.SaveChanges();
            }
            catch (Exception e)
            {
               Debug.WriteLine(e.Message);
            }
         }
      }

      public void Delete(object id)
      {
         using (var context = new HSERPEntities(_connectString))
         {
            TEntity entity = context.Set<TEntity>().Find(id);
            context.Set<TEntity>().Remove(entity);
            Save(context);
         }
      }

      public void Save(HSERPEntities context)
      {
         context.SaveChanges();
      }
   }
}
